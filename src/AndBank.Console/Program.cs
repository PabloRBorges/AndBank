using AndBank.Data.Context;
using AndBank.Data.Repository;
using AndBank.Processs.Aplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using System.Text;

string environment;

#region Parametros
#if DEBUG
environment = "Development";
#else
        environment = "Production";
#endif

var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

IConfiguration configuration = builder.Build();
var connectionString = configuration.GetConnectionString("DefaultConnection");
var apiUrl = configuration["EndPoinAndBank:UrlApi"];
var passw = configuration["EndPoinAndBank:Password"];
#endregion

Console.WriteLine($"Iniciando a importação da API{apiUrl}");

#region Polly
var retryPolicy = Policy
           .Handle<HttpRequestException>()
           .Or<TaskCanceledException>()
           .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                              (exception, timeSpan, retryCount, context) =>
                              {
                                  Console.WriteLine($"Tentativa {retryCount} falhou: {exception.Message}. Tentando novamente em {timeSpan}.");
                              });

#endregion

#region Chamada da Api Externa
using (HttpClient client = new HttpClient())
{
    client.DefaultRequestHeaders.Add("X-test-Key", passw);

    await retryPolicy.ExecuteAsync(async () =>
    {
        using (HttpResponseMessage response = await client.GetAsync(apiUrl, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();

            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                await ProcessStreamAsync(stream);
            }
        }
    });
}
#endregion

async Task ProcessStreamAsync(Stream stream)
{
    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
    {
        using (JsonTextReader jsonReader = new JsonTextReader(reader))
        {
            JsonSerializer serializer = new JsonSerializer();
            List<PositionModel> batch = new List<PositionModel>();

            var lote = 1;
            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    PositionModel data = serializer.Deserialize<PositionModel>(jsonReader);
                    batch.Add(data);
                    if (batch.Count >= 50000)
                    {
                        Console.WriteLine($"\nLote: {lote}   ");
                        await ProcessBatch(batch);
                        batch.Clear();
                        lote++;
                    }
                }
            }

            if (batch.Count > 0) // caso sobre dados no lote de importação
            {
                await ProcessBatch(batch);
            }
            Console.WriteLine("\nDownload completo.");
        }
    }
    Console.WriteLine("\nDownload completo.");
}


async Task ProcessBatch(List<PositionModel> batch)
{
    var serviceProvider = new ServiceCollection()
          .AddDbContext<PositionDbContext>(options =>
              options.UseNpgsql(connectionString))
          .BuildServiceProvider();

    using (var context = serviceProvider.GetRequiredService<PositionDbContext>())
    {
        //varifica a estrutura do banco
        context.Database.EnsureCreated();

        PositionRepository positionRepository = new PositionRepository(context);
        await positionRepository.InsertAsync(batch);
    }
}
