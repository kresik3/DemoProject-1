﻿using System;
using DemoProject.DAL;
using DemoProject.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoProject.WebApi.Infrastructure
{
  public static class ApplicationBuilderExtensions
  {
    public static void ApplyMigrationAndDatabaseSeed(this IApplicationBuilder app, bool isDatabaseRestore)
    {
      using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        try
        {
          var context = scope.ServiceProvider.GetRequiredService<EFContext>();
          var databaseCreator = context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
          var isDatabaseExisted = databaseCreator.Exists();

          context.Database.Migrate();

          if (!isDatabaseExisted || isDatabaseRestore)
          {
            context.ClearDatabase();
            scope.ServiceProvider.GetRequiredService<SeedService>().SeedDatabase(context);
          }
        }
        catch (Exception ex)
        {
          var logger = scope.ServiceProvider.GetService<ILogger<Startup>>();
          if (logger != null && logger.IsEnabled(LogLevel.Error))
          {
            logger.LogError(ex, "Failed to migrate or seed database.");
          }
        }
      }
    }

    public static void SetupDefaultPage(this IApplicationBuilder app, string url)
    {
      var options = new RewriteOptions();
      options.AddRedirect("^$", url);
      app.UseRewriter(options);
    }
  }
}
