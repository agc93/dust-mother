using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace DustMother.App
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class DispatcherBase : INotifyBase
    {
        protected DependencyObject _self;
        protected bool SetNullValues { get; set; } = true;

        protected DispatcherBase()
        {

        }

        protected DispatcherBase(DependencyObject self)
        {
            _self = self;
        }

        public void SetContext(DependencyObject obj)
        {
            _self = obj;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task OnPropertyChangedAsync([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            //changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
            try
            {
                if (_self != null)
                {
                    await _self.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    });
                }
                else
                {
                    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    });
                }
            }
            catch (Exception ex)
            {
                //ignored
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingStore"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <param name="onChanged"></param>
        /// <returns></returns>
        public bool SetProperty<T>(ref T backingStore,
            T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChangedAsync(propertyName);
            //OnPropertyChangedThreaded(propertyName);

            return true;
        }
    }

    

}
