using System;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace Keyless_Entry_Authentication.Interfaces
{
    public interface ISMSService
    {
        MessageResource SendMessage(PhoneNumber to, PhoneNumber from, string body);
    }
}
