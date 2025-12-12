using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CarRentalApp.Frontend.BackOfficeUI.Payments
{
    /// <summary>
    /// Interaction logic for AddPayment.xaml
    /// </summary>
    public partial class AddPayment : Page
    {
        private readonly PaymentService paymentService;
        private readonly ReservationService reservationService;
        private readonly UserService userService;
        private readonly CarService carService;
        private readonly Action onPaymentChanged;

        public AddPayment(Action onPaymentChangedCallback)
        {
            InitializeComponent();
            paymentService = new PaymentService();
            reservationService = new ReservationService();
            userService = new UserService();
            carService = new CarService();
            onPaymentChanged = onPaymentChangedCallback;
            LoadData();
        }

        private void LoadData()
        {
            // Load active reservations with client and car info
            var reservations = reservationService.GetAllReservations();
            var users = userService.GetAllClients().ToDictionary(u => u.Id);
            var cars = carService.GetAllCars().ToDictionary(c => c.Id);

            ReservationComboBox.ItemsSource = reservations.Select(r =>
            {
                var user = users.ContainsKey(r.UserId) ? users[r.UserId] : null;
                var car = cars.ContainsKey(r.CarId) ? cars[r.CarId] : null;
                string userName = user != null ? $"{user.FirstName} {user.LastName}" : $"User {r.UserId}";
                string carName = car != null ? $"{car.Brand} {car.Model}" : $"Car {r.CarId}";
                return new
                {
                    Id = r.Id,
                    DisplayName = $"Reservation #{r.Id} - {userName} - {carName} ({r.StartDate:yyyy-MM-dd} to {r.EndDate:yyyy-MM-dd}) - {r.Status}"
                };
            }).ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a reservation.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
            {
                MessageBox.Show("Please enter a valid amount.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int reservationId = (int)ReservationComboBox.SelectedValue;

            DateTime paymentDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(PaymentDateTextBox.Text) &&
                DateTime.TryParse(PaymentDateTextBox.Text, out DateTime parsedDate))
            {
                paymentDate = parsedDate;
            }

            var payment = new Payment
            {
                ReservationId = reservationId,
                Amount = amount,
                PaymentDate = paymentDate,
                PaymentMethod = PaymentMethodComboBox.SelectedItem is ComboBoxItem selectedMethod 
                    ? selectedMethod.Content.ToString() 
                    : "CASH",
                Status = StatusComboBox.SelectedItem is ComboBoxItem selectedStatus 
                    ? selectedStatus.Content.ToString() 
                    : "PENDING"
            };

            if (paymentService.AddPayment(payment))
            {
                MessageBox.Show("Payment added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                onPaymentChanged?.Invoke();
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Failed to add payment. Check the reservation and amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}


