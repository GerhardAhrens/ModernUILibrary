//-----------------------------------------------------------------------
// <copyright file="DomainObjectOfTBase.cs" company="Lifeprojects.de">
//     Class: DomainObjectOfTBase
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>16.06.2025</date>
//
// <summary>
// Basis Klasse zur Erstellung von Generic Custom Value Type
// </summary>
//-----------------------------------------------------------------------

namespace Mainova.Tools.BaseClass
{
    public abstract class DomainObjectOfTBase<T> where T : DomainObjectOfTBase<T>
    {
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;

            if (ReferenceEquals(valueObject, null))
            {
                return false;
            }

            return EqualsCore(valueObject);
        }

        protected abstract bool EqualsCore(T other);

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected abstract int GetHashCodeCore();

        public static bool operator ==(DomainObjectOfTBase<T> a, DomainObjectOfTBase<T> b)
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

        public static bool operator !=(DomainObjectOfTBase<T> a, DomainObjectOfTBase<T> b)
        {
            return !(a == b);
        }
    }
}
