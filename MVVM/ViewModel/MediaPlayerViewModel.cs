using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MediaPortal.MVVM.ViewModel
{
    public class MediaPlayerViewModel : INotifyPropertyChanged
    {
        private string _filePath; // Stores the path to the media file
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _volume; // Stores the current volume level
        public double Volume
        {
            get { return _volume; }
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isPaused; // Indicates if media playback is paused
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PlayPauseButtonContent)); // Update button content when paused state changes
                }
            }
        }

        private bool _isRepeating; // Indicates if media should repeat playback
        public bool IsRepeating
        {
            get { return _isRepeating; }
            set
            {
                if (_isRepeating != value)
                {
                    _isRepeating = value;
                    OnPropertyChanged(nameof(IsRepeating));
                }
            }
        }

        private double _currentPosition; // Current position in the media playback
        public double CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                if (_currentPosition != value)
                {
                    _currentPosition = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _mediaDuration; // Total duration of the media file
        public double MediaDuration
        {
            get { return _mediaDuration; }
            set
            {
                if (_mediaDuration != value)
                {
                    _mediaDuration = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _formattedCurrentTime; // Formatted current playback time
        public string FormattedCurrentTime
        {
            get { return _formattedCurrentTime; }
            set
            {
                if (_formattedCurrentTime != value)
                {
                    _formattedCurrentTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _formattedRemainingTime; // Formatted remaining playback time
        public string FormattedRemainingTime
        {
            get { return _formattedRemainingTime; }
            set
            {
                if (_formattedRemainingTime != value)
                {
                    _formattedRemainingTime = value;
                    OnPropertyChanged();
                }
            }
        }

        // Provides the content for the play/pause button based on the current state
        public BitmapImage PlayPauseButtonContent
        {
            get
            {
                return _isPaused ? new BitmapImage(new Uri("pack://application:,,,/MVVM/Images/MediaControl/play.png"))
                                 : new BitmapImage(new Uri("pack://application:,,,/MVVM/Images/MediaControl/pause.png"));
            }
        }

        private DispatcherTimer _timer; // Timer to update playback time

        public MediaPlayerViewModel(string filePath)
        {
            FilePath = filePath; // Initialize with the path of the media file
            Volume = 0.5; // Default volume level (0.0 to 1.0)
            IsPaused = false; // Assume media should play automatically
            IsRepeating = false; // Default is not repeating
            CurrentPosition = 0; // Default start position
            MediaDuration = 0; // Default duration

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Update every second
            _timer.Tick += Timer_Tick; // Event handler for timer tick
            _timer.Start(); // Start the timer
        }

        // Event handler for timer tick (updates formatted times if not paused)
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsPaused)
            {
                UpdateFormattedTimes(); // Update the formatted times
            }
        }

        // Updates formatted current and remaining playback times
        public void UpdateFormattedTimes()
        {
            TimeSpan currentTimeSpan = TimeSpan.FromSeconds(CurrentPosition); // Current position as TimeSpan
            TimeSpan remainingTimeSpan = TimeSpan.FromSeconds(MediaDuration - CurrentPosition); // Remaining time as TimeSpan

            FormattedCurrentTime = currentTimeSpan.ToString(@"hh\:mm\:ss"); // Format current time
            FormattedRemainingTime = remainingTimeSpan.ToString(@"hh\:mm\:ss"); // Format remaining time
        }

        // Toggles between play and pause states
        public void TogglePlayback()
        {
            IsPaused = !IsPaused; // Toggle paused state
        }

        // Toggles repeat playback mode
        public void ToggleRepeat()
        {
            IsRepeating = !IsRepeating; // Toggle repeating state
        }

        // INotifyPropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // Helper method to raise property change events
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
