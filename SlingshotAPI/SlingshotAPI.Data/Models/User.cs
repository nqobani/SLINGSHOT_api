using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.Data.Models
{
    public class User
    {
        public long Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string type { get; set; }
    }
}
