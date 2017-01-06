using SlingshotAPI.ApplicationLogicLayer.Services;
using SlingshotAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;


namespace SlingshotAPI.Controllers
{
    [RoutePrefix("api/campaign")]
    public class CampaignController : ApiController
    {
        UserService obj = new UserService();
        [Route("send")]
        public HistoryModel sendCampaigns(int userId, int campId, string toEmail)
        {
            return obj.sendCampaign(userId, campId, toEmail);
        }
        [Route("add")]
        public CompleteCampaign addCampaign(int creatorId, string campaignName = "No Name", string thumbnail = "HTTPS", string subject = "TESTIING", string HTML = "<!DOCTYPE html>", string fileName = "jack.png", string file = @"dfjhkd\dgjh\dfjhghs\jack.png", string status = "public")
        {
            UserService obj = new UserService();

            return obj.createCampaign(creatorId, campaignName, thumbnail, subject, HTML, fileName, file, status);
        }
        [Route("get")]
        public IEnumerable<CampaingModel> getCampaigns(int userId, string name = "")
        {
            UserService obj = new UserService();
            return obj.getCampaigns(userId, name);
        }
        [Route("uploadImage")]
        public void uploadImage()
        {
            Directory.CreateDirectory(@"C:\Users\User\Music\images");
            string sourceFile = Path.Combine(@"C:\Users\User\Music\", "banner.jpg");
            string destFile = Path.Combine(@"C:\Users\User\Music\images\", "banner.jpg");
            File.Copy(sourceFile, destFile, true);
        }
    }
}
