using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Twilio.Types;
using Keyless_Entry_Authentication.Interfaces;
using System.Configuration;
using System.Data;

namespace Keyless_Entry_Authentication.Services
{
    public class KeylessEntryAuthentication : IKeylessEntryAuthentication
    {
        private readonly byte[] _key;
        private readonly ISMSService _smsService;
        private static readonly Random rdm = new Random();

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
            var databaseId = 394812;

            var conn = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString; //db connection.
            using (SqlConnection sqlConn = new SqlConnection(conn))
            {
                try
                {
                    sqlConn.Open();
                    //Verify hard coded ID to see if registered if cool if not insert into table.
                    string search = "Select Id from CarInfo WHERE Id = " + databaseId;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in SQL in TFA. Error: " + ex.ToString());
                }
            }

                if (id == databaseId)
                {
                    return Authenticate(transmission);
                }

            /* TODO: Generate random authentication key
             *       Send email/text with key
             *       Wait for user input into console
             *       Authenticate ID then authenticate transmission and return
             */
            var to = new PhoneNumber("+14055882799");
            var from = new PhoneNumber("+12028835325");
            var body = "Your keyless entry verification code is: ";
            var code = GenerateRandomKey(); 

            body += code;

            try
            {
                _smsService.SendMessage(to, from, body);

                // TODO: Prompt user for email or text preference
                //       (Currently only sends text message)
                //var message = _smsService.SendMessage(to, from, body);

                String input = Console.ReadLine();

                if (input == code.ToString())
                {
                    return Authenticate(transmission);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
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
