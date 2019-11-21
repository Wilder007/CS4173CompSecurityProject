using System;
using System.Collections.Generic;
using System.Text;

namespace Keyless_Entry_Authentication.DAL
{
    public class CarInfo
    {
        public int Id { get; set; }

        public int SendSMS { get; set; }

        public string PhoneNum { get; set; }
        public string Email { get; set; }

        //public CarInfo()
        //{
        //    //default.
        //}

        //public CarInfo(int Id, int SendSMS, string PhoneNum, string Email)
        //{
        //    this.Id = Id;
        //    this.SendSMS = SendSMS;
        //    this.PhoneNum = PhoneNum;
        //    this.Email = Email;
        //}
    }
}
