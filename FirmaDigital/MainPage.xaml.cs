
using FirmaDigital.Models;
using FirmaDigital.Controllers;
using Syncfusion.Maui.Core.Internals;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace FirmaDigital
{
    public partial class MainPage : ContentPage
    {
        private Database _database;


        public MainPage()
        {
            InitializeComponent();
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Firma.db3"));

        }

        private async void OnSignatureDrawCompleted(object sender, EventArgs e)
        {
            var imageStream = await signaturePad.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Png);
            byte[] signatureBytes;
            
            using (MemoryStream ms = new MemoryStream())
            {
                await imageStream.CopyToAsync(ms);  
                signatureBytes = ms.ToArray();  
            }

            var previewImage = new Image { Source = ImageSource.FromStream(() => new MemoryStream(signatureBytes))};
            stackLayout.Children.Add(previewImage);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var imageStream = await signaturePad.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Png);
            byte[] signatureBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                await imageStream.CopyToAsync(ms);
                signatureBytes = ms.ToArray();
            }

            DigitalSignature digitalSignature = new DigitalSignature
            {
                Name = nameEntry.Text,
                Description = descriptionEntry.Text,
                Imagen = signatureBytes
            };

            int result = await _database.SaveDigitalSignature(digitalSignature);

            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Error", "La aplicación necesita permisos para guardar la firma", "OK");
                    return;
                }
            }
            string nombre = nameEntry.Text;

            string imagePath = GuardarFirmaComoImagen(signatureBytes, nombre);

            if (result > 0 && !string.IsNullOrEmpty(imagePath))
            {
                await DisplayAlert("Exito", "Firma guardada correctamente", "OK");

                await SaveImageToGallery("MiAlbum", nombre, signatureBytes);

                nameEntry.Text = string.Empty;
                descriptionEntry.Text = string.Empty;
                signaturePad.Clear();


            }
            else
            {
                await DisplayAlert("Error", "No se ha podido guardar la firma", "OK");
            }
        }

        private async Task SaveImageToGallery(string album, string filename, byte[] imageBytes)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    if (status != PermissionStatus.Granted)
                    {
                        return;
                    }
                }

                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);

                File.WriteAllBytes(filePath, imageBytes);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al guardar la imagen en la galería", "OK");
                Console.WriteLine($"Error al guardar la imagen en la galería: {ex.Message}");
            }
        }


        private string GuardarFirmaComoImagen(byte[] firmaData, string nombre)
        {
            try
            {
                // Crear un nombre único para la imagen
                string imageName = $"{nombre}_{DateTime.Now:yyyyMMddHHmmss}.png";

                // Obtener la ruta del directorio de imágenes del sistema
                string imageDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                // Combinar la ruta del directorio con el nombre de la imagen
                string imagePath = System.IO.Path.Combine(imageDirectory, imageName);

                // Guardar el byte array como archivo de imagen
                System.IO.File.WriteAllBytes(imagePath, firmaData);

                return imagePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la firma como imagen: {ex.Message}");
                return null;
            }
        }

    }
}
