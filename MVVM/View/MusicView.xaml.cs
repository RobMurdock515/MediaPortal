using MediaPortal.MVVM.ViewModel;
using System.Windows.Controls;

namespace MediaPortal.MVVM.View
{
    public partial class MusicView : UserControl
    {
        public MusicView()
        {
            // Initialize the user control
            InitializeComponent(); 
        }

        // Event handler for double-click on ListBoxItem
        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Check if sender is ListBoxItem and its DataContext is MusicFile
            if (sender is ListBoxItem listBoxItem && listBoxItem.DataContext is MusicFile musicFile)
            {
                // Get the view model associated with this view
                var viewModel = DataContext as MusicViewModel;
                // Execute the PlayMusicCommand on the view model with the selected MusicFile
                viewModel?.PlayMusicCommand.Execute(musicFile);
            }
        }
    }
}
