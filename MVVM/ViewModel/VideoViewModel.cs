using MediaPortal.MVVM.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MediaPortal.MVVM.ViewModel
{
    internal class VideoViewModel : ObservableObject
    {
        // Represents a video file with title and file path
        public class VideoFile
        {
            public string Title { get; set; }
            public string FilePath { get; set; }
        }

        private ObservableCollection<VideoFile> _videoFiles; // Collection of video files
        public ObservableCollection<VideoFile> VideoFiles
        {
            get { return _videoFiles; }
            set
            {
                _videoFiles = value;
                OnPropertyChanged();
            }
        }

        private VideoFile _currentVideoFile; // Currently selected video file
        public VideoFile CurrentVideoFile
        {
            get { return _currentVideoFile; }
            set
            {
                _currentVideoFile = value;
                OnPropertyChanged();
            }
        }

        // Commands for playing video, next video, and previous video
        public ICommand PlayVideoCommand { get; }
        public ICommand PlayNextVideoCommand { get; }
        public ICommand PlayPreviousVideoCommand { get; }

        public VideoViewModel()
        {
            _videoFiles = new ObservableCollection<VideoFile>(); // Initialize video files collection
            // Initialize commands
            PlayVideoCommand = new RelayCommand(ExecutePlayVideo);
            PlayNextVideoCommand = new RelayCommand(ExecutePlayNextVideo);
            PlayPreviousVideoCommand = new RelayCommand(ExecutePlayPreviousVideo);
        }

        // Executes when PlayVideoCommand is invoked
        private void ExecutePlayVideo(object parameter)
        {
            if (parameter is VideoFile videoFile)
            {
                // Replace current view with MediaPlayerView and pass file path
                var mainViewModel = App.Current.MainWindow.DataContext as MainViewModel;
                if (mainViewModel != null)
                {
                    CurrentVideoFile = videoFile; // Set current video file
                    mainViewModel.CurrentView = new MediaPlayerViewModel(videoFile.FilePath); // Switch to MediaPlayerView
                }
            }
        }

        // Executes when PlayNextVideoCommand is invoked
        private void ExecutePlayNextVideo(object parameter)
        {
            if (CurrentVideoFile != null && VideoFiles != null && VideoFiles.Any())
            {
                int currentIndex = VideoFiles.IndexOf(CurrentVideoFile);
                int nextIndex = (currentIndex + 1) % VideoFiles.Count;
                var nextVideoFile = VideoFiles[nextIndex];
                ExecutePlayVideo(nextVideoFile); // Play the next video
            }
        }

        // Executes when PlayPreviousVideoCommand is invoked
        private void ExecutePlayPreviousVideo(object parameter)
        {
            if (CurrentVideoFile != null && VideoFiles != null && VideoFiles.Any())
            {
                int currentIndex = VideoFiles.IndexOf(CurrentVideoFile);
                int previousIndex = (currentIndex - 1 + VideoFiles.Count) % VideoFiles.Count;
                var previousVideoFile = VideoFiles[previousIndex];
                ExecutePlayVideo(previousVideoFile); // Play the previous video
            }
        }

        // Loads video files from a specified folder path
        public async void LoadVideoFiles(string folderPath)
        {
            VideoFiles.Clear(); // Clear existing video files

            // Filter and enumerate video files in the specified folder and its subdirectories
            var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => new[] { ".mp4", ".mov", ".avi", ".wmv", ".mkv" }.Contains(Path.GetExtension(file).ToLower()));

            // Add each video file to the VideoFiles collection
            foreach (var file in files)
            {
                var videoFile = new VideoFile
                {
                    Title = Path.GetFileNameWithoutExtension(file),
                    FilePath = file
                };

                VideoFiles.Add(videoFile); // Add video file to collection
            }
        }
    }
}
