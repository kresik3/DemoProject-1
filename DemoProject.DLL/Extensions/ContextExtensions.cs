﻿using System;
using System.Threading.Tasks;
using DemoProject.DLL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.DLL.Extensions
{
  public static class ContextExtensions
  {
    public static Task<ServiceResult> SaveChangesSafeAsync(this DbContext context)
    {
      return context.SaveChangesSafeAsync(string.Empty);
    }

    public static Task<ServiceResult> SaveChangesSafeAsync(this DbContext context, Guid modelId)
    {
      return context.SaveChangesSafeAsync(string.Empty, modelId);
    }

    public static async Task<ServiceResult> SaveChangesSafeAsync(this DbContext context, string code, Guid modelId)
    {
      var result = await context.SaveChangesSafeAsync(code);
      if (result.Key == ServiceResultKey.Success)
      {
        result = ServiceResultFactory.EntityCreatedResult(modelId);
      }

      return result;
    }

    public static async Task<ServiceResult> SaveChangesSafeAsync(this DbContext context, string code)
    {
      try
      {
        await context.SaveChangesAsync();
        return ServiceResultFactory.Success;
      }
      catch (DbUpdateConcurrencyException ex)
      {
        return ServiceResultFactory.BadRequestResult(string.Empty, ex.InnerException.Message);
      }
      catch (DbUpdateException ex)
      {
        return ServiceResultFactory.BadRequestResult(string.Empty, ex.InnerException.Message);
      }
      catch (Exception ex)
      {
        return ServiceResultFactory.InternalServerErrorResult(ex.InnerException.Message);
      }
    }
  }
}
