using System;

namespace Keyless_Entry_Authentication.Interfaces
{
    public interface IBinaryService
    {
        byte[] ByteGenerator();

        string BinaryRepresentation(byte[] transmission);

        byte[] ConvertByte(int num);
    }
}
