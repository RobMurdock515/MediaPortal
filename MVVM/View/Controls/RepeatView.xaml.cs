using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MediaPortal.MVVM.ViewModel;
using System.ComponentModel;

namespace MediaPortal.MVVM.View.Controls
{
    public partial class RepeatView : UserControl
    {
        public RepeatView()
        {
            InitializeComponent();

            // Subscribe to DataContextChanged event to track ViewModel changes
            DataContextChanged += RepeatView_DataContextChanged;
        }

        // Handle DataContext changes to bind to MainViewModel
        private void RepeatView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MainViewModel oldViewModel)
            {
                oldViewModel.PropertyChanged -= MainViewModel_PropertyChanged;
            }

            if (e.NewValue is MainViewModel newViewModel)
            {
                newViewModel.PropertyChanged += MainViewModel_PropertyChanged;
            }
        }

        // Handle property changes in MainViewModel to update UI
        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.CurrentView))
            {
                UpdateRepeatButtonBackground(); // Update repeat button background based on current view
            }
        }

        // Handle click event on the repeat button
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel && mainViewModel.CurrentView is MediaPlayerViewModel mediaPlayerViewModel)
            {
                mediaPlayerViewModel.ToggleRepeat(); // Toggle repeat mode in MediaPlayerViewModel
                UpdateRepeatButtonBackground(); // Update UI after toggling repeat mode
            }
        }

        // Update the background of the repeat button based on MediaPlayerViewModel state
        private void UpdateRepeatButtonBackground()
        {
            if (DataContext is MainViewModel mainViewModel && mainViewModel.CurrentView is MediaPlayerViewModel mediaPlayerViewModel)
            {
                if (mediaPlayerViewModel.IsRepeating)
                {
                    // Set background to a specific color when repeating
                    RepeatButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7976"));
                }
                else
                {
                    // Set background to transparent when not repeating
                    RepeatButton.Background = Brushes.Transparent;
                }
            }
            else
            {
                // Set background to transparent or default color if no valid ViewModel context
                RepeatButton.Background = Brushes.Transparent;
            }
        }
    }
}
