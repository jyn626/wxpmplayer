using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace mxpmplayer.Models
{
    public class Song
    {
        public string Title { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string Artist { get; set; } = "";
        public BitmapImage AlbumImage { get; set; }
    }
}
