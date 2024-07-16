using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MediaPortal.MVVM.Core
{
    /// <summary>
    /// A base class that implements the INotifyPropertyChanged interface for data binding.
    /// </summary>
    class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that notifies clients that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify subscribers of a property change.
        /// </summary>
        /// <param name="name">The name of the property that changed. Automatically provided by CallerMemberName attribute.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
