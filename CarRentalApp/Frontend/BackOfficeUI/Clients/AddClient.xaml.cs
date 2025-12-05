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

using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Services;

namespace CarRentalApp.Frontend.BackOfficeUI.Clients
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Page
    {
        private readonly UserService userService;
        private readonly Action onClientChanged;

        public AddClient(Action onClientChangedCallback)
        {
            InitializeComponent();
            userService = new UserService();
            onClientChanged = onClientChangedCallback;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Phone = PhoneTextBox.Text,
                Email = EmailTextBox.Text,
                Password = PasswordBox.Password
            };

            if (userService.AddClient(user))
            {
                MessageBox.Show("Client added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                onClientChanged?.Invoke();
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Failed to add client. Check required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
