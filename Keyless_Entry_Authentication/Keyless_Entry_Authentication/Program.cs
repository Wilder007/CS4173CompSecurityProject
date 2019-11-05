using System;
using System.Timers;
using Keyless_Entry_Authentication.Interfaces;
using Keyless_Entry_Authentication.Service;
using Keyless_Entry_Authentication.Services;

namespace Keyless_Entry_Authentication
{
    public class Program
    {
        private static DateTime _now;
        private static DateTime _end;
        private static int _attempts;
        private static bool _canAuthenticate = true;
        private static readonly int _allowedAttempts = 5;
        private static IBinaryService _binaryService;

        public static void Main(string[] args)
        {
            _end = DateTime.Now.AddSeconds(10);
            _binaryService = new BinaryService();

            var id = 1; // TODO: Placeholder value meant to represent the unique Key ID
            var authentication = new KeylessEntryAuthentication();

            while (true)
            {
                _now = DateTime.Now;

                if (_canAuthenticate)
                {
                    if (_attempts < _allowedAttempts)
                    {
                        // Generates a random five byte array (since key transmissions are 40 bits in length)
                        var transmission = _binaryService.ByteGenerator();
                        var result = authentication.TwoFactorAuthenticate(id, transmission);

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

        public static void CheckTimer(DateTime now, DateTime end)
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
