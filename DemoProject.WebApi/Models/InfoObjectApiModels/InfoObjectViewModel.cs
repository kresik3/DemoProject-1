﻿using System;
using DemoProject.DLL.Models;

namespace DemoProject.WebApi.Models.InfoObjectApiModels
{
  public class InfoObjectViewModel
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public Guid DiscountId { get; set; }

    public static InfoObjectViewModel Map(InfoObject model)
    {
      if (model == null)
      {
        return null;
      }

      return new InfoObjectViewModel
      {
        Id = model.Id,
        DiscountId = model.DiscountId,
        Content = model.Content,
        Type = model.Type.ToString()
      };
    }
  }
}