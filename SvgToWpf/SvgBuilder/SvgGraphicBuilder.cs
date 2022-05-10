using System.Collections.Generic;
using System.Windows.Media;

namespace SvgToWpf.SvgBuilder
{
    internal class SvgGraphicBuilder
    {
        private readonly List<SvgElement> _graphicElementsList;

        public IReadOnlyList<SvgElement> GraphicElements { get; }

        public SvgGraphicBuilder()
        {
            _graphicElementsList = new List<SvgElement>();
            GraphicElements = _graphicElementsList.AsReadOnly();
        }

        internal void AddElement(SvgElement element)
        {
            _graphicElementsList.Add(element);
        }

        public Geometry CreateGeometry()
        {
            if (_graphicElementsList.Count == 0) return null;
            if (_graphicElementsList.Count == 1 && _graphicElementsList[0] is UnknownSvg) return null;
            if (_graphicElementsList.Count == 1)
            {
                return _graphicElementsList[0].CreateGeometry();
            }

            CombinedGeometry combinedGeometry = null;

            foreach (var graphicElement in _graphicElementsList)
            {
                if (graphicElement is UnknownSvg) continue;

                combinedGeometry = new CombinedGeometry(GeometryCombineMode.Union, combinedGeometry ?? Geometry.Empty, graphicElement.CreateGeometry());
            }

            return combinedGeometry;
        }
    }
}