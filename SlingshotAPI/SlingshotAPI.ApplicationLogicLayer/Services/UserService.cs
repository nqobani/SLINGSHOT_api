using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlingshotAPI.Data;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using SlingshotAPI.Data.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using SlingshotAPI.ApplicationLogicLayer.Services;
using Newtonsoft.Json;
using System.IO;

namespace SlingshotAPI.ApplicationLogicLayer.Services
{
    public class UserService
    {
        DbConnection dbCon = new DbConnection();
        ValidationHandler _validationHandler = new ValidationHandler();
        ///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION
        public User createUser(string email, string password, string type)
        {
            try
            {
                return dbCon.createUser(email, password, type);
            }
            catch (ErrorMessage c)
            {
                throw c;
            }

        }
        public IEnumerable<SlingshotAPI.Data.Models.VCard> GetUserVCards(long userID)
        {
            SlingshotAPI.Data.Models.VCard[] _vCard = new SlingshotAPI.Data.Models.VCard[1];
            if (_validationHandler.UserExist(userID))
            {
                return dbCon.GetUserVCards(userID);
            }
            else
            {
                /*Needs Attention*/
                _vCard[0] = new SlingshotAPI.Data.Models.VCard { };
                return _vCard;
            }
        }
        public SlingshotAPI.Data.Models.VCard CreateVCard(long userId, string firstName, string lastName, string company, string jobTitle, string email, string webPageAddress, string twitter, string businessPhoneNumber, string mobilePhone, string country, string city, string cityCode, string imageLink)
        {
            Boolean userExists= _validationHandler.UserExist(userId);
            if(userExists)
            {
                return dbCon.createVCard( userId, firstName, lastName, company, jobTitle, email, webPageAddress, twitter, businessPhoneNumber, mobilePhone, country, city, cityCode, imageLink);
               // return dbCon.CreateVCard(userId, firstName, lastName, company, jobTitle, email, webPageAddress, twitter, businessPhoneNumber, mobilePhone, country, city, cityCode, imageLink);
            }
            else
            {
                return new SlingshotAPI.Data.Models.VCard { };
            }
        }

        ///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION
        public CompleteCampaign createCampaign(long creatorId, string campaignName, string thumbnail, string subject, string HTML, string attechmentsJSONString, string status = "public")
        {
            
            var campaign = dbCon.createCampaign(creatorId, campaignName, thumbnail, subject);

            long campID = campaign.Id;

            dbCon.userCampaign(creatorId, campID);

            var email = dbCon.createEmail(campID, subject, HTML);
            long eID = email.Id;

            var attechmentObjs = JsonConvert.DeserializeObject<List<AttachmentUserLevelModel>>(attechmentsJSONString);

            for (int i = 0; i < attechmentObjs.Count; i++)
            {

                string fileName = Path.GetFileName(attechmentObjs[i].filePath);
                string destinationFilePath = Path.Combine(Directory.GetCurrentDirectory() + @"\attachment", fileName);

                System.IO.File.Copy(attechmentObjs[i].filePath, destinationFilePath, true);

                dbCon.createAttachment(eID, attechmentObjs[i].name, attechmentObjs[i].filePath);
            }

            return new CompleteCampaign
            {
                campiagn = campaign,
            };
        }
        public Boolean ShareCampaigns(long userId, long campId)
        {
            Boolean shared = false;
            if (_validationHandler.CanUserShare(userId, campId))
            {
                dbCon.userCampaign(userId, campId);
                shared = true;
            }
            return shared;
        }


        public History sendCampaign(long userId, long vcardId, long campId, string toEmail)
        {
            Boolean hasAccess = _validationHandler.UserCampaignValidation(userId, campId);
            if (hasAccess)
            {
                string fromEmail = dbCon.GetUserEmail(userId);
                Data.Models.Email email = dbCon.GetEmail(campId);

                string subject = email.subject;
                string html = email.html;

                Data.Models.Attachment[] attechments= dbCon.GetAttachmentByEmailId(email.Id).ToArray();

                SendEmail(fromEmail, toEmail, subject, vcardId, html, attechments).Wait();

                return dbCon.createHistory(userId, campId, toEmail, 0);
            }
            else
            {
                return new History { };
            }
        }
        public IEnumerable<Campaign> getCampaigns(long userId, string campName)
        {
            return dbCon.getAllCampaigns(userId, campName);
        }
        public async Task SendEmail(string fromEmail, string toEmail, string subj, long vCardId, string HTML, Data.Models.Attachment[] emailAttechments)
        {
            string apiKey = Environment.GetEnvironmentVariable("sendgrid_api_key", EnvironmentVariableTarget.User);
            dynamic sg = new SendGridAPIClient(apiKey);

            SendGrid.Helpers.Mail.Email from = new SendGrid.Helpers.Mail.Email(fromEmail);
            string subject = subj;
            SendGrid.Helpers.Mail.Email to = new SendGrid.Helpers.Mail.Email(toEmail);
            SendGrid.Helpers.Mail.Content content = new SendGrid.Helpers.Mail.Content("text/html", HTML);
            Mail mail = new Mail(from, subject, to, content);

            Boolean hasVCard = VcardManager.LoadVCardData(dbCon.GetVCard(vCardId));

            if(hasVCard)
            {
                string vCardPath = Path.Combine(Directory.GetCurrentDirectory() + @"\vCard", "vCard.vcf");
                var attachment = _validationHandler.GetAttechmentData(vCardPath);
                var att = new SendGrid.Helpers.Mail.Attachment
                {
                    Filename = attachment.Filename,
                    Type = attachment.Type,
                    Disposition = attachment.Disposition,
                    ContentId = "kjhlknmnjhjkk",
                    Content = attachment.Content
                };


                mail.AddAttachment(att);
            }
            
            for (int i = 0; i < emailAttechments.Length; i++)
            {
                var eAttechment = _validationHandler.GetAttechmentData(emailAttechments[i].file);
                eAttechment.Filename = emailAttechments[i].name;

                var eAtt = new SendGrid.Helpers.Mail.Attachment
                {
                    Filename = eAttechment.Filename,
                    Type = eAttechment.Type,
                    Disposition = eAttechment.Disposition,
                    ContentId = "kjhlknmnjhjkk",
                    Content = eAttechment.Content
                };

                mail.AddAttachment(eAtt);
            }

            
            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}
