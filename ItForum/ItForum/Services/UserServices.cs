﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ItForum.Controllers.DTO.UserController;
using ItForum.Models;
using ItForum.Services.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ItForum.Services
{
    public class UserServices
    {
        private readonly IMapper _mapper;
        private readonly DataContext _db;
        private readonly JwtServices _jwt;

        public UserServices(DataContext dataContext, JwtServices jwt, IMapper mapper)
        {
            _db = dataContext;
            _jwt = jwt;
            _mapper = mapper;
        }

        public async Task<User> RegisterUser(User user, IEnumerable<Claim> claims, bool IsVerified = false)
        {
            if (!IsCorrectInfor(user))
                return null;

            if (await GetUserByUserName(user.Username) != null)
                return null;

            user.IsVerified = IsVerified;
            await _db.Users.AddAsync(user);
            var jobs = new List<Task>();

            foreach (var claim in claims)
                jobs.Add(_db.UserClaims.AddAsync(new UserClaim
                {
                    UserId = user.UserId,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                }));

            await Task.WhenAll(jobs);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUserName(string username)
        {
            var userInDb = await _db.Users.FirstOrDefaultAsync(model => model.Username == username);

            return userInDb;
        }

        public async Task<User> Login(string username, string passwordHash)
        {
            var innerUser =
                await _db.Users.FirstOrDefaultAsync(
                    model => model.Username == username && model.PasswordHash == passwordHash);
            return innerUser;
        }

        public static bool IsCorrectInfor(User user)
        {
            if (string.IsNullOrEmpty(user.Username)
                || string.IsNullOrEmpty(user.PasswordHash)
                || string.IsNullOrEmpty(user.FullName)
                || string.IsNullOrEmpty(user.Faculty)
                || string.IsNullOrEmpty(user.Email)
                || string.IsNullOrEmpty(user.Phone))
                return false;
            return true;
        }

        public async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>();

            await _db.Entry(user).Collection(u => u.UserClaims).LoadAsync();
            claims.AddRange(user.UserClaims.Select(uc => uc.ToClaim()));

            claims.Add(new Claim(ClaimTypes.Name, user.UserId));
            return claims;
        }

        public async Task<string> GenerateJwt(User user)
        {
            var claims = await GetClaimsAsync(user);

            var jwtToken = new JwtSecurityToken(
                _jwt.Issuer,
                _jwt.Audience,
                claims,
                _jwt.NotBefore,
                _jwt.Expiration,
                _jwt.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return encodedJwt;
        }

        public async Task<object> GetUserProfile(string userId)
        {
            await Task.Yield();
            var user = await _db.Users.Include(u => u.UserTags).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return null;
            var profile = _mapper.Map<GetProfileResponseData>(user);

            return profile;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task VerifyUser(string id)
        {
            var innerUser = await GetUserByIdAsync(id);
            innerUser.IsVerified = true;
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsTdt(string studentId)
        {
            await Task.Yield();
            return true;
        }
    }


    public static class UserServicesExtensions
    {
        public static void AddUserServices(this IServiceCollection builder)
        {
            builder.AddScoped(typeof(UserServices));
        }
    }
}