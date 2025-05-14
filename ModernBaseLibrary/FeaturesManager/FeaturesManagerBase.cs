/*
 * <copyright file="FeaturesManagerBase.cs" company="Lifeprojects.de">
 *     Class: FeaturesManagerBase
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.01.2023 15:47:23</date>
 * <Project>ModernBaseLibrary</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
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

namespace ModernBaseLibrary.Features
{
    using System;

    using ModernBaseLibrary.Core;

    public abstract class FeaturesManagerBase : DisposableCoreBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturesManagerBase"/> class.
        /// </summary>
        protected FeaturesManagerBase()
        {
        }

        public abstract void InitFeatures();
    }
}
