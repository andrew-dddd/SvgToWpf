namespace SvgToWpf
{
    public interface ISvgParser
    {
        SvgParseResult ParseSvg(string svgFile);
    }
}
