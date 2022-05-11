using FluentAssertions;
using System.Windows.Media;
using System.Xml.Linq;
using Xunit;

namespace SvgToWpf.Tests
{
    public class SvgParserTests
    {
        private readonly SvgParser _svgParser;

        public SvgParserTests()
        {
            _svgParser = new SvgParser(SvgElementFactory.DefaultFactory());
        }

        [Fact]
        public void SvgBuilder_ShouldReadCurve()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\curve.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
            (pg as CombinedGeometry).Geometry2.Should().BeOfType<PathGeometry>();
        }

        [Fact]
        public void SvgBuilder_ShouldReadEllipse()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\ellipse.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
            (pg as CombinedGeometry).Geometry2.Should().BeOfType<EllipseGeometry>();
        }

        [Fact]
        public void SvgBuilder_ShouldReadParametricCurve_AsPathGeometry()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\parametric_curve.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
            (pg as CombinedGeometry).Geometry2.Should().BeOfType<PathGeometry>();
        }

        [Fact]
        public void SvgBuilder_ShouldReadPolygon_AsPathGeometry()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\polygon.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
            (pg as CombinedGeometry).Geometry2.Should().BeOfType<PathGeometry>();
        }

        [Fact]
        public void SvgBuilder_ShouldRead_CoupleDifferentShapes()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\random.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
        }

        [Fact]
        public void SvgBuilder_ShouldReadRectangle()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\rectangle.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
            (pg as CombinedGeometry).Geometry2.Should().BeOfType<RectangleGeometry>();
        }

        [Fact]
        public void SvgBuilder_ShouldReadSpiral_AsPathGeometry()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\spiral.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
            (pg as CombinedGeometry).Geometry2.Should().BeOfType<PathGeometry>();
        }

        [Fact]
        public void SvgBuilder_ShouldRandomText_WithTranslateTransform()
        {
            // Arrange
            var svg = XDocument.Load(".\\Files\\random_text.svg");

            // Act
            var builder = _svgParser.CreateSvgBuilder(svg.Root);
            var pg = builder.CreateGeometry();

            // Assert
            (pg as CombinedGeometry).Geometry2.Should().BeOfType<CombinedGeometry>();

            var transform = ((pg as CombinedGeometry).Geometry2 as CombinedGeometry).Geometry2.Transform as TranslateTransform;

            transform.Should().NotBeNull();
            transform.X.Should().Be(-26.39142D);
            transform.Y.Should().Be(9.53669D);
        }
    }
}
