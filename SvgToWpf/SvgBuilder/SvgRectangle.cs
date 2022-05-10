using System.Windows.Media;
using System.Xml.Linq;
using System.Windows;

namespace SvgToWpf.SvgBuilder
{
    internal class SvgRectangle : SvgElement
    {
        public SvgRectangle(XElement element) : base(element)
        {
        }

        public override Geometry CreateGeometry()
        {
            return ParseRectangleGeometry(Element);
        }

        private Geometry ParseRectangleGeometry(XElement rectangle)
        {
            var radiusY = this.GetAttributeValue<double>(rectangle, "y");
            var radiusX = this.GetAttributeValue<double>(rectangle, "x");
            var rect = new Rect
            {
                Width = this.GetAttributeValue<double>(rectangle, "width"),
                Height = this.GetAttributeValue<double>(rectangle, "height"),
                X = radiusX,
                Y = radiusY
            };

            var rectGeometry = new RectangleGeometry(rect, 0, 0, Transform);
            return rectGeometry;
        }
    }
}