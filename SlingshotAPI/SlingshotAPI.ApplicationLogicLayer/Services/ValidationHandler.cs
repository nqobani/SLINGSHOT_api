using SlingshotAPI.ApplicationLogicLayer.Models;
using SlingshotAPI.Data;
using SlingshotAPI.Data.Entity_Framework;
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
        private ApplicationDbContext con = new ApplicationDbContext();

        public Boolean UserExist(long userId)
        {
            var userC = con.tblUsers.SingleOrDefault(s => s.Id == userId);
            Boolean userExists = false;
            if(userC.Id==userId)
            {
                userExists = true;
            }
            return userExists;
        }

        public Boolean UserCampaignValidation(long useId, long campId)
        {
            //var userCamp = (from uc in con.tblUserCampaigns
            //                where uc.userId == useId && uc.campaignId == campId
            //                select new UserCampaign {
            //                    userId=uc.userId,
            //                    campaignId=uc.campaignId
            //                }).FirstOrDefault();
            //Boolean hasAccess = false;

            //if(userCamp.campaignId==campId && useId== userCamp.userId)
            //{
            //    hasAccess = true;
            //}
            return true;
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


        public string GetUserType(long userId, long campaignId)
        {
            string userType;

            var user = (from u in con.tblUsers
                       where u.Id == userId
                       select new User {
                           type=u.type
                       }).FirstOrDefault();
            userType = user.type;
            return userType;
        }
        public Boolean IsCreator(long userId, long campaignId)
        {
            Boolean isCreator = false;
            var camp = (from c in con.tblCampaigns
                        where c.creatorId == userId && c.Id == campaignId
                        select new Campaign {
                            creatorId=c.creatorId,
                            Id=c.Id
                        }).FirstOrDefault();
            if(camp.Id==campaignId && camp.creatorId==userId)
            {
                isCreator = true;
            }
            return isCreator;
        }
        public Boolean CanUserShare(long userId, long campaignId)
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
