using MediaPortal.MVVM.View.Controls;
using MediaPortal.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MediaPortal.MVVM.View
{
    public partial class MediaPlayerView : UserControl
    {
        private DispatcherTimer _timer; // Timer for updating media playback position

        public MediaPlayerView()
        {
            InitializeComponent();
            Loaded += MediaPlayerView_Loaded; // Subscribe to Loaded event
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Set interval for timer tick (1 second)
            _timer.Tick += Timer_Tick; // Subscribe to timer tick event
        }

        private void MediaPlayerView_Loaded(object sender, RoutedEventArgs e)
        {
            // When the view is loaded, set up event handlers and start playback if not paused
            if (DataContext is MediaPlayerViewModel mediaPlayerViewModel)
            {
                mediaElement.MediaOpened += MediaElement_MediaOpened; // Subscribe to MediaOpened event
                mediaElement.MediaEnded += MediaElement_MediaEnded; // Subscribe to MediaEnded event

                if (!mediaPlayerViewModel.IsPaused)
                {
                    PlayMedia(); // Start playing media if not paused
                }

                // Find PlayPauseView in visual tree and update its icon
                var playPauseView = FindVisualChild<PlayPauseView>(Application.Current.MainWindow);
                if (playPauseView != null)
                {
                    playPauseView.UpdateButtonIcon(mediaPlayerViewModel.IsPaused);
                }

                _timer.Start(); // Start the timer
            }
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            // When media is opened, update media duration and initial position in view model
            if (DataContext is MediaPlayerViewModel mediaPlayerViewModel)
            {
                mediaPlayerViewModel.MediaDuration = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                mediaPlayerViewModel.CurrentPosition = 0;
            }
        }

        public void PlayMedia()
        {
            // Play the media
            if (mediaElement != null)
            {
                mediaElement.Play();
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            // When media playback ends, handle repeat or pause based on view model state
            if (DataContext is MediaPlayerViewModel mediaPlayerViewModel)
            {
                if (mediaPlayerViewModel.IsRepeating)
                {
                    mediaElement.Position = TimeSpan.Zero; // Rewind media to start if repeating
                    PlayMedia(); // Play media again
                }
                else
                {
                    mediaPlayerViewModel.IsPaused = true; // Pause media playback

                    // Find PlayPauseView in visual tree and update its icon
                    var playPauseView = FindVisualChild<PlayPauseView>(Application.Current.MainWindow);
                    if (playPauseView != null)
                    {
                        playPauseView.UpdateButtonIcon(mediaPlayerViewModel.IsPaused);
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update current media position in view model every timer tick
            if (mediaElement != null && mediaElement.NaturalDuration.HasTimeSpan)
            {
                if (DataContext is MediaPlayerViewModel viewModel)
                {
                    viewModel.CurrentPosition = mediaElement.Position.TotalSeconds;
                }
            }
        }

        public void SetMediaPosition(double position)
        {
            // Set media playback position
            if (mediaElement != null)
            {
                mediaElement.Position = TimeSpan.FromSeconds(position);
            }
        }

        // Helper method to find a visual child of a specific type in the visual tree
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
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
