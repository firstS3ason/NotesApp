using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.App.ModelViews;
using Notes.Db;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Notes.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static bool isDesignMode = true;
        internal static string currentDirectory => isDesignMode ? Path.GetDirectoryName(GetSourceCodePath()) : Environment.CurrentDirectory;
        private static IHost? _host;
        internal static IHost host => _host ??= Program
            .CreateHostBuilder(Environment.GetCommandLineArgs())
            .Build();
        protected override async void OnStartup(StartupEventArgs e)
        {
            isDesignMode = false;
            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            _host = null;
        }
        internal static void ConfigureServices(HostBuilderContext context, IServiceCollection collection) => collection
            .AddSingleton<MainWindowViewModel>()
            .AddDbContext<NotesDbContext>(options => options.UseSqlServer("Data Source=MainPC;Database=NoteApp.db;Integrated Security=True;TrustServerCertificate=True;"));
        private static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}
