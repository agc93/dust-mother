using DustMother.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DustMother.App
{
    [Obsolete("Breaks VM separation, move to SaveViewModel asap", false)]
    public abstract class WidgetSaveViewModel<T> where T : WingmanSave
    {
        protected WidgetSaveViewModel(INotifyBase notify)
        {
            _notify = notify;
            _reader = new ProtectedSaveReader();
            //FileReadable = false;
            fileReadable = false;
            //_reader.CheckFileAccessAsync().ContinueWith(t => FileReadable = t.Result);
            
        }

        private readonly INotifyBase _notify;

        public async Task CheckFileAccessAsync()
        {
            FileReadable = await _reader.CheckFileAccessAsync();
        }

        protected readonly ProtectedSaveReader _reader;
        private T _saveData;
        private bool saveLoaded;
        private bool? fileReadable;

        private bool _loading;
        public bool Loading { get => _loading; set => _notify.SetProperty(ref _loading, value); }

        public bool? FileReadable { get => fileReadable; set => _notify.SetProperty(ref fileReadable, value == null ? false : value); }
        public bool SaveLoaded { get => saveLoaded; set => _notify.SetProperty(ref saveLoaded, value); }
        public T SaveData
        {
            get => _saveData;
            private set
            {
                _notify.SetProperty(ref _saveData, value);
                SaveLoaded = true;
            }
        }

        protected async Task RefreshSaveAsync(Func<ProtectedSaveReader, Task<T>> readFunc, Func<T, bool> checkFunc = null)
        {
            checkFunc ??= (r => r != null);
            var result = await readFunc(_reader);
            SaveData = result;
            OnSaveLoad?.Invoke(this, result);
            SaveLoaded = checkFunc(result);
            //FileReadable = SaveLoaded;
        }

        public abstract Task Refresh();

        protected event SaveLoadHandler OnSaveLoad;

        public delegate void SaveLoadHandler(object sender, T data);
    }
    
    public abstract class SaveViewModel<T> : DispatcherBase where T : WingmanSave
    {
        public SaveViewModel()
        {
            _reader = new ProtectedSaveReader();
        }
        [Obsolete("Initial loading is now skipped by default", false)]
        protected SaveViewModel(bool skipInit)
        {
            _reader = new ProtectedSaveReader();
        }

        protected SaveViewModel(DependencyObject self) : base(self)
        {
            _reader = new ProtectedSaveReader();
        }

        public virtual async Task Load()
        {
            //fileReadable = false;
            await CheckFileAccessAsync();
            if (FileReadable == true)
            {
                await Refresh();
            }
            Loading = false;
        }

        protected readonly ProtectedSaveReader _reader;
        private T _saveData;
        private bool saveLoaded;
        private bool? fileReadable;

        private bool _loading;
        public bool Loading { get => _loading; set => SetProperty(ref _loading, value); }

        public bool? FileReadable { get => fileReadable; set => SetProperty(ref fileReadable, value == null ? false : value); }
        public bool SaveLoaded { get => saveLoaded; set => SetProperty(ref saveLoaded, value); }
        public T SaveData
        {
            get => _saveData;
            private set
            {
                SetProperty(ref _saveData, value);
                SaveLoaded = true;
            }
        }

        protected async Task RefreshSaveAsync(Func<ProtectedSaveReader, Task<T>> readFunc, Func<T, bool> checkFunc = null)
        {
            checkFunc = checkFunc ?? (r => r != null);
            var result = await readFunc(_reader);
            SaveData = result;
            OnSaveLoad?.Invoke(this, result);
            SaveLoaded = checkFunc(result);
            //FileReadable = SaveLoaded;
        }

        public async Task CheckFileAccessAsync()
        {
            FileReadable = await _reader.CheckFileAccessAsync();
        }

        public abstract Task Refresh();

        protected event SaveLoadHandler OnSaveLoad;

        public delegate void SaveLoadHandler(object sender, T data);
    }
}
