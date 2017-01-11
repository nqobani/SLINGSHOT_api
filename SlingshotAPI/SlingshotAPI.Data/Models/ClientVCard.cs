using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.Data.Models
{
    public class ClientVCard
    {
        [Key]
        public long Id { get; set; }
        public long userId { get; set; }
        public long imageId { get; set; }
        public string profileImage { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string company { get; set; }
        public string jobTitle { get; set; }
        public string fileAs { get; set; }
        public string email { get; set; }
        public string twitter { get; set; }
        public string webPageAddress { get; set; }
        public string businessPhoneNumber { get; set; }
        public string mobileNUmber { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string code { get; set; }
    }
}
