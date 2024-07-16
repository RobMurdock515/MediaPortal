using MediaPortal.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaPortal.MVVM.View.Controls
{
    /// <summary>
    /// Interaction logic for PlayPauseView.xaml
    /// </summary>
    public partial class PlayPauseView : UserControl
    {
        public PlayPauseView()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the DataContext of the PlayControl
            if (DataContext is MainViewModel mainViewModel)
            {
                // Access the MediaPlayerViewModel from MainViewModel
                if (mainViewModel.CurrentView is MediaPlayerViewModel mediaPlayerViewModel)
                {
                    // Toggle playback
                    mediaPlayerViewModel.TogglePlayback();

                    // Update button icon based on playback state
                    var playPauseView = FindVisualChild<PlayPauseView>(Application.Current.MainWindow);
                    if (playPauseView != null)
                    {
                        playPauseView.UpdateButtonIcon(mediaPlayerViewModel.IsPaused);
                    }

                    // Access MediaPlayerView's MediaElement and control playback
                    var mediaPlayerView = FindVisualChild<MediaPlayerView>(Application.Current.MainWindow);
                    if (mediaPlayerView != null)
                    {
                        if (mediaPlayerViewModel.IsPaused)
                            mediaPlayerView.mediaElement.Pause();
                        else
                        {
                            // If media ended and user hits play again, restart from the beginning
                            if (mediaPlayerView.mediaElement.Position == mediaPlayerView.mediaElement.NaturalDuration)
                            {
                                mediaPlayerView.mediaElement.Position = TimeSpan.Zero;
                            }
                            mediaPlayerView.mediaElement.Play();
                        }
                    }
                }
            }
        }

        // Update button icon based on playback state
        public void UpdateButtonIcon(bool isPaused)
        {
            if (isPaused)
            {
                // Change button icon to play
                PlayButton.Content = FindResource("PlayIcon");
            }
            else
            {
                // Change button icon to pause
                PlayButton.Content = FindResource("PauseIcon");
            }
        }

        // Helper method to find a child of a specific type in the visual tree
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
