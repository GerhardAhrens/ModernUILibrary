//-----------------------------------------------------------------------
// <copyright file="ModelBase.cs" company="Lifeprojects.de">
//     Class: ModelBase
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2025 13:29:40</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernUILibrary.MVVM.Base
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    [DebuggerStepThrough]
    [Serializable]
    public abstract class ModelBase<TModel> : IDisposable
    {
        private bool classIsDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        ~ModelBase()
        {
            this.Dispose(false);
        }

        public static T ToClone<T>(T source)
        {
            var constructorInfo = typeof(T).GetConstructor(new Type[] { });
            if (constructorInfo != null)
            {
                var target = (T)constructorInfo.Invoke(new object[] { });

                const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;
                var sourceProperties = source.GetType().GetProperties(Flags);

                foreach (PropertyInfo pi in sourceProperties)
                {
                    if (pi.CanWrite == true)
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

            return default(T);
        }

        public static T ToClone<T>(object source)
        {
            var constructorInfo = typeof(T).GetConstructor(new Type[] { });
            if (constructorInfo != null)
            {
                var target = (T)constructorInfo.Invoke(new object[] { });

                const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;
                var sourceProperties = source.GetType().GetProperties(Flags);

                foreach (PropertyInfo pi in sourceProperties)
                {
                    if (pi.CanWrite == true)
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

            return default(T);
        }

        public override string ToString()
        {
            try
            {
                PropertyInfo[] propInfo = this.GetType().GetProperties();
                StringBuilder outText = new StringBuilder();
                outText.AppendFormat("{0} ", this.GetType().Name);
                foreach (PropertyInfo propItem in propInfo)
                {
                    outText.AppendFormat("{0}:{1};", propItem.Name, propItem.GetValue(this, null));
                }

                return outText.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Compare<T>(T object1, T object2)
        {
            Type type = typeof(T);

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

        public int CalculateHash(params Func<object>[] memberThunks)
        {
            /* Overflow is okay; just wrap around */
            unchecked
            {
                int hash = 5;
                foreach (var member in memberThunks)
                {
                    if (member() != null)
                    {
                        hash = hash * 33 + member().GetHashCode();
                    }
                }
                return hash;
            }
        }

        #region Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                }
            }

            this.classIsDisposed = true;
        }

        #endregion Dispose
    }
}
