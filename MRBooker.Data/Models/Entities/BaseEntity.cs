using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MRBooker.Data.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        [Required]
        public Int64 Id
        {
            get;
            set;
        }

        [Display(Name = "Added Date")]
        public DateTime AddedDate
        {
            get;
            set;
        }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate
        {
            get;
            set;
        }

        [Display(Name = "IP Address")]
        public string IPAddress
        {
            get;
            set;
        }
    }
}
