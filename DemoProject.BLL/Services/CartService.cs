﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DemoProject.BLL.Interfaces;
using DemoProject.BLL.PageModels;
using DemoProject.DAL;
using DemoProject.DAL.Models;
using DemoProject.Shared;
using DemoProject.Shared.Extensions;
using DemoProject.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.BLL.Services
{
  public class CartService : ICartService
  {
    private readonly EFContext _context;

    public CartService(EFContext context)
    {
      _context = context;
    }

    public async Task<IPage<Cart>> GetPageAsync(int pageIndex, int pageSize, Expression<Func<Cart, bool>> filter = null)
    {
      var query = _context.Carts.AsNoTracking();

      if (filter != null)
      {
        query = query.Where(filter);
      }

      var totalCount = await query.CountAsync();

      var page = new CartPage
      {
        CurrentPage = pageIndex,
        PageSize = pageSize,
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
      };

      page.Records = await query
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .Include(x => x.CartShopItems)
        .ThenInclude(x => x.ShopItemDetail)
        .ThenInclude(x => x.ShopItem)
        .ToListAsync();

      return page;
    }

    public async Task<List<Cart>> GetListAsync(Expression<Func<Cart, bool>> filter = null)
    {
      var query = _context.Carts.AsNoTracking();

      if (filter != null)
      {
        query = query.Where(filter);
      }

      return await query
        .Include(x => x.CartShopItems)
        .ThenInclude(x => x.ShopItemDetail)
        .ThenInclude(x => x.ShopItem)
        .ToListAsync();
    }

    public Task<Cart> FindByAsync(Expression<Func<Cart, bool>> filter)
    {
      Check.NotNull(filter, nameof(filter));

      return _context.Carts.AsNoTracking()
        .Include(x => x.CartShopItems)
        .ThenInclude(x => x.ShopItemDetail)
        .ThenInclude(x => x.ShopItem)
        .FirstOrDefaultAsync(filter);
    }

    public async Task<ServiceResult> AddItemToCartAsync(Guid cartId, Guid shopItemDetailId, int count)
    {
      if (count < 1)
      {
        return ServiceResultFactory.BadRequestResult(nameof(count), $"'{count}' should be positive.");
      }

      if (await _context.Carts.AnyAsync(x => x.Id == cartId) == false)
      {
        return ServiceResultFactory.BadRequestResult(nameof(cartId), $"Cart not found with id: '{cartId}'.");
      }

      var shopItemDetail = await _context.ShopItemDetails.FirstOrDefaultAsync(x => x.Id == shopItemDetailId);
      if (shopItemDetail == null)
      {
        return ServiceResultFactory.BadRequestResult(nameof(shopItemDetailId), $"ShopItemDetail not found with id: '{shopItemDetailId}'.");
      }

      var oldCartShopItem = await _context.CartShopItems
        .FirstOrDefaultAsync(x => x.CartId == cartId && x.ShopItemDetailId == shopItemDetailId);

      if (oldCartShopItem == null)
      {
        // add new item
        await _context.CartShopItems.AddAsync(new CartShopItem
        {
          CartId = cartId,
          ShopItemDetailId = shopItemDetailId,
          Price = shopItemDetail.Price,
          Count = count
        });
      }
      else
      {
        // increase count and update price
        oldCartShopItem.Count += count;
        oldCartShopItem.Price = shopItemDetail.Price;
        _context.CartShopItems.Update(oldCartShopItem);
      }

      var result =  await _context.SaveAsync(nameof(AddItemToCartAsync), null);
      if (result.Key == ServiceResultKey.ModelUpdated)
      {
        result.Model = await this.FindByAsync(x => x.Id == cartId);
      }

      return result;
    }

    public async Task<ServiceResult> RemoveItemFromCartAsync(Guid cartId, Guid shopItemDetailId, bool shouldBeRemovedAllItems)
    {
      if (await _context.Carts.AnyAsync(x => x.Id == cartId) == false)
      {
        return ServiceResultFactory.BadRequestResult(nameof(cartId), $"Cart not found with id: '{cartId}'.");
      }

      if (await _context.ShopItemDetails.AnyAsync(x => x.Id == shopItemDetailId) == false)
      {
        return ServiceResultFactory.BadRequestResult(nameof(shopItemDetailId), $"ShopItemDetail not found with id: '{shopItemDetailId}'.");
      }

      var cartShopItem = await _context.CartShopItems
        .FirstOrDefaultAsync(x => x.CartId == cartId && x.ShopItemDetailId == shopItemDetailId);
      if (cartShopItem == null)
      {
        return ServiceResultFactory.BadRequestResult(nameof(cartShopItem), $"CartShopItem not found with such complex id: '{cartId}-{shopItemDetailId}'.");
      }

      if (shouldBeRemovedAllItems || cartShopItem.Count <= 1)
      {
        // remove cartshopitem record
        _context.CartShopItems.Remove(cartShopItem);
      }
      else
      {
        // reduce count
        cartShopItem.Count -= 1;
        _context.CartShopItems.Update(cartShopItem);
      }

      var result = await _context.SaveAsync(nameof(RemoveItemFromCartAsync), null);
      if (result.Key == ServiceResultKey.ModelUpdated)
      {
        result.Model = await this.FindByAsync(x => x.Id == cartId);
      }

      return result;
    }

    public Task<bool> ExistAsync(Expression<Func<Cart, bool>> filter)
    {
      Check.NotNull(filter, nameof(filter));

      return _context.Carts.AnyAsync(filter);
    }

    public Task<ServiceResult> AddAsync(Cart model)
    {
      Check.NotNull(model, nameof(model));

      _context.Carts.Add(model);

      return _context.SaveAsync(nameof(AddAsync), model.Id);
    }

    public Task<ServiceResult> UpdateAsync(Cart model)
    {
      throw new NotImplementedException();
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
      var model = await _context.Carts.FirstOrDefaultAsync(x => x.Id == id);
      if (model == null)
      {
        return ServiceResultFactory.Success;
      }

      _context.Carts.Remove(model);

      return await _context.SaveAsync(nameof(DeleteAsync));
    }

    public void Dispose()
    {
      _context.Dispose();
    }
  }
}
