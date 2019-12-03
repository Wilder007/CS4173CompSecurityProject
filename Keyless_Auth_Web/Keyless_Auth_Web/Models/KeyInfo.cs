using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keyless_Auth_Web.Models
{
    public class KeyInfo
    {
        public string Id { get; set; }
        public DateTime Date_Created { get; set; }
        public int Car_Id { get; set; }

        public int Times_Called { get; set; }

        public int Times_Successful { get; set; }

        public KeyInfo(String Id)
        {
            this.Id = Id;
            Date_Created = DateTime.Now;
            Car_Id = 0;
            Times_Called = 0;
            Times_Successful = 0;
        }

    }
}
