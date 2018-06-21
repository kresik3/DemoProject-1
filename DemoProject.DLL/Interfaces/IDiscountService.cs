﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DemoProject.DLL.Models;
using Microsoft.AspNetCore.Identity;

namespace DemoProject.DLL.Interfaces
{
  public interface IDiscountService : IDisposable
  {
    Task<List<Discount>> GetDiscountsAsync(Expression<Func<Discount, bool>> filter = null);
    Task<Discount> FindByAsync(Expression<Func<Discount, bool>> filter);

    Task<IdentityResult> AddAsync(Discount discount);
    Task<IdentityResult> DeleteAsync(Guid discountId);

    Task<IdentityResult> AddInfoObjectAsync(Guid discountId, InfoObject infoObject);
    Task<IdentityResult> DeleteInfoObjectFromDiscountAsync(Guid discountId, Guid infoObjectId);
  }
}
