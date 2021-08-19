using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.System;
using Windows.UI.Core;

namespace DustMother.App
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class BindableBase : INotifyBase, INotifyPropertyChanged
    {
        public BindableBase()
        {

        }

        protected BindableBase(DependencyObject self) : this()
        {
            if (self.Dispatcher != null)
            {
                Dispatcher = self.Dispatcher;
            }
        }
        protected Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        //protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null, bool setNull = false)
        //{
        //    if (!setNull && value == null)
        //    {
        //        return false;
        //    }
        //    if (object.Equals(storage, value)) return false;

        //    storage = value;
        //    this.OnPropertyChanged(propertyName);
        //    return true;
        //}

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                if (Dispatcher != null)
                {
                    Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)));
                }
                else
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            } catch (Exception ex) when (ex.Source == "Windows" && ex.Message.Contains("RPC_E_WRONG_THREAD"))
            {
                //ignored
            }
            
        }

        public bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }

    

}
