using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Assignment6AirlineReservation
{
    public class clsFlightManager
    {
        #region attributes

        /// <summary>
        /// Data access class object 
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// Create observable collection for Passengers
        /// </summary>
        public static ObservableCollection<clsPassenger> PassengerLst;

        /// <summary>
        /// Create observable collection for Flights
        /// </summary>
        public static ObservableCollection<clsFlight> FlightLst;

        #endregion

        #region constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public clsFlightManager()
        {
            // Instantiate a database object 
            db = new clsDataAccess();

            // Instantiate observable collection object
            PassengerLst = new ObservableCollection<clsPassenger>();
            FlightLst = new ObservableCollection<clsFlight>();

            // Data access class variables 

            // SQL statement for DB query
            string sSQL;    
            
            // Number of values returned from DB query
            int iRet = 0;   

            // Holds actual values from DB query
            DataSet ds = new DataSet(); 

            // SQL Select statement to get flight information
            sSQL = "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT";

            // Pull information into dataset
            ds = db.ExecuteSQLStatement(sSQL, ref iRet);

            // For each value in Select statement results
            // Add data to flight collection
            for (int i = 0; i < iRet; i++)
            {
                FlightLst.Add(new clsFlight
                {
                    flightID = ds.Tables[0].Rows[i][0].ToString(),
                    flightNumber = ds.Tables[0].Rows[i]["Flight_Number"].ToString(),
                    AircraftType = ds.Tables[0].Rows[i]["Aircraft_Type"].ToString()
                });
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Return passenger list
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns> PassengerLst </returns>
        public ObservableCollection<clsPassenger> GetPassengers(string flightId)
        {
            try
            {
                // Initiate observable collections
                FlightLst = new ObservableCollection<clsFlight>();
                PassengerLst = new ObservableCollection<clsPassenger>();

                // Data access class variables 
                // SQL statement for DB query
                string sSQL;

                // Number of values returned from DB query
                int iRet = 0;

                // Holds actual values from DB query
                DataSet ds = new DataSet();

                // SQL Select statement to get passenger information
                sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, Flight_ID, Seat_Number " +
                    "FROM Passenger, Flight_Passenger_Link " +
                    "WHERE Passenger.Passenger_ID = Flight_Passenger_Link.Passenger_ID AND " +
                    "Flight_id = " + flightId;

                //// Pull information into dataset
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                // For each value in Select statement results
                // Add data to passenger collection
                for (int i = 0; i < iRet; i++)
                {
                    PassengerLst.Add(new clsPassenger
                    {
                        firstName = ds.Tables[0].Rows[i]["First_Name"].ToString(),
                        lastName = ds.Tables[0].Rows[i]["Last_Name"].ToString(),
                        passengerID = ds.Tables[0].Rows[i][0].ToString(),
                        flight = ds.Tables[0].Rows[i][3].ToString(),
                        seat = ds.Tables[0].Rows[i][4].ToString()
                    });
                }
                // return passenger list
                return PassengerLst;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Return flight list
        /// </summary>
        /// <returns> FlightLst </returns>
        public ObservableCollection<clsFlight> GetFlights()
        {
            try
            {
                // returns flight list
                return FlightLst;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Add passenger 
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Last"></param>
        public void AddPassenger(string First, string Last, string Seat, string FlightID)
        {
            try
            {
                // SQL Insert Statement
                string sSQL = "INSERT INTO PASSENGER(First_Name, Last_Name) VALUES('" + First + "','" + Last + "')";   

                // number of rows
                int iRet;

                // Add passenger to database
                iRet = db.ExecuteNonQuery(sSQL);
            
                // Get passenger ID
                sSQL = "SELECT Passenger_ID from Passenger where First_Name = '" + First + "' AND Last_Name = '" + Last + "'";

                // Get new passenger ID 
                string PassengerId = db.ExecuteScalarSQL(sSQL);

                // Insert into link table
                sSQL = "INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID, Seat_Number) " + "VALUES( " + FlightID + " , " + PassengerId + " , " + Seat + ")";

                // Add passenger into database
                iRet = db.ExecuteNonQuery(sSQL);

                // Add new passenger to observable collection 
                PassengerLst.Add(new clsPassenger
                {
                    firstName = First,
                    lastName = Last,
                    passengerID = PassengerId,
                    flight = FlightID,
                    seat = Seat
                });
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Delete Passenger 
        /// </summary>
        public void DeletePassenger(string FlightID, string First, string Last)
        {
            try
            {
                // Get passenger ID
                string sSQL = "SELECT Passenger_ID from Passenger where First_Name = '" + First + "' AND Last_Name = '" + Last + "'";

                // Get new passenger ID 
                string PassengerID = db.ExecuteScalarSQL(sSQL);
                 
                // Deleting the link table
                sSQL = "Delete FROM FLIGHT_PASSENGER_LINK " +
                             "WHERE FLIGHT_ID = " + FlightID + " AND " +
                             "PASSENGER_ID = " + PassengerID + "";

                // Add passenger to database
                int iRet = db.ExecuteNonQuery(sSQL);

                // Delete passenger
                sSQL = "Delete FROM PASSENGER " +
                   "WHERE PASSENGER_ID = " + PassengerID + "";

                // Add passenger into database
                iRet = db.ExecuteNonQuery(sSQL);

                // Remove passenger from passenger list
                PassengerLst.Remove(new clsPassenger() { firstName = First });

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Update seat
        /// </summary>
        /// <param name="FlightID"></param>
        /// <param name="First"></param>
        /// <param name="Last"></param>
        /// <param name="SeatNumber"></param>
        public void UpdateSeat(string FlightID, string First, string Last, string SeatNumber)
        {
            try
            {
                // Get passenger ID
                string sSQL = "SELECT Passenger_ID from Passenger where First_Name = '" + First + "' AND Last_Name = '" + Last + "'";

                // Get new passenger ID 
                string PassengerID = db.ExecuteScalarSQL(sSQL);

                // Updating seat numbers
                sSQL = "UPDATE FLIGHT_PASSENGER_LINK " +
                          "SET Seat_Number = '" + SeatNumber + "' " +
                          "WHERE FLIGHT_ID = " + FlightID + " AND PASSENGER_ID = " + PassengerID + "";

                // Add passenger into database
                int iRet = db.ExecuteNonQuery(sSQL);

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