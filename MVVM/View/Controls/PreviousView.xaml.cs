using MediaPortal.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MediaPortal.MVVM.View.Controls
{
    /// <summary>
    /// Interaction logic for PreviousView.xaml
    /// </summary>
    public partial class PreviousView : UserControl
    {
        public PreviousView()
        {
            InitializeComponent();
        }

        // Handle click event on the previous button
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the MainViewModel from the application's main window DataContext
            var mainViewModel = App.Current.MainWindow.DataContext as MainViewModel;
            if (mainViewModel != null)
            {
                // Switch based on the current view type to execute the appropriate command
                switch (mainViewModel.CurrentViewType)
                {
                    case MainViewModel.ViewType.Music:
                        mainViewModel.MusicVM.PlayPreviousMusicCommand.Execute(null); // Execute command to play previous music
                        break;
                    case MainViewModel.ViewType.Video:
                        mainViewModel.VideoVM.PlayPreviousVideoCommand.Execute(null); // Execute command to play previous video
                        break;
                }
            }
        }
    }
}
