using Accessibility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows.Media;
using wxpmplayer.Models;

namespace wxpmplayer.Services
{
    public class AudioPlayer
    {

        private readonly MediaPlayer _player;
        public bool isPaused = false;
        public bool isMediaEnded = false;

        public AudioPlayer()
        {
            _player = new MediaPlayer();
            _player.MediaEnded += _player_MediaEnded;
        }

        private void _player_MediaEnded(object? sender, EventArgs e)
        {
            isMediaEnded = true;
        }


        public void Play(Song song)
        {
            if (isPaused)
            {
                isPaused = false;
                _player.Play();
                return;
            }

            _player.Open(new Uri(song.FilePath));
            _player.Play();
        }

        public void Resume()
        {
            _player.Play();
        }

        public void Pause()
        {
            isPaused = true;
            _player.Pause();
        }

        public void Stop()
        {
            _player.Stop();
        }

        public double GetPlaybackPositionInSeconds()
        {
            return _player.Position.TotalSeconds;
        }

        public TimeSpan GetPlaybackCurrentPosition()
        {
            return TimeSpan.FromSeconds(GetPlaybackPositionInSeconds());
        }

        public void SetPlaybackPosition(double value)
        {
            _player.Position = TimeSpan.FromSeconds(value);
        }

        
    }
}
