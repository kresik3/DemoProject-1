﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoProject.Shared.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ValidateModelStateAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        var serviceResult = ServiceResultFactory.BadRequestResult(context.ModelState);
        context.Result = new BadRequestObjectResult(serviceResult.Errors);
      }
    }
  }
}
