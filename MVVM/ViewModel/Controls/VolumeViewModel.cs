using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaPortal.MVVM.ViewModel.Controls
{
    internal class VolumeViewModel : INotifyPropertyChanged
    {
        private double _volume; // Backing field for the volume property

        public double Volume
        {
            get { return _volume; }
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    OnPropertyChanged(); // Notify property changed when the value changes
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; // Event to notify subscribers of property changes

        // Helper method to raise the PropertyChanged event
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
