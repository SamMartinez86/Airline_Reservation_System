using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for wndAddPassenger.xaml
    /// </summary>
    public partial class wndAddPassenger : Window
    {

        #region attributes
        
        /// <summary>
        /// Object reference for copy of passenger list
        /// </summary>
        clsFlightManager myFlightManager;

        /// <summary>
        /// If passenger is added to database than True
        /// </summary>
        bool pPassengerAdded = false;

        /// <summary>
        /// Passenger first name
        /// </summary>
        string firstName;

        /// <summary>
        /// Passenger last name 
        /// </summary>
        string lastName;

        #endregion

        #region properties

        /// <summary>
        /// Property for passenger added
        /// </summary>
        public bool PassengerAdded
        {
            get
            {
                return pPassengerAdded;
            }
            set
            {
                pPassengerAdded = value;
            }
        }


        /// <summary>
        /// Property for passenger first name
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }
        }

        /// <summary>
        /// Property for passenger last name
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }
        }

        /// <summary>
        /// Property pass copy of flight manager in
        /// </summary>
        public clsFlightManager CopyFlightManager
        {
            set
            {
                myFlightManager = value;
            }
        }

        /// <summary>
        /// Property Passenger added for
        /// </summary>
        public bool NewPassenger
        {
            get
            {
                return pPassengerAdded;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public wndAddPassenger()
        {
            try
            {
                InitializeComponent();

                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Allow only keys to only have letter input
        /// </summary>
        /// <param name="sender">sent object</param>
        /// <param name="e">key argument</param>
        private void txtLetterInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Only allow letters to be entered
                if (!(e.Key >= Key.A && e.Key <= Key.Z))
                {
                    // Allow the user to use the backspace, delete, tab and enter
                    if (!(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter))
                    {
                        // No other keys allowed besides numbers, backspace, delete, tab, and enter
                        e.Handled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Save Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                // If both text boxes aren't filled out
                if (txtFirstName.Text != "" && txtLastName.Text != "")
                {
                    // Add names to database
                    firstName = txtFirstName.Text.ToString();
                    lastName = txtLastName.Text.ToString();
                    pPassengerAdded = true;

                    
                    this.Hide();

                    // Clear text boxes
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                }
            }
            catch (System.Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Exception handling with Stack trace
        /// </summary>
        /// <param name="sClass">the class</param>
        /// <param name="sMethod">the method</param>
        /// <param name="sMessage">the error message</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
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

        /// <summary>
        /// Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // uses IsCancel but I kept getting errors without the method
        }

        #endregion
    }
}
