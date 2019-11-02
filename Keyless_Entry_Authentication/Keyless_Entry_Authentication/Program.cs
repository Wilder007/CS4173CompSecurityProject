using System;
using Keyless_Entry_Authentication.Service;

namespace Keyless_Entry_Authentication
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * TODO: Get user input first and assign id and transmission values
             * While loop to continuously ask for these values
             * (Possibly store some test values in DB for testing of rapid authentication attempts)
             */

            int id = 0, transmission = 0;

            IKeylessEntryAuthentication authentication = new KeylessEntryAuthentication();

            //Check DateTime value for authentication cap

            authentication.TwoFactorAuthenticate(id, transmission);
        }
    }
}
