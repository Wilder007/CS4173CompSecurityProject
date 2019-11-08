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
            var _transmissionService = new TransmissionService();
            _transmissionService.CreateTransmissions();
        }
    }
}
