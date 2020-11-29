using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region attributes

        /// <summary>
        /// Data access class object
        /// </summary>
        clsDataAccess clsData;

        /// <summary>
        /// add passenger window 
        /// </summary>
        wndAddPassenger wndAddPass;

        /// <summary>
        /// flight manager object 
        /// </summary>
        clsFlightManager currentFlightManager;

        /// <summary>
        /// the current canvas being displayed
        /// </summary>
        Canvas CurrentCanvas;

        /// <summary>
        /// is set to true if the user clicked change seat
        /// </summary>
        bool bChangeSeat;

        #endregion

        #region constructors
        public MainWindow()
        {

            InitializeComponent();

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            // Instantiate a database object     
            clsData = new clsDataAccess();      

            // Instantiate flight manager object
            currentFlightManager = new clsFlightManager();

            // Instantiate add passenger window
            wndAddPass = new wndAddPassenger();

            // Get flights from flight manager bind to combo box
            cbChooseFlight.ItemsSource = currentFlightManager.GetFlights();

            // Copy flight manager object to Add passenger window
            wndAddPass.CopyFlightManager = currentFlightManager;

            //initialize string to false
            bChangeSeat = false;


        }
        #endregion

        #region method

        /// <summary>
        /// Flight combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChooseFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Combo box is empty
                cbChoosePassenger.SelectedItem = null;

                // Passenger seat number is empty
                lblPassengersSeatNumber.Content = "";

                // Call disable buttons 
                DisableButtons();

                // Getting specific selected flight 
                clsFlight flight = (clsFlight)cbChooseFlight.SelectedItem;

                // If user control equals this flight, otherwise pick the other flight
                if (flight.ToString() == "102 - Airbus A380")
                {
                    Canvas767.Visibility = Visibility.Hidden;
                    CanvasA380.Visibility = Visibility.Visible;
                }
                else
                {
                    CanvasA380.Visibility = Visibility.Hidden;
                    Canvas767.Visibility = Visibility.Visible;
                }

                // Load passengers of specific flight
                loadPassengers(flight.flightID);

                //enable passengers combo box
                cbChoosePassenger.IsEnabled = true;
                cmdAddPassenger.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Passenger combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePassengerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // If null return
                if (cbChoosePassenger.SelectedItem == null)
                {
                    return;
                }

                // Create Passenger object
                clsPassenger Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                // Get labels in canvas 
                IEnumerable<Label> CanvasLabel = CurrentCanvas.Children.OfType<Label>();

                // Set passenger seat number label
                lblPassengersSeatNumber.Content = Passenger.seat.ToString();

                // Loop through canvas to find matching passenger seat
                foreach (Label seat in CanvasLabel)
                {
                    if (seat.Content.ToString() == Passenger.seat)
                    {
                        Label CrntLabel = (Label)seat;
                        UpdateSeat(CrntLabel);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Top level method exception handling
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } 

        /// <summary>
        /// Seat click event for user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seatClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Get label user clicked
                Label CrntLabel = (Label)sender;  

                // if passenger added and background is blue
                if (wndAddPass.PassengerAdded == true && CrntLabel.Background == Brushes.Blue)
                {
                    // get new seat with current label
                    SelectNewSeat(CrntLabel);
                }
                else
                {
                    // update seat with current label
                    UpdateSeat(CrntLabel);
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// displays the add passenger window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                this.Hide();

                // Show Add passenger
                wndAddPass.ShowDialog();

                 
                this.Show();

                // If new passenger is true
                if (wndAddPass.NewPassenger == true)
                {
                    // Disable form for new seats
                    DisableForm();
                }
            }
            catch (Exception ex)
            {
                // This is the top level method so we want to handle the exception
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Delete passenger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePassenger_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Get labels in canvas
                IEnumerable<Label> CanvasLabel = CurrentCanvas.Children.OfType<Label>();

                // Find the green label for deletion
                foreach (Label seat in CanvasLabel)
                {
                    if (seat.Background == Brushes.Green)
                    {
                        // Change back to blue
                        seat.Background = Brushes.Blue;

                        // Get flight
                        clsFlight flight = (clsFlight)cbChooseFlight.SelectedItem;

                        // Get the selected passenger
                        clsPassenger passenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                        // Call delete passenger
                        currentFlightManager.DeletePassenger(flight.flightID, passenger.firstName, passenger.lastName);

                        // Clear out seat number 
                        lblPassengersSeatNumber.Content = "";

                        // load new correct passengers
                        loadPassengers(flight.flightID);

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // This is the top level method so we want to handle the exception
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Change seat Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdChangeSeat_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // check if flight is not null and passenger is not null
                if (cbChooseFlight.SelectedIndex != -1 && cbChoosePassenger.SelectedIndex != -1)
                {
                    //when the change seat button is pressed the rest of the form is disabled 
                    DisableForm();
                    //display helpful message
                    lblSelectSeat.Visibility = Visibility.Visible;
                    //they will then pick a new seat that is BLUE and then it will change the seat for them.
                    bChangeSeat = true;
                }
            }
            catch (Exception ex)
            {
                // Top level method exception handling
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// this method calls the get passengers function 
        /// </summary>
        /// <param name="flightID"></param>
        private void loadPassengers(string flightID)
        {
            try
            {
                cbChoosePassenger.ItemsSource = currentFlightManager.GetPassengers(flightID);

                //color all the passenger seats
                seatColor();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Change selected seat to new seat
        /// </summary>
        /// <param name="CrntLabel"></param>
        private void ChangeCurrentSeat(Label CrntLabel)
        {
            try
            {
                // Get labels in canvas
                IEnumerable<Label> CanvasLabel = CurrentCanvas.Children.OfType<Label>();

                // Seat number
                string sSeatNumber; 

                // Passenger list
                clsPassenger Passenger; 

                // Loop through canvas and change seat from green to blue
                foreach (Label seat in CanvasLabel)
                {
                    if (seat.Background == Brushes.Green)
                    {
                        seat.Background = Brushes.Blue;
                        break;
                    }
                }

                // Changes label to green
                CrntLabel.Background = Brushes.Green;

                // Pull seat number 
                sSeatNumber = CrntLabel.Content.ToString();

                // Pull passenger from combo box for seat change
                Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;
                clsFlight Flight = (clsFlight)cbChooseFlight.SelectedItem;

                // Change selected to seat to passenger
                currentFlightManager.UpdateSeat(Flight.flightID, Passenger.firstName, Passenger.lastName, sSeatNumber);

                // Change seat box label
                lblPassengersSeatNumber.Content = sSeatNumber;

                // Reset seat change 
                bChangeSeat = false;

                // Hide change seat label
                lblSelectSeat.Visibility = Visibility.Hidden;

                // Load correct passengers
                loadPassengers(Flight.flightID);

                // Loop through the items in the combo box
                for (int i = 0; i < cbChoosePassenger.Items.Count; i++)
                {
                    // Pull the passengers from the combo box
                    Passenger = (clsPassenger)cbChoosePassenger.Items[i];

                    // If seat number matches change passenger
                    if (sSeatNumber == Passenger.seat)
                    {
                        // change seat number
                        cbChoosePassenger.SelectedIndex = i;

                        //change passenger label index label
                        lblPassengersSeatNumber.Content = sSeatNumber.ToString();

                        //enable buttons
                        EnableForm();
                    }
                }
                EnableForm();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Change color of seat
        /// </summary>
        private void seatColor()
        {
            try
            {
                // Find current canvas 
                if (Canvas767.Visibility == Visibility.Visible)
                {
                    CurrentCanvas = c767_Seats;
                }
                else
                {
                    CurrentCanvas = cA380_Seats;
                }

                //Getting labels in canvas
                IEnumerable<Label> CanvasLabel = CurrentCanvas.Children.OfType<Label>();

                foreach (clsPassenger passenger in cbChoosePassenger.ItemsSource)
                {
                    foreach (Label seat in CanvasLabel)
                    {
                        if (seat.Content.ToString() == passenger.seat)
                        {
                            seat.Background = System.Windows.Media.Brushes.Red;
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Update seat as well as color
        /// </summary>
        /// <param name="sender"></param>
        private void UpdateSeat(Label CrntLabel)
        {
            try
            {
                // Get labels in Canvas
                IEnumerable<Label> CanvasLabel = CurrentCanvas.Children.OfType<Label>();

                string sSeatNumber; //The seat number
                clsPassenger Passenger; //The Passenger

                //Check to see if the seat back color is read
                if (CrntLabel.Background == Brushes.Red && wndAddPass.PassengerAdded == false)
                {

                    //if a label is already being displayed as green, then switch it back to red when another is picked
                    foreach (Label seat in CanvasLabel)
                    {
                        if (seat.Background == Brushes.Green)
                        {
                            seat.Background = Brushes.Red;
                            break;//condition is met and label is changed back
                        }
                    }
                    //Turn the seat green
                    CrntLabel.Background = Brushes.Green;

                    //Get the seat number
                    sSeatNumber = CrntLabel.Content.ToString();

                    //Loop through the items in the combo box
                    for (int i = 0; i < cbChoosePassenger.Items.Count; i++)
                    {
                        //Extract the passenger from the combo box
                        Passenger = (clsPassenger)cbChoosePassenger.Items[i];

                        //If the seat number matches then select the passenger in the combo box
                        if (sSeatNumber == Passenger.seat)
                        {
                            cbChoosePassenger.SelectedIndex = i;
                            lblPassengersSeatNumber.Content = sSeatNumber.ToString();
                            //enable buttons
                            EnableButtons();
                        }
                    }
                }
                else if (CrntLabel.Background == Brushes.Blue && bChangeSeat == true)
                {
                    //the user wants to change the seat of the selected passenger 
                    ChangeCurrentSeat(CrntLabel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        } 

        /// <summary>
        /// Disable buttons until seat is picked
        /// </summary>
        private void SelectNewSeat(Label CrntLabel)
        {
            try
            {
                //Getting labels in canvas
                IEnumerable<Label> CanvasLabel = CurrentCanvas.Children.OfType<Label>();

                // If label is green switch back to red
                foreach (Label seats in CanvasLabel)
                {
                    if (seats.Background == Brushes.Green)
                    {
                        seats.Background = Brushes.Red;
                        break;
                    }
                }

                // Change label to green
                CrntLabel.Background = Brushes.Green;

                
                //display seat number in seat number box
                string seat = CrntLabel.Content.ToString();

                // Add first and last name
                string firstName = wndAddPass.FirstName;
                string LastName = wndAddPass.LastName;

                // Get selected flight
                clsFlight flight = (clsFlight)cbChooseFlight.SelectedItem;

                // Send name, seat and flight ID to Add Passenger
                currentFlightManager.AddPassenger(firstName, LastName, seat, flight.flightID);

                // Get passenger name
                clsPassenger Passenger; 

                //Loop through the items in the combo box
                for (int i = 0; i < cbChoosePassenger.Items.Count; i++)
                {
                    // Pull passenger from combo box
                    Passenger = (clsPassenger)cbChoosePassenger.Items[i];

                    // If seat # equals select passenger
                    if (seat == Passenger.seat)
                    {
                        cbChoosePassenger.SelectedIndex = i;
                        lblPassengersSeatNumber.Content = seat.ToString();

                        // Enable form 
                        EnableForm();

                        // Reset passenger added
                        wndAddPass.PassengerAdded = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);

            }
        }


        /// <summary>
        /// Enable form
        /// </summary>
        private void EnableForm()
        {
            try
            {
                // Is enabled to true
                gbPassengerInformation.IsEnabled = true;
                gPassengerCommands.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Enable delete, change, and add passenger buttons
        /// </summary>
        private void EnableButtons()
        {
            try
            {
                // Is enabled to true
                cmdChangeSeat.IsEnabled = true;
                cmdDeletePassenger.IsEnabled = true;
                cmdAddPassenger.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Disable delete and change passenger buttons 
        /// </summary>
        private void DisableButtons()
        {
            try
            {
                // Is enabled to false
                cmdChangeSeat.IsEnabled = false;
                cmdDeletePassenger.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Disable form
        /// </summary>
        private void DisableForm()
        {
            try
            {
                // Is enabled to false
                gbPassengerInformation.IsEnabled = false;
                gPassengerCommands.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Exception handling with Stack trace 
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void handleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                // error message
                MessageBox.Show(sClass + "." + sMethod + "->" + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Window closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Hide();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

    }
}
