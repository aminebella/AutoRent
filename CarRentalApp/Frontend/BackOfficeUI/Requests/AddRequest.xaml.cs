using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CarRentalApp.Frontend.BackOfficeUI.Requests
{
    /// <summary>
    /// Interaction logic for AddRequest.xaml
    /// </summary>
    public partial class AddRequest : Page
    {
        private readonly RequestService requestService;
        private readonly UserService userService;
        private readonly CarService carService;
        private readonly Action onRequestChanged;

        public AddRequest(Action onRequestChangedCallback)
        {
            InitializeComponent();
            requestService = new RequestService();
            userService = new UserService();
            carService = new CarService();
            onRequestChanged = onRequestChangedCallback;
            LoadData();
        }

        private void LoadData()
        {
            // Load clients
            var clients = userService.GetAllClients();
            ClientComboBox.ItemsSource = clients.Select(c => new { Id = c.Id, DisplayName = $"{c.FirstName} {c.LastName} ({c.Email})" }).ToList();

            // Load cars
            var cars = carService.GetAllCars();
            CarComboBox.ItemsSource = cars.Select(c => new { Id = c.Id, DisplayName = $"{c.Brand} {c.Model} ({c.Year}) - {c.Status}" }).ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedValue == null || CarComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a client and a car.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!DateTime.TryParse(StartDateTextBox.Text, out DateTime startDate) ||
                !DateTime.TryParse(EndDateTextBox.Text, out DateTime endDate))
            {
                MessageBox.Show("Please enter valid dates (yyyy-MM-dd).", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int clientId = (int)ClientComboBox.SelectedValue;
            int carId = (int)CarComboBox.SelectedValue;

            var request = new Request
            {
                ClientId = clientId,
                CarId = carId,
                StartDate = startDate,
                EndDate = endDate,
                Message = string.IsNullOrWhiteSpace(MessageTextBox.Text) ? null : MessageTextBox.Text,
                Status = "PENDING"
            };

            if (requestService.AddRequest(request))
            {
                MessageBox.Show("Request added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                onRequestChanged?.Invoke();
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Failed to add request. Check required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}


