using System;
using MongoDB.Driver;
using dotenv.net;
using Polly;

namespace StoreOps.Database
{
    public class DatabaseConnection
    {
        public IMongoDatabase Database { get; }

        public DatabaseConnection()
        {
            try
            {
                Console.WriteLine("Tentando se conectar ao banco de dados..");

                var options = new DotEnvOptions(envFilePaths: new[] { "./.env" });

                DotEnv.Load(options);

                var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
                var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");

                if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(databaseName))
                {
                    throw new Exception("A string de conexão ou o nome do banco de dados não foram fornecidos.");
                }

                var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                
    
                settings.MaxConnectionPoolSize = 100;
                settings.MinConnectionPoolSize = 10;

                var client = CreateMongoClientWithRetryPolicy(settings);
                
                Database = client.GetDatabase(databaseName);

                Console.WriteLine($"Conectado com sucesso ao banco de dados '{databaseName}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao se conectar ao banco de dados: {ex.Message}");
                Environment.Exit(-1);
            }
        }

        private static MongoClient CreateMongoClientWithRetryPolicy(MongoClientSettings settings)
        {
            var policy = Policy.Handle<TimeoutException>()
                               .WaitAndRetry(new[]
                               {
                                   TimeSpan.FromSeconds(1),
                                   TimeSpan.FromSeconds(2),
                                   TimeSpan.FromSeconds(3)
                               });

            return policy.Execute(() => new MongoClient(settings));
        }
    }
}
