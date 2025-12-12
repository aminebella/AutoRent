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

namespace CarRentalApp.Frontend.BackOfficeUI.Requests
{
    /// <summary>
    /// Interaction logic for IndexRequests.xaml
    /// </summary>
    public partial class IndexRequests : Page
    {
        private readonly RequestService requestService;
        private List<Request> allRequests;

        public IndexRequests()
        {
            InitializeComponent();
            requestService = new RequestService();
            LoadRequests();
        }

        private void LoadRequests()
        {
            allRequests = requestService.GetAllRequests();
            RequestsDataGrid.ItemsSource = allRequests;
        }

        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is Request selected)
            {
                if (selected.Status != "PENDING")
                {
                    MessageBox.Show("Only pending requests can be accepted.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to accept request #{selected.Id}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool success = requestService.AcceptRequest(selected.Id);
                    if (success)
                    {
                        MessageBox.Show("Request accepted successfully. Reservation created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadRequests();
                    }
                    else
                    {
                        MessageBox.Show("Failed to accept request. Car may not be available during this period.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a request to accept.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RefuseRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is Request selected)
            {
                if (selected.Status != "PENDING")
                {
                    MessageBox.Show("Only pending requests can be refused.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to refuse request #{selected.Id}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool success = requestService.RefuseRequest(selected.Id);
                    if (success)
                    {
                        MessageBox.Show("Request refused successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadRequests();
                    }
                    else
                    {
                        MessageBox.Show("Failed to refuse request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a request to refuse.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }

        private void FilterAll_Click(object sender, RoutedEventArgs e)
        {
            RequestsDataGrid.ItemsSource = allRequests;
        }

        private void FilterPending_Click(object sender, RoutedEventArgs e)
        {
            var pending = requestService.GetPendingRequests();
            RequestsDataGrid.ItemsSource = pending;
        }

        private void FilterAccepted_Click(object sender, RoutedEventArgs e)
        {
            var accepted = requestService.GetACCEPTEDRequests();
            RequestsDataGrid.ItemsSource = accepted;
        }

        private void FilterDenied_Click(object sender, RoutedEventArgs e)
        {
            var denied = requestService.GetDENIEDRequests();
            RequestsDataGrid.ItemsSource = denied;
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRequest(OnRequestChanged));
        }

        private void EditRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is Request selected)
            {
                NavigationService.Navigate(new EditRequest(selected, OnRequestChanged));
            }
            else
            {
                MessageBox.Show("Please select a request to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnRequestChanged()
        {
            LoadRequests();
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is Request selected)
            {
                if (MessageBox.Show($"Are you sure you want to delete request #{selected.Id}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool success = requestService.DeleteRequest(selected.Id);
                    if (success)
                    {
                        MessageBox.Show("Request deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadRequests();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a request to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is Request selected)
            {
                Request request = requestService.GetRequestById(selected.Id);
                if (request != null)
                {
                    string details = $"Request Details:\n\n" +
                                    $"ID: {request.Id}\n" +
                                    $"Client ID: {request.ClientId}\n" +
                                    $"Car ID: {request.CarId}\n" +
                                    $"Start Date: {request.StartDate:yyyy-MM-dd}\n" +
                                    $"End Date: {request.EndDate:yyyy-MM-dd}\n" +
                                    $"Status: {request.Status}\n" +
                                    $"Message: {request.Message ?? "No message"}\n" +
                                    $"Created At: {request.CreatedAt:yyyy-MM-dd HH:mm}\n" +
                                    $"Updated At: {(request.UpdatedAt.HasValue ? request.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm") : "Never")}";
                    MessageBox.Show(details, "Request Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a request to view details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FilterByClient_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ClientIdFilterTextBox.Text, out int clientId))
            {
                var requests = requestService.GetRequestsByClient(clientId);
                RequestsDataGrid.ItemsSource = requests;
                if (requests.Count == 0)
                {
                    MessageBox.Show($"No pending requests found for client ID {clientId}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid client ID.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FilterByCar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CarIdFilterTextBox.Text, out int carId))
            {
                var requests = requestService.GetRequestsByCar(carId);
                RequestsDataGrid.ItemsSource = requests;
                if (requests.Count == 0)
                {
                    MessageBox.Show($"No pending requests found for car ID {carId}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid car ID.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClientIdFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterByClient_Click(sender, e);
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

