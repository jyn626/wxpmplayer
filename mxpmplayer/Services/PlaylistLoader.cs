using mxpmplayer.Models;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using TagLib; 

namespace mxpmplayer.Services
{
    public class PlaylistLoader
    {
        public static List<Song> LoadSongs(string folder)
        {
           
            string[] files = Directory.GetFiles(folder, "*.mp3");
            List<Song> songs = new();

            foreach (string file in files)
            {
                var _file = TagLib.File.Create(file);
                string titleMetadata = _file.Tag.Title;
                string artist = _file.Tag.FirstPerformer ?? "Artist not found.";

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

                    string title = Path.GetFileNameWithoutExtension(file);
                    songs.Add(new Song { Title = string.IsNullOrWhiteSpace(titleMetadata) ? title : titleMetadata, FilePath = file, Artist = artist, AlbumImage = bitmap });

                } else
                {
                    string title = Path.GetFileNameWithoutExtension(file);
                    songs.Add(new Song { Title = string.IsNullOrWhiteSpace(titleMetadata) ? title : titleMetadata, FilePath = file, Artist = artist, AlbumImage = null });
                }
            }


            return songs;
        }
    }

}
