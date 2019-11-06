using System.Configuration;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Keyless_Entry_Authentication.Interfaces;

namespace Keyless_Entry_Authentication.Services
{
    public class SMSService : ISMSService
    {
        private readonly string _accountSid;
        private readonly string _authToken;

        public SMSService()
        {
            _accountSid = ConfigurationManager.AppSettings["sid"];
            _authToken = ConfigurationManager.AppSettings["authtoken"];
        }

        public MessageResource SendMessage(PhoneNumber to, PhoneNumber from, string body)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var message = MessageResource.Create(
                body: body,
                from: from,
                to: to
            );

            return message;
        }
    }
}
