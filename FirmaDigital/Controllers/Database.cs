using FirmaDigital.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigital.Controllers
{
    public class Database
    {
        readonly SQLiteAsyncConnection _db;
        public Database(String path)
        {
            _db = new SQLiteAsyncConnection(path);
            _db.CreateTableAsync<DigitalSignature>().Wait();
        }

        public async Task<List<DigitalSignature>> GetDigitalSignaturesAsync()
        {
            try
            {
                return await _db.Table<DigitalSignature>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Manejar o registrar la excepción según tus necesidades
                Console.WriteLine($"Error al obtener firmas digitales: {ex.Message}");
                return null;
            }
        }

        public async Task<int> SaveDigitalSignature(DigitalSignature digitalSignature)
        {
            try
            {
                return digitalSignature.ID != 0 ? await _db.UpdateAsync(digitalSignature) : await _db.InsertAsync(digitalSignature);
            }
            catch (Exception ex)
            {
                // Manejar o registrar la excepción según tus necesidades
                Console.WriteLine($"Error al guardar la firma digital: {ex.Message}");
                return -1; // Otra indicación de error según tus necesidades
            }
        }

    }
}
