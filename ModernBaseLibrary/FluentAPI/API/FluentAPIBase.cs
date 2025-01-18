//-----------------------------------------------------------------------
// <copyright file="FluentBuilderBase.cs" company="Lifeprojects.de">
//     Class: FluentBuilderBase
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.06.2020</date>
//
// <summary>
//      Abstrakte Klasse für Fluent Design
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    [DebuggerStepThrough]
    [Serializable]
    public abstract class FluentAPIBase<TTyp>
    {
        public static TTyp ToClone(TTyp source)
        {
            var constructorInfo = typeof(TTyp).GetConstructor(new Type[] { });
            if (constructorInfo != null)
            {
                var target = (TTyp)constructorInfo.Invoke(new object[] { });

                const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;
                var sourceProperties = source.GetType().GetProperties(Flags);

                foreach (PropertyInfo pi in sourceProperties)
                {
                    if (pi.CanWrite)
                    {
                        var propInfoObj = target.GetType().GetProperty(pi.Name);
                        if (propInfoObj != null)
                        {
                            var propValue = pi.GetValue(source, null);
                            propInfoObj.SetValue(target, propValue, null);
                        }
                    }
                }

                return target;
            }

            return default(TTyp);
        }

        public static bool ToCompare(TTyp object1, TTyp object2)
        {
            Type type = typeof(TTyp);

            if (object1 == null || object2 == null)
            {
                return false;
            }

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string object1Value = string.Empty;
                    string object2Value = string.Empty;

                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                    {
                        object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    }

                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                    {
                        object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    }

                    if (object1Value.Trim() != object2Value.Trim())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public abstract TTyp Get();

        public virtual string GetTypeName()
        {
            return Activator.CreateInstance(typeof(TTyp)).ToString();
        }

        public override string ToString()
        {
            PropertyInfo[] propInfo = typeof(TTyp).GetProperties();
            StringBuilder outText = new StringBuilder();
            outText.Append($"{typeof(TTyp).Name}");
            foreach (PropertyInfo propItem in propInfo)
            {
                outText.AppendFormat("{0}:{1};", propItem.Name, propItem.GetValue(typeof(TTyp), null));
            }

            return outText.ToString();
        }
    }
}
