using System;

namespace Keyless_Entry_Authentication.Interfaces
{
    public interface IKeylessEntryAuthentication
    {
        bool Authenticate(byte[] transmission);

        bool TwoFactorAuthenticate(int id, byte[] transmission);
    }
}
