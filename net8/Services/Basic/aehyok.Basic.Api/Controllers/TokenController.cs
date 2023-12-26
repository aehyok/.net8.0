﻿using aehyok.Basic.Dtos;
using aehyok.Basic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aehyok.Basic.Api.Controllers
{

    /// <summary>
    /// Token 管理
    /// </summary>
    public class TokenController : BasicControllerBase
    {
        public IUserTokenService userTokenService;

        public TokenController(IUserTokenService userTokenService)
        {
            this.userTokenService = userTokenService;
        }
        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("captcha")]
        [AllowAnonymous]
        public Task<CaptchaDto> GetCaptchaAsync()
        {
            return this.userTokenService.GenerateCaptchaAsync();
        }
    }
}