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
    /// Interaction logic for EditClient.xaml
    /// </summary>
    public partial class EditClient : Page
    {
        private readonly UserService userService;
        private readonly User client;
        private readonly Action onClientChanged;
        public EditClient(User clientToEdit, Action onClientChangedCallback)
        {
            InitializeComponent();
            userService = new UserService();
            client = clientToEdit;
            onClientChanged = onClientChangedCallback;

            // Load existing values
            FirstNameTextBox.Text = client.FirstName;
            LastNameTextBox.Text = client.LastName;
            PhoneTextBox.Text = client.Phone;
            EmailTextBox.Text = client.Email;
            PasswordBox.Password = client.Password;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            client.FirstName = FirstNameTextBox.Text;
            client.LastName = LastNameTextBox.Text;
            client.Phone = PhoneTextBox.Text;
            client.Email = EmailTextBox.Text;
            client.Password = PasswordBox.Password;

            if (userService.UpdateClient(client))
            {
                MessageBox.Show("Client updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                onClientChanged?.Invoke();
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Failed to update client.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
