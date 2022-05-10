using System.Xml.Linq;
using System.Globalization;
using SvgToWpf.SvgBuilder;

namespace SvgToWpf
{
    public class SvgParser : ISvgParser
    {
        public SvgParseResult ParseSvg(string svgFilePath)
        {
            var svgDocument = XDocument.Load(svgFilePath);
            var svg = svgDocument.Root;

            if (!TryParseLength(svg.Attribute("width").Value, out var width, out var widthUnits)) throw new System.Exception("Unknown unit");
            if (!TryParseLength(svg.Attribute("height").Value, out var height, out var heightUnits)) throw new System.Exception("Unknown unit");

            var pg = CreateSvgBuilder(svg);

            var targetGeometry = pg.CreateGeometry().GetOutlinedPathGeometry();
            return new SvgParseResult(width, height, widthUnits, heightUnits, targetGeometry);
        }

        private static bool TryParseLength(string input, out double length, out string units)
        {
            units = null;
            if (TryParse(input, out length))
            {
                units = "px";
            }
            else if (input.EndsWith("px") && TryParse(input.Replace("px", ""), out length))
            {
                units = "px";
            }
            else if (input.EndsWith("mm") && TryParse(input.Replace("mm", ""), out length))
            {
                units = "mm";
            }
            else if (input.EndsWith("in") && TryParse(input.Replace("in", ""), out length))
            {
                units = "in";
            }

            return length != 0 && units != null;
        }

        private static bool TryParse(string s, out double value) => double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);

        internal SvgGraphicBuilder CreateSvgBuilder(XElement svgRootElement)
        {
            SvgGraphicBuilder svg = new SvgGraphicBuilder();
            foreach (var svgElement in svgRootElement.Elements())
            {
                var parsedElement = ParseSvgElement(svgElement);
                svg.AddElement(parsedElement);
            }

            return svg;
        }

        private SvgElement ParseSvgElement(XElement element)
        {
            switch (element.Name.LocalName.ToLower())
            {
                default:
                    return new UnknownSvg(element);

                case "g":
                    SvgGroup group = new SvgGroup(element);
                    ParseGroupChildElements(group, element);
                    return group;
                case "path":
                    return new SvgPath(element);
                case "rectangle":
                case "rect":
                    return new SvgRectangle(element);
                case "ellipse":
                    return new SvgEllipse(element);
                case "polygon":
                    return new SvgPolygon(element);
            }
        }

        private void ParseGroupChildElements(SvgGroup group, XElement groupElement)
        {
            foreach (var elementInGroup in groupElement.Elements())
            {
                group.AddChildElement(ParseSvgElement(elementInGroup));
            }
        }
    }
}