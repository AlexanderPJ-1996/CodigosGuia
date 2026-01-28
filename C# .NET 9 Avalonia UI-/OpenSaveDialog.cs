// Librerías para abrir OpenDialog y/o SaveDialog de Avalonia UI
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace DialogImg
{
	public class ImgDiags : UserControl
    {
        // OpenDialog para cargar una imagen desde archivo en control Image
		public async Task OpenImgs(Image Img, Visual Modulo)
        {
            var LevelTop = TopLevel.GetTopLevel(Modulo);
			
            var File = await LevelTop.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Seleccionar imagen",
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("Imagenes PNG") {Patterns = ["*.png"] }, 
                    new FilePickerFileType("Imagenes JPG/JPEG") {Patterns = ["*.jpg", "*.jpeg"] }
                ]
            });
			
            if (File.Count > 0)
            {
                await using var Stream = await File[0].OpenReadAsync();
                Bitmap BM = new(Stream);
                Img.Source = BM;
            }
        }
		
		// SaveDialog para guardar una imagen cargada en control Image como imagen PNG (.png)
        public async Task SaveImgs(Image Img, Visual Modulo)
        {
            if (Img.Source is Bitmap BM)
            {
                var LevelTop = TopLevel.GetTopLevel(Modulo);
				
                var File = await LevelTop.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions 
                {
                    Title = "Guardar imagen",
                    FileTypeChoices =
                    [
                        new FilePickerFileType("Imagenes PNG") {Patterns = ["*.png"] }
                    ]
                });
				
                if (File is not null)
                {
                    await using var Stream = await File.OpenWriteAsync();
                    BM.Save(Stream);
                }
            }
        }

        // Guardar imagen sin abrir cuadro de díalogo usando String con ruta de archivo
        public async Task SaveAuto(ImageFileMachine Img, String RutaImgs)
        {
            if (Img.Source is Bitmap BM)
            {
            }
        }
    }
	
	public static class Procesos
	{
		// Abrir OpenDialog
		public static async void ImgsOpen(Image Img, Window MainWindow)
		{
			ImgDiags Diag = new();
			await Diag.OpenImgs(Img, MainWindow);
		}
		
		// Abrir SaveDialog
		public static async void ImgsOpen(Image Img, Window MainWindow)
		{
			ImgDiags Diag = new();
			await Diag.SaveImgs(Img, MainWindow);
		}
	}
}