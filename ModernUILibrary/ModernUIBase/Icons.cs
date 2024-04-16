namespace ModernIU.Base
{
    using System.Windows.Media;

    using Shapes = System.Windows.Shapes;

    public static class IconsDevs
    {
        private const string ICON_COPY = "M19,21H8V7H19M19,5H8A2,2 0 0,0 6,7V21A2,2 0 0,0 8,23H19A2,2 0 0,0 21,21V7A2,2 0 0,0 19,5M16,1H4A2,2 0 0,0 2,3V17H4V3H16V1Z";
        private const string ICON_PASTE = "M19,20H5V4H7V7H17V4H19M12,2A1,1 0 0,1 13,3A1,1 0 0,1 12,4A1,1 0 0,1 11,3A1,1 0 0,1 12,2M19,2H14.82C14.4,0.84 13.3,0 12,0C10.7,0 9.6,0.84 9.18,2H5A2,2 0 0,0 3,4V20A2,2 0 0,0 5,22H19A2,2 0 0,0 21,20V4A2,2 0 0,0 19,2Z";
        private const string ICON_DELETE = "M9,3V4H4V6H5V19A2,2 0 0,0 7,21H17A2,2 0 0,0 19,19V6H20V4H15V3H9M7,6H17V19H7V6M9,8V17H11V8H9M13,8V17H15V8H13Z";
        private const string ICON_CLOCK = "M12,20A8,8 0 0,0 20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22C6.47,22 2,17.5 2,12A10,10 0 0,1 12,2M12.5,7V12.25L17,14.92L16.25,16.15L11,13V7H12.5Z";
        private const string ICON_ADD = "M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z";
        private const string ICON_EQUALS = "M19,10H5V8H19V10M19,16H5V14H19V16Z";
        private const string ICON_MINUS = "M19,13H5V11H19V13Z";
        private const string ICON_MULT = "M11,3H13V10.27L19.29,6.64L20.29,8.37L14,12L20.3,15.64L19.3,17.37L13,13.72V21H11V13.73L4.69,17.36L3.69,15.63L10,12L3.72,8.36L4.72,6.63L11,10.26V3Z";
        private const string ICON_DIV = "M7 21L14.9 3H17L9.1 21H7Z";
        private const string ICON_PERCENT = "M18.5 3.5L20.5 5.5L5.5 20.5L3.5 18.5L18.5 3.5M7 4C8.66 4 10 5.34 10 7C10 8.66 8.66 10 7 10C5.34 10 4 8.66 4 7C4 5.34 5.34 4 7 4M17 14C18.66 14 20 15.34 20 17C20 18.66 18.66 20 17 20C15.34 20 14 18.66 14 17C14 15.34 15.34 14 17 14M7 6C6.45 6 6 6.45 6 7C6 7.55 6.45 8 7 8C7.55 8 8 7.55 8 7C8 6.45 7.55 6 7 6M17 16C16.45 16 16 16.45 16 17C16 17.55 16.45 18 17 18C17.55 18 18 17.55 18 17C18 16.45 17.55 16 17 16Z";
        private const string ICON_MARKALL = "M0.41,13.41L6,19L7.41,17.58L1.83,12M22.24,5.58L11.66,16.17L7.5,12L6.07,13.41L11.66,19L23.66,7M18,7L16.59,5.58L10.24,11.93L11.66,13.34L18,7Z";
        private const string ICON_UNMARK = "M19,3H5C3.89,3 3,3.89 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5C21,3.89 20.1,3 19,3M19,5V19H5V5H19Z";

        public static string IconCopy
        {
            get { return ICON_COPY; }
        }

        public static string IconPaste
        {
            get { return ICON_PASTE; }
        }

        public static string IconDelete
        {
            get { return ICON_DELETE; }
        }

        public static string IconClock
        {
            get { return ICON_CLOCK; }
        }

        public static string IconAdd
        {
            get { return ICON_ADD; }
        }

        public static string IconEquals
        {
            get { return ICON_EQUALS; }
        }

        public static string IconMinus
        {
            get { return ICON_MINUS; }
        }

        public static string IconMult
        {
            get { return ICON_MULT; }
        }

        public static string IconDiv
        {
            get { return ICON_DIV; }
        }

        public static string IconPercent
        {
            get { return ICON_PERCENT; }
        }

        public static string IconCheckAll
        {
            get { return ICON_MARKALL; }
        }

        public static string IconUnCheck
        {
            get { return ICON_UNMARK; }
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <param name="iconColor">Icon Farbe</param>
        /// <returns></returns>
        public static Shapes.Path GetPathGeometry(string iconString, Color iconColor, int size = 24)
        {
            var path = new Shapes.Path
            {
                Height = size,
                Width = size,
                Fill = new SolidColorBrush(iconColor),
                Data = Geometry.Parse(iconString)
            };

            return path;
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <returns></returns>
        public static Shapes.Path GetPathGeometry(string iconString, int size = 24)
        {
            return GetPathGeometry(iconString, Colors.Black, size);
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <returns></returns>
        public static Shapes.Path GetPathGeometry(string iconString)
        {
            return GetPathGeometry(iconString, Colors.Black);
        }
    }
}
