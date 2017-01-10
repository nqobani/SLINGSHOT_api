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
        public UserModel createUser(string email, string password, string type)
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
        public IEnumerable<VCardModel> GetUserVCards(int userID)
        {
            VCardModel[] _vCardModel = new VCardModel[1];
            if (_validationHandler.UserExist(userID))
            {
                return dbCon.GetUserVCards(userID);
            }
            else
            {
                /*Needs Attention*/
                _vCardModel[0] = new VCardModel { };
                return _vCardModel;
            }
        }

        ///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION
        public CompleteCampaign createCampaign(int creatorId, string campaignName, string thumbnail, string subject, string HTML, string attechmentsJSONString, string status = "public")
        {
            var campaign = dbCon.createCampain(creatorId, campaignName, thumbnail, status);

            int campID = campaign.id;

            dbCon.userCampaign(creatorId, campID);

            var email = dbCon.createEmail(campID, subject, HTML);
            int eID = email.id;

            var attechmentObjs = JsonConvert.DeserializeObject<List<AttachmentUserLevelModel>>(attechmentsJSONString);

            for (int i = 0; i < attechmentObjs.Count; i++)
            {

                string fileName = Path.GetFileName(attechmentObjs[i].filePath);
                string destinationFilePath = Path.Combine(Directory.GetCurrentDirectory() + @"\attachment", fileName);

                System.IO.File.Copy(attechmentObjs[i].filePath, destinationFilePath, true);

                dbCon.createAttecment(eID, attechmentObjs[i].name, attechmentObjs[i].filePath);
            }
            


            return new CompleteCampaign
            {
                campiagn = campaign,
                email = email,
                attechment = dbCon.GetAttachmentByEmailId(eID)
            };
        }
        public Boolean ShareCampaigns(int userId, int campId)
        {
            Boolean shared = false;
            if (_validationHandler.CanUserShare(userId, campId))
            {
                dbCon.userCampaign(userId, campId);
                shared = true;
            }
            return shared;
        }


        public HistoryModel sendCampaign(int userId,int vcardId, int campId, string toEmail)
        {
            Boolean hasAccess = _validationHandler.UserCampaignValidation(userId, campId);
            if (hasAccess)
            {
                string fromEmail = dbCon.GetUserEmail(userId);
                EmailModel email = dbCon.GetEmail(campId);

                string subject = email.subject;
                string html = email.html;

                AttachmentsModel[] attechments= dbCon.GetAttachmentByEmailId(email.id).ToArray();

                SendEmail(fromEmail, toEmail, subject, vcardId, html, attechments).Wait();

                return dbCon.createHistory(userId, campId, toEmail, 0);
            }
            else
            {
                return new HistoryModel { };
            }
        }
        public IEnumerable<CampaingModel> getCampaigns(int userId, string campName)
        {
            return dbCon.getAllCampaigns(userId, campName);
        }
        public async Task SendEmail(string fromEmail, string toEmail, string subj, int vCardId, string HTML, AttachmentsModel[] emailAttechments)
        {
            string apiKey = Environment.GetEnvironmentVariable("sendgrid_api_key", EnvironmentVariableTarget.User);
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email(fromEmail);
            string subject = subj;
            Email to = new Email(toEmail);
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
