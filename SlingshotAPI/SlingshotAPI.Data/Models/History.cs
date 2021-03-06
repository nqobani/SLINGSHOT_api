﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.Data.Models
{
    public class History
    {
        [Key]
        public long Id { get; set; }
        public long userId { get; set; }
        public long imageId { get; set; }
        public long campaignId { get; set; }
        public DateTime sentDateTime { get; set; }
        public string toEmail { get; set; }
    }
}
