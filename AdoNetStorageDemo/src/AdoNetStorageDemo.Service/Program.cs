﻿using AdoNetStorageDemo.Actors;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.Service
{
    class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        const string connStr = "server=.\\SQLEXPRESS;database=OrleansDemo;user id=user;password=password";
        private static async Task<int> RunMainAsync()
        {
            try
            {
                sqlConnectionTest();

                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static void sqlConnectionTest()
        {
            using (var connection = new SqlConnection(connStr))
            {
                connection.Open();
                Console.WriteLine("SQL Connection successful.");


                var query = "select 1";
                Console.WriteLine("Executing: {0}", query);
                var command = new SqlCommand(query, connection);
                command.ExecuteScalar();
                Console.WriteLine("SQL Query execution successful.");
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {



            // define the cluster configuration
            var builder = new SiloHostBuilder()
                    .UseLocalhostClustering()
                    .AddAdoNetGrainStorage("OrleansStorage", options =>
                    {
                        options.Invariant = "System.Data.SqlClient";
                        options.ConnectionString = connStr;
                        options.UseJsonFormat = true;
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "dev";
                        options.ServiceId = "dev";
                    })
                    .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                    .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(CustomerGrain).Assembly).WithReferences())
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                    });

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
