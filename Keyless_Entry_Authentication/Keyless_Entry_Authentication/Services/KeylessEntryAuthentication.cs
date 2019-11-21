using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Twilio.Types;
using Keyless_Entry_Authentication.Interfaces;
using System.Configuration;
using System.Data;
using Keyless_Entry_Authentication.DAL;
using System.Collections.Generic;

namespace Keyless_Entry_Authentication.Services
{
    public class KeylessEntryAuthentication : IKeylessEntryAuthentication
    {
        private readonly byte[] _key;
        private readonly ISMSService _smsService;
        private static readonly int carId = 876345;
        private static readonly Random rdm = new Random();
        private readonly string conn = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString; //db connection.
        
        public KeylessEntryAuthentication()
        {
            var random = new Random();
            var bytes = new byte[5];
            random.NextBytes(bytes);

            _key = bytes;
            _smsService = new SMSService();
        }

        public bool Authenticate(byte[] transmission)
        {
            return transmission == _key;
        }

        public bool TwoFactorAuthenticate(int id, byte[] transmission)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn))
            {
                try
                {
                    //Verify hard coded ID to see if registered.
                    sqlConn.Open();

                    var search = "Select * from CarInfo";
                    var command = new SqlCommand(search, sqlConn);
                    var dataReader = command.ExecuteReader();
                    var Car = new CarInfo();

                    while (dataReader.Read())
                    {
                        var result = dataReader["Id"].ToString();

                        if (result.Equals(carId.ToString()))
                        {
                            Console.WriteLine("Car Id Authenticated!");
                            Car.Id = carId;
                            Car.SendSMS = (int)dataReader["SendSMS"];
                            Car.PhoneNum = dataReader["PhoneNum"].ToString();
                            Car.Email = dataReader["Email"].ToString();

                            //Car matches DB now see if key matches with the car.
                            var keyAuthenticated = CompareKeys(carId, id);
                            if (keyAuthenticated)
                            {
                                return true;
                            }
                        }
                    }
                    sqlConn.Close();
                    //Car is not authenticated.
                    Console.WriteLine("Car/Key is not Authenticated.");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in SQL in TFA. Error: " + ex.ToString());
                }
            }

                if (id == carId)
                {
                    return Authenticate(transmission);
                }

            return false;
        }

        public bool CompareKeys(CarInfo car, int keyId)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn))
            {
                try
                {
                    sqlConn.Open();
                    //Verify hard coded ID to see if registered.

                    string search = "Select * from KeyInfo";

                    SqlCommand command = new SqlCommand(search, sqlConn);
                    SqlDataReader dataReader = command.ExecuteReader();
                    KeyInfo keyInfo = new KeyInfo();  //New key information.
                    while (dataReader.Read())
                    {
                        /*Read to see if the key id is in the table. 
                          If the key is there see if it is associated with the car id. 
                          If not then populate then insert into the table with
                          the carId assoicated with it.
                        */

                        string result = dataReader["Id"].ToString();
                        int times_called = (int)dataReader["Times_Called"];
                        int times_successful = (int)dataReader["Times_Successful"];
                        if (result.Equals(keyId.ToString()))
                        {
                            Console.WriteLine("Key Id found!\nAutheticating with Car...");
                            string carId2 = dataReader["Car_Id"].ToString();
                            if (car.Id.ToString().Equals(carId2))
                            {
                                times_called++;
                                times_successful++;
                                //Update key info.
                                UpdateKeyInfo(keyId, times_called, times_successful);
                                return true; //return true.
                            }
                            else
                            {
                                times_called++;
                                //Update key info.
                                UpdateKeyInfo(keyId, times_called, times_successful);

                                return false;
                            }
                            //Car matches DB now see if key matches with the car.
                        }
                    }
                    sqlConn.Close(); //Don't need anymore close it.
                    //Key not found. Send message to authenticte the key.
                    var to = new PhoneNumber(car.PhoneNum);
                    var from = new PhoneNumber("+12028835325");
                    var body = "Your keyless entry verification code is: ";
                    var code = GenerateRandomKey();

                    body += code;
                    _smsService.SendMessage(to, from, body);
                    
                    String input = Console.ReadLine();

                    if (input == code.ToString())
                    {
                        //Create new key fob in the table.
                        AuthenticateKeyFob(car.Id, keyId);
                        return true;
                    }
                    Console.WriteLine("Incorrect code.. Cannot Authenticate!");
                    return false; //placeholder.
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in CompareKeys. Error:" + ex.ToString());
                    return false;
                }
            }
        }

        public void UpdateKeyInfo(int keyId, int timesCalled, int timesSucc)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn))
            {
                try
                {
                    sqlConn.Open();
                    //Verify hard coded ID to see if registered.

                    string search = "Update KeyInfo Set times_called = " + timesCalled
                        + ", times_successful = " + timesSucc + "WHERE Id = " + keyId;

                    SqlCommand command = new SqlCommand(search, sqlConn);
                    int result = command.ExecuteNonQuery();
                    if (result != 1)
                        Console.WriteLine("Error updating KeyInfo.");

                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in UpdateKeyInfo. Error: " + ex.ToString());
                }
            }
        }

        public void AuthenticateKeyFob(int carId, int keyId)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn))
            {
                try
                {
                    sqlConn.Open();
                    //Verify hard coded ID to see if registered.

                    string insert = "Insert into KeyInfo (Id, Car_Id, Times_Called, Times_Successful)" +
                        " VALUES (" + keyId + "," + carId + "," + 1 + "," + 1 + ")";
                    SqlCommand command = new SqlCommand(insert, sqlConn);
                    int result = command.ExecuteNonQuery();
                    if (result == 1)
                        Console.WriteLine("Authenticated Key Fob!");
                    
                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in AuthenticateKeyFob. Error:" + ex.ToString());
                }
            }
        }
        public int GenerateRandomKey()
        {
            int result;
            result =  rdm.Next(100000, 1000000); //generate number from 100000 - 999999
            Console.WriteLine("Random Num: " + result); //debug
            return result;
        }
    }
}
