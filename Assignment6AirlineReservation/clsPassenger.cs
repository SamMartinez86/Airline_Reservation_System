using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Assignment6AirlineReservation
{
    public class clsPassenger
    {
        #region attributes
        /// <summary>
        /// Passenger's first name 
        /// </summary>
        private string FirstName;

        /// <summary>
        /// Passenger's last name 
        /// </summary>
        private string LastName;

        /// <summary>
        /// Passenger's ID #
        /// </summary>
        private string PassengerID;

        /// <summary>
        /// Passengers flight #
        /// </summary>
        private string Flight;

        /// <summary>
        /// Passenger's seat #
        /// </summary>
        private string Seat;

        #endregion

        #region properties

        /// <summary>
        /// Property for First Name
        /// </summary>
        public string firstName
        {
            get
            {
                return FirstName;
            }
            set
            {
                FirstName = value;
            }
        }

        /// <summary>
        /// Property for last name
        /// </summary>
        public string lastName
        {
            get
            {
                return LastName;
            }
            set
            {
                LastName = value;
            }
        }

        /// <summary>
        /// Property for passenger ID #
        /// </summary>
        public string passengerID
        {
            get
            {
                return PassengerID;
            }
            set
            {
                PassengerID = value;
            }
        }

        /// <summary>
        /// Property for flight ID #
        /// </summary>
        public string flight
        {
            get
            {
                return Flight;
            }
            set
            {
                Flight = value;
            }
        }

        /// <summary>
        /// Property for passenger seat #
        /// </summary>
        public string seat
        {
            get
            {
                return Seat;
            }
            set
            {
                Seat = value;
            }
        }

        #endregion

        #region constructors
        /// <summary>
        /// Constructor 
        /// </summary>
        public clsPassenger()
        {
            // Initialize to blank
            FirstName = "";
            LastName = "";
            PassengerID = "";
            Seat = "";
            Flight = "";
        }
        #endregion

        #region methods

        /// <summary>
        /// Overridden for first and last name display
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return FirstName + " " + LastName;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
        #endregion
    }
}
