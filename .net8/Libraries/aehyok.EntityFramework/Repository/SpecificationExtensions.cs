﻿using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.EntityFramework.Repository
{
    public static class SpecificationExtensions
    {
        public static Specifications<TEntity> Create<TEntity>(this Specification<TEntity> specification)
        {
            //return new Specification<TEntity>();
            return null;
        }
    }
}
