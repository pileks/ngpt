using Augur.Entity.Entities;
using Google.Apis.Auth;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngpt.Repositories.Users
{
    public class UsersRepository
    {
        private readonly RootDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly string clientId;

        public UsersRepository(RootDbContext dbContext, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.clientId = configuration.GetSection("Google")["client_id"];
        }

        public bool AreCredentialsValid(string email, string password, out User user)
        {
            user = this.GetByEmail(email);
            if (user == null)
            {
                return false;
            }

            var isValid = !string.IsNullOrWhiteSpace(password) &&
                this.passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success;

            if (!isValid)
            {
                user = null;
            }

            return isValid;
        }

        private User GetByEmail(string email)
        {
            return this.dbContext.Set<User>()
                .Include(u => u.AccountInfo)
                .SingleOrDefault(u => u.Email == email);
        }

        public async Task<string> ValidateToken(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { this.clientId }
            };

            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch (Exception)
            {
                return null;
            }

            return payload.Subject;
        }

        public AugurUser AddAndHashPassword(User entity)
        {
            entity.Password = this.passwordHasher.HashPassword(entity, entity.Password);
            entity.IsActive = true;

            this.dbContext.Set<User>().Add(entity);

            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.dbContext.SaveChangesAsync();
        }
    }
}
