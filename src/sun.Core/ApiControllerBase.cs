﻿using sun.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sun.Core
{
    /// <summary>
    /// 项目中所有控制器的基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = "Authorization-Token")]
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// 可直接使用AutoMapper
        /// </summary>
        public IMapper Mapper
        {
            get
            {
                return HttpContext.RequestServices.GetService<IMapper>();
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public ICurrentUser CurrentUser
        {
            get
            {
                return HttpContext.RequestServices.GetService<ICurrentUser>();
            }
        }
    }
}
