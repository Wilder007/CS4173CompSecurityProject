using Keyless_Entry_Authentication.Services;

namespace Keyless_Entry_Authentication
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var _transmissionService = new TransmissionService();
            _transmissionService.CreateTransmissions();
        }
    }
}
