using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CharacterSheetGenerator
{
    public abstract class NotifyBase : INotifyPropertyChanged
    {

        private readonly Dictionary<string, object> propertyValues;

        protected NotifyBase()
        {
            propertyValues = new Dictionary<string, object>();
        }

        protected void Set<T>(T value, [CallerMemberName]string propertyName = null)
        {
            if (propertyValues.ContainsKey(propertyName))
            {
                propertyValues[propertyName] = value;
            }
            else
            {
                propertyValues.Add(propertyName, value);
            }
            OnPropertyChanged(propertyName);
        }

        protected T Get<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyValues.ContainsKey(propertyName))
            {
                return (T)propertyValues[propertyName];
            }
            return default(T);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}