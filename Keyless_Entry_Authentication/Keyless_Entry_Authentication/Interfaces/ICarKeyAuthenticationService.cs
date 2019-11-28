using Keyless_Entry_Authentication.DAL;

namespace Keyless_Entry_Authentication.Interfaces
{
    public interface ICarKeyAuthenticationService
    {
        bool AuthenticateKey(int carId, int keyId);

        bool CompareKeys(CarInfo car, int keyId);

        void UpdateKeyInfo(int keyId, int timesCalled, int timesSucc);

        void CreateKeyFobEntry(int carId, int keyId);

        int GenerateRandomKey();
    }
}
