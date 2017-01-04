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
        public async Task<CompleteCampaign> createCampaign(int creatorId, string campaignName, string thumbnail, string subject, string HTML, string fileName, string file, string status = "public")
        {
            var campaign = dbCon.createCampain(creatorId, campaignName, thumbnail, status);

            int campID = campaign.id;

            var email = dbCon.createEmail(campID, subject, HTML);
            int eID = email.id;
            var attechments = dbCon.createAttecment(eID, fileName, file);
            await SendEmail("nqobani@immedia.co.za", "nqobani.zulu15@gmail.com", "School");
            return new CompleteCampaign
            {
                campiagn = campaign,
                email = email,
                attechment = attechments
            };
        }



        public IEnumerable<CampaingModel> getCampaigns(string campName)
        {
            return dbCon.getAllCampaigns(campName);
        }
        public async Task SendEmail(string fromEmail, string toEmail, string subj, string HTML=null)
        {
            string apiKey = Environment.GetEnvironmentVariable("sendgrid_api_key", EnvironmentVariableTarget.User);
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email(fromEmail);
            string subject = subj;
            Email to = new Email(toEmail);
            SendGrid.Helpers.Mail.Content content = new SendGrid.Helpers.Mail.Content("text/html", "<!DOCTYPE html><html><body><h1>and easy to do anywhere, even with C#</h1></body></html>");
            Mail mail = new Mail(from, subject, to, content);

            VcardManager.LoadVCardData(dbCon.GetVCard(11));

            byte[] imageArray = System.IO.File.ReadAllBytes(@"C:\Users\User\Music\images\vCard.vcf");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            var att = new SendGrid.Helpers.Mail.Attachment
            {
                Filename = @"vCard.vcf",
                Type = "vcf",
                Disposition = "inline",
                ContentId = "kjhlknmnjhjkk",
                Content = base64ImageRepresentation
            };
            mail.AddAttachment(att);
            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}
