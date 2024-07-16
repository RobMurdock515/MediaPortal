using System;
using System.IO;
using System.Text.Json;
using MediaPortal.MVVM.Core;
using MediaPortal.MVVM.Model;
using System.Windows.Forms;

namespace MediaPortal.MVVM.ViewModel
{
    internal class SettingsViewModel : ObservableObject
    {
        // Path to save user settings file
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MediaPortal",
            "userSettings.json");

        private string _musicLibraryLocation; // Stores the path to the music library
        public string MusicLibraryLocation
        {
            get { return _musicLibraryLocation; }
            set
            {
                if (_musicLibraryLocation != value)
                {
                    _musicLibraryLocation = value;
                    OnPropertyChanged(); // Notify UI of property change
                    MusicVM.LoadMusicFiles(value); // Load music files from the specified location
                    SaveSettings(); // Save settings to the user settings file
                }
            }
        }

        private string _videoLibraryLocation; // Stores the path to the video library
        public string VideoLibraryLocation
        {
            get { return _videoLibraryLocation; }
            set
            {
                if (_videoLibraryLocation != value)
                {
                    _videoLibraryLocation = value;
                    OnPropertyChanged(); // Notify UI of property change
                    VideoVM.LoadVideoFiles(value); // Load video files from the specified location
                    SaveSettings(); // Save settings to the user settings file
                }
            }
        }

        public RelayCommand SelectMusicLibraryLocationCommand { get; private set; } // Command to select music library location
        public RelayCommand SelectVideoLibraryLocationCommand { get; private set; } // Command to select video library location
        public RelayCommand RefreshCommand { get; private set; } // Command to refresh libraries

        public MusicViewModel MusicVM { get; set; } // Instance of MusicViewModel to manage music-related operations
        public VideoViewModel VideoVM { get; set; } // Instance of VideoViewModel to manage video-related operations

        public SettingsViewModel(MusicViewModel musicVM, VideoViewModel videoVM)
        {
            MusicVM = musicVM; // Initialize MusicViewModel
            VideoVM = videoVM; // Initialize VideoViewModel

            // Commands initialization
            SelectMusicLibraryLocationCommand = new RelayCommand(SelectMusicLibraryLocation);
            SelectVideoLibraryLocationCommand = new RelayCommand(SelectVideoLibraryLocation);
            RefreshCommand = new RelayCommand(RefreshLibraries);

            LoadSettings(); // Load previously saved settings
        }

        // Method to select music library location using FolderBrowserDialog
        private void SelectMusicLibraryLocation(object parameter)
        {
            string selectedFolder = SelectFolder();
            if (!string.IsNullOrEmpty(selectedFolder))
                MusicLibraryLocation = selectedFolder; // Update MusicLibraryLocation property
        }

        // Method to select video library location using FolderBrowserDialog
        private void SelectVideoLibraryLocation(object parameter)
        {
            string selectedFolder = SelectFolder();
            if (!string.IsNullOrEmpty(selectedFolder))
                VideoLibraryLocation = selectedFolder; // Update VideoLibraryLocation property
        }

        // Opens FolderBrowserDialog to select a folder and returns the selected path
        private string SelectFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }
            return null;
        }

        // Refreshes music and video libraries based on the current library locations
        private void RefreshLibraries(object parameter)
        {
            if (!string.IsNullOrEmpty(MusicLibraryLocation))
            {
                MusicVM.LoadMusicFiles(MusicLibraryLocation); // Reload music files
            }

            if (!string.IsNullOrEmpty(VideoLibraryLocation))
            {
                VideoVM.LoadVideoFiles(VideoLibraryLocation); // Reload video files
            }
        }

        // Ensures that the directory for saving settings exists
        private void EnsureSettingsDirectoryExists()
        {
            string directory = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        // Saves current MusicLibraryLocation and VideoLibraryLocation to the user settings file
        private void SaveSettings()
        {
            EnsureSettingsDirectoryExists(); // Ensure directory exists
            var settings = new UserSettings // Create UserSettings object
            {
                MusicLibraryLocation = MusicLibraryLocation, // Set music library location
                VideoLibraryLocation = VideoLibraryLocation  // Set video library location
            };
            string json = JsonSerializer.Serialize(settings); // Convert settings to JSON
            File.WriteAllText(SettingsFilePath, json); // Write JSON to file
        }

        // Loads saved settings from the user settings file
        private void LoadSettings()
        {
            if (File.Exists(SettingsFilePath)) // Check if settings file exists
            {
                string json = File.ReadAllText(SettingsFilePath); // Read JSON from file
                var settings = JsonSerializer.Deserialize<UserSettings>(json); // Deserialize JSON to UserSettings object
                MusicLibraryLocation = settings?.MusicLibraryLocation; // Set music library location from settings
                VideoLibraryLocation = settings?.VideoLibraryLocation; // Set video library location from settings
            }
        }
    }
}
