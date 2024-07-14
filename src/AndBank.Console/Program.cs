using AndBank.Data.Context;
using AndBank.Data.Repository;
using AndBank.Processs.Aplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;




while (true)
{
    Console.Clear();
    Console.WriteLine("Menu:");
    Console.WriteLine("1. Importar informações da API AndBank");
    Console.WriteLine("2. Sair");
    Console.Write("Escolha uma opção: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await ImportDataFromApi();
            break;
        case "2":
            return;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

async Task ImportDataFromApi()
{
    Console.Clear();
    Console.WriteLine("Importando informações da API...");
    var passw = "a2mpznLX6F8rD";

    string url = "https://api.andbank.com.br/candidate/positions"; // Substitua pela URL da sua API

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
            Console.ReadKey();
        }
    }
}



async Task ProcessBatch(List<PositionModel> batch)
{
    var serviceProvider = new ServiceCollection()
          .AddDbContext<PositionDbContext>(options =>
              options.UseNpgsql("Host=localhost;Database=positionsdb;Username=postgres;Password=1q2w3e4r@"))
          .BuildServiceProvider();

    using (var context = serviceProvider.GetRequiredService<PositionDbContext>())
    {
        PositionRepository positionRepository = new PositionRepository(context);
        await positionRepository.InsertAsync(batch);
        // await positionRepository.UnitOfWork.Commit();
    }
}



