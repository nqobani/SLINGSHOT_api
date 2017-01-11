using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SlingshotAPI.Data.Models;
using SlingshotAPI.Data.Entity_Framework;

namespace SlingshotAPI.Data
{


    public class DbConnection
    {
        private DataClasses1DataContext con = new DataClasses1DataContext();
        ApplicationDbContext dbCon = new ApplicationDbContext();

        //public UserModel createUsers(string email, string password, string type)
        //{
        //    var userC = (from user in con.tblUsers
        //                 where user.email == email
        //                 select new UserModel
        //                 {
        //                     id = user.Id,
        //                     email = user.email,
        //                     password = user.password
        //                 }).ToList();

        //    if (userC.Count > 0)
        //    {
        //        throw new ErrorMessage
        //        {
        //            message = "Registration Failed",
        //            course = "User with the same email addres already exists"
        //        };
        //        //User with the same email address already exist, try another one or sign in
        //    }
        //    else
        //    {
        //        tblUser newUser = new tblUser();
        //        newUser.email = email;
        //        newUser.password = password;
        //        newUser.type = type;

        //        con.tblUsers.InsertOnSubmit(newUser);
        //        con.SubmitChanges();
        //        int ids = newUser.Id;

        //        var newUsers = (from user in con.tblUsers
        //                        where user.Id == ids
        //                        select new UserModel
        //                        {
        //                            id = user.Id,
        //                            email = user.email,
        //                            password = user.password,
        //                            type=user.type
        //                        }).FirstOrDefault();
        //        return newUsers;
        //    }

        //}
        public User createUser(string email, string password, string type)
        {
            User newUser =new  User();
            newUser.email = email;
            newUser.password = password;
            newUser.type = type;
            dbCon.tblUsers.Add(newUser);

            dbCon.SaveChanges();
            long userId = newUser.Id;

            User user = dbCon.tblUsers.SingleOrDefault(u => u.Id == userId);
            return user;

        }

        //public string GetUserEmail(int userId)
        //{
        //    var user = (from u in con.tblUsers
        //                where u.Id == userId
        //                select new UserModel
        //                {
        //                    email = u.email
        //                }).FirstOrDefault();
        //    return user.email;
        //}
        public string GetUserEmail(long userId)
        {
            User user = dbCon.tblUsers.FirstOrDefault(s=>s.Id == userId);
            string email = user.email;
            return email;
        }

        //public VCardModel CreateVCard(int userId, string firstName, string lastName, string company, string jobTitle, string email, string webPageAddress, string twitter, string businessPhoneNumber, string mobilePhone, string country, string city, string cityCode, string imageLink)
        //{
        //    tblVCard newVCard = new tblVCard();
        //    newVCard.userID = userId;
        //    newVCard.firstName = firstName;
        //    newVCard.lastName = lastName;
        //    newVCard.company = company;
        //    newVCard.jobTitle = jobTitle;
        //    newVCard.email = email;
        //    newVCard.webPageAddress = webPageAddress;
        //    newVCard.twitter = twitter;
        //    newVCard.businessPhoneNumber = businessPhoneNumber;
        //    newVCard.mobileNumber = mobilePhone;
        //    newVCard.country = country;
        //    newVCard.city = city;
        //    newVCard.code = cityCode;
        //    newVCard.profileImage = imageLink;

        //    con.tblVCards.InsertOnSubmit(newVCard);
        //    con.SubmitChanges();

        //    int vCardId = newVCard.Id;

        //    var vCard = (from v in con.tblVCards
        //                 where v.Id == vCardId
        //                 select new VCardModel
        //                 {
        //                     id = v.Id,
        //                     UserId = v.userID,
        //                     firstName = v.firstName,
        //                     lastName = v.lastName,
        //                     company = v.company,
        //                     jobTitle = v.jobTitle,
        //                     fileAs = v.fileAs,
        //                     email = v.email,
        //                     webPageAddress = v.webPageAddress,
        //                     twitter = v.twitter,
        //                     businessPhoneNumber = v.businessPhoneNumber,
        //                     mobilePhoneNumber = v.mobileNumber,
        //                     country = v.country,
        //                     code = v.code,
        //                     profilePicturePath = v.profileImage
        //                 }).FirstOrDefault();
        //    return vCard;
        //}
        public VCard createVCard(long userId, string firstName, string lastName, string company, string jobTitle, string email, string webPageAddress, string twitter, string businessPhoneNumber, string mobilePhone, string country, string city, string cityCode, string imageLink)
        {
            VCard newVCard = new VCard();
            newVCard.userId = userId;
            newVCard.firstName = firstName;
            newVCard.lastName = lastName;
            newVCard.company = company;
            newVCard.jobTitle = jobTitle;
            newVCard.email = email;
            newVCard.webPageAddress = webPageAddress;
            newVCard.twitter = twitter;
            newVCard.businessPhoneNumber = businessPhoneNumber;
            newVCard.mobileNumber = mobilePhone;
            newVCard.country = country;
            newVCard.city = city;
            newVCard.code = cityCode;
            newVCard.profileImage = imageLink;

            dbCon.tblVCards.Add(newVCard);
            dbCon.SaveChanges();

            long vCardId = newVCard.Id;

            VCard vcard = dbCon.tblVCards.FirstOrDefault(v => v.Id == vCardId);
            return vcard;

        }

        //public VCardModel GetVCard(int vCardId)
        //{
        //    var vCard = (from v in con.tblVCards
        //                 where v.Id == vCardId
        //                 select new VCardModel
        //                 {
        //                     id = v.Id,
        //                     UserId = v.userID,
        //                     firstName = v.firstName,
        //                     lastName = v.lastName,
        //                     company = v.company,
        //                     jobTitle = v.jobTitle,
        //                     fileAs = v.fileAs,
        //                     email = v.email,
        //                     webPageAddress = v.webPageAddress,
        //                     twitter = v.twitter,
        //                     businessPhoneNumber = v.businessPhoneNumber,
        //                     mobilePhoneNumber = v.mobileNumber,
        //                     country = v.country,
        //                     code = v.code,
        //                     profilePicturePath = v.profileImage
        //                 }).FirstOrDefault();
        //    if(vCard==null)
        //    {
        //        vCard = null;
        //    }
        //    return vCard;
        //}
        public VCard GetVCard(long vCardId)
        {
            VCard vcard = dbCon.tblVCards.FirstOrDefault(v => v.Id == vCardId);
            return vcard;
        }
        //public IEnumerable<VCardModel> GetUserVCards(int userId)
        //{
        //    var vCards = (from vc in con.tblVCards
        //                  where vc.userID == userId
        //                  select new VCardModel
        //                  {
        //                      id = vc.Id,
        //                      UserId = vc.userID,
        //                      profilePicturePath = vc.profileImage,
        //                      firstName = vc.firstName,
        //                      lastName = vc.lastName,
        //                      company = vc.company,
        //                      jobTitle = vc.jobTitle,
        //                      fileAs = vc.fileAs,
        //                      email = vc.email,
        //                      twitter = vc.twitter,
        //                      webPageAddress = vc.webPageAddress,
        //                      businessPhoneNumber = vc.businessPhoneNumber,
        //                      mobilePhoneNumber = vc.mobileNumber,
        //                      country = vc.country,
        //                      city = vc.city,
        //                      code = vc.code
        //                  }).ToList();
        //    return vCards;
        //}
        public IEnumerable<VCard> GetUserVCards(long userId)
        {
            IEnumerable<VCard> vcards = dbCon.tblVCards.ToList();
            return vcards;
        }

        //public void userCampaign(int userId, int campaignId)
        //{
        //    tblUserCampaign uc = new tblUserCampaign();

        //    uc.userId = userId;
        //    uc.campaignId = campaignId;

        //    con.tblUserCampaigns.InsertOnSubmit(uc);
        //    con.SubmitChanges();
        //}
        public void userCampaign(long userId, long campaignId)
        {
            UserCampaign usercampaign = new UserCampaign();
            usercampaign.campaignId = campaignId;
            usercampaign.userId = userId;

            dbCon.tblUserCampaigns.Add(usercampaign);
            dbCon.SaveChanges();
        }


        //public CampaingModel createCampain(int creatorId, string name, string thumbnail, string status = "public")
        //{
        //    tblCampaign newCampaign = new tblCampaign();
        //    newCampaign.creatorId = creatorId;
        //    newCampaign.name = name;
        //    newCampaign.thumbnail = thumbnail;
        //    newCampaign.status = status;
        //    con.tblCampaigns.InsertOnSubmit(newCampaign);
        //    con.SubmitChanges();

        //    int campaignId = newCampaign.Id;

        //    var newCampaignData = (from c in con.tblCampaigns
        //                           where c.Id == campaignId
        //                           select new CampaingModel
        //                           {
        //                               id = c.Id,
        //                               name = c.name,
        //                               status = c.status,
        //                               thumbnails = c.thumbnail
        //                           }).FirstOrDefault();

        //    return newCampaignData;
        //}
        public Campaign createCampaign(long creatorId, string name, string thumbnail, string status = "public")
        {
            Campaign newCampaign = new Campaign();
            newCampaign.creatorId = creatorId;
            newCampaign.name = name;
            newCampaign.thumbnail = thumbnail;
            newCampaign.status = status;
            dbCon.tblCampaigns.Add(newCampaign);
            dbCon.SaveChanges();

            long campaignId = newCampaign.Id;
            Campaign campaign = dbCon.tblCampaigns.SingleOrDefault(c => c.Id == campaignId);
            return campaign;
        }


        //public EmailModel createEmail(int campaignId, string subject, string HTML)
        //{
        //    tblEmail newEmail = new tblEmail();
        //    newEmail.campaignId = campaignId;
        //    newEmail.subject = subject;
        //    newEmail.html = HTML;

        //    con.tblEmails.InsertOnSubmit(newEmail);
        //    con.SubmitChanges();
        //    int emailId = newEmail.Id;

        //    var newEmailData = (from e in con.tblEmails
        //                        where e.Id == emailId
        //                        select new EmailModel
        //                        {
        //                            id = e.Id,
        //                            campaignId = e.campaignId,
        //                            subject = e.subject,
        //                            html = e.html
        //                        }).FirstOrDefault();
        //    return newEmailData;
        //}
        public Email createEmail(long campaignId, string subject, string HTML)
        {
            Email email = new Email();
            email.campaignId = campaignId;
            email.subject = subject;
            email.html = HTML;

            dbCon.tblEmails.Add(email);
            dbCon.SaveChanges();

            long emailId = email.Id;
            Email mail = dbCon.tblEmails.FirstOrDefault(em => em.Id == emailId);
            return mail;
        }

        //public AttachmentsModel createAttecment(int emailId, string name, string file/*Path*/)
        //{
        //    tblAttachment newAttechment = new tblAttachment();
        //    newAttechment.emailId = emailId;
        //    newAttechment.name = name;
        //    newAttechment.file = file;
        //    con.tblAttachments.InsertOnSubmit(newAttechment);
        //    con.SubmitChanges();
        //    int attID = newAttechment.Id;

        //    var newAttechmentData = (from a in con.tblAttachments
        //                             where a.Id == attID
        //                             select new AttachmentsModel
        //                             {
        //                                 id = a.Id,
        //                                 emailId = a.emailId,
        //                                 name = a.name,
        //                                 file = a.file
        //                             }).FirstOrDefault();
        //    return newAttechmentData;
        //}
        public Attachment createAttachment(long emailId, string name, string file/*Path*/)
        {
            Attachment newAttechment = new Attachment();
            newAttechment.emailId = emailId;
            newAttechment.name = name;
            newAttechment.file = file;
            dbCon.tblAttachments.Add(newAttechment);
            dbCon.SaveChanges();

            long attId = newAttechment.Id;
            Attachment attachment = dbCon.tblAttachments.SingleOrDefault(a => a.Id == attId);
            return attachment;
        }
        //public IEnumerable<AttachmentsModel> GetAttachmentByEmailId(int emailId)
        //{
        //    var attechments = (from a in con.tblAttachments
        //                       where a.emailId == emailId
        //                       select new AttachmentsModel
        //                       {
        //                           id = a.Id,
        //                           emailId = a.emailId,
        //                           name = a.name,
        //                           file = a.file
        //                       }).ToList();
        //    return attechments;
        //}
        public IEnumerable<Attachment> GetAttachmentByEmailId(long emailId)
        {
            Attachment[] atts = { dbCon.tblAttachments.FirstOrDefault(s => s.emailId == emailId) };

            //var attechments = (from a in dbCon.tblAttachments
            //                   where a.emailId == emailId
            //                   select new Attachment
            //                   {
            //                       Id = a.Id,
            //                       emailId = a.emailId,
            //                       name = a.name,
            //                       file = a.file
            //                   }).ToList();
            return atts.ToList();
        }

        public IEnumerable<Campaign> getAllCampaigns(long userId, string campName)
        {
            var campaigns = (from c in con.vUserCampaigns
                             where c.name.Contains(campName) && c.userId==userId
                             select new Campaign
                             {
                                 Id = c.Id,
                                 name = c.name,
                                 thumbnail = c.thumbnail,
                                 status = c.status
                             }).ToList();
            return campaigns;
        }

        public Email GetEmail(long campId)
        {
            var email = dbCon.tblEmails.FirstOrDefault(e => e.campaignId == campId);
            return email;
        }


        //public HistoryModel createHistory(int userId, int campaignId, string toEMail, int imageId = 0)
        //{
        //    tblHistory newHistory = new tblHistory();
        //    newHistory.userId = userId;
        //    newHistory.imageId = imageId;
        //    newHistory.campaignId = campaignId;
        //    newHistory.toEMail = toEMail;
        //    newHistory.sentDateTime = DateTime.Now;
        //    con.tblHistories.InsertOnSubmit(newHistory);
        //    con.SubmitChanges();
        //    int histID = newHistory.Id;

        //    var newHistoryData = (from h in con.tblHistories
        //                          where h.Id == histID
        //                          select new HistoryModel
        //                          {
        //                              id = h.Id,
        //                              userId = h.userId,
        //                              campaignId = h.campaignId,
        //                              sentDateTime = Convert.ToDateTime(h.sentDateTime),
        //                              toMail = h.toEMail
        //                          }).FirstOrDefault();
        //    return newHistoryData;
        //}
        public History createHistory(long userId, long campaignId, string toEMail, long imageId = 0)
        {
            History newHistory = new History();
            newHistory.userId = userId;
            newHistory.imageId = imageId;
            newHistory.campaignId = campaignId;
            newHistory.toEmail = toEMail;
            newHistory.sentDateTime = DateTime.Now;

            dbCon.tblHistory.Add(newHistory);
            dbCon.SaveChanges();

            long histId = newHistory.Id;
            History history = dbCon.tblHistory.FirstOrDefault(h => h.Id == histId);
            return history;
        }


    }
}
