﻿using System;
using System.ComponentModel.DataAnnotations;
using DemoProject.DLL.Models;

namespace DemoProject.WebApi.Models.DeliveryApiModels
{
  public class DeliveryEditModel
  {
    [Required]
    public int Order { get; set; }

    public static ContentGroup Map(DeliveryEditModel model, Guid id)
    {
      if (model == null)
      {
        return null;
      }

      return new ContentGroup
      {
        Id = id,
        Order = model.Order,
        GroupName = GroupName.Delivery
      };
    }
  }
}