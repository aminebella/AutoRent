using CarRentalApp.Backend.Models;
using CarRentalApp.Backend.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CarRentalApp.Frontend.BackOfficeUI.Requests
{
    /// <summary>
    /// Interaction logic for EditRequest.xaml
    /// </summary>
    public partial class EditRequest : Page
    {
        private readonly RequestService requestService;
        private readonly Request request;
        private readonly Action onRequestChanged;

        public EditRequest(Request requestToEdit, Action onRequestChangedCallback)
        {
            InitializeComponent();
            requestService = new RequestService();
            request = requestToEdit;
            onRequestChanged = onRequestChangedCallback;

            // Load existing values
            StartDateTextBox.Text = request.StartDate.ToString("yyyy-MM-dd");
            EndDateTextBox.Text = request.EndDate.ToString("yyyy-MM-dd");
            MessageTextBox.Text = request.Message ?? "";
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.TryParse(StartDateTextBox.Text, out DateTime startDate) &&
                DateTime.TryParse(EndDateTextBox.Text, out DateTime endDate))
            {
                request.StartDate = startDate;
                request.EndDate = endDate;
                request.Message = MessageTextBox.Text;

                if (requestService.UpdateRequest(request))
                {
                    MessageBox.Show("Request updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    onRequestChanged?.Invoke();
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Failed to update request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter valid dates.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}



