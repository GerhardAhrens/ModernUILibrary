//-----------------------------------------------------------------------
// <copyright file="OrientedSize.cs" company="Lifeprojects.de">
//     Class: OrientedSize
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.03.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows.Controls;

    internal struct OrientedSize
    {
        public Orientation Orientation { get; }

        public double Direct { get; set; }

        public double Indirect { get; set; }

        public double Width
        {
            get
            {
                return (this.Orientation == Orientation.Vertical) ? this.Direct : this.Indirect;
            }
            set
            {
                if (this.Orientation == Orientation.Vertical)
                {
                    this.Direct = value;
                }
                else
                {
                    this.Indirect = value;
                }
            }
        }

        public double Height
        {
            get
            {
                return (this.Orientation != Orientation.Vertical) ? this.Direct : this.Indirect;
            }
            set
            {
                if (this.Orientation != Orientation.Vertical)
                {
                    this.Direct = value;
                }
                else
                {
                    this.Indirect = value;
                }
            }
        }

        public OrientedSize(Orientation orientation)
        {
            this = new OrientedSize(orientation, 0.0, 0.0);
        }

        public OrientedSize(Orientation orientation, double width, double height)
        {
            this.Orientation = orientation;
            this.Direct = 0.0;
            this.Indirect = 0.0;
            this.Width = width;
            this.Height = height;
        }
    }
}
