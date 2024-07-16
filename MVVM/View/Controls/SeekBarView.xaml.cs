using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MediaPortal.MVVM.ViewModel;

namespace MediaPortal.MVVM.View.Controls
{
    public partial class SeekBarView : UserControl
    {
        private DispatcherTimer _timer; // Timer for updating seek bar and remaining time
        private bool _isDragging; // Flag to track if the thumb is being dragged

        public SeekBarView()
        {
            InitializeComponent();

            // Initialize timer for updating UI
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Event handlers for seek bar interaction
            seekBar.AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(SeekBar_DragStarted));
            seekBar.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(SeekBar_DragCompleted));
            seekBar.ValueChanged += SeekBar_ValueChanged;
            seekBar.PreviewMouseLeftButtonDown += SeekBar_PreviewMouseLeftButtonDown;

            // Listen for DataContext changes to bind to view model
            DataContextChanged += SeekBarView_DataContextChanged;
        }

        // Handle DataContext changes to bind to MainViewModel
        private void SeekBarView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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

        // Handle property changes in MainViewModel
        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Update timer based on CurrentView property changes
            if (e.PropertyName == nameof(MainViewModel.CurrentView))
            {
                _timer.IsEnabled = (DataContext as MainViewModel)?.CurrentView is MediaPlayerViewModel;
            }
        }

        // Handle seek bar value changes
        private void SeekBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update media position if dragging and current view is MediaPlayerViewModel
            if (_isDragging && DataContext is MainViewModel mainViewModel && mainViewModel.CurrentView is MediaPlayerViewModel mediaPlayerViewModel)
            {
                MediaPlayerView mediaPlayerView = FindVisualChild<MediaPlayerView>(Application.Current.MainWindow);
                mediaPlayerView?.SetMediaPosition(seekBar.Value);
            }
        }

        // Handle seek bar drag started
        private void SeekBar_DragStarted(object sender, DragStartedEventArgs e)
        {
            _isDragging = true;
            _timer.Stop(); // Pause timer while dragging
        }

        // Handle seek bar drag completed
        private void SeekBar_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _isDragging = false;
            _timer.Start(); // Resume timer after dragging

            // Update media position and play if MediaPlayerViewModel
            if (DataContext is MainViewModel mainViewModel && mainViewModel.CurrentView is MediaPlayerViewModel mediaPlayerViewModel)
            {
                MediaPlayerView mediaPlayerView = FindVisualChild<MediaPlayerView>(Application.Current.MainWindow);
                mediaPlayerView?.SetMediaPosition(seekBar.Value);
                mediaPlayerView?.PlayMedia();
            }
        }

        // Handle mouse click on seek bar to change position
        private void SeekBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel && mainViewModel.CurrentView is MediaPlayerViewModel mediaPlayerViewModel)
            {
                Point position = e.GetPosition(seekBar);
                double percentage = position.X / seekBar.ActualWidth;
                double newPosition = percentage * seekBar.Maximum;

                seekBar.Value = newPosition;
                mediaPlayerViewModel.CurrentPosition = newPosition;

                MediaPlayerView mediaPlayerView = FindVisualChild<MediaPlayerView>(Application.Current.MainWindow);
                mediaPlayerView?.SetMediaPosition(newPosition);
                mediaPlayerView?.PlayMedia();
            }
        }

        // Update UI based on timer tick
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_isDragging && DataContext is MainViewModel mainViewModel && mainViewModel.CurrentView is MediaPlayerViewModel mediaPlayerViewModel && !mediaPlayerViewModel.IsPaused)
            {
                // Update seek bar and remaining time text
                seekBar.Maximum = mediaPlayerViewModel.MediaDuration;
                seekBar.Value = mediaPlayerViewModel.CurrentPosition;

                double remainingTime = mediaPlayerViewModel.MediaDuration - mediaPlayerViewModel.CurrentPosition;
                TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
                remainingTimeText.Text = timeSpan.ToString(remainingTime >= 3600 ? @"hh\:mm\:ss" : @"mm\:ss");
                remainingTimeText.Visibility = Visibility.Visible;
            }
            else
            {
                remainingTimeText.Visibility = Visibility.Collapsed;
            }
        }

        // Helper method to find visual child of a specific type
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
