using System;
using System.ComponentModel;

namespace DustMother.App
{
    public interface INotifyBase : INotifyPropertyChanged
    {
        bool SetProperty<T>(ref T storage, T value, string propertyName = null, Action onChanged = null);
    }

    

}
