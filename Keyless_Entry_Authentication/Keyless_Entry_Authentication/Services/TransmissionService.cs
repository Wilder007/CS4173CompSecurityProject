﻿using System;
using Keyless_Entry_Authentication.Interfaces;

namespace Keyless_Entry_Authentication.Services
{
    /*
     * This class is deprecated and the functionality for it has been moved
     * to the front end
     */
    public class TransmissionService : ITransmissionService
    {
        private static int _attempts;
        private static bool _canAuthenticate = true;
        private static readonly int _allowedAttempts = 2;
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
            
            var keyId = 567432; // TODO: Placeholder value meant to represent the unique Key ID

            while (true)
            {
                _now = DateTime.Now;

                if (_canAuthenticate)
                {
                    if (_attempts < _allowedAttempts)
                    {
                        // Generates a random five byte array (since key transmissions are 40 bits in length)
                        var transmission = _binaryService.ConvertByte(keyId);
                        Console.WriteLine("Attempting to authenticate the following 40-bit key:\n{0}",
                            _binaryService.BinaryRepresentation(transmission));
                        var result = _authenticationService.TwoFactorAuthenticate(keyId, transmission);

                        if (!result)
                        {
                            Console.WriteLine("Authentication failed.");
                        }
                        else
                        {
                            Console.WriteLine("Authentication successful!");
                            break;
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
                    if (_attempts == _allowedAttempts)
                    {
                        Console.WriteLine("The allowed number of authentication attempts has been exceeded.");
                    }

                    _attempts++;
                    CheckTimer(_now, _end);
                }
            }
        }

        /*
         * TODO: Move this function to its own service, possibly
         * returning bool. Have attempts and can authenticate reset
         * based on boolean return.
         */
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
