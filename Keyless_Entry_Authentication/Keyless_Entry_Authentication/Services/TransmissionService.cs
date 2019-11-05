using System;
using Keyless_Entry_Authentication.Interfaces;
using Keyless_Entry_Authentication.Services;

namespace Keyless_Entry_Authentication.Services
{
    public class TransmissionService : ITransmissionService
    {
        private static int _attempts;
        private static bool _canAuthenticate = true;
        private static readonly int _allowedAttempts = 5;
        private static DateTime _now;
        private static DateTime _end;
        private static IBinaryService _binaryService;
        private static IKeylessEntryAuthentication _authenticationService;       

        public TransmissionService()
        {
            _binaryService = new BinaryService();
            _authenticationService = new KeylessEntryAuthentication();
        }

        public void CreateTransmissions()
        {
            _end = DateTime.Now.AddSeconds(10);
            
            var id = 1; // TODO: Placeholder value meant to represent the unique Key ID

            while (true)
            {
                _now = DateTime.Now;

                if (_canAuthenticate)
                {
                    if (_attempts < _allowedAttempts)
                    {
                        // Generates a random five byte array (since key transmissions are 40 bits in length)
                        var transmission = _binaryService.ByteGenerator();
                        var result = _authenticationService.TwoFactorAuthenticate(id, transmission);

                        if (!result)
                        {
                            Console.WriteLine("Authentication failed.");
                        }
                        else
                        {
                            Console.WriteLine("Authentication successful!");
                        }

                        _attempts++;
                    }
                    else
                    {
                        _canAuthenticate = false;
                    }

                    CheckTimer(_now, _end);
                }
                else
                {
                    if (_attempts == 5)
                    {
                        Console.WriteLine("The allowed number of authentication attempts has been exceeded.");
                    }

                    _attempts++;
                    CheckTimer(_now, _end);
                }
            }
        }

        private void CheckTimer(DateTime now, DateTime end)
        {
            if (now > end)
            {
                _attempts = 0;
                _canAuthenticate = true;
                _end = DateTime.Now.AddSeconds(10);
            }
        }
    }
}
