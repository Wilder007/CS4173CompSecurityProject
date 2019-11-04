using System;
using Keyless_Entry_Authentication.Service;

namespace Keyless_Entry_Authentication
{
    public class CreateAndAuthenticate
    {
        private readonly IKeylessEntryAuthentication _authentication;
        public CreateAndAuthenticate(IKeylessEntryAuthentication authentication)
        {
            _authentication = authentication;
        }

        public void Create()
        {

        }

        public void Authenticate()
        {

        }

        public byte[] ByteGenerator()
        {
            byte[] bytes = new byte[5];
            var random = new Random();

            random.NextBytes(bytes);

            Console.WriteLine("Attempting to authenticate the following 40-bit key:\n{0}", BinaryRepresentation(bytes));

            return bytes;
        }

        public string BinaryRepresentation(byte[] transmission)
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
