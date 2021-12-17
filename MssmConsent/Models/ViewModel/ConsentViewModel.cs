using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MssmConsent.Models.ViewModel
{
    public class ConsentViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Language ID")]
        public int LanguageID { get; set; }

        [Display(Name = "Consent Name")]
        public string ConsentName { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [Display(Name = "Last Modified At")]
        public DateTime LastModifiedAt { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Modified By")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }


    }
}
