using System.Collections.ObjectModel;
using System.ComponentModel;

using CodeSnipperManager1a.MVVM.Model;

namespace CodeSnipperManager1a.MVVM.ViewModel
{
    class UsersViewModel : INotifyPropertyChanged
    {
		private ObservableCollection<User> _items;

		public ObservableCollection<User> Items
		{
			get { return _items; }
			set 
			{ 
				_items = value;
				OnPropertyChanged(nameof(Items));
			}
		}

        public UsersViewModel()
        {
            _items = new ObservableCollection<User>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
