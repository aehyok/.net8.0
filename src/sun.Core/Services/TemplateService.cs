﻿using sun.Core.Domains;
using sun.EntityFrameworkCore.Repository;
using sun.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sun.Core.Services
{
    public class TemplateService(DbContext dbContext, IMapper mapper) : ServiceBase<Template>(dbContext, mapper), ITemplateService, IScopedDependency
    {
    }
}
