using DoIt.Views;
using Microsoft.Maui.Controls;
using DoIt.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using DoIt.ViewModels;

namespace DoIt
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }
}
