using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class EmailMessage
    {
        public int emailId { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string message { get; set; }

        public EmailMessage(int emailId, string from, string to, string subject, string message)
        {
            this.emailId = emailId;
            this.from = from;
            this.to = to;
            this.subject = subject;
            this.message = message;
        }

        public EmailMessage()
        {
        }
    }
}
