//-----------------------------------------------------------------------
// <copyright file="JsonExtensions.cs" company="Lifeprojects.de">
//     Class: JsonExtensions
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.02.2025</date>
//
// <summary>
// Extension Class for Text Json
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Data;
    using System.IO;
    using System.Text.Json;

    public static class JsonExtensions
    {
        public static string ToJson(this DataTable @this)
        {
            if (@this == null)
            {
                return string.Empty;
            }

            IEnumerable<Dictionary<string, object>> data = @this.Rows.OfType<DataRow>()
                        .Select(row => @this.Columns.OfType<DataColumn>()
                            .ToDictionary(col => col.ColumnName, c => row[c]));

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string jsonText = System.Text.Json.JsonSerializer.Serialize(data, jsonOptions);

            return jsonText;
        }

        public static bool ToJson(this DataTable @this, string jsonFile)
        {
            bool result = false;
            if (@this == null)
            {
                return false;
            }

            IEnumerable<Dictionary<string, object>> data = @this.Rows.OfType<DataRow>()
                        .Select(row => @this.Columns.OfType<DataColumn>()
                            .ToDictionary(col => col.ColumnName, c => row[c]));

            if (data == null || data.Count() == 0)
            {
                return false;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            };

            string jsonText = System.Text.Json.JsonSerializer.Serialize(data, jsonOptions);

            if (string.IsNullOrEmpty(jsonText) == false)
            {
                File.WriteAllText(jsonFile, jsonText);
                if (File.Exists(jsonFile) == true)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool ToJson<T>(this List<T> @this, string jsonFile)
        {
            bool result = false;
            if (@this == null)
            {
                return false;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string jsonText = System.Text.Json.JsonSerializer.Serialize<List<T>>(@this, jsonOptions);

            if (string.IsNullOrEmpty(jsonText) == false)
            {
                File.WriteAllText(jsonFile, jsonText);
                if (File.Exists(jsonFile) == true)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool ToJson<T>(this IEnumerable<T> @this, string jsonFile)
        {
            bool result = false;
            if (@this == null)
            {
                return false;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string jsonText = System.Text.Json.JsonSerializer.Serialize<IEnumerable<T>>(@this, jsonOptions);

            if (string.IsNullOrEmpty(jsonText) == false)
            {
                File.WriteAllText(jsonFile, jsonText);
                if (File.Exists(jsonFile) == true)
                {
                    result = true;
                }
            }

            return result;
        }

        public static DataTable JsonToDataTable<T>(this string @this, string tableName = "") where T : class, new()
        {
            DataTable result = null;
            if (string.IsNullOrEmpty(@this) == true)
            {
                return null;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            };

            List<T> listFromJson = (List<T>)System.Text.Json.JsonSerializer.Deserialize<List<T>>(@this, jsonOptions);
            if (listFromJson != null && listFromJson.Count > 0)
            {
                result = listFromJson.ToDataTable<T>(tableName);
            }

            return result;
        }

        public static List<T> JsonToList<T>(this string @this) where T : class, new()
        {
            List<T> result = null;
            if (string.IsNullOrEmpty(@this) == true)
            {
                return null;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            };

            result = (List<T>)System.Text.Json.JsonSerializer.Deserialize<List<T>>(@this, jsonOptions);

            return result;
        }
    }
}
