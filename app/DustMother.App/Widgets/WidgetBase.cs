using DustMother.Core;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DustMother.App.Widgets
{
    [Obsolete("Duplicates behaviour of WidgetPage, but poorly", false)]
    public abstract class WidgetBasePage<TSave> : Page, INotifyBase where TSave : WingmanSave
    {
        protected WidgetBasePage()
        {
            _reader = new ProtectedSaveReader();
            //FileReadable = false;
            fileReadable = false;
        }

        protected WidgetBasePage(WidgetHandle self) : this()
        {
            _self = self;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is WidgetHandle)
            {
                _self = e.Parameter as WidgetHandle;
                _instance = new XboxGameBarWidgetControl(_self.Widget);
            }
            await this.CheckFileAccessAsync();
            if (FileReadable == true)
            {
                await this.Refresh();
            }
            
        }

        protected readonly ProtectedSaveReader _reader;
        protected WidgetHandle _self;
        private XboxGameBarWidgetControl _instance;
        private TSave _saveData;
        private bool saveLoaded;
        private bool? fileReadable;

        private bool _loading;
        public bool IsLoading { get => _loading; set => SetProperty(ref _loading, value); }

        public bool? FileReadable { get => fileReadable; set => SetProperty(ref fileReadable, value == null ? false : value); }

        public bool SaveLoaded { get => saveLoaded; set => SetProperty(ref saveLoaded, value); }
        public TSave SaveData
        {
            get => _saveData;
            private set
            {
                SetProperty(ref _saveData, value);
                SaveLoaded = true;
            }
        }

        protected async Task RefreshSaveAsync(Func<ProtectedSaveReader, Task<TSave>> readFunc, Func<TSave, bool> checkFunc = null)
        {
            checkFunc = checkFunc ?? (r => r != null);
            var result = await readFunc(_reader);
            SaveData = result;
            OnSaveLoad?.Invoke(this, result);
            SaveLoaded = checkFunc(result);
        }

        public async Task CheckFileAccessAsync()
        {
            FileReadable = await _reader.CheckFileAccessAsync();
        }

        public abstract Task Refresh();

        protected event SaveLoadHandler OnSaveLoad;

        public delegate void SaveLoadHandler(object sender, TSave data);

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
                    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    });
                } else {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
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

        private async Task OnPropertyChangedThreaded([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null) return;

            try
            {
                var primaryTask = Task.Run(async () =>
                {
                    await Dispatcher.RunTaskAsync(async () =>
                    {
                        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    });
                });

                var continueTask = primaryTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        // If the task was faulted, you will need to look for an inner exception, and if it exists, re-throw that exception
                        // This will ensure that the right exception will come back to you when ExecuteAsync returns
                        if (t.Exception.InnerException != null)
                        {
                            throw t.Exception.InnerException;
                        }
                    }

                    return true;
                }).AsAsyncOperation();

                await continueTask;
            } catch (Exception ex)
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
