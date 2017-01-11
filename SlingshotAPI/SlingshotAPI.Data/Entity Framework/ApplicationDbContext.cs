using SlingshotAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.Data.Entity_Framework
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<User> tblUsers { get; set; }
        public DbSet<VCard> tblVCards { get; set; }

        public DbSet<Recipient> tblRecipients { get; set; }
        public DbSet<ClientVCard> tblClientVCards { get; set; }

        public DbSet<Campaign> tblCampaigns { get; set; }
        public DbSet<Email> tblEmails { get; set; }
        public DbSet<Attachment> tblAttachments { get; set; }

        public DbSet<UserCampaign> tblUserCampaigns { get; set; }

        public DbSet<Image> tblImages { get; set; }

        public DbSet<History> tblHistory { get; set; }

        public DbSet<Event> tblEvents { get; set; }

    }
}
