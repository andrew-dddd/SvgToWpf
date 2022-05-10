using System.Windows.Media;
using System.Xml.Linq;

namespace SvgToWpf.SvgBuilder
{
    internal class SvgEllipse : SvgElement
    {
        public SvgEllipse(XElement element) : base(element)
        {
        }

        public override Geometry CreateGeometry()
        {
            return CreateEllipseGeometry(Element);
        }

        private Geometry CreateEllipseGeometry(XElement element)
        {
            var cx = GetAttributeValue<double>(element, "cx");
            var cy = GetAttributeValue<double>(element, "cy");

            var radiusX = GetAttributeValue<double>(element, "rx");
            var radiusY = GetAttributeValue<double>(element, "ry");

            return new EllipseGeometry(new System.Windows.Point(cx, cy), radiusX, radiusY, Transform);
        }
    }
}
