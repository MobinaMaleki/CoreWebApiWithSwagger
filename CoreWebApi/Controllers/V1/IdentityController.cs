﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CoreWebApi.Contacts;
using CoreWebApi.Contacts.V1;
using CoreWebApi.Contacts.V1.Requests;
using CoreWebApi.Contacts.V1.Responses;
using CoreWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityServices identityServices;
        public IdentityController(IIdentityServices _identityServices)
        {
            identityServices = _identityServices;
        }
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult>Register([FromBody] UserRegistrationRequest request)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(new AuthFailedRespons
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
                });
            }
            var authresponse = await identityServices.RegisterAsync(request.Email, request.Password);
            if (!authresponse.Success)
            {
                return BadRequest(new AuthFailedRespons {                 
                Errors=authresponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token=authresponse.Token

            });

        }
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authresponse = await identityServices.LoginAsync(request.Email, request.Password);
            if (!authresponse.Success)
            {
                return BadRequest(new AuthFailedRespons
                {
                    Errors = authresponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authresponse.Token

            });

        }


    }
}
