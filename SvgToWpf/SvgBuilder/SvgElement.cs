using System.Windows.Media;
using System.Xml.Linq;
using System.Linq;
using System.Globalization;
using System;
using System.Collections.Generic;

namespace SvgToWpf.SvgBuilder
{
    public abstract class SvgElement
    {
        protected readonly XElement SvgXmlElement;
        protected readonly Transform Transform;

        protected SvgElement(XElement element)
        {
            SvgXmlElement = element;
            Transform = CreateElementTransform(element);
        }

        public abstract Geometry CreateGeometry();

        public virtual void AddChildElement(SvgElement svgElement)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the attribue value
        /// </summary>
        /// <param name="element">The XML Element</param>
        /// <param name="attribute">The attribute name</param>
        /// <returns>The attribute value</returns>
        protected T GetAttributeValue<T>(XElement element, string attribute)
        {
            var value = string.Empty;

            if (element != null && !string.IsNullOrEmpty(attribute))
            {
                value = element.Attribute(attribute)?.Value;
            }

            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            return (T)System.Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the descendants
        /// </summary>
        /// <param name="parentElement">The parent element</param>
        /// <param name="descendantName">The descendant name</param>
        /// <returns>Descendants list</returns>
        protected List<XElement> GetDescendants(XElement parentElement, string descendantName)
        {
            var descendants = new List<XElement>();
            if (parentElement != null && !string.IsNullOrEmpty(descendantName))
            {
                descendants =
                    (from svgPath in parentElement.Descendants(this.GetDescendantQuery(descendantName)) select svgPath)
                        .ToList();
            }

            return descendants;
        }

        /// <summary>
        /// Creates the descendants query string
        /// </summary>
        /// <param name="descendantName">The descendant name</param>
        /// <returns>The descendant query</returns>
        private string GetDescendantQuery(string descendantName)
        {
            var descendantQuery = string.Empty;

            if (!string.IsNullOrEmpty(descendantName))
            {
                descendantQuery = string.Format("{{http://www.w3.org/2000/svg}}{0}", descendantName);
            }

            return descendantQuery;
        }


        private Transform CreateElementTransform(XElement element)
        {
            Transform parentTransform = null;
            var parent = (from svgPath in element?.Attributes("transform") select svgPath).ToList();
            if (parent.Any())
            {
                parentTransform = this.GetTransform(parent[0]?.Value);
            }

            return parentTransform?.Clone();
        }

        private Transform GetTransform(string transForm)
        {
            Transform transform = null;
            if (!string.IsNullOrEmpty(transForm))
            {
                var transformType = transForm.Substring(0, transForm.IndexOf("(", StringComparison.Ordinal));
                var transformArray = this.GetTransformArray(transForm);
                if (transformArray.Any())
                {
                    switch (transformType)
                    {
                        case "rotate":
                            transform = this.GetUnderlyingTransform(TransformType.RotateTransform, transformArray);
                            break;
                        case "translate":
                            transform = this.GetUnderlyingTransform(TransformType.TranslateTransform, transformArray);
                            break;
                        case "matrix":
                            transform = this.GetUnderlyingTransform(TransformType.MatrixTransform, transformArray);
                            break;
                    }
                }
            }

            return transform?.Clone();
        }

        /// <summary>
        /// Get transform array
        /// </summary>
        /// <param name="transform">The transform string</param>
        /// <returns>The transform array</returns>
        private double[] GetTransformArray(string transform)
        {
            double[] transformArray = new double[] { };
            if (!string.IsNullOrEmpty(transform))
            {
                return transform.Split('(').Last().TrimEnd(')').Split(',').Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            }

            return transformArray;
        }

        /// <summary>
        /// Get the transformations
        /// </summary>
        /// <param name="transformType">Type of transform type</param>
        /// <param name="parameters">The transform parameters</param>
        /// <returns>The transform</returns>
        private Transform GetUnderlyingTransform(TransformType transformType, params double[] parameters)
        {
            Transform transform = null;
            switch (transformType)
            {
                case TransformType.RenderTransform:
                    break;
                case TransformType.RotateTransform:
                    transform = new RotateTransform
                    {
                        Angle = parameters[0],
                        CenterX = parameters[1],
                        CenterY = parameters[2]
                    };
                    break;
                case TransformType.TranslateTransform:
                    transform = new TranslateTransform { X = parameters.ElementAtOrDefault(0), Y = parameters.ElementAtOrDefault(1) };
                    break;
                case TransformType.MatrixTransform:
                    transform = new MatrixTransform(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                    break;
            }

            return transform?.Clone();
        }
    }
}