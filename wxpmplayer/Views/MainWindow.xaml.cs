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
        private DispatcherTimer _timer;
        private MediaPlayer _player;
        private AudioPlayer _audioPlayer;
        private int _currentIndex = -1;

        private List<Song> _songs = new List<Song>();
        private List<Song> _currentPlaylist = new List<Song>();

        private bool isDragging = false;
        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _player = new MediaPlayer();
            _audioPlayer = new AudioPlayer();


            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (isDragging)
            {
                return;
            }

            if (_audioPlayer.isMediaEnded)
            {
                GoToNextTrack();
                _audioPlayer.isMediaEnded = false;
                return;
            }

            double currentPosition = _audioPlayer.GetPlaybackPositionInSeconds();
            ProgressSlider.Value = currentPosition;
            CurrentPositionTextBlock.Text = _audioPlayer.GetPlaybackCurrentPosition().ToString("mm':'ss");
        }

        private void Play(Song song)
        {
            _audioPlayer.Play(song);
            DurationTextBlock.Text = song.Duration.ToString("mm':'ss");
        
            ProgressSlider.Maximum = song.Duration.TotalSeconds;
            _timer.Start();
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
                    _currentPlaylist = _songs;

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


                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrackComboBox.SelectedItem is Song selectedSong) {
                _currentIndex = TrackComboBox.SelectedIndex;
                Song song = _currentPlaylist[_currentIndex];
                Play(song);
            }
        }

        private void GoToNextTrack()
        {
            if (_currentPlaylist.Count <= 0)
            {
                return;
            }

            int nextIndex = _currentIndex + 1;

            if (nextIndex >= _currentPlaylist.Count)
            {
                _currentIndex = 0;
                Play(_currentPlaylist[_currentIndex]);
                TrackComboBox.SelectedIndex = _currentIndex;
                return;

            }

            _currentIndex++;
            Play(_currentPlaylist[_currentIndex]);
            TrackComboBox.SelectedIndex = _currentIndex;
        }

        private void GoToPrevTrack()
        {
            if (_currentPlaylist.Count <= 0)
            {
                return;
            }

            int nextIndex = _currentIndex - 1;

            if (nextIndex < 0)
            {
                _currentIndex = _currentPlaylist.Count - 1;
                TrackComboBox.SelectedIndex = _currentIndex;
                Play(_currentPlaylist[_currentIndex]);
                return;

            }

            _currentIndex--;
            TrackComboBox.SelectedIndex = _currentIndex;
            Play(_currentPlaylist[_currentIndex]);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            GoToNextTrack();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            GoToPrevTrack();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _audioPlayer.Pause();
            _timer.Stop();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _audioPlayer.Stop();
            _timer.Stop();
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

            Play(_song);
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

            _audioPlayer.Stop();

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
            _audioPlayer.SetPlaybackPosition(ProgressSlider.Value);
        }

        private void AlwaysShow_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void AlwaysShow_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }
    }
}