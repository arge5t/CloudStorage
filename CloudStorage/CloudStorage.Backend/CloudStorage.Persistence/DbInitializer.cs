namespace CloudStorage.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext fileContext)
        {
            fileContext.Database.EnsureDeleted();
            fileContext.Database.EnsureCreated();
        }
    }
}
