using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class Vendor
    {
		public int vendorId { set; get; }
		public string firstName { set; get; }
		public string lastName { set; get; }
		public string email { set; get; }
		public string password { set; get; }
		public string contactNumber { set; get; }
		public string residentialAddress { set; get; }
		public string companyName { set; get; }

        public Vendor(int vendorId, string firstName, string lastName, string email, string password, string contactNumber, string residentialAddress, string companyName)
        {
            this.vendorId = vendorId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
            this.contactNumber = contactNumber;
            this.residentialAddress = residentialAddress;
            this.companyName = companyName;
        }

        public Vendor()
        {
        }
    }
}
