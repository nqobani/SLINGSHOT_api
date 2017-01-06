using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SlingshotAPI.Data.Models;

namespace SlingshotAPI.Data
{


    public class DbConnection
    {
        private DataClasses1DataContext con = new DataClasses1DataContext();


        public UserModel createUser(string email, string password)
        {
            var userC = (from user in con.tblUsers
                         where user.email == email
                         select new UserModel
                         {
                             id = user.Id,
                             email = user.email,
                             password = user.password
                         }).ToList();

            if (userC.Count > 0)
            {
                throw new ErrorMessage
                {
                    message = "Registration Failed",
                    course = "User with the same email addres already exists"
                };
                //User with the same email address already exist, try another one or sign in
            }
            else
            {
                tblUser newUser = new tblUser();
                newUser.email = email;
                newUser.password = password;

                con.tblUsers.InsertOnSubmit(newUser);
                con.SubmitChanges();
                int ids = newUser.Id;

                var newUsers = (from user in con.tblUsers
                                where user.Id == ids
                                select new UserModel
                                {
                                    id = user.Id,
                                    email = user.email,
                                    password = user.password
                                }).FirstOrDefault();
                return newUsers;
            }

        }
        public string GetUserEmail(int userId)
        {
            var user = (from u in con.tblUsers
                        where u.Id == userId
                        select new UserModel
                        {
                            email = u.email
                        }).FirstOrDefault();
            return user.email;
        }

        public VCardModel CreateVCard(int userId, string firstName, string lastName, string company, string jobTitle, string email, string webPageAddress, string twitter, string businessPhoneNumber, string mobilePhone, string country, string city, string cityCode, string imageLink)
        {
            tblVCard newVCard = new tblVCard();
            newVCard.userID = userId;
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

            con.tblVCards.InsertOnSubmit(newVCard);
            con.SubmitChanges();

            int vCardId = newVCard.Id;

            var vCard = (from v in con.tblVCards
                         where v.Id == vCardId
                         select new VCardModel
                         {
                             id = v.Id,
                             UserId = v.userID,
                             firstName = v.firstName,
                             lastName = v.lastName,
                             company = v.company,
                             jobTitle = v.jobTitle,
                             fileAs = v.fileAs,
                             email = v.email,
                             webPageAddress = v.webPageAddress,
                             twitter = v.twitter,
                             businessPhoneNumber = v.businessPhoneNumber,
                             mobilePhoneNumber = v.mobileNumber,
                             country = v.country,
                             code = v.code,
                             profilePicturePath = v.profileImage
                         }).FirstOrDefault();
            return vCard;
        }
        public VCardModel GetVCard(int vCardId)
        {
            var vCard = (from v in con.tblVCards
                         where v.Id == vCardId
                         select new VCardModel
                         {
                             id = v.Id,
                             UserId = v.userID,
                             firstName = v.firstName,
                             lastName = v.lastName,
                             company = v.company,
                             jobTitle = v.jobTitle,
                             fileAs = v.fileAs,
                             email = v.email,
                             webPageAddress = v.webPageAddress,
                             twitter = v.twitter,
                             businessPhoneNumber = v.businessPhoneNumber,
                             mobilePhoneNumber = v.mobileNumber,
                             country = v.country,
                             code = v.code,
                             profilePicturePath = v.profileImage
                         }).FirstOrDefault();
            return vCard;
        }
        public IEnumerable<VCardModel> GetUserVCards(int userId)
        {
            var vCards = (from vc in con.tblVCards
                          where vc.userID == userId
                          select new VCardModel
                          {
                              id = vc.Id,
                              UserId = vc.userID,
                              profilePicturePath = vc.profileImage,
                              firstName = vc.firstName,
                              lastName = vc.lastName,
                              company = vc.company,
                              jobTitle = vc.jobTitle,
                              fileAs = vc.fileAs,
                              email = vc.email,
                              twitter = vc.twitter,
                              webPageAddress = vc.webPageAddress,
                              businessPhoneNumber = vc.businessPhoneNumber,
                              mobilePhoneNumber = vc.mobileNumber,
                              country = vc.country,
                              city = vc.city,
                              code = vc.code
                          }).ToList();
            return vCards;
        }

        public void userCampaign(int userId, int campaignId)
        {
            tblUserCampaign uc = new tblUserCampaign();

            uc.userId = userId;
            uc.campaignId = campaignId;

            con.tblUserCampaigns.InsertOnSubmit(uc);
            con.SubmitChanges();
        }


        public CampaingModel createCampain(int creatorId, string name, string thumbnail, string status = "public")
        {
            tblCampaign newCampaign = new tblCampaign();
            newCampaign.creatorId = creatorId;
            newCampaign.name = name;
            newCampaign.thumbnail = thumbnail;
            newCampaign.status = status;
            con.tblCampaigns.InsertOnSubmit(newCampaign);
            con.SubmitChanges();

            int campaignId = newCampaign.Id;

            var newCampaignData = (from c in con.tblCampaigns
                                   where c.Id == campaignId
                                   select new CampaingModel
                                   {
                                       id = c.Id,
                                       name = c.name,
                                       status = c.status,
                                       thumbnails = c.thumbnail
                                   }).FirstOrDefault();

            return newCampaignData;
        }
        public EmailModel createEmail(int campaignId, string subject, string HTML)
        {
            tblEmail newEmail = new tblEmail();
            newEmail.campaignId = campaignId;
            newEmail.subject = subject;
            newEmail.html = HTML;

            con.tblEmails.InsertOnSubmit(newEmail);
            con.SubmitChanges();
            int emailId = newEmail.Id;

            var newEmailData = (from e in con.tblEmails
                                where e.Id == emailId
                                select new EmailModel
                                {
                                    id = e.Id,
                                    campaignId = e.campaignId,
                                    subject = e.subject,
                                    html = e.html
                                }).FirstOrDefault();
            return newEmailData;
        }
        public AttechmentsModel createAttecment(int emailId, string name, string file/*Path*/)
        {
            tblAttachment newAttechment = new tblAttachment();
            newAttechment.emailId = emailId;
            newAttechment.name = name;
            newAttechment.file = file;
            con.tblAttachments.InsertOnSubmit(newAttechment);
            con.SubmitChanges();
            int attID = newAttechment.Id;

            var newAttechmentData = (from a in con.tblAttachments
                                     where a.Id == attID
                                     select new AttechmentsModel
                                     {
                                         id = a.Id,
                                         emailId = a.emailId,
                                         name = a.name,
                                         file = a.file
                                     }).FirstOrDefault();
            return newAttechmentData;
        }


        public IEnumerable<CampaingModel> getAllCampaigns(int userId, string campName)
        {
            var campaigns = (from c in con.vUserCampaigns
                             where c.name.Contains(campName)
                             select new CampaingModel
                             {
                                 id = c.Id,
                                 name = c.name,
                                 thumbnails = c.thumbnail,
                                 status = c.status
                             }).ToList();
            return campaigns;
        }
        public EmailModel GetEmail(int capmId)
        {
            var email = (from e in con.tblEmails
                         where e.campaignId == capmId
                         select new EmailModel
                         {
                             id = e.Id,
                             campaignId = e.campaignId,
                             subject = e.subject,
                             html = e.html
                         }).FirstOrDefault();
            return email;
        }


        public HistoryModel createHistory(int userId, int campaignId, string toEMail, int imageId = 0)
        {
            tblHistory newHistory = new tblHistory();
            newHistory.userId = userId;
            newHistory.imageId = imageId;
            newHistory.campaignId = campaignId;
            newHistory.toEMail = toEMail;
            newHistory.sentDateTime = DateTime.Now;
            con.tblHistories.InsertOnSubmit(newHistory);
            con.SubmitChanges();
            int histID = newHistory.Id;

            var newHistoryData = (from h in con.tblHistories
                                  where h.Id == histID
                                  select new HistoryModel
                                  {
                                      id = h.Id,
                                      userId = h.userId,
                                      campaignId = h.campaignId,
                                      sentDateTime = Convert.ToDateTime(h.sentDateTime),
                                      toMail = h.toEMail
                                  }).FirstOrDefault();
            return newHistoryData;
        }


    }
}
