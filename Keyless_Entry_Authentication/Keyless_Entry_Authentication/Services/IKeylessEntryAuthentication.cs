using System;
namespace Keyless_Entry_Authentication.Service
{
    public interface IKeylessEntryAuthentication
    {
        bool Authenticate(byte[] transmission);

        bool TwoFactorAuthenticate(int id, byte[] transmission);
    }
}
