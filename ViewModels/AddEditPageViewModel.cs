using DoIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DoIt.ViewModels
{
    public class AddEditPageViewModel
    {
        public TodoItem TodoItem { get; set; } = new TodoItem();
        public ICommand SaveCommand { get; }

        public AddEditPageViewModel(TodoItem todoItem)
        {
            TodoItem = todoItem ?? new TodoItem(); 
            SaveCommand = new Command(OnSave);
        }

        private async void OnSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TodoItem.Title))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Field can't be empty.", "OK");
                    return;
                }

                await App.Database.SaveItemAsync(TodoItem);

                await Application.Current.MainPage.Navigation.PopAsync();
                MessagingCenter.Send(this, "ReloadItems"); 
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Unable to save item: {ex.Message}", "OK");
            }
        }
    }
}
