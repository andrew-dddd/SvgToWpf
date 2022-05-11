using System.Windows.Media;
using System.Xml.Linq;
using System.Collections.Generic;

namespace SvgToWpf.SvgBuilder
{
    internal class SvgGroup : SvgElement
    {
        private readonly List<SvgElement> _childElements = new List<SvgElement>();
        private readonly SvgElementFactory _factory;

        public SvgGroup(XElement element, SvgElementFactory factory) : base(element)
        {
            _factory = factory;
            ParseGroupChildElements(SvgXmlElement);
        }

        public override void AddChildElement(SvgElement svgElement)
        {
            _childElements.Add(svgElement);
        }

        public override Geometry CreateGeometry()
        {
            if (_childElements.Count == 0) return null;
            if (_childElements.Count == 1 && _childElements[0] is UnknownSvg) return null;
            if (_childElements.Count == 1)
            {
                return _childElements[0].CreateGeometry();
            }

            CombinedGeometry combinedGeometry = null;
            foreach (var graphicElement in _childElements)
            {
                if (graphicElement is UnknownSvg) continue;

                combinedGeometry = new CombinedGeometry(GeometryCombineMode.Union, combinedGeometry ?? Geometry.Empty, graphicElement.CreateGeometry());
            }

            combinedGeometry.Transform = Transform;
            return combinedGeometry;
        }

        private void ParseGroupChildElements(XElement groupElement)
        {
            foreach (var elementInGroup in groupElement.Elements())
            {
                AddChildElement(_factory.ParseSvgElement(elementInGroup));
            }
        }
    }
}