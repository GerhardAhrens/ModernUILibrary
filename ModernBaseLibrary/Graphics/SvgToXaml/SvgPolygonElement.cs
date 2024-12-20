
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Media;
    using System.Xml.Linq;

    internal class SvgPolygonElement : SvgDrawableBaseElement
    {
        public readonly List<SvgPoint> Points = new List<SvgPoint>();


        public SvgPolygonElement(SvgDocument document, SvgBaseElement parent, XElement polygonElement)
          : base(document, parent, polygonElement)
        {
            XAttribute points_attribute = polygonElement.Attribute("points");
            if (points_attribute != null)
            {
                List<double> coordinates = new List<double>();

                string[] points = points_attribute.Value.Split(',', ' ', '\t');
                foreach (string coordinate_value in points)
                {
                    string coordinate = coordinate_value.Trim();
                    if (coordinate == "")
                        continue;
                    coordinates.Add(Double.Parse(coordinate, CultureInfo.InvariantCulture.NumberFormat));
                }

                for (int i = 0; i < coordinates.Count - 1; i += 2)
                    Points.Add(new SvgPoint(coordinates[i], coordinates[i + 1]));
            }
        }

        public override Geometry GetBaseGeometry()
        {
            if (Points.Count == 0)
                return null;

            PathFigure path_figure = new PathFigure();

            path_figure.StartPoint = Points[0].ToPoint();
            path_figure.IsClosed = true;
            path_figure.IsFilled = true;

            for (int i = 1; i < Points.Count; ++i)
                path_figure.Segments.Add(new LineSegment(Points[i].ToPoint(), true));

            PathGeometry path_geometry = new PathGeometry();
            path_geometry.Figures.Add(path_figure);

            return path_geometry;
        }
    }
}
