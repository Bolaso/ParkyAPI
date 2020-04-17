using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly AppSettings _appSettings;


        public UserRepository(ApplicationDBContext applicationDBContext, IOptions<AppSettings> appsettings)
        {
            _applicationDBContext = applicationDBContext;
            _appSettings = appsettings.Value;
        }
        public User Authenticate(string username, string password)
        {
            var user = _applicationDBContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            //user not found
            if (user == null)
            {
                return null;
            }

            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

            var token = tokenHandler.CreateToken(tokenDescription);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = ""; //Don't send password back to front

            return user;
        
        }



        public bool IsUniqueUser(string username)
        {
            var user = _applicationDBContext.Users.SingleOrDefault(x=>x.Username == username);

            //return false if user not found
            if (user == null)
                return true;

            return false;

        }

        public User Register(string username, string password)
        {
            User user = new User()
            {
                Username = username,
                Password = password,
                Role = "Admin"
            };

            _applicationDBContext.Users.Add(user);
            _applicationDBContext.SaveChanges();

            user.Password = "";
            return user;
        }
    }
}
