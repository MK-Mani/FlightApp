using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LoginAPIService.Models
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLUserRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public bool RegisterUser(User userDetail)
        {            
            if (ValidateEmailId(userDetail.UserId))
            {
                CreatePasswordHash(userDetail.Password, out byte[] passwordHash, out byte[] passwordSalt);

                userDetail.PasswordHash = passwordHash;
                userDetail.PasswordSalt = passwordSalt;
                userDetail.Password = "";

                _dbContext.User.Add(userDetail);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public User ValidateUser(User user)
        {
            var users = _dbContext.User;
            
            if (users.Count() > 0)
            {
                var getUser = users.Where(item => item.UserId == user.UserId).FirstOrDefault();

                if(getUser != null)
                {
                    if (VerifyPasswordHash(user.Password, getUser.PasswordHash, getUser.PasswordSalt))
                    {
                        getUser.PasswordHash = null;
                        getUser.PasswordSalt = null;
                        return getUser;
                    }
                }
            }

            return null;
        }

        public User UpdateLastName(User userDetails)
        {
            var user = _dbContext.User.Find(userDetails.UserRecId);

            if(user != null)
            {
                user.LastName = userDetails.LastName;
                _dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
            }

            return user;
        }

        private bool ValidateEmailId(string emailId)
        {
            var user = _dbContext.User.Where(item => item.UserId == emailId).FirstOrDefault();

            return user == null || string.IsNullOrEmpty(user.UserId);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash =  hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
