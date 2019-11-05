using System;
using Keyless_Entry_Authentication.Interfaces;

namespace Keyless_Entry_Authentication.Services
{
    public class BinaryService : IBinaryService
    {
        /*
         * Function to generate a random five byte array
         * */
        public byte[] ByteGenerator()
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
