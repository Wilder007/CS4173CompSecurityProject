using System.Text;
using Keyless_Entry_Authentication.Interfaces;

namespace Keyless_Entry_Authentication.Services
{
    public class KeylessEntryAuthentication : IKeylessEntryAuthentication
    {
        private readonly byte[] _carTransmission;
        private static readonly int carId = 320912;
        private readonly ICarKeyAuthenticationService _carKeyAuthenticationService;
        
        public KeylessEntryAuthentication()
        {
            var bitString = "";
            for (int i = 0; i < 40; i++)
            {
                bitString += "0";
            }

            _carTransmission = Encoding.ASCII.GetBytes(bitString);
            _carKeyAuthenticationService = new CarKeyAuthenticationService();
        }

        public bool Authenticate(byte[] keyTransmission)
        {
            return Encoding.ASCII.GetString(keyTransmission) == Encoding.ASCII.GetString(_carTransmission);
        }

        public bool TwoFactorAuthenticate(int keyId, byte[] keyTransmission)
        {
            var keyIsAuthenticated = _carKeyAuthenticationService.AuthenticateKey(carId, keyId);

            if (keyIsAuthenticated)
            {
                return Authenticate(keyTransmission);
                //return true;
            }

            return false;
        }
    }
}
