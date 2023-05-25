
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Notes.App
{
    class Program
    {
        [STAThread()]
        public static void Main(string[] args)
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .UseContentRoot(App.currentDirectory)
            .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            configurationBuilder
            .SetBasePath(App.currentDirectory))
            .ConfigureServices(App.ConfigureServices);
    }
}
