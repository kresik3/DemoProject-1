﻿using System;
using System.ComponentModel.DataAnnotations;
using DemoProject.DLL.Models;

namespace DemoProject.WebApi.Models.ShopItemDetailApiModels
{
  public class ShopItemDetailEditModel
  {
    public int SubOrder { get; set; }

    [Required]
    [MaxLength(100)]
    public string Kind { get; set; }

    [Required]
    [MaxLength(100)]
    public string Quantity { get; set; }

    public decimal Price { get; set; }

    public Guid ShopItemId { get; set; }

    public static ShopItemDetail Map(ShopItemDetailEditModel model, Guid id)
    {
      if (model == null)
      {
        return null;
      }

      return new ShopItemDetail
      {
        Id = id,
        SubOrder = model.SubOrder,
        Kind = model.Kind,
        Quantity = model.Quantity,
        Price = model.Price,
        ShopItemId = model.ShopItemId
      };
    }
  }
}
