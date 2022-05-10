using System.Windows.Media;
using System.Xml.Linq;

namespace SvgToWpf.SvgBuilder
{
    internal class SvgPath : SvgElement
    {
        public SvgPath(XElement element) : base(element)
        {
        }

        public override Geometry CreateGeometry()
        {
            return CreateGeometry(SvgXmlElement);
        }

        private Geometry CreateGeometry(XElement element)
        {
            var path = this.ParseGeometry(this.GetAttributeValue<string>(element, "d"));
            path.Transform = Transform;
            return path;
        }

        /// <summary>
        /// Parse the string as Geometry
        /// </summary>
        /// <param name="pathData">The path data</param>
        /// <returns>The geometry object</returns>
        private Geometry ParseGeometry(string pathData)
        {
            return PathGeometry.CreateFromGeometry(Geometry.Parse(pathData));
        }
    }
}