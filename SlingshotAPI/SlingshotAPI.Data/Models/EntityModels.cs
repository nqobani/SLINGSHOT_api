using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.Data.Models
{
    public class  ErrorMessage : Exception
    {
        public string message { set; get; }
        public string course { get; set; }
    }
    public class UserModel
    {
        public int id { set; get; }
        public string email { set; get; }
        public string password { set; get; }
        public string type { set; get; }
    }

    public class CompleteCampaign
    {
        public CampaingModel campiagn { get; set; }
        public EmailModel email { get; set; }
        public IEnumerable<AttachmentsModel> attechment { get; set; }
    }

    public class CampaingModel
    {
        public int id { set; get; }
        public string name { set; get; }
        public string thumbnails { set; get; }
        public string status { set; get; }
        public int creatorId { set; get; }
    }
    public class EmailModel
    {
        public int id { set; get; }
        public int campaignId { get; set; }
        public string subject { set; get; }
        public string html { get; set; }
    }
    public class AttachmentsModel
    {
        public int id { set; get; }
        public int emailId { set; get; }
        public string name { get; set; }
        public string file { get; set; }
    }
    public class AttachmentUserLevelModel
    {
        public string name { get; set; }
        public string filePath { get; set; }
    }


    public class VCardModel
    {
        public int id { set; get; }
        public int UserId { set; get; }
        public string profilePicturePath { get; set; }
        public string firstName { set; get; }
        public string lastName { get; set; }
        public string company { get; set; }
        public string jobTitle { get; set; }
        public string fileAs { get; set; }
        public string email { get; set; }
        public string twitter { get; set; }
        public string webPageAddress { set; get; }
        public string businessPhoneNumber { set; get; }
        public string mobilePhoneNumber { set; get; }
        public string country { set; get; }
        public string city { set; get; }
        public string code { set; get; }
    }
    public class ClientVCardModel
    {
        public int id { set; get; }
        public int UserId { set; get; }
        public int clientId { set; get; }
        public string profilePicturePath { get; set; }
        public string firstName { set; get; }
        public string lastName { get; set; }
        public string company { get; set; }
        public string jobTitle { get; set; }
        public string fileAs { get; set; }
        public string email { get; set; }
        public string twitter { get; set; }
        public string webPageAddress { set; get; }
        public string businessPhoneNumber { set; get; }
        public string mobilePhoneNumber { set; get; }
        public string country { set; get; }
        public string city { set; get; }
        public int code { set; get; }
    }
    public class EventModel
    {
        public int id { get; set; }
        public string title { set; get; }
        public DateTime startDateTime { set; get; }
        public DateTime endDateTime { set; get; }
    }
    public class HistoryModel
    {
        public int id { get; set; }
        public int userId { set; get; }
        public int imageId { set; get; }
        public int campaignId { set; get; }
        public DateTime sentDateTime { set; get; }
        public string toMail { set; get; }
    }
    public class UserCampaign
    {
        public int userId { set; get; }
        public int campaignId { set; get; }
    }

}
