using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using wxpmplayer.Models;

namespace wxpmplayer.Services
{
    public class MetadataService
    {
        public MetadataService() { }
    
        public static Song ReadMetadata(string file)
        {
            var _file = TagLib.File.Create(file);
            string titleWithExt = _file.Tag.Title;
            string title = Path.GetFileNameWithoutExtension(file);
            string artist = _file.Tag.FirstPerformer ?? "Artist not found.";
            TimeSpan duration = _file.Properties.Duration;
            var picture = _file.Tag.Pictures.Length > 0 ? _file.Tag.Pictures[0] : null;

            if (picture != null)
            {
                byte[] imageData = picture.Data.Data;
                using var stream = new MemoryStream(imageData);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();


                return new Song {
                    Title = string.IsNullOrWhiteSpace(titleWithExt) ? title : titleWithExt,
                    FilePath = file,
                    Artist = artist,
                    AlbumImage = bitmap,
                    Duration = duration
                };

            }
            else
            {
                return new Song
                {
                    Title = string.IsNullOrWhiteSpace(titleWithExt) ? title : titleWithExt,
                    FilePath = file,
                    Artist = artist,
                    Duration = duration
                };
            }
        }
    }
}
