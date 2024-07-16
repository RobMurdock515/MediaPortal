using MediaPortal.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MediaPortal.MVVM.View.Controls
{
    /// <summary>
    /// Interaction logic for NextView.xaml
    /// </summary>
    public partial class NextView : UserControl
    {
        public NextView()
        {
            InitializeComponent();
        }

        // Handle click event on the next button
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the MainViewModel from the application's main window DataContext
            var mainViewModel = App.Current.MainWindow.DataContext as MainViewModel;
            if (mainViewModel != null)
            {
                // Switch based on the current view type to execute the appropriate command
                switch (mainViewModel.CurrentViewType)
                {
                    case MainViewModel.ViewType.Music:
                        mainViewModel.MusicVM.PlayNextMusicCommand.Execute(null); // Execute command to play next music
                        break;
                    case MainViewModel.ViewType.Video:
                        mainViewModel.VideoVM.PlayNextVideoCommand.Execute(null); // Execute command to play next video
                        break;
                }
            }
        }
    }
}
