﻿using System;
using System.IO;
using Newtonsoft.Json;

namespace DemoProject.DLL.Infrastructure
{
  public class ImageJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(string);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      string path = Path.Combine(Directory.GetCurrentDirectory(), reader.Value.ToString());

      return File.ReadAllBytes(path);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}