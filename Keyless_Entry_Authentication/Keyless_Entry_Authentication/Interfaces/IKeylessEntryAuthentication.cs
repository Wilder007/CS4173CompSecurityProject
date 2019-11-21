using System;

namespace Keyless_Entry_Authentication.Interfaces
{
    public interface IKeylessEntryAuthentication
    {
        bool Authenticate(byte[] transmission);

        bool TwoFactorAuthenticate(int id, byte[] transmission);

        bool CompareKeys(int carId, int keyId);

        void UpdateKeyInfo(int keyId, int timesCalled, int timeSucc);

        void AuthenticateKeyFob(int carId, int keyId);

        int GenerateRandomKey();
    }
}
