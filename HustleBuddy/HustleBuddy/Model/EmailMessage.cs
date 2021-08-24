using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IOSG_REST_API.Models
{
    public class EmailMessage
    {
        public int emailId { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string message { get; set; }

        public EmailMessage(string to, string subject, string message)
        {
            this.emailId = 0;
            this.from = "autoreply.hustlebuddy@gmail.com";
            this.to = to;
            this.subject = subject;
            this.message = message;
        }

        public EmailMessage()
        {
            // Default Constructor
        }
    }
}
