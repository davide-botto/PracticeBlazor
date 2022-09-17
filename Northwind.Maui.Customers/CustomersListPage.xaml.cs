using Microsoft.Maui.Controls; // ContentPage, ListView
using System;
using System.Threading.Tasks;

namespace Northwind.Maui.Customers;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomersListPage : ContentPage
{
    public CustomersListPage()
    {
        InitializeComponent();

        CustomersListViewModel viewModel = new();

        viewModel.AddSampleData();
        BindingContext = viewModel;

    }

    async void Customer_Tapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is not CustomerDetailViewModel c) return;

        // ** Navigate to detail view and show tappec dustomer
        await Navigation.PushAsync(new CustomerDetailPage(
            BindingContext as CustomersListViewModel, c));
    }

    async void Customers_Refreshing(object sender, EventArgs e)
    {
        if (sender is not ListView listView) return;

        listView.IsRefreshing = true;

        // ** Simulate a refresh
        await Task.Delay(1500);

        listView.IsRefreshing = false;
    }

    void Customer_Deleted(object sender, EventArgs e)
    {
        MenuItem menuItem = sender as MenuItem;
        if (menuItem.BindingContext is not CustomerDetailViewModel c) return;
        (BindingContext as CustomersListViewModel).Remove(c);
    }

    async void Customer_Phoned(object sender, EventArgs e)
    {
        MenuItem menuItem = sender as MenuItem;
        if (menuItem.BindingContext is not CustomerDetailViewModel c) return;

        if (await DisplayAlert("Dial a Number", "Would you like to call " + c.Phone + "?", "Yes", "No"))
        {
            PhoneDialer.Open(c.Phone);
        }
    }

    async void Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CustomerDetailPage(BindingContext as CustomersListViewModel));
    }
}