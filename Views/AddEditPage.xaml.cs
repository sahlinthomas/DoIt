using DoIt.Models;
using DoIt.ViewModels;

namespace DoIt.Views;


public partial class AddEditPage : ContentPage
{
    private TodoItem _todoItem;

    public AddEditPage(TodoItem todoItem = null) 
    {
        InitializeComponent();
        _todoItem = todoItem ?? new TodoItem(); 
        BindingContext = new AddEditPageViewModel(_todoItem);

        
        var navigationPage = Application.Current.MainPage as NavigationPage;
        if (navigationPage != null)
        {
            navigationPage.BarBackgroundColor = Color.FromArgb("#121212"); 
            navigationPage.BarTextColor = Colors.White; 
        }
    }
}