using MediaPortal.MVVM.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using TagLib;

namespace MediaPortal.MVVM.ViewModel
{
    internal class MusicViewModel : ObservableObject
    {
        private ObservableCollection<MusicFile> _musicFiles; // Collection of music files
        public ObservableCollection<MusicFile> MusicFiles
        {
            get { return _musicFiles; }
            set
            {
                _musicFiles = value;
                OnPropertyChanged();
            }
        }

        private MusicFile _currentMusicFile; // Currently selected music file
        public MusicFile CurrentMusicFile
        {
            get { return _currentMusicFile; }
            set
            {
                _currentMusicFile = value;
                OnPropertyChanged();
            }
        }

        // Commands for playing music, next music, and previous music
        public ICommand PlayMusicCommand { get; }
        public ICommand PlayNextMusicCommand { get; }
        public ICommand PlayPreviousMusicCommand { get; }

        public MusicViewModel()
        {
            _musicFiles = new ObservableCollection<MusicFile>(); // Initialize music files collection
            // Initialize commands
            PlayMusicCommand = new RelayCommand(ExecutePlayMusic);
            PlayNextMusicCommand = new RelayCommand(ExecutePlayNextMusic);
            PlayPreviousMusicCommand = new RelayCommand(ExecutePlayPreviousMusic);
        }

        // Executes when PlayMusicCommand is invoked
        private void ExecutePlayMusic(object parameter)
        {
            if (parameter is MusicFile musicFile)
            {
                var mainViewModel = App.Current.MainWindow.DataContext as MainViewModel;
                if (mainViewModel != null)
                {
                    CurrentMusicFile = musicFile; // Set current music file
                    mainViewModel.CurrentView = new MediaPlayerViewModel(musicFile.FilePath); // Switch to MediaPlayerView
                }
            }
        }

        // Executes when PlayNextMusicCommand is invoked
        private void ExecutePlayNextMusic(object parameter)
        {
            if (CurrentMusicFile != null && MusicFiles != null && MusicFiles.Any())
            {
                int currentIndex = MusicFiles.IndexOf(CurrentMusicFile);
                int nextIndex = (currentIndex + 1) % MusicFiles.Count;
                CurrentMusicFile = MusicFiles[nextIndex];
                ExecutePlayMusic(CurrentMusicFile); // Play the next music
            }
        }

        // Executes when PlayPreviousMusicCommand is invoked
        private void ExecutePlayPreviousMusic(object parameter)
        {
            if (CurrentMusicFile != null && MusicFiles != null && MusicFiles.Any())
            {
                int currentIndex = MusicFiles.IndexOf(CurrentMusicFile);
                int previousIndex = (currentIndex - 1 + MusicFiles.Count) % MusicFiles.Count;
                CurrentMusicFile = MusicFiles[previousIndex];
                ExecutePlayMusic(CurrentMusicFile); // Play the previous music
            }
        }

        // Loads music files from a specified folder path
        public void LoadMusicFiles(string folderPath)
        {
            MusicFiles.Clear(); // Clear existing music files

            // Filter and enumerate music files in the specified folder and its subdirectories
            var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => new[] { ".mp3", ".wav", ".flac", ".wma", ".aac" }.Contains(Path.GetExtension(file).ToLower()));

            foreach (var file in files)
            {
                var musicFile = new MusicFile();

                try
                {
                    var tagFile = TagLib.File.Create(file); // Read tags from the music file
                    musicFile.Title = tagFile.Tag.Title;
                    musicFile.Artist = tagFile.Tag.FirstPerformer;
                    musicFile.Album = tagFile.Tag.Album;
                    musicFile.Year = tagFile.Tag.Year.ToString();
                    musicFile.Duration = tagFile.Properties.Duration.ToString(@"mm\:ss");
                }
                catch (Exception ex)
                {
                    // Handle exceptions when reading tags
                    // For example, log the error or set default values for properties
                }

                musicFile.FilePath = file; // Set the file path
                musicFile.DisplayText = $"{musicFile.Title} - {musicFile.Artist} - {musicFile.Album} ({musicFile.Year}) - {musicFile.Duration}";
                MusicFiles.Add(musicFile); // Add music file to collection
            }
        }
    }

    // Represents a music file with metadata properties
    public class MusicFile
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Duration { get; set; }
        public string FilePath { get; set; } // File path of the music file
        public string DisplayText { get; set; } // Display text for UI
    }
}
