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

namespace CarRentalApp.Frontend.BackOfficeUI.Payments
{
    /// <summary>
    /// Interaction logic for IndexPayments.xaml
    /// </summary>
    public partial class IndexPayments : Page
    {
        private readonly PaymentService paymentService;
        private List<Payment> allPayments;

        public IndexPayments()
        {
            InitializeComponent();
            paymentService = new PaymentService();
            LoadPayments();
        }

        private void LoadPayments()
        {
            allPayments = paymentService.GetAllPayments();
            PaymentsDataGrid.ItemsSource = allPayments;
        }

        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPayment(OnPaymentChanged));
        }

        private void OnPaymentChanged()
        {
            LoadPayments();
        }

        private void MarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsDataGrid.SelectedItem is Payment selected)
            {
                if (selected.Status == "COMPLETED")
                {
                    MessageBox.Show("Payment is already completed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to mark payment #{selected.Id} as completed?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool success = paymentService.UpdatePaymentStatus(selected.Id, "COMPLETED");
                    if (success)
                    {
                        MessageBox.Show("Payment marked as completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadPayments();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update payment status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a payment to mark as completed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadPayments();
        }

        private void FilterAll_Click(object sender, RoutedEventArgs e)
        {
            PaymentsDataGrid.ItemsSource = allPayments;
        }

        private void FilterPending_Click(object sender, RoutedEventArgs e)
        {
            var pending = paymentService.GetPendingPayments();
            PaymentsDataGrid.ItemsSource = pending;
        }

        private void FilterCompleted_Click(object sender, RoutedEventArgs e)
        {
            var completed = paymentService.GetCompletedPayments();
            PaymentsDataGrid.ItemsSource = completed;
        }

        private void FilterFailed_Click(object sender, RoutedEventArgs e)
        {
            var failed = paymentService.GetFailedPayments();
            PaymentsDataGrid.ItemsSource = failed;
        }

        private void MarkFailed_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsDataGrid.SelectedItem is Payment selected)
            {
                if (selected.Status == "FAILED")
                {
                    MessageBox.Show("Payment is already marked as failed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to mark payment #{selected.Id} as failed?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool success = paymentService.UpdatePaymentStatus(selected.Id, "FAILED");
                    if (success)
                    {
                        MessageBox.Show("Payment marked as failed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadPayments();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update payment status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a payment to mark as failed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsDataGrid.SelectedItem is Payment selected)
            {
                Payment payment = paymentService.GetPaymentById(selected.Id);
                if (payment != null)
                {
                    string details = $"Payment Details:\n\n" +
                                    $"ID: {payment.Id}\n" +
                                    $"Reservation ID: {payment.ReservationId}\n" +
                                    $"Amount: {payment.Amount:C}\n" +
                                    $"Payment Date: {payment.PaymentDate:yyyy-MM-dd}\n" +
                                    $"Payment Method: {payment.PaymentMethod}\n" +
                                    $"Status: {payment.Status}\n" +
                                    $"Created At: {payment.CreatedAt:yyyy-MM-dd HH:mm}";
                    MessageBox.Show(details, "Payment Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a payment to view details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FilterByReservation_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ReservationIdFilterTextBox.Text, out int reservationId))
            {
                var payments = paymentService.GetPaymentsByReservation(reservationId);
                PaymentsDataGrid.ItemsSource = payments;
                if (payments.Count == 0)
                {
                    MessageBox.Show($"No payments found for reservation ID {reservationId}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid reservation ID.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ReservationIdFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterByReservation_Click(sender, e);
            }
        }
    }
}

