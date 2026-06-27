using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace wxpmplayer.Models
{
    public class Song
    {
        public string Title { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string Artist { get; set; } = "";

        public TimeSpan Duration { get; set; }
        public BitmapImage AlbumImage { get; set; }
    }
}
