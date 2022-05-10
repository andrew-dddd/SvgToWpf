using System.Windows.Media;
using System.Xml.Linq;

namespace SvgToWpf.SvgBuilder
{
    internal class SvgPolygon : SvgElement
    {
        public SvgPolygon(XElement element) : base(element)
        {
        }

        public override Geometry CreateGeometry()
        {
            return ParsePolygon(Element);
        }

        private Geometry ParsePolygon(XElement poly)
        {
            var polygon = this.ParseGeometry(string.Format("M{0}Z", this.GetAttributeValue<string>(poly, "points")));
            polygon.Transform = Transform;
            return polygon;
        }

        /// <summary>
        /// Parse the string as Geometry
        /// </summary>
        /// <param name="pathData">The path data</param>
        /// <returns>The geometry object</returns>
        private Geometry ParseGeometry(string pathData)
        {
            return PathGeometry.CreateFromGeometry(Geometry.Parse(pathData).Clone());
        }
    }
}