using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Assignment6AirlineReservation
{
    public class clsFlight
    {
        #region attributes

        /// <summary>
        /// Flight's number
        /// </summary>
        private string FlightNumber;

        /// <summary>
        /// Flight's ID
        /// </summary>
        private string FlightID;

        /// <summary>
        /// Aircraft type
        /// </summary>
        private string AAircraftType;

        #endregion

        #region properties

        /// <summary>
        /// Property for flight #
        /// </summary>
        public string flightNumber
        {
            get
            {
                return FlightNumber;
            }
            set
            {
                FlightNumber = value;
            }
        }

        /// <summary>
        /// Property for flight ID #
        /// </summary>
        public string flightID
        {
            get
            {
                return FlightID;
            }
            set
            {
                FlightID = value;
            }
        }

        /// <summary>
        /// Property for aircraft type
        /// </summary>
        public string AircraftType
        {
            get
            {
                return AAircraftType;
            }
            set
            {
                AAircraftType = value;
            }
        }
        #endregion

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public clsFlight()
        {
            // Initialize to blank
            FlightNumber = "";
            FlightID = "";
            AAircraftType = "";
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
                return FlightNumber + " - " + AAircraftType;
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
