using MediaPortal.MVVM.Core;
using System.Windows.Input;

namespace MediaPortal.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        // Commands for window control
        public RelayCommand MinimizeCommand { get; private set; }
        public RelayCommand MaximizeCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }

        // Commands for view navigation
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MusicViewCommand { get; set; }
        public RelayCommand VideoViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }

        // View models for different views
        public HomeViewModel HomeVM { get; set; }
        public MusicViewModel MusicVM { get; set; }
        public VideoViewModel VideoVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        // Property to track the current view
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
                UpdateActiveViewStates();
            }
        }

        // Property to track the current view type
        private ViewType _currentViewType;
        public ViewType CurrentViewType
        {
            get { return _currentViewType; }
            set
            {
                _currentViewType = value;
                OnPropertyChanged(nameof(CurrentViewType));
            }
        }

        // Constructor
        public MainViewModel()
        {
            // Initialize view models
            HomeVM = new HomeViewModel();
            MusicVM = new MusicViewModel();
            VideoVM = new VideoViewModel();
            SettingsVM = new SettingsViewModel(MusicVM, VideoVM);

            // Set Home view as default
            CurrentView = HomeVM;
            CurrentViewType = ViewType.Home; // Set initial view type

            // Initialize commands for window control
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            ExitCommand = new RelayCommand(Exit);

            // Initialize commands for view navigation
            HomeViewCommand = new RelayCommand(o => { CurrentView = HomeVM; CurrentViewType = ViewType.Home; });
            MusicViewCommand = new RelayCommand(o => { CurrentView = MusicVM; CurrentViewType = ViewType.Music; });
            VideoViewCommand = new RelayCommand(o => { CurrentView = VideoVM; CurrentViewType = ViewType.Video; });
            SettingsViewCommand = new RelayCommand(o => { CurrentView = SettingsVM; CurrentViewType = ViewType.Settings; });
        }

        // Methods for window control commands
        private void Minimize(object obj) => App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        private void Maximize(object obj) => App.Current.MainWindow.WindowState = App.Current.MainWindow.WindowState == System.Windows.WindowState.Maximized ? System.Windows.WindowState.Normal : System.Windows.WindowState.Maximized;
        private void Exit(object obj) => App.Current.Shutdown();

        // Method to update active view states based on CurrentView
        private void UpdateActiveViewStates()
        {
            IsHomeViewActive = CurrentView == HomeVM;
            IsMusicViewActive = CurrentView == MusicVM;
            IsVideoViewActive = CurrentView == VideoVM;
        }

        // Property to track if Home view is active
        private bool _isHomeViewActive = true; // Assuming Home view is initially active
        public bool IsHomeViewActive
        {
            get { return _isHomeViewActive; }
            set
            {
                if (_isHomeViewActive != value)
                {
                    _isHomeViewActive = value;
                    OnPropertyChanged(nameof(IsHomeViewActive));
                }
            }
        }

        // Property to track if Music view is active
        private bool _isMusicViewActive;
        public bool IsMusicViewActive
        {
            get { return _isMusicViewActive; }
            set
            {
                if (_isMusicViewActive != value)
                {
                    _isMusicViewActive = value;
                    OnPropertyChanged(nameof(IsMusicViewActive));
                }
            }
        }

        // Property to track if Video view is active
        private bool _isVideoViewActive;
        public bool IsVideoViewActive
        {
            get { return _isVideoViewActive; }
            set
            {
                if (_isVideoViewActive != value)
                {
                    _isVideoViewActive = value;
                    OnPropertyChanged(nameof(IsVideoViewActive));
                }
            }
        }

        // Enum to represent different view types
        public enum ViewType
        {
            Home,
            Music,
            Video,
            Settings
        }
    }
}
