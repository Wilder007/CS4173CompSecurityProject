using System;
using System.Collections.Generic;
using System.Text;

namespace Keyless_Entry_Authentication.DAL
{
    public class KeyInfo
    {
        public int Id { get; private set; }
        public int Car_id { get; private set; }
        public int Times_Used { get; private set; }
        public int Times_Success { get; private set; }
    }
}
