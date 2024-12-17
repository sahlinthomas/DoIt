using DoIt.Models;
using DoIt.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DoIt.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<GroupedTodoItems> GroupedTodoItems { get; set; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCompletedCommand { get; }
        //public ICommand ItemSelectedCommand { get; }

        public MainPageViewModel()
        {
            GroupedTodoItems = new ObservableCollection<GroupedTodoItems>();

            AddCommand = new Command(async () => await OnAddClicked());
            EditCommand = new Command<TodoItem>(OnEditItem);
            DeleteCommand = new Command<TodoItem>(async (item) => await OnDeleteItem(item));
            ClearCompletedCommand = new Command(async () => await ClearCompletedItems());
            LoadItemsAsync();
            MessagingCenter.Subscribe<AddEditPageViewModel>(this, "ReloadItems", async (sender) =>
            {
                await LoadItemsAsync();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task LoadItemsAsync()
        {
            try
            {
                var items = await App.Database.GetItemsAsync();

                var incompleteGroup = new GroupedTodoItems { Name = "Incomplete" };
                var completedGroup = new GroupedTodoItems { Name = "Completed" };

                foreach (var item in items)
                {
                    if (item.IsCompleted)
                        completedGroup.Add(item);
                    else
                        incompleteGroup.Add(item);

                    item.PropertyChanged += TodoItem_PropertyChanged;
                }

                GroupedTodoItems.Clear();
                GroupedTodoItems.Add(incompleteGroup);
                GroupedTodoItems.Add(completedGroup);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load items: {ex.Message}");
            }
        }

        private async void TodoItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TodoItem.IsCompleted))
            {
                var item = sender as TodoItem;
                if (item != null)
                {
                    await App.Database.SaveItemAsync(item);
                    await LoadItemsAsync();
                }
            }
        }

        private async Task OnAddClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddEditPage());
        }

        private async Task OnDeleteItem(TodoItem item)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", $"Do you want to delete '{item.Title}'?", "Yes", "No");
            if (confirm)
            {
                await App.Database.DeleteItemAsync(item);
                await LoadItemsAsync();
            }
        }
        private async void OnEditItem(TodoItem item)
        {
            if (item != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddEditPage(item));
            }
        }
        private async Task ClearCompletedItems()
        {
            var completedGroup = GroupedTodoItems.FirstOrDefault(g => g.Name == "Completed");

            if (completedGroup != null)
            {
                var itemsToRemove = completedGroup.ToList();

                foreach (var item in itemsToRemove)
                {
                    await App.Database.DeleteItemAsync(item);
                    completedGroup.Remove(item);
                }
            }
        }

    }
}
