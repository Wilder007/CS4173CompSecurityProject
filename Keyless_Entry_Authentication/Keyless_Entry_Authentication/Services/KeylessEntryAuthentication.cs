using System;
namespace Keyless_Entry_Authentication.Service
{
    public class KeylessEntryAuthentication : IKeylessEntryAuthentication
    {
        private readonly int _key;

        public KeylessEntryAuthentication()
        {
            _key = 0;
        }

        public bool Authenticate(int transmission)
        {
            throw new NotImplementedException();
        }

        public bool TwoFactorAuthenticate(int id, int transmission)
        {
            var databaseId = 0;

            if (id == databaseId)
            {
                return Authenticate(transmission);
            }
            else
            {
                /* TODO: Generate random authentication key
                 *       Send email/text with key
                 *       Wait for user input into console
                 *       Authenticate ID then authenticate transmission and return
                 */
            }

            return false;
        }
    }
}
