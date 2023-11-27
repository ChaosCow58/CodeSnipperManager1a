using CodeSnipperManager1a.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CodeSnipperManager1a.MVVM.ModelView
{
    #pragma warning disable CS8612, CS8618
    public class SnippetsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Snippet> _items;

        public ObservableCollection<Snippet> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public SnippetsViewModel()
        {
            // Initialize the Items collection
            _items = new ObservableCollection<Snippet>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
