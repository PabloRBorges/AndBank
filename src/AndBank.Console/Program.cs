using AndBank.Data.Context;
using AndBank.Data.Repository;
using AndBank.Processs.Aplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;

string environment;

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

Console.WriteLine($"Iniciando a importação da API{apiUrl}");
await ImportDataFromApi();

async Task ImportDataFromApi()
{
    Console.Clear();
    Console.WriteLine("Importando informações da API...");
    string url = apiUrl ?? "";

    using (HttpClient client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("x-Test-Key", passw);

        using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        {
            var totalTime = new Stopwatch();
            totalTime.Start();

            response.EnsureSuccessStatusCode();

            long? totalBytes = response.Content.Headers.ContentLength;

            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                using (StreamReader reader = new StreamReader(stream))
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
            }

            totalTime.Stop();
            long elapsedMilliseconds = totalTime.ElapsedMilliseconds;
            double elapsedSeconds = elapsedMilliseconds / 1000.0;
            Console.WriteLine("\nImportação concluída. Tempo total: {0:F2} segundos", elapsedSeconds);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
            Console.ReadLine();
        }
    }
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
