﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiTdtItForum.Security;
using ApiTdtItForum.Controllers.SharedObject.Request;
using ApiTdtItForum.Services;
using AutoMapper;
using ApiTdtItForum.Models;
using ApiTdtItForum.SharedObject;
using ApiTdtItForum.Controllers.SharedObject.ResponseCode;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiTdtItForum.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(RegisteredPolicys.Adminstrator)]
    public class TagController : Controller
    {
        TagServices _services;
        readonly IMapper _mapper;

        public TagController(TagServices services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagCreateData data)
        {
            var tag = _mapper.Map<Tag>(data);
            Payload payload = new Payload();
            if (TagServices.IsDataCorrect(tag))
            {
                payload.StatusCode = (int)TagCreateCode.Incorrect;
            }
            else if (await _services.IsTagExisted(tag))
            {
                payload.StatusCode = (int)TagCreateCode.Existed;
            }
            else
            {
                payload.Data = JsonConvert.SerializeObject(await _services.CreateTag(tag), Formatting.Indented);
            }
            return Json(payload);
        }
    }
}
