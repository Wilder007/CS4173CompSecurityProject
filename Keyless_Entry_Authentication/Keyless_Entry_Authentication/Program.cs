using System;
using System.Timers;
using Keyless_Entry_Authentication.Service;

namespace Keyless_Entry_Authentication
{
    public class Program
    {
        private static DateTime _now;
        private static DateTime _end;
        private static int _attempts;
        private static bool _canAuthenticate = true;
        private static readonly int _allowedAttempts = 5;

        public static void Main(string[] args)
        {
            _end = DateTime.Now.AddSeconds(10); 

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
                        var transmission = ByteGenerator();
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

        /*
         * Function to check if the current time has exceeded the initial ten second interval
         * */
        public static void CheckTimer(DateTime now, DateTime end)
        {
            if (now > end)
            {
                _end = DateTime.Now.AddSeconds(10);
                _attempts = 0;
                _canAuthenticate = true;
            }
        }

        /*
         * Function to generate a random five byte array
         * */
        public static byte[] ByteGenerator()
        {
            byte[] bytes = new byte[5];
            var random = new Random();

            random.NextBytes(bytes);

            Console.WriteLine("Attempting to authenticate the following 40-bit key:\n{0}", BinaryRepresentation(bytes));

            return bytes;
        }

        /*
         * Function to print the binary representation of the byte array
         * */
        public static string BinaryRepresentation(byte[] transmission) 
        {
            var res = "";

            for (int i = 0; i < transmission.Length; i++)
            {
                var bin = Convert.ToString(transmission[i], 2).PadLeft(8, '0');
                res += bin.Substring(0, 4) + " " + bin.Substring(4, 4) + " ";
            }

            return res;
        }
    }
}
