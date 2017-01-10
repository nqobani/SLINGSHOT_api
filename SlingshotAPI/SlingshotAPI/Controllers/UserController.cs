using SlingshotAPI.ApplicationLogicLayer.Services;
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
        public UserModel register(string email, string password, string type="member")
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

        [Route("getvcards")]
        public IEnumerable<VCardModel> GetUserVCards(int userId)
        {
            return obj.GetUserVCards(userId);
        }
    }
}
