using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keyless_Auth_Web.Models
{
    public class CarInfo
    {
        public int Id { get; set; }

        public DateTime Date_Created { get; set; }

        public int SendSMS { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Owner { get; set; }
    }
}
