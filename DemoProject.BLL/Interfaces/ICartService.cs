﻿using System;
using System.Threading.Tasks;
using DemoProject.DAL.Models;
using DemoProject.DAL.Models.Pages;
using DemoProject.Shared;
using DemoProject.Shared.Interfaces;

namespace DemoProject.BLL.Interfaces
{
  public interface ICartService : IChangeableService<Cart>, IReadableService<Cart, CartPage>
  {
    Task<ServiceResult> AddItemToCartAsync(Guid cartId, Guid shopItemDetailId, int count);
    Task<ServiceResult> RemoveItemFromCartAsync(Guid cartId, Guid shopItemDetailId, bool shouldBeRemovedAllItems);
  }
}