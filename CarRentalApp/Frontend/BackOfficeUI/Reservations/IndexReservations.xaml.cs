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

namespace CarRentalApp.Frontend.BackOfficeUI.Reservations
{
    /// <summary>
    /// Interaction logic for IndexReservations.xaml
    /// </summary>
    public partial class IndexReservations : Page
    {
        private readonly ReservationService reservationService;
        private List<Reservation> allReservations;

        public IndexReservations()
        {
            InitializeComponent();
            reservationService = new ReservationService();
            LoadReservations();
        }

        private void LoadReservations()
        {
            allReservations = reservationService.GetAllReservations();
            ReservationsDataGrid.ItemsSource = allReservations;
        }

        private void AddReservation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddReservation(OnReservationChanged));
        }

        private void OnReservationChanged()
        {
            LoadReservations();
        }

        private void FinishReservation_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsDataGrid.SelectedItem is Reservation selected)
            {
                if (selected.Status != "ACTIVE")
                {
                    MessageBox.Show("Only active reservations can be finished.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to finish reservation #{selected.Id}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool success = reservationService.FinishReservation(selected.Id);
                    if (success)
                    {
                        MessageBox.Show("Reservation finished successfully. Car is now available.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadReservations();
                    }
                    else
                    {
                        MessageBox.Show("Failed to finish reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to finish.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsDataGrid.SelectedItem is Reservation selected)
            {
                if (selected.Status != "ACTIVE")
                {
                    MessageBox.Show("Only active reservations can be cancelled.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to cancel reservation #{selected.Id}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool success = reservationService.CancelReservation(selected.Id);
                    if (success)
                    {
                        MessageBox.Show("Reservation cancelled successfully. Car is now available.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadReservations();
                    }
                    else
                    {
                        MessageBox.Show("Failed to cancel reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to cancel.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadReservations();
        }

        private void FilterAll_Click(object sender, RoutedEventArgs e)
        {
            ReservationsDataGrid.ItemsSource = allReservations;
        }

        private void FilterActive_Click(object sender, RoutedEventArgs e)
        {
            var active = reservationService.GetActiveReservations();
            ReservationsDataGrid.ItemsSource = active;
        }

        private void FilterFinished_Click(object sender, RoutedEventArgs e)
        {
            var finished = reservationService.GetFinishedReservations();
            ReservationsDataGrid.ItemsSource = finished;
        }

        private void FilterCancelled_Click(object sender, RoutedEventArgs e)
        {
            var cancelled = reservationService.GetCancelledReservations();
            ReservationsDataGrid.ItemsSource = cancelled;
        }

        private void AutoFinishExpired_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This will automatically finish all expired reservations. Continue?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                reservationService.RunReservationAutoFinish();
                MessageBox.Show("Expired reservations have been finished automatically.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadReservations();
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsDataGrid.SelectedItem is Reservation selected)
            {
                Reservation reservation = reservationService.GetReservationById(selected.Id);
                if (reservation != null)
                {
                    string details = $"Reservation Details:\n\n" +
                                    $"ID: {reservation.Id}\n" +
                                    $"Car ID: {reservation.CarId}\n" +
                                    $"User ID: {reservation.UserId}\n" +
                                    $"Start Date: {reservation.StartDate:yyyy-MM-dd}\n" +
                                    $"End Date: {(reservation.EndDate.HasValue ? reservation.EndDate.Value.ToString("yyyy-MM-dd") : "Not set")}\n" +
                                    $"Total Price: {reservation.TotalPrice:C}\n" +
                                    $"Status: {reservation.Status}\n" +
                                    $"Payment Status: {reservation.PaymentStatus}\n" +
                                    $"Created At: {reservation.CreatedAt:yyyy-MM-dd HH:mm}\n" +
                                    $"Updated At: {reservation.UpdatedAt:yyyy-MM-dd HH:mm}";
                    MessageBox.Show(details, "Reservation Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to view details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FilterByUser_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(UserIdFilterTextBox.Text, out int userId))
            {
                var reservations = reservationService.GetReservationsByUser(userId);
                ReservationsDataGrid.ItemsSource = reservations;
                if (reservations.Count == 0)
                {
                    MessageBox.Show($"No reservations found for user ID {userId}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid user ID.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FilterByCar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CarIdFilterTextBox.Text, out int carId))
            {
                var reservations = reservationService.GetReservationsByCar(carId);
                ReservationsDataGrid.ItemsSource = reservations;
                if (reservations.Count == 0)
                {
                    MessageBox.Show($"No reservations found for car ID {carId}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid car ID.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UserIdFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterByUser_Click(sender, e);
            }
        }

        private void CarIdFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterByCar_Click(sender, e);
            }
        }
    }
}

