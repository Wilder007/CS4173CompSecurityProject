using System;
using System.Configuration;
using System.Data.SqlClient;
using Keyless_Entry_Authentication.DAL;
using Keyless_Entry_Authentication.Interfaces;
using Twilio.Types;

namespace Keyless_Entry_Authentication.Services
{
    /*
     *     BASIC LOGIC FLOW OF ALGORITHM:
     *
     * --> Get all Cars from Car table.
     * --> Check if the Car we want is in the list of cars.
     * --> If it is: grab the associated info with the Car (contact info/preference).
     * --> Check to see if the Key we want to authenticate is associated with the Car.
     * --> If the key isn't found in the list of keys associated with the car, send
     *     text/email to authenticate whether or not an authorized user is attempting to
     *     associate a new Key with the Car.
     *     
     * --> If all checks pass return true and attempt to authenticate transmission.
     * --> If not return false and fail authentication attempt.
     */
    public class CarKeyAuthenticationService : ICarKeyAuthenticationService
    {
        private readonly ISMSService _smsService;

        private static readonly Random rdm = new Random();
        private readonly string conn = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString; //db connection.

        public CarKeyAuthenticationService()
        {
            _smsService = new SMSService();
        }

        public bool AuthenticateKey(int carId, int keyId)
        {
            using (var sqlConn = new SqlConnection(conn))
            {
                try
                {
                    //Verify hard coded ID to see if registered.
                    sqlConn.Open();

                    var search = "Select * from CarInfo";
                    using (var command = new SqlCommand(search, sqlConn))
                    {
                        var dataReader = command.ExecuteReader();
                        var car = new CarInfo();

                        while (dataReader.Read())
                        {
                            var result = dataReader["Id"].ToString();

                            if (result.Equals(carId.ToString()))
                            {
                                Console.WriteLine("Car Id Authenticated!");

                                car.Id = carId;
                                car.SendSMS = (int)dataReader["SendSMS"];
                                car.PhoneNum = dataReader["PhoneNum"].ToString();
                                car.Email = dataReader["Email"].ToString();

                                //Car matches DB now see if key matches with the car.
                                return CompareKeys(car, keyId);
                            }
                        }
                    }
                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in SQL in TFA. Error: " + ex.ToString());
                }
            }

            // Car is not authenticated.
            Console.WriteLine("Car not found");

            return false;
        }

        public bool CompareKeys(CarInfo car, int keyId)
        {
            using var sqlConn = new SqlConnection(conn);
            try
            {
                sqlConn.Open();

                //Verify hard coded ID to see if registered.
                var search = "Select * from KeyInfo";
                var command = new SqlCommand(search, sqlConn);
                var dataReader = command.ExecuteReader();
                var timesCalled = (int)dataReader["Times_Called"];
                var timesSuccessful = (int)dataReader["Times_Successful"];

                /*
                 * Read to see if the key id is in the table. 
                 * If the key is there see if it is associated with the car id. 
                 * If not then populate then insert into the table with
                 * the carId associated with it.
                 */
                while (dataReader.Read())
                { 
                    var result = dataReader["Id"].ToString();

                    if (result.Equals(keyId.ToString()))
                    {
                        Console.WriteLine("Key Id found!\nAutheticating with Car...");
                        var dbCarId = dataReader["Car_Id"].ToString();

                        /*
                         * If the key ID has an entry in its table that matches
                         * the car we're sending transmissions to return true
                         */
                        if (car.Id.ToString().Equals(dbCarId))
                        {
                            timesCalled++;
                            timesSuccessful++;
                            UpdateKeyInfo(keyId, timesCalled, timesSuccessful);
                            return true;
                        }

                        //Update key info.
                        timesCalled++;
                        UpdateKeyInfo(keyId, timesCalled, timesSuccessful);
                    }
                }
                sqlConn.Close();

                //Key not found. Send message to authenticte the key.
                var to = new PhoneNumber(car.PhoneNum);
                var from = new PhoneNumber("+12028835325");
                var body = "Your keyless entry verification code is: ";
                var code = GenerateRandomKey();
                body += code;

                _smsService.SendMessage(to, from, body);

                var input = Console.ReadLine();

                if (input == code.ToString())
                {
                    //Create new key fob in the table.
                    CreateKeyFobEntry(car.Id, keyId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CompareKeys. Error:" + ex.ToString());
            }

            Console.WriteLine("Incorrect code.. Cannot Authenticate!");
            return false;
        }

        public void UpdateKeyInfo(int keyId, int timesCalled, int timesSucc)
        {
            using SqlConnection sqlConn = new SqlConnection(conn);
            try
            {
                sqlConn.Open();

                //Verify hard coded ID to see if registered
                var search = "Update KeyInfo Set times_called = " + timesCalled
                    + ", times_successful = " + timesSucc + "WHERE Id = " + keyId;
                var command = new SqlCommand(search, sqlConn);
                var result = command.ExecuteNonQuery();

                if (result != 1)
                {
                    Console.WriteLine("Error updating KeyInfo.");
                }

                sqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateKeyInfo. Error: " + ex.ToString());
            }
        }

        public void CreateKeyFobEntry(int carId, int keyId)
        {
            using SqlConnection sqlConn = new SqlConnection(conn);
            try
            {
                //Verify hard coded ID to see if registered.
                sqlConn.Open();

                var insert = "Insert into KeyInfo (Id, Car_Id, Times_Called, Times_Successful)" +
                    " VALUES (" + keyId + "," + carId + "," + 1 + "," + 1 + ")";
                var command = new SqlCommand(insert, sqlConn);
                var result = command.ExecuteNonQuery();

                if (result == 1)
                {
                    Console.WriteLine("Authenticated Key Fob!");
                }

                sqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AuthenticateKeyFob. Error:" + ex.ToString());
            }
        }

        public int GenerateRandomKey()
        {
            return rdm.Next(100000, 1000000);
        }
    }
}
