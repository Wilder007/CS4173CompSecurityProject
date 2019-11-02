using System;
namespace Keyless_Entry_Authentication.Service
{
    public interface IKeylessEntryAuthentication
    {
        bool Authenticate(int transmission);

        bool TwoFactorAuthenticate(int id, int transmission);
    }
}
