using mxpmplayer.Models;
using mxpmplayer.Services;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mxpmplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MediaPlayer _player = new();
        private bool _isPaused = false;
        private int _currentIndex = -1;
        private bool artistChange = false;
        private List<Song> artistSongs = new List<Song>();

        private List<Song> _songs = new List<Song>();
        private List<Song> _currentPlaylist = new List<Song>();

        private void PlaySong(Song song)
        {
            //if (!song)
            //{
            //    return;
            //}

            //if (index >= 0 && index <= _songs.Count)
            //{
            //    return;
            //}

            //_currentIndex = index;
            //TrackComboBox.SelectedIndex = _currentIndex;
            _player.Open(new Uri(song.FilePath));
            //_player.Open(new Uri(_currentPlaylist[_currentIndex].FilePath));
            _player.Play();
          
        }

        private void LoadPlaylist_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            
            if (dialog.ShowDialog() == true)
            {
                string folder = dialog.FolderName;
                _songs = PlaylistLoader.LoadSongs(folder);

                if (_songs.Count > 0)
                {
                    List<string> artists = _songs
                        .Select(song => song.Artist)
                        .Distinct()
                        .OrderBy(artist => artist)
                        .ToList();

                    artists.Insert(0, "All");

                    ArtistComboBox.ItemsSource = artists;
                    TrackComboBox.ItemsSource = _currentPlaylist;


                    _currentIndex = 0;

                    TrackComboBox.SelectedIndex = _currentIndex;
                    ArtistComboBox.SelectedIndex = 0;

                    _currentPlaylist = _songs;

                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {

            if (_isPaused)
            {
                _player.Play();
                _isPaused = false;
                return;
            }

            if (TrackComboBox.SelectedItem is Song selectedSong) {
                _currentIndex = TrackComboBox.SelectedIndex;
                Song song = _currentPlaylist[_currentIndex];
                PlaySong(song);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPlaylist.Count <= 0)
            {
                return;
            }

            int nextIndex = _currentIndex + 1;

            if (nextIndex >= _currentPlaylist.Count)
            {
                _currentIndex = 0;
                PlaySong(_currentPlaylist[_currentIndex]);
                return;

            }

            _currentIndex++;
            Song song = _currentPlaylist[_currentIndex];
            PlaySong(song);
            TrackComboBox.SelectedIndex = _currentIndex;
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPlaylist.Count <= 0)
            {
                return;
            }

            int nextIndex = _currentIndex - 1;

            if (nextIndex <= 0)
            {
                _currentIndex = _currentPlaylist.Count - 1;
                PlaySong(_currentPlaylist[_currentIndex]);
                return;

            }

            _currentIndex--;
            Song song = _currentPlaylist[_currentIndex];
            PlaySong(song);
            TrackComboBox.SelectedIndex = _currentIndex;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Pause();
            _isPaused = true;
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();
        }


        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TrackComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrackComboBox.SelectedItem is not Song song) return;
            
            _currentIndex = TrackComboBox.SelectedIndex;
            Song _song = _currentPlaylist[_currentIndex];

            if (_song.AlbumImage != null)
            {
                AlbumArt.Source = _song.AlbumImage;
                NoAlbumArtText.Visibility = Visibility.Collapsed;
            } else
            {
                AlbumArt.Source = null;
                NoAlbumArtText.Visibility = Visibility.Visible;
            }

            PlaySong(_song);
        }

        private void ArtistComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //artistChange = true;
            string selectedArtist = ArtistComboBox.SelectedItem as string;
            if (selectedArtist == null) return;

            if (selectedArtist == "All")
            {
                _currentPlaylist = _songs;
                _currentIndex = 0;
                TrackComboBox.ItemsSource = _currentPlaylist;
                TrackComboBox.SelectedItem = _currentPlaylist[_currentIndex];
                return;
            }

            _player.Stop();

            var filteredSongs = _songs
                .Where(song => song.Artist == selectedArtist)
                .ToList();

            _currentPlaylist = filteredSongs;
            _currentIndex = 0;
            TrackComboBox.ItemsSource = _currentPlaylist;
            TrackComboBox.SelectedItem = _currentPlaylist[_currentIndex];
        }
    }
}