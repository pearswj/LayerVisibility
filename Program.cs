using System;
using System.Linq;
using Rhino.DocObjects;
using Rhino.FileIO;

namespace LayerVisibility
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            if (args.Length != 1)
            {
                Console.Error.WriteLine($"Usage: {nameof(LayerVisibility)}.exe file");
                return 1;
            }

            var path = args[0];
            var model = File3dm.Read(path);

            foreach (var layer in model.AllLayers)
            {
                var nest = GetNestLevel(model.AllLayers, layer);
                var indent = string.Concat(Enumerable.Repeat("  ", nest));

                Console.WriteLine($"{indent}- {layer.Name} : IsVisible={layer.IsVisible}, GetPersistentVisibility={layer.GetPersistentVisibility()}");
            }

            return 0;
        }

        static int GetNestLevel(File3dmLayerTable table, Layer layer)
        {
            if (layer.ParentLayerId == Guid.Empty)
                return 0;

            var parent = table.FindId(layer.ParentLayerId);

            return 1 + GetNestLevel(table, parent);
        }
    }
}
