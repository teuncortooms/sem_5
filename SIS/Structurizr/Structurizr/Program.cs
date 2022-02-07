using System;
using Microsoft.Extensions.Configuration;
using Structurizr.Api;

namespace Structurizr
{
    class Program
    {
        static void Main()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string apiKey = config["apiKey"];
            string apiSecret = config["apiSecret"];
            if (!long.TryParse(config["workspaceId"], out var workspaceId)) Console.WriteLine("Invalid Workspace Id");
            
            new StructurizrWorkspace(apiKey, apiSecret, workspaceId).PutWorkspace();
        }
    }
}