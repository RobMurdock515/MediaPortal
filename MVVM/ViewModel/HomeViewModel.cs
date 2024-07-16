using MediaPortal.MVVM.Core;
using Microsoft.Win32;
using System.Windows;

namespace MediaPortal.MVVM.ViewModel
{
    internal class HomeViewModel
    {
        public RelayCommand OpenFilesCommand { get; private set; }

        public HomeViewModel()
        {
            // Command to open files
            OpenFilesCommand = new RelayCommand(OpenFiles);
        }

        private void OpenFiles(object parameter)
        {
            // Open file dialog to select media files
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files (*.mp3;*.mp4;*.flac;*.wav;*.wma;*.aac;*.avi;*.mkv;*.mov;*.wmv)|*.mp3;*.mp4;*.flac;*.wav;*.wma;*.aac;*.avi;*.mkv;*.mov;*.wmv|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Check if the selected file extension is supported
                string extension = System.IO.Path.GetExtension(openFileDialog.FileName);
                if (extension == ".mp3" || extension == ".mp4" || extension == ".flac" || extension == ".wav" || extension == ".wma" ||
                    extension == ".aac" || extension == ".avi" || extension == ".mkv" || extension == ".mov" || extension == ".wmv")
                {
                    // Replace current view with MediaPlayerView and pass file path
                    var mainViewModel = App.Current.MainWindow.DataContext as MainViewModel;
                    if (mainViewModel != null)
                    {
                        mainViewModel.CurrentView = new MediaPlayerViewModel(openFileDialog.FileName);
                    }
                }
                else
                {
                    // Show error message for unsupported file types
                    MessageBox.Show("Unsupported file type.");
                }
            }
        }
    }
}
