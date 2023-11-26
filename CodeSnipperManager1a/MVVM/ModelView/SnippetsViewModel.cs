using CodeSnipperManager1a.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSnipperManager1a.MVVM.ModelView
{
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
