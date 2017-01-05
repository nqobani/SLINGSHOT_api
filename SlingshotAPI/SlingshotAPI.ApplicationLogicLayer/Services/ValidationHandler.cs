using SlingshotAPI.ApplicationLogicLayer.Models;
using SlingshotAPI.Data;
using SlingshotAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlingshotAPI.ApplicationLogicLayer.Services
{
    public class ValidationHandler
    {
        private DataClasses1DataContext con = new DataClasses1DataContext();

        public Boolean UserExist(int userId)
        {
            var userC = (from user in con.tblUsers
                         where user.Id == userId
                         select new UserModel
                         {
                             id = user.Id,
                             email = user.email,
                             password = user.password
                         }).FirstOrDefault();
            Boolean userExists = false;
            if(userC.id==userId)
            {
                userExists = true;
            }
            return userExists;
        }

        public Boolean UserCampaignValidation(int useId, int campId)
        {
            var userCamp = (from uc in con.tblUserCampaigns
                            where uc.userId == useId && uc.campaignId == campId
                            select new UserCampaign {
                                userId=uc.userId,
                                campaignId=uc.campaignId
                            }).FirstOrDefault();
            Boolean hasAccess = false;

            if(userCamp.campaignId==campId && useId== userCamp.userId)
            {
                hasAccess = true;
            }
            return hasAccess;
        }
        public Attechment GetAttechmentData(string filePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(@"C:\Users\User\Music\images\vCard.vcf");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            string fileName= filePath.Substring(filePath.LastIndexOf('\\') + 1);
            string type = filePath.Substring(filePath.LastIndexOf('.') + 1);

            return new Attechment {
                Filename=fileName,
                Type=type,
                Disposition = "inline",
                ContentId = "kjhlknmnjhjkk",
                Content = base64ImageRepresentation
            };
        }
    }

    
}
