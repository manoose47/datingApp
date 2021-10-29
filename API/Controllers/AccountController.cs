using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext __dataContext;
        public AccountController(DataContext _dataContext)
        {
            __dataContext = _dataContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var appUser = new AppUser()
                {
                    UserName = username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                    PasswordSalt = hmac.Key
                };

                __dataContext.Add(appUser);
                await __dataContext.SaveChangesAsync();

                return appUser;

            }
        }

    }
}