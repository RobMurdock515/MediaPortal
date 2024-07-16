using NAudio.Gui;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MediaPortal.MVVM.View.Controls
{
    /// <summary>
    /// UserControl for managing volume control UI.
    /// </summary>
    public partial class VolumeView : UserControl
    {
        public VolumeView()
        {
            InitializeComponent();
        }

        /// Dependency property for controlling the volume level.
        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(double), typeof(VolumeView), new PropertyMetadata(0.5));

        /// Gets or sets the current volume level.
        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        /// Handles the click event for the volume button to toggle visibility of the volume slider.
        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the visibility of the volume slider
            VolumeSlider.Visibility = VolumeSlider.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
