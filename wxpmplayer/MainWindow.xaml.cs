using wxpmplayer.Models;
using wxpmplayer.Services;
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
using System.Windows.Threading;

namespace wxpmplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private MediaPlayer _player;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            _player = new MediaPlayer();

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        private bool _isPaused = false;
        private int _currentIndex = -1;

        private List<Song> _songs = new List<Song>();
        private List<Song> _currentPlaylist = new List<Song>();

        private bool isDragging = false;


        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (isDragging)
            {
                return;
            }
            double currentPosition = _player.Position.TotalSeconds;
            ProgressSlider.Value = currentPosition;
        }
      
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

            timer.Start();
            ProgressSlider.Maximum = song.Duration.TotalSeconds;
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
                timer.Start();
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
            timer.Stop();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();
            timer.Stop();
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

        private void ProgressSlider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;

        }

        private void ProgressSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            _player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);

        }
    }
}