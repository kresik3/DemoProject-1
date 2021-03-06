﻿using System.Collections.Generic;
using System.Linq;
using DemoProject.DAL.Models;
using DemoProject.Shared.Interfaces;
using DemoProject.WebApi.Models.DeliveryApiModels;

namespace DemoProject.WebApi.Models.Pages
{
  public class DeliveryPageModel
  {
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public ICollection<DeliveryViewModel> Records { get; set; } = new List<DeliveryViewModel>();

    public static DeliveryPageModel Map(IPage<ContentGroup> model)
    {
      if (model == null)
      {
        return null;
      }

      if (model.Records == null)
      {
        model.Records = new List<ContentGroup>();
      }

      return new DeliveryPageModel
      {
        CurrentPage = model.CurrentPage,
        PageSize = model.PageSize,
        TotalPages = model.TotalPages,
        Records = model.Records.Select(DeliveryViewModel.Map).ToList()
      };
    }
  }
}
