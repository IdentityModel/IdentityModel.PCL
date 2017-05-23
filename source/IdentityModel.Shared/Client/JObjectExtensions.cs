﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityModel.Client
{
  /// <summary>
  /// Extensions for JObject
  /// </summary>
  public static class JObjectExtensions
  {
    /// <summary>
    /// Tries to get a value from a JObject
    /// </summary>
    /// <param name="json">The json.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static JToken TryGetValue(this JObject json, string name)
    {
      JToken value;
      if (json != null && json.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out value))
      {
        return value;
      }

      return null;
    }

    /// <summary>
    /// Tries to get an int from a JObject
    /// </summary>
    /// <param name="json">The json.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static int? TryGetInt(this JObject json, string name)
    {
      var value = json.TryGetString(name);

      if (value != null)
      {
        int intValue;
        if (int.TryParse(value, out intValue))
        {
          return intValue;
        }
      }

      return null;
    }

    /// <summary>
    /// Tries to get a string from a JObject
    /// </summary>
    /// <param name="json">The json.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static string TryGetString(this JObject json, string name)
    {
      JToken value = json.TryGetValue(name);
      return value?.ToString();
    }

    /// <summary>
    /// Tries to get a boolean from a JObject
    /// </summary>
    /// <param name="json">The json.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static bool? TryGetBoolean(this JObject json, string name)
    {
      var value = json.TryGetString(name);

      bool result;
      if (bool.TryParse(value, out result))
      {
        return result;
      }

      return null;
    }

    /// <summary>
    /// Tries to get a string array from a JObject
    /// </summary>
    /// <param name="json">The json.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static IEnumerable<string> TryGetStringArray(this JObject json, string name)
    {
      var values = new List<string>();

      var array = json.TryGetValue(name) as JArray;
      if (array != null)
      {
        foreach (var item in array)
        {
          values.Add(item.ToString());
        }
      }

      return values;
    }
  }
}