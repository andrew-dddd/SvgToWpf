using System.Windows.Media;
using System.Xml.Linq;
using System;

namespace SvgToWpf.SvgBuilder
{
    internal class UnknownSvg : SvgElement
    {
        public UnknownSvg(XElement element) : base(element)
        {
        }

        public override Geometry CreateGeometry()
        {
            throw new NotImplementedException();
        }
    }
}