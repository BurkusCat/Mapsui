﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Samples.Common.Helpers;
using Mapsui.Styles;
using Mapsui.Utilities;

namespace Mapsui.Samples.Common.Maps
{
    public static class SvgSample
    {
        private const string SvgLayerName = "Svg Layer";
        
        public static Map CreateMap()
        {
            var map = new Map();

            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            map.Layers.Add(CreateSvgLayer(map.Envelope));
            map.HoverLayers.Add(map.Layers.First(l => l.Name == SvgLayerName));

            return map;
        }

        private static ILayer CreateSvgLayer(BoundingBox envelope)
        {
            return new MemoryLayer
            {
                Name = SvgLayerName,
                DataSource = CreateMemoryProviderWithDiverseSymbols(envelope, 2000),
                Style = null
            };
        }

        public static MemoryProvider CreateMemoryProviderWithDiverseSymbols(BoundingBox envelope, int count = 100)
        {
            return new MemoryProvider(CreateSvgFeatures(RandomPointHelper.GenerateRandomPoints(envelope, count)));
        }

        private static Features CreateSvgFeatures(IEnumerable<IGeometry> randomPoints)
        {
            var features = new Features();
            var counter = 0;
            foreach (var point in randomPoints)
            {
                var feature = new Feature { Geometry = point, ["Label"] = counter.ToString() };
                feature.Styles.Add(CreateSvgStyle("Mapsui.Samples.Common.Images.Pin.svg", 0.5));
                features.Add(feature);
                counter++;
            }
            return features;
        }

        private static SymbolStyle CreateSvgStyle(string embeddedResourcePath, double scale)
        {
            var bitmapId = GetBitmapIdForEmbeddedResource(embeddedResourcePath);
            return new SymbolStyle { BitmapId = bitmapId, SymbolType = SymbolType.Svg, SymbolScale = scale, SymbolOffset = new Offset(0.0, 0.5, true) };
        }

        private static int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            var assembly = typeof(PointsSample).GetTypeInfo().Assembly;
            var image = assembly.GetManifestResourceStream(imagePath);
            return BitmapRegistry.Instance.Register(image);
        }
    }
}