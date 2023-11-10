using FirmaDigital.Controllers;
using System;
using System.IO;


namespace FirmaDigital
{
    public partial class App : Application
    {
        static Database _database;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        public static Database Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Firmas.db3");
                    _database = new Database(dbPath);
                }
                return _database;
            }
        }
    }
}
