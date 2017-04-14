﻿using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ItForum.Controllers.DTO;
using ItForum.Controllers.DTO.UserController;
using ItForum.Models;
using ItForum.Services;
using ItForum.Services.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItForum.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        //private readonly IMapper _mapper;
        private readonly UserServices _services;

        public UserController(UserServices services, IMapper mapper)
        {
            _services = services;
            //_mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var innerUser = await _services.Login(user.Username, user.PasswordHash);
            var payload = new Payload();
            if (innerUser == null)
            {
                var userInDb = await _services.GetUserByUserName(user.Username);
                payload.StatusCode = userInDb != null? LoginResponseCode.Incorrect: LoginResponseCode.NotExist;
                return Json(payload);
            }

            var jwt = await _services.GenerateJwt(innerUser);

            payload.Data = jwt;
            return Json(payload);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var payload = new Payload();

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, RegisteredRoles.User)
            };

            var result = await _services.RegisterUser(user, claims);

            if (result == null)
            {
                if (await _services.GetUserByUserName(user.Username) != null)
                {
                    payload.StatusCode = (int)RegisterResponseCode.Existed;
                    return Json(payload);
                }
                payload.StatusCode = (int)RegisterResponseCode.Incorrect;
                return Json(payload);
            }

            payload.StatusCode = (int)RegisterResponseCode.Created;
            return Json(payload);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var payload = new Payload { Data = await _services.GetUserProfile(id) };
            payload.StatusCode = payload.Data != null ? GetProfileResponseCode.Ok : GetProfileResponseCode.NotExist;
            return Json(payload);
        }

        [HttpGet]
        [Authorize(RegisteredPolicys.User)]
        public async Task<IActionResult> GetProfile()
        {
            var payload = new Payload { Data = await _services.GetUserProfile(User.Identity.Name) };
            payload.StatusCode = payload.Data != null ? GetProfileResponseCode.Ok : GetProfileResponseCode.NotExist;
            return Json(payload);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(RegisteredPolicys.Adminstrator)]
        public async Task<IActionResult> VerifyUser(string id)
        {
            var payload = new Payload();
            var innerUser = await _services.GetUserByIdAsync(id);
            if (innerUser == null)
            {
                payload.StatusCode = VerifyResponseCode.NotExist;
                return Json(payload);
            }
            payload.StatusCode = VerifyResponseCode.Ok;
            await _services.VerifyUser(id);
            return Json(payload);
        }
    }
}