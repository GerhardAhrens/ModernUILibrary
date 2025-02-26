namespace ModernIU.Controls
{
    using System.Windows.Media;

    using Shapes = System.Windows.Shapes;

    public static class Icons
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
        private const string ICON_FORMAT_BOLD = "M13.5,15.5H10V12.5H13.5A1.5,1.5 0 0,1 15,14A1.5,1.5 0 0,1 13.5,15.5M10,6.5H13A1.5,1.5 0 0,1 14.5,8A1.5,1.5 0 0,1 13,9.5H10M15.6,10.79C16.57,10.11 17.25,9 17.25,8C17.25,5.74 15.5,4 13.25,4H7V18H14.04C16.14,18 17.75,16.3 17.75,14.21C17.75,12.69 16.89,11.39 15.6,10.79Z";
        private const string ICON_FORMAT_ITALIC = "M10,4V7H12.21L8.79,15H6V18H14V15H11.79L15.21,7H18V4H10Z";
        private const string ICON_FORMAT_UNDERLINE = "M5,21H19V19H5V21M12,17A6,6 0 0,0 18,11V3H15.5V11A3.5,3.5 0 0,1 12,14.5A3.5,3.5 0 0,1 8.5,11V3H6V11A6,6 0 0,0 12,17Z";
        private const string ICON_FORMAT_STRIKETHROUGH = "M7.2 9.8C6 7.5 7.7 4.8 10.1 4.3C13.2 3.3 17.7 4.7 17.6 8.5H14.6C14.6 8.2 14.5 7.9 14.5 7.7C14.3 7.1 13.9 6.8 13.3 6.6C12.5 6.3 11.2 6.4 10.5 6.9C9 8.2 10.4 9.5 12 10H7.4C7.3 9.9 7.3 9.8 7.2 9.8M21 13V11H3V13H12.6C12.8 13.1 13 13.1 13.2 13.2C13.8 13.5 14.3 13.7 14.5 14.3C14.6 14.7 14.7 15.2 14.5 15.6C14.3 16.1 13.9 16.3 13.4 16.5C11.6 17 9.4 16.3 9.5 14.1H6.5C6.4 16.7 8.6 18.5 11 18.8C14.8 19.6 19.3 17.2 17.3 12.9L21 13Z";
        private const string ICON_INSERT_IMAGE = "M19,19H5V5H19M19,3H5A2,2 0 0,0 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5A2,2 0 0,0 19,3M13.96,12.29L11.21,15.83L9.25,13.47L6.5,17H17.5L13.96,12.29Z";
        private const string ICON_INSERT_LINK = "M3.9,12C3.9,10.29 5.29,8.9 7,8.9H11V7H7A5,5 0 0,0 2,12A5,5 0 0,0 7,17H11V15.1H7C5.29,15.1 3.9,13.71 3.9,12M8,13H16V11H8V13M17,7H13V8.9H17C18.71,8.9 20.1,10.29 20.1,12C20.1,13.71 18.71,15.1 17,15.1H13V17H17A5,5 0 0,0 22,12A5,5 0 0,0 17,7Z";
        private const string ICON_CHANGE_COLOR = "M19,11.5C19,11.5 17,13.67 17,15A2,2 0 0,0 19,17A2,2 0 0,0 21,15C21,13.67 19,11.5 19,11.5M5.21,10L10,5.21L14.79,10M16.56,8.94L7.62,0L6.21,1.41L8.59,3.79L3.44,8.94C2.85,9.5 2.85,10.47 3.44,11.06L8.94,16.56C9.23,16.85 9.62,17 10,17C10.38,17 10.77,16.85 11.06,16.56L16.56,11.06C17.15,10.47 17.15,9.5 16.56,8.94Z";
        private const string ICOM_3DOT_MENU = "M12,16A2,2 0 0,1 14,18A2,2 0 0,1 12,20A2,2 0 0,1 10,18A2,2 0 0,1 12,16M12,10A2,2 0 0,1 14,12A2,2 0 0,1 12,14A2,2 0 0,1 10,12A2,2 0 0,1 12,10M12,4A2,2 0 0,1 14,6A2,2 0 0,1 12,8A2,2 0 0,1 10,6A2,2 0 0,1 12,4Z";

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

        public static string IconFormatBold
        {
            get { return ICON_FORMAT_BOLD; }
        }

        public static string IconFormatItalic
        {
            get { return ICON_FORMAT_ITALIC; }
        }

        public static string IconFormatUnderline
        {
            get { return ICON_FORMAT_UNDERLINE; }
        }

        public static string IconFormatStrikethrough
        {
            get { return ICON_FORMAT_STRIKETHROUGH; }
        }

        public static string IconInsertInmage
        {
            get { return ICON_INSERT_IMAGE; }
        }

        public static string IconInsertLink
        {
            get { return ICON_INSERT_LINK; }
        }

        public static string IconChangeColor
        {
            get { return ICON_CHANGE_COLOR; }
        }

        public static string Icon3DotMenu
        {
            get { return ICOM_3DOT_MENU; }
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

        public static GeometryDrawing PathToGeometryIcon(string pathData, Brush pathColor = null)
        {
            if (pathColor == null)
            {
                pathColor = Brushes.Orange;
            }

            Geometry vectorIcon = Geometry.Parse(pathData);
            GeometryDrawing geometryIcon = new GeometryDrawing();
            geometryIcon.Brush = pathColor;
            geometryIcon.Geometry = vectorIcon;
            return geometryIcon;
        }
    }
}
