using DoIt.Models;
using DoIt.Services;

namespace DoIt
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "DoItDatabase.db3");
            Database = new DatabaseService(dbPath);

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromArgb("#121212"),
                BarTextColor = Colors.White
            };
        }
    }
}
