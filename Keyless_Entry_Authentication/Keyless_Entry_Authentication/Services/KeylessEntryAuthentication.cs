using System;
using Keyless_Entry_Authentication.Interfaces;

namespace Keyless_Entry_Authentication.Service
{
    public class KeylessEntryAuthentication : IKeylessEntryAuthentication
    {
        private readonly byte[] _key;

        public KeylessEntryAuthentication()
        {
            var random = new Random();
            var bytes = new byte[5];
            random.NextBytes(bytes);

            _key = bytes;
        }

        public bool Authenticate(byte[] transmission)
        {
            return transmission == _key;
        }

        public bool TwoFactorAuthenticate(int id, byte[] transmission)
        {
            var databaseId = 1;

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
