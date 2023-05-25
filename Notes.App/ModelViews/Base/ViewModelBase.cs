using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notes.App.ModelViews.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        protected virtual bool Set<T>(ref T outerValue, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(outerValue, value)) 
                return false;

            outerValue = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
