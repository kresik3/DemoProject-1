﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.WebApi.Infrastructure
{
  public class InternalServerErrorResult : StatusCodeResult
  {
    public InternalServerErrorResult() : base(StatusCodes.Status500InternalServerError)
    {
    }
  }
}
