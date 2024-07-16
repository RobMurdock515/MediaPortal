using MediaPortal.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace MediaPortal.MVVM.View
{
    public partial class VideoView : UserControl
    {
        public VideoView()
        {
            InitializeComponent(); // Initialize the user control
        }

        // Event handler for double-click on ListBoxItem
        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Check if sender is ListBoxItem and its DataContext is VideoViewModel.VideoFile
            if (sender is ListBoxItem listBoxItem && listBoxItem.DataContext is VideoViewModel.VideoFile videoFile)
            {
                // Get the view model associated with this view
                var viewModel = DataContext as VideoViewModel;
                // Execute the PlayVideoCommand on the view model with the selected VideoFile
                viewModel?.PlayVideoCommand.Execute(videoFile);
            }
        }
    }
}
