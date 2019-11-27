﻿using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Twilio.Types;
using Keyless_Entry_Authentication.Interfaces;
using System.Configuration;
using System.Data;
using Keyless_Entry_Authentication.DAL;
using System.Collections.Generic;

namespace Keyless_Entry_Authentication.Services
{
    public class KeylessEntryAuthentication : IKeylessEntryAuthentication
    {
        private readonly byte[] _carTransmission;
        private static readonly int carId = 320912;
        private readonly ICarKeyAuthenticationService _carKeyAuthenticationService;
        
        public KeylessEntryAuthentication()
        {
            var random = new Random();
            var bytes = new byte[5];
            random.NextBytes(bytes);

            _carTransmission = bytes;
            _carKeyAuthenticationService = new CarKeyAuthenticationService();
        }

        public bool Authenticate(byte[] keyTransmission)
        {
            return keyTransmission == _carTransmission;
        }

        public bool TwoFactorAuthenticate(int keyId, byte[] keyTransmission)
        {
            var keyIsAuthenticated = _carKeyAuthenticationService.AuthenticateKey(carId, keyId);

            if (keyIsAuthenticated)
            {
                return Authenticate(keyTransmission);
            }

            return false;
        }
    }
}
