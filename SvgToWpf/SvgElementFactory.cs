using System;
using System.Collections.Generic;
using System.Xml.Linq;
using SvgToWpf.SvgBuilder;

namespace SvgToWpf
{
    public class SvgElementFactory
    {
        private readonly Dictionary<string, Func<XElement, SvgElementFactory, SvgElement>> _elementFactories = new Dictionary<string, Func<XElement, SvgElementFactory, SvgElement>>();

        public SvgElementFactory()
        {
        }

        public SvgElementFactory ConfigureElementFactory(string elementName, Func<XElement, SvgElementFactory, SvgElement> factory)
        {
            if (_elementFactories.ContainsKey(elementName))
            {
                throw new InvalidOperationException($"Factory for element {elementName} already exist");
            }

            _elementFactories.Add(elementName, factory);
            return this;
        }

        public SvgElementFactory ConfigureElementFactory(Func<XElement, SvgElementFactory, SvgElement> factory, params string[] elementNames)
        {
            foreach (var elementName in elementNames)
            {
                ConfigureElementFactory(elementName, factory);
            }

            return this;
        }

        internal SvgElement ParseSvgElement(XElement element)
        {
            if (!_elementFactories.TryGetValue(element.Name.LocalName.ToLower(), out var factory))
            {
                return new UnknownSvg(element);
            }

            return factory(element, this);
        }

        public static SvgElementFactory DefaultFactory()
        {
            var defaultFactory = new SvgElementFactory();
            defaultFactory.ConfigureElementFactory((element, factory) => new SvgGroup(element, factory), "g");
            defaultFactory.ConfigureElementFactory((element, factory) => new SvgPath(element), "path");
            defaultFactory.ConfigureElementFactory((element, factory) => new SvgRectangle(element), "rectangle", "rect");
            defaultFactory.ConfigureElementFactory((element, factory) => new SvgEllipse(element), "ellipse");
            defaultFactory.ConfigureElementFactory((element, factory) => new SvgPath(element), "polygon");

            return defaultFactory;
        }
    }
}