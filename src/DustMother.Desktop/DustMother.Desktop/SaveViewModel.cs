using DustMother.Core;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DustMother.App
{
    public abstract class SaveViewModel<T> : BindableBase where T : WingmanSave
    {
        internal DelegateCommand LoadSave;
        internal DelegateCommand SaveCurrent;

        public SaveViewModel()
        {
            _reader = new DirectSaveReader();
            LoadSave = new DelegateCommand(async () =>
            {
                await Load();
            });
            SaveCurrent = new DelegateCommand(async () =>
            {
                await WriteSave();
            });
        }

        protected SaveViewModel(DependencyObject self) : base(self)
        {
            _reader = new ProtectedSaveReader();
        }

        public virtual async Task Load()
        {
            await CheckFileAccessAsync();
            if (FileReadable == true)
            {
                await Refresh();
            }
            Loading = false;
        }

        protected readonly ISaveReader _reader;
        private T _saveData;
        private bool saveLoaded;
        private bool? fileReadable;
        private bool _pendingChanges;
        private bool _loading;
        public bool Loading { get => _loading; set => SetProperty(ref _loading, value); }
        public bool? FileReadable { get => fileReadable; set => SetProperty(ref fileReadable, value == null ? false : value); }
        public bool SaveLoaded { get => saveLoaded; set => SetProperty(ref saveLoaded, value); }
        public bool PendingChanges { get => _pendingChanges; set => SetProperty(ref _pendingChanges, value); }

        public T SaveData
        {
            get => _saveData;
            private set
            {
                SetProperty(ref _saveData, value);
                SaveLoaded = true;
            }
        }

        protected async Task RefreshSaveAsync(Func<ISaveReader, Task<T>> readFunc, Func<T, bool> checkFunc = null)
        {
            checkFunc ??= (r => r != null);
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

        public virtual async Task WriteSave()
        {
            var result = await _reader.WriteSaveAsync(SaveData);
            PendingChanges = !result;
        }
    }
}
