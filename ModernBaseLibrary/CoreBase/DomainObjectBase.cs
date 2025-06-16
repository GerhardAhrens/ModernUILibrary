//-----------------------------------------------------------------------
// <copyright file="DomainObjectBase.cs" company="Lifeprojects.de">
//     Class: DomainObjectBase
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>16.06.2025</date>
//
// <summary>
// Basis Klasse zur Erstellung von Custom Value Type
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.CoreBase
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class DomainObjectBase
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var valueObject = (DomainObjectBase)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(DomainObjectBase a, DomainObjectBase b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(DomainObjectBase a, DomainObjectBase b)
        {
            return !(a == b);
        }
    }
}
