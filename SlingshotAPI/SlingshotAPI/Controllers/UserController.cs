using SlingshotAPI.ApplicationLogicLayer.Services;
using SlingshotAPI.Data.Entity_Framework;
using SlingshotAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SlingshotAPI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        UserService obj = new UserService();

        [Route("registerUser")]
        public User register(string email, string password, string type="member")
        {

            try
            {
                return obj.createUser(email, password, type);
            }
            catch (ErrorMessage c)
            {
                throw c;
            }

        }

        //[Route("getvcards")]
        //public IEnumerable<Data.Models.VCard> GetUserVCards(long userId)
        //{
        //    return obj.GetUserVCards(userId);
        //}

        [Route("createVCard")]
        public Data.Models.VCard createVCard(long userId, string firstName, string lastName, string company, string jobTitle, string email, string webPageAddress, string twitter, string businessPhoneNumber, string mobilePhone, string country, string city, string cityCode, string imageLink)
        {
            return obj.CreateVCard(userId, firstName, lastName, company, jobTitle, email, webPageAddress, twitter, businessPhoneNumber, mobilePhone, country, city, cityCode, imageLink);
        }

    }
}
