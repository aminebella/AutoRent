using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CarRentalApp.Frontend.BackOfficeUI.Reservations
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    public partial class AddReservation : Page
    {
        private readonly ReservationService reservationService;
        private readonly UserService userService;
        private readonly CarService carService;
        private readonly Action onReservationChanged;

        public AddReservation(Action onReservationChangedCallback)
        {
            InitializeComponent();
            reservationService = new ReservationService();
            userService = new UserService();
            carService = new CarService();
            onReservationChanged = onReservationChangedCallback;
            LoadData();
        }

        private void LoadData()
        {
            // Load clients/users
            var users = userService.GetAllClients();
            UserComboBox.ItemsSource = users.Select(u => new { Id = u.Id, DisplayName = $"{u.FirstName} {u.LastName} ({u.Email})" }).ToList();

            // Load cars
            var cars = carService.GetAllCars();
            CarComboBox.ItemsSource = cars.Select(c => new { Id = c.Id, DisplayName = $"{c.Brand} {c.Model} ({c.Year}) - {c.Status}" }).ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedValue == null || CarComboBox.SelectedValue == null)
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

            int userId = (int)UserComboBox.SelectedValue;
            int carId = (int)CarComboBox.SelectedValue;

            decimal totalPrice = 0;
            bool hasCustomPrice = decimal.TryParse(TotalPriceTextBox.Text, out totalPrice);

            var reservation = new Reservation
            {
                CarId = carId,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
                TotalPrice = hasCustomPrice ? totalPrice : 0, // Will be calculated if 0
                Status = StatusComboBox.SelectedItem is ComboBoxItem selectedStatus 
                    ? selectedStatus.Content.ToString() 
                    : "ACTIVE",
                PaymentStatus = PaymentStatusComboBox.SelectedItem is ComboBoxItem selectedPayment 
                    ? selectedPayment.Content.ToString() 
                    : "UNPAID"
            };

            if (reservationService.AddReservation(reservation))
            {
                MessageBox.Show("Reservation added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                onReservationChanged?.Invoke();
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Failed to add reservation. Car may not be available during this period or check required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}


