/*
PURPOSE

WRITTEN
  29/08/2022 17:42:17 (NetForemost)
COPYRIGHT
  Copyright © 2021–2022 NaturalSlim. All Rights Reserved.
WARNING
  This software is copyrighted! Any use of this software or other software
  whose copyright is held by IntelliProp or any software derived from such
  software without the prior written consent of the copyright holder is a
  violation of federal law punishable by imprisonment, fine or both.
  IntelliProp will pay a reward of three thousand dollars ($3,000) for
  information leading to successful civil litigation or criminal conviction
  of anyone violating a copyright held by IntelliProp.
*/

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetForemost.Infrastructure.Extensions
{
    public static class JsonExtension
    {
        private static readonly JsonSerializerOptions _jsonOptions = ConfigureOptions(new JsonSerializerOptions());

        public static JsonSerializerOptions ConfigureOptions(JsonSerializerOptions serializerOptions)
        {
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.PropertyNameCaseInsensitive = true;
            serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            // Configures JsonSerializer to serialize enums as Strings.
            serializerOptions.Converters.Add(new JsonStringEnumConverter());

            return serializerOptions;
        }

        public static T? FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }

        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, _jsonOptions);
        }
    }
}
