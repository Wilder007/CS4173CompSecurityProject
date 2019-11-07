using System;
using Twilio.Types;
using Keyless_Entry_Authentication.Interfaces;

namespace Keyless_Entry_Authentication.Services
{
    public class KeylessEntryAuthentication : IKeylessEntryAuthentication
    {
        private readonly byte[] _key;
        private readonly ISMSService _smsService;

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
            var databaseId = 11;

            if (id == databaseId)
            {
                return Authenticate(transmission);
            }

            /* TODO: Generate random authentication key
             *       Send email/text with key
             *       Wait for user input into console
             *       Authenticate ID then authenticate transmission and return
             */
            var to = new PhoneNumber("+14058500738");
            var from = new PhoneNumber("+12028835325");
            var body = "Your keyless entry verification code is: ";
            var code = ""; // TODO: Create a function for creating a six digit verification code
                           //       then set code to be that function ToString()

            body += code;


            try
            {
                // TODO: Prompt user for email or text preference
                //       (Currently only sends text message)
                //var message = _smsService.SendMessage(to, from, body);

                var input = Console.ReadLine();

                if (input == code)
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
    }
}
