//-----------------------------------------------------------------------
// <copyright file="VersionExtensions.cs" company="Lifeprojects.de">
//     Class: VersionExtensions
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>05.04.2023</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System;

    public static class VersionExtensions
    {
        public static VersionResultEnum CompareTo(this Version version, Version otherVersion, int significantParts)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
            if (otherVersion == null)
            {
                return VersionResultEnum.NotEqual;
            }

            if (version.Major != otherVersion.Major && significantParts >= 1)
            {
                if (version.Major > otherVersion.Major)
                {
                    return VersionResultEnum.MajorHigher;
                }
                else
                {
                    return VersionResultEnum.MajorLower;
                }
            }

            if (version.Minor != otherVersion.Minor && significantParts >= 2)
            {
                if (version.Minor > otherVersion.Minor)
                {
                    return VersionResultEnum.MinorHigher;
                }
                else
                {
                    return VersionResultEnum.MinorLower;
                }
            }

            if (version.Build != otherVersion.Build && significantParts >= 3)
            {
                if (version.Build > otherVersion.Build)
                {
                    return VersionResultEnum.BuildHigher;
                }
                else
                {
                    return VersionResultEnum.BuildLower;
                }
            }

            if (version.Revision != otherVersion.Revision && significantParts >= 4)
            {
                if (version.Revision > otherVersion.Revision)
                {
                    return VersionResultEnum.RevisionHigher;
                }
                else
                {
                    return VersionResultEnum.RevisionLower;
                }
            }

            return VersionResultEnum.Equal;
        }
    }

    public enum VersionResultEnum : int 
    {
        Equal = 0,
        NotEqual = 1,
        MajorHigher = 1,
        MajorLower = 2,
        MinorHigher = 3,
        MinorLower = 4,
        BuildHigher = 5,
        BuildLower = 6,
        RevisionHigher = 7,
        RevisionLower = 8,
    }
}
