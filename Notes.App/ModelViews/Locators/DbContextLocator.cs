using Microsoft.Extensions.DependencyInjection;
using Notes.Db;

namespace Notes.App.ModelViews.Locators
{
    internal class DbContextLocator
    {
        public static NotesDbContext notesDbContext => App
            .host
            .Services
            .GetRequiredService<NotesDbContext>();
    }
}
