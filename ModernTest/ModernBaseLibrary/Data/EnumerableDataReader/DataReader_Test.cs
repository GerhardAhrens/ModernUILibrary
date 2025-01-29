namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using global::ModernBaseLibrary.Reader;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataReader_Test : BaseTest
    {
        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void ToDictionary_Test()
        {
            SampleClass[] data = Enumerable.Range(0, 100)
                            .Select(i =>
                            {
                                return new SampleClass()
                                {
                                    a = i.ToString(),
                                    b = (short)(i & 0xffff),
                                    c = i,
                                    d = (byte)(i & 0xff),
                                    e = i,
                                    f = i,
                                    g = (char)i,
                                    h = DateTime.MinValue.AddDays(i),
                                    i = Guid.Empty,
                                    j = i ,
                                    k = Enumerable.Range(0, i).Select(j => (byte)(i & 0xff)).ToArray()
                                };
                            }).ToArray();

            Dictionary<string,PropertyInfo> properties = typeof(SampleClass).GetTypeInfo().GetProperties().ToDictionary(x => x.Name, x => x);

            Assert.IsTrue(properties.Count == 11);
            Assert.IsTrue(data.Count() == 100);

            using (var dr = data.AsDataReader())
            {
                int cnt = 0;
                foreach (var x in dr.ToDictionary().Select((dic, i) => new { dic, i }))
                {
                    Trace($"{properties.Count}, {x.dic.Count}");
                    foreach (var kv in x.dic)
                    {
                        Trace($"{properties[kv.Key].GetValue(data[x.i])}, {kv.Value}");
                    }

                    cnt++;
                }

                Trace($"{data.Length}, {cnt}");
            }

        }

        [TestMethod]
        public void AsDataReaderFromDictionary_Test()
        {
            IEnumerable<Dictionary<string, object>> data = Enumerable.Range(0, 100)
                .Select(i =>
                {
                    return new Dictionary<string, object>()
                    {
                        {"a",i },
                        {"b",$"BlubberBlub-{i}" },
                        {"c",DateTime.MinValue.AddDays(i) },
                        {"d",i * 1.1 },
                        {"e",i * (decimal)1 }
                    };
                }).ToArray();

            Assert.IsTrue(data.Count() == 100);

            using (var dr = data.AsDataReaderFromDictionary())
            {
                int i = 0;
                while (dr.Read())
                {
                    var aa = dr.GetOrdinal("b");
                    Trace($"{5}, {dr.FieldCount}");
                    Trace($"{i},{dr.GetInt32(dr.GetOrdinal("a"))}");
                    Trace($"BlubberBlub-{i},{dr.GetString(dr.GetOrdinal("b"))}");
                    Trace($"{DateTime.MinValue.AddDays(i)},{dr.GetDateTime(dr.GetOrdinal("c"))}");
                    Trace($"{i * 1.1}, {dr.GetDouble(dr.GetOrdinal("d"))}");
                    Trace($"{i * (decimal)1}, {dr.GetDecimal(dr.GetOrdinal("e"))}");
                    i += 1;
                }
            }
        }

        [TestMethod]
        public void AsDataReader_Test()
        {
            SampleClass[] data = Enumerable.Range(0, 100)
                .Select(i =>
                {
                    return new SampleClass()
                    {
                        a = i.ToString(),
                        b = (short)(i & 0xffff),
                        c = i,
                        d = (byte)(i & 0xff),
                        e = i,
                        f = i,
                        g = (char)i,
                        h = DateTime.MinValue.AddDays(i),
                        i = Guid.Empty,
                        j = i,
                        k = Enumerable.Range(0, i).Select(j => (byte)(i & 0xff)).ToArray()
                    };
                }).ToArray();

            Assert.IsTrue(data.Count() == 100);

            var ti = typeof(SampleClass).GetTypeInfo();
            PropertyInfo[] properties = ti.GetProperties();

            Assert.IsTrue(properties.Count() == 11);

            using (var reader = data.AsDataReader(typeof(SampleClass)))
            {
                int i = 0;
                while (reader.Read())
                {
                    foreach (var pi in properties)
                    {
                        var ord = reader.GetOrdinal(pi.Name);
                        Trace($"{pi.Name};{reader.GetName(ord)}");
                        Trace($"{pi.PropertyType};{reader.GetFieldType(ord)}");
                    }

                    Assert.AreEqual($"{data[i].a}", $"{reader.GetString(reader.GetOrdinal("a"))}");
                    Assert.AreEqual($"{data[i].b}", $"{reader.GetString(reader.GetOrdinal("b"))}");
                    Assert.AreEqual($"{data[i].c}", $"{reader.GetString(reader.GetOrdinal("c"))}");
                    Assert.AreEqual($"{data[i].d}", $"{reader.GetString(reader.GetOrdinal("d"))}");
                    Assert.AreEqual($"{data[i].e}", $"{reader.GetString(reader.GetOrdinal("e"))}");
                    Assert.AreEqual($"{data[i].f}", $"{reader.GetString(reader.GetOrdinal("f"))}");
                    Assert.AreEqual($"{data[i].g}", $"{reader.GetString(reader.GetOrdinal("g"))}");
                    Assert.AreEqual($"{data[i].h}", $"{reader.GetString(reader.GetOrdinal("h"))}");
                    Assert.AreEqual($"{data[i].i}", $"{reader.GetString(reader.GetOrdinal("i"))}");

                    Trace($"{reader.IsDBNull(reader.GetOrdinal("j"))}");
                    Assert.AreEqual($"{data[i].j}", $"{reader.GetString(reader.GetOrdinal("j"))}");

                    var values = new object[properties.Length];
                    Console.WriteLine($"{properties.Length}", $"{reader.GetValues(values)}");

                    var buf = new byte[128];
                    var bytesread = reader.GetBytes(reader.GetOrdinal("k"), 0, buf, 0, buf.Length);
                    Trace($"{data[i].k.Count}");
                    Assert.AreEqual($"{data[i].k.Count}", $"{bytesread}");
                    i++;
                }
            }

        }

        [TestMethod]
        public void ReadAttributeTestClass_Test()
        {
            using (var dr = Enumerable.Range(0, 10).Select(i => new AttributeTestClass() { a = i, b = i.ToString(), c = i }).AsDataReader())
            {
                int cnt = 0;
                while (dr.Read())
                {
                    Trace("{0}", $"{dr.FieldCount}");
                    Trace("{0}", $"{dr.GetInt32(dr.GetOrdinal("a"))}");
                    cnt++;
                }
            }
        }

        [TestMethod]
        public void ReadAttributeTestClassFieldNameAs_Test()
        {
            using (var dr = Enumerable.Range(0, 10).Select(i => new AttributeTestClass() { a = i, b = i.ToString(), c = i }).AsDataReader())
            {
                int cnt = 0;
                while (dr.Read())
                {
                    Trace("{0}", $"{dr.GetDouble(dr.GetOrdinal("x"))}");
                    cnt++;
                }
            }
        }

        [TestMethod]
        public void AsDataReaderByClass_Test()
        {
            using (var dr = Personen().AsDataReader(typeof(Person)))
            {
                int cnt = 0;
                while (dr.Read())
                {
                    Trace("{0}", $"{dr.GetString(dr.GetOrdinal("Vorname"))}");
                    cnt++;
                }
            }
        }

        private static IEnumerable<Person> Personen()
        {
            List<Person> personen = new List<Person>();
            personen.Add(new Person() { Id = Guid.NewGuid(), Vorname = "Charlie", Name = "Charlie", Alter = 2 });
            personen.Add(new Person() { Id = Guid.NewGuid(), Vorname = "Beate", Name = "Mustermann", Alter = 64 });
            personen.Add(new Person() { Id = Guid.NewGuid(), Vorname = "Gerhard", Name = "Mustermann", Alter = 65 });

            return personen;
        }
    }

    public class SampleClass
    {
        public string a { get; set; }
        public short b { get; set; }
        public int c { get; set; }
        public byte d { get; set; }
        public long e { get; set; }
        public decimal f { get; set; }
        public char g { get; set; }
        public DateTime h { get; set; }
        public Guid i { get; set; }
        public int? j { get; set; }
        public IList<byte> k { get; set; }
    }

    public class AttributeTestClass
    {
        public int a { get; set; }

        [IgnoreField]
        public string b { get; set; }

        [FieldNameAs("x")]
        public double c { get; set; }
    }

    public class Person
    {
        public Guid Id { get; set; }

        public string Vorname { get; set; }

        public string Name { get; set; }

        public string Land { get; set; }

        public int Alter { get; set; }
    }
}