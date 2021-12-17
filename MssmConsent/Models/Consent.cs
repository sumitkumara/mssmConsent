using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MssmConsent.Models
{
    public class Consent
    {
        public int ID { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Active { get; set; }

        public string CreatedBy { get; set; }
        public string ConsentName { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public string LastModifiedBy { get; set; }

    }
}
