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


namespace SlingshotAPI.ApplicationLogicLayer.Services
{
    public class UserService
    {
        DbConnection dbCon = new DbConnection();
        ValidationHandler _validationHandler = new ValidationHandler();
        ///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION///USER SECTION
        public UserModel createUser(string email, string password)
        {
            try
            {
                return dbCon.createUser(email, password);
            }
            catch (ErrorMessage c)
            {
                throw c;
            }

        }
        public IEnumerable<VCardModel> GetUserVCards(int userID)
        {
            VCardModel[] _vCardModel= new VCardModel[1];
            if (/*_validationHandler.UserExist(userID)*/true)
            {
                return GetUserVCards(userID);
            }
            else
            {
                /*Needs Attention*/
               _vCardModel[0] = new VCardModel { };
                return _vCardModel;
            }
        }

        ///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION///CAMPAIGN SECTION
        public CompleteCampaign createCampaign(int creatorId, string campaignName, string thumbnail, string subject, string HTML, string fileName, string file, string status = "public")
        {
            var campaign = dbCon.createCampain(creatorId, campaignName, thumbnail, status);

            int campID = campaign.id;

            var email = dbCon.createEmail(campID, subject, HTML);
            int eID = email.id;
            var attechments = dbCon.createAttecment(eID, fileName, file);
            return new CompleteCampaign
            {
                campiagn = campaign,
                email = email,
                attechment = attechments
            };
        }


        public HistoryModel sendCampaign(int userId, int campId, string toEmail)
        {
            Boolean hasAccess = _validationHandler.UserCampaignValidation(userId, campId);
            if (hasAccess)
            {
                string fromEmail = dbCon.GetUserEmail(userId);
                EmailModel email = dbCon.GetEmail(campId);

                string subject = email.subject;
                string html = email.html;

                SendEmail(fromEmail, toEmail, subject, 11, "<!DOCTYPE html><html><body><h1>and easy to do anywhere, even with C#<img src='https://s-media-cache-ak0.pinimg.com/originals/6a/0b/b7/6a0bb733cbbbf05cbe53d024e09e8816.jpg' </h1></body></html>").Wait();
                return dbCon.createHistory(userId, campId, toEmail, 0);
            }
            else
            {
                return new HistoryModel { };
            }
        }
        public IEnumerable<CampaingModel> getCampaigns(string campName)
        {
            return dbCon.getAllCampaigns(campName);
        }
        public async Task SendEmail(string fromEmail, string toEmail, string subj, int vCardId,string HTML)
        {
            string apiKey = Environment.GetEnvironmentVariable("sendgrid_api_key", EnvironmentVariableTarget.User);
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email(fromEmail);
            string subject = subj;
            Email to = new Email(toEmail);
            SendGrid.Helpers.Mail.Content content = new SendGrid.Helpers.Mail.Content("text/html", HTML);
            Mail mail = new Mail(from, subject, to, content);

            VcardManager.LoadVCardData(dbCon.GetVCard(vCardId));

            var attachment = _validationHandler.GetAttechmentData(@"C:\Users\User\Music\images\vCard.vcf");
            var att = new SendGrid.Helpers.Mail.Attachment
            {
                Filename = attachment.Filename,
                Type = attachment.Type,
                Disposition = attachment.Disposition,
                ContentId = "kjhlknmnjhjkk",
                Content = attachment.Content
            };
            mail.AddAttachment(att);
            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}
