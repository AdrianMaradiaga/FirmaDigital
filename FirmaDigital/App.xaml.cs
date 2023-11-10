using FirmaDigital.Controllers;

namespace FirmaDigital
{
    public partial class App : Application
    {
#pragma warning disable CS0649 // Field 'App._database' is never assigned to, and will always have its default value null
        static Database _database;
#pragma warning restore CS0649 // Field 'App._database' is never assigned to, and will always have its default value null
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public static Database Database
        {
            get
            {
                if( _database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Firmas.db3");
                }
                return _database;
            }
        }
    }
}