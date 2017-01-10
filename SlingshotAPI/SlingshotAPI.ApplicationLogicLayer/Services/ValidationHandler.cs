using SlingshotAPI.ApplicationLogicLayer.Models;
using SlingshotAPI.Data;
using SlingshotAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
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


        public string GetUserType(int userId, int campaignId)
        {
            string userType;

            var user = (from u in con.tblUsers
                       where u.Id == userId
                       select new UserModel {
                           type=u.type
                       }).FirstOrDefault();
            userType = user.type;
            return userType;
        }
        public Boolean IsCreator(int userId, int campaignId)
        {
            Boolean isCreator = false;
            var camp = (from c in con.tblCampaigns
                        where c.creatorId == userId && c.Id == campaignId
                        select new CampaingModel {
                            creatorId=c.creatorId,
                            id=c.Id
                        }).FirstOrDefault();
            if(camp.id==campaignId && camp.creatorId==userId)
            {
                isCreator = true;
            }
            return isCreator;
        }
        public Boolean CanUserShare(int userId, int campaignId)
        {
            Boolean share = false;
            string userType = GetUserType(userId, campaignId);
            if(userType.ToLower().Equals("admin"))
            {
                share = true;
            }
            else
            {
                share = IsCreator(userId, campaignId);
            }
            return share;
        }
    }
}
