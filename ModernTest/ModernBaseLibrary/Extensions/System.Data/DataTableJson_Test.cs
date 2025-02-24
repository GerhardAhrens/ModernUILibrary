/*
 * <copyright file="ExpandoObjectTests.cs" company="Lifeprojects.de">
 *     Class: ExpandoObjectTests
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>23.02.2025</date>
 * <Project>ModernTest.ModernBaseLibrary</Project>
 *
 * <summary>
 * Test Klasse für den Typ ExpandoObject
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;

    using DemoDataGeneratorLib.Base;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for DataTable_Test
    /// </summary>
    [TestClass]
    public class DataTableJson_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;
        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void DataTableToJson()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Extensions\\System.Data\\DemoData\\DataTabelToJson_A.json");

            DataTable usersDt = BuildDemoData<UserDemoDaten>.CreateForDataTable<UserDemoDaten>(ConfigObject, 1);
            if (usersDt.Rows.Count > 0)
            {
                string jsonText = usersDt.ToJson();
                File.WriteAllText(pathFileName, jsonText);
            }
        }

        [TestMethod]
        public void JsonToDataTable()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Extensions\\System.Data\\DemoData\\DataTabelToJson_A.json");
            string jsonText = File.ReadAllText(pathFileName);
            var aa = jsonText.JsonToDataTable<UserDemoDaten>(nameof(UserDemoDaten));
        }

        [TestMethod]
        public void JsonToDataTableNativ()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Extensions\\System.Data\\DemoData\\DataTabelToJson_A.json");
            string jsonText = File.ReadAllText(pathFileName);
            using (JsonDocument document = JsonDocument.Parse(jsonText))
            {
                JsonElement root = document.RootElement;
                var aa = root[0];
            }
        }

        private UserDemoDaten ConfigObject(UserDemoDaten demoDaten, int counter)
        {
            var timeStamp = BuildDemoData.SetTimeStamp();
            demoDaten.UserName = BuildDemoData.Username();
            demoDaten.Betrag = BuildDemoData.CurrencyValue(1_000, 10_000);
            demoDaten.IsDeveloper = BuildDemoData.Boolean();
            demoDaten.City = BuildDemoData.City();
            demoDaten.CreateOn = timeStamp.CreateOn;
            demoDaten.CreateBy = timeStamp.CreateBy;
            demoDaten.ModifiedOn = timeStamp.ModifiedOn;
            demoDaten.ModifiedBy = timeStamp.ModifiedBy;

            return demoDaten;
        }


        [DebuggerDisplay("UserName={this.UserName}")]
        private class UserDemoDaten
        {
            public string UserName { get; set; }
            public bool IsDeveloper { get; set; }
            public decimal Betrag { get; set; }
            public string City { get; set; }
            public DateTime CreateOn { get; set; }
            public string CreateBy { get; set; }
            public DateTime ModifiedOn { get; set; }
            public string ModifiedBy { get; set; }
        }

        /// <summary>
        /// https://madhawapolkotuwa.medium.com/mastering-json-serialization-in-c-with-system-text-json-01f4cec0440d
        /// </summary>
        private class CustomDateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.ParseExact(reader.GetString(), "yyyy-MM-dd", null);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
            }
        }

        public class DataTableJsonConverter : JsonConverter<DataTable>
        {
            public override DataTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var jsonDoc = JsonDocument.ParseValue(ref reader);
                var rootElement = jsonDoc.RootElement;
                var dataTable = rootElement.JsonElementToDataTable();
                return dataTable;
            }

            public override void Write(Utf8JsonWriter writer, DataTable value, JsonSerializerOptions options)
            {
                WriteDataTable(writer, value, options);
            }
        }

        public static void WriteDataTable(Utf8JsonWriter jsonWriter, DataTable source, JsonSerializerOptions options)
        {
            jsonWriter.WriteStartArray();
            foreach (DataRow dr in source.Rows)
            {
                jsonWriter.WriteStartObject();
                foreach (DataColumn col in source.Columns)
                {
                    var key = col.ColumnName.Trim();
                    var valueString = dr[col].ToString();
                    switch (col.DataType.FullName)
                    {
                        case "System.Guid":
                            jsonWriter.WriteString(key, valueString);
                            break;
                        case "System.Char":
                        case "System.String":
                            jsonWriter.WriteString(key, valueString);
                            break;
                        case "System.Boolean":
                            Boolean.TryParse(valueString, out bool boolValue);
                            jsonWriter.WriteBoolean(key, boolValue);
                            break;
                        case "System.DateTime":
                            var dateValue = DateTime.Parse(valueString);
                            jsonWriter.WriteString(key, dateValue);
                            break;
                        case "System.TimeSpan":
                            var timeSpanValue = TimeSpan.Parse(valueString);
                            jsonWriter.WriteString(key, timeSpanValue.ToString());
                            break;
                        case "System.Byte":
                        case "System.SByte":
                        case "System.Decimal":
                        case "System.Double":
                        case "System.Single":
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.UInt16":
                        case "System.UInt32":
                        case "System.UInt64":
                            if (long.TryParse(valueString, out long intValue))
                            {
                                jsonWriter.WriteNumber(key, intValue);
                            }
                            else
                            {
                                double.TryParse(valueString, out double doubleValue);
                                jsonWriter.WriteNumber(key, doubleValue);
                            }
                            break;
                        default:
                            jsonWriter.WriteString(key, valueString);
                            break;
                    }
                }
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }
    }
}
