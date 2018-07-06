﻿using System;
using System.Collections.Generic;
using System.Linq;
using DemoProject.DLL.Models;
using DemoProject.WebApi.Models.InfoObjectApiModels;

namespace DemoProject.WebApi.Models.DiscountApiModels
{
  public class DiscountViewModel
  {
    public Guid Id { get; set; }
    public string Title { get; set; }
    public ICollection<InfoObjectViewModel> Items { get; set; } = new List<InfoObjectViewModel>();

    public static DiscountViewModel Map(Discount model)
    {
      if (model == null)
      {
        return null;
      }

      return new DiscountViewModel
      {
        Id = model.Id,
        Title = model.Title,
        Items = model.Items.Select(x => InfoObjectViewModel.Map(x)).ToList()
      };
    }
  }
}
