using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SlingshotAPI.Data.Models
{
    public class Campaign
    {
        [Key]
        public long Id { get; set; }
        public long creatorId { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string thumbnail { get; set; }
        public ISet<Email> email { get; set; }
    }
}
