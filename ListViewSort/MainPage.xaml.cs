namespace ListViewSort
{
    using System;
    using System.Linq;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public static class MyExtension
    {
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector, bool descending = false)
        {
            var sortedSource = descending ? source.OrderByDescending(keySelector).ToList() : source.OrderBy(keySelector).ToList();

            for (var i = 0; i < sortedSource.Count; i++)
            {
                var itemToSort = sortedSource[i];

                // If the item is already at the right position, leave it and continue.
                var index = source.IndexOf(itemToSort);
                if (index == i)
                {
                    continue;
                }

                source.Move(index, i);
            }
        }
    }

    public class ListViewItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return;
            }
            field = value;
            OnPropertyChanged(propertyName);
        }


        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }
        private int _count;


        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }
        private bool _isSelected;
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return;
            }
            field = value;
            OnPropertyChanged(propertyName);
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<ListViewItemViewModel> ListViewItemViewModels = new ObservableCollection<ListViewItemViewModel>();

        public ListViewItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!_isSorting)
                {
                    Set(ref _selectedItem, value);
                }
            }
        }

        private ListViewItemViewModel _selectedItem;

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            ListViewItemViewModels.Clear();
            for (var i = 0; i < 10; i++)
            {
                ListViewItemViewModels.Add(new ListViewItemViewModel() { Count = rand.Next(100) });
            }
        }

        private bool _isSorting;

        private async void SortButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tmp = SelectedItem;
            //_isSorting = true;
            ListViewItemViewModels.Sort(x => x.Count);
            _isSorting = false;
            //SelectedItem = tmp;
            //OnPropertyChanged(nameof(SelectedItem));

            tmp.IsSelected = true;
            await Task.Delay(1000);

            tmp.IsSelected = true;
        }
    }
}
