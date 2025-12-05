using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarRentalApp.Frontend.BackOfficeUI.Clients
{
    /// <summary>
    /// Interaction logic for IndexClient.xaml
    /// </summary>
    public partial class IndexClient : Page
    {
        private readonly UserService userService;
        public IndexClient()
        {
            InitializeComponent();
            userService = new UserService();
            LoadClients();
        }

        private void LoadClients()
        {
            var clients = userService.GetAllClients();
            ClientsDataGrid.ItemsSource = clients;
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddClient(OnClientChanged));
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is User selected)
            {
                NavigationService.Navigate(new EditClient(selected, OnClientChanged));
            }
            else
            {
                MessageBox.Show("Please select a client to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is User selected)
            {
                if (MessageBox.Show($"Are you sure to delete {selected.FirstName}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    userService.DeleteClient(selected.Id);
                    LoadClients();
                }
            }
            else
            {
                MessageBox.Show("Please select a client to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnClientChanged()
        {
            LoadClients();
        }
    }
}
