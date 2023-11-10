using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace FirmaDigital.Models
{
    public class DigitalSignature
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public string ImageEncrypted { get; set; }

        [Ignore]
        public byte[] Imagen
        {
            get { return Convert.FromBase64String(ImageEncrypted); }
            set { ImageEncrypted = Convert.ToBase64String(value); }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public string NombreToString => $"Nombre: {Name}";

        public string DescripToString => $"Descripción: {Description}";
    }
}