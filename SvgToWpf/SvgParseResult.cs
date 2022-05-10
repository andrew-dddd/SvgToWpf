using System.Windows.Media;

namespace SvgToWpf
{
    public sealed class SvgParseResult
    {
        public SvgParseResult(double width, double height, string widthUnits, string heightUnits, PathGeometry pathGeometry)
        {
            Width = width;
            Height = height;
            WidthUnits = widthUnits;
            HeightUnits = heightUnits;
            PathGeometry = pathGeometry;
        }

        public double Width { get; }
        public double Height { get; }
        public string WidthUnits { get; }
        public string HeightUnits { get; }
        public PathGeometry PathGeometry { get; }
    }
}
