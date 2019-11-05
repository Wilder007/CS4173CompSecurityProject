using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Keyless_Entry_Authentication.Services;
using System.Configuration;

namespace Keyless_Entry_Authentication
{
    public class Program
    {

        public static void Main(string[] args)
        {
            // Find your Account Sid and Token at twilio.com/console
            // DANGER! This is insecure. See http://twil.io/secure
            var accountSid = "AC757f1654c71aa266af7f9f4cb94b66fa";
            var authToken = "ddd8819c1aca71da525d7aa4f9c38926";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Fugg u bitch",
                from: new Twilio.Types.PhoneNumber("+12028835325"),
                to: new Twilio.Types.PhoneNumber("+14055882799")
            );

            Console.WriteLine(message.Sid);

            //var _transmissionService = new TransmissionService();
            //_transmissionService.CreateTransmissions();
        }
    }
}
