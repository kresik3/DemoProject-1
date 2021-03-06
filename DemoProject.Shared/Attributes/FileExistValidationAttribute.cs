﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DemoProject.Shared.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public sealed class FileExistValidationAttribute : ValidationAttribute
  {
    private readonly string _relativePath;

    public FileExistValidationAttribute(string relativePath)
    {
      _relativePath = relativePath;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (File.Exists(_relativePath + (string)value))
      {
        return ValidationResult.Success;
      }
      else
      {
        return new ValidationResult(
          $"File should exist on the server.", 
          new[] { validationContext.MemberName });
      }
    }
  }
}
