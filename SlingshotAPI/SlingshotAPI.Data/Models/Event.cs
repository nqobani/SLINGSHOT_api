using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.Data.Models
{
    public class Event
    {
        [Key]
        public long Id { get; set; }
        public string title { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
    }
}
