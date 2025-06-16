//-----------------------------------------------------------------------
// <copyright file="ID.cs" company="NRM Netzdienste Rhein-Main GmbH">
//     Class: ID
//     Copyright © NRM Netzdienste Rhein-Main GmbH 2024
// </copyright>
//
// <author>DeveloperName - NRM Netzdienste Rhein-Main GmbH</author>
// <email>DeveloperName@nrm-netzdienste.de</email>
// <date>18.06.2024 10:26:52</date>
//
// <summary>
// Klasse für Domain Typ ID
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System;
    using System.Collections.Generic;

    using ModernBaseLibrary.CoreBase;

    public sealed class ID : DomainObjectBase
    {
        public ID(int id)
        {
            this.Value = id;
        }

        public ID(Guid id)
        {
            this.Value = id;
        }

        public object Value { get; }

        public IDStatus Status { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static implicit operator ID(int value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(), nameof(value));
            IDStatus state = IDStatus.None;

            if (value < 1)
            {
                value = -1;
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            ID instance = new ID(value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID(long value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(), nameof(value));

            IDStatus state = IDStatus.None;

            if (value < 1)
            {
                value = -1;
                state = IDStatus.New;
            }
            else if (value > int.MaxValue)
            {
                state = IDStatus.Error;
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Wert ist größer als Int Max ({value})");
            }
            else
            {
                state = IDStatus.Edit;
            }

            ID instance = new ID((int)value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID(decimal value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(), nameof(value));
            IDStatus state = IDStatus.None;

            if (value < 1)
            {
                value = -1;
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            ID instance = new ID((int)value)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(), nameof(value));
            IDStatus state = IDStatus.None;

            if (value == string.Empty)
            {
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            int outInt = -1;
            if (int.TryParse(value, out outInt) == false)
            {
                state = IDStatus.Error;
            }
            else
            {
                if (outInt < 1)
                {
                    outInt = -1;
                    state = IDStatus.New;
                }
                else
                {
                    state = IDStatus.Edit;
                }
            }

            ID instance = new ID((int)outInt)
            {
                Status = state
            };

            return instance;
        }

        public static implicit operator ID(Guid value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value.ToString(), nameof(value));
            IDStatus state = IDStatus.None;

            if (value == Guid.Empty)
            {
                state = IDStatus.New;
            }
            else
            {
                state = IDStatus.Edit;
            }

            Guid outInt = Guid.Empty;
            if (Guid.TryParse(value.ToString(), out outInt) == false)
            {
                state = IDStatus.Error;
            }
            else
            {
                if (outInt == Guid.Empty)
                {
                    outInt = Guid.Empty;
                    state = IDStatus.New;
                }
                else
                {
                    state = IDStatus.Edit;
                }
            }

            ID instance = new ID((Guid)outInt)
            {
                Status = state
            };

            return instance;
        }

        public enum IDStatus
        {
            None = 0,
            New = 1,
            Edit = 2,
            Error = 3,
        }

    }
}
