using Microsoft.Msagl.Drawing;
using Color = Microsoft.Msagl.Drawing.Color;
using Shape = Microsoft.Msagl.Drawing.Shape;

namespace VisualizeOOP
{
    public class GraphBuilderService
    {
        public Graph BuildUmlGraph(List<ClassModel> classes, DiagramModes mode)
        {
            //Initialize the graph
            var graph = new Graph("OOP Class Diagram");
            graph.Attr.LayerDirection = LayerDirection.TB;

            //Convert the passed classes to hashset to ensure no dulicates and fast lookups
            var knownClassNames = new HashSet<string>(classes.Select(c => c.Name));

            //Create nodes for the classes (The block containing the class data)
            foreach (var cls in classes)
            {
                Node node = graph.AddNode(cls.Name);

                node.LabelText = CreateNodeLabel(cls, mode);

                if (node.Label != null)
                {
                    node.Label.FontColor = Color.White;
                }

                node.Attr.Shape = Shape.Box;
                node.Attr.FillColor = new Color(37, 37, 38);
                node.Attr.Color = new Color(86, 156, 214);
            }

            //Connect the classes with their relationships
            foreach (var cls in classes)
            {
                //Class Inheritance (Base Class)
                if (!string.IsNullOrEmpty(cls.ParentClass) && knownClassNames.Contains(cls.ParentClass))
                {
                    Edge edge = graph.AddEdge(cls.Name, cls.ParentClass);
                    edge.Attr.Color = new Color(86, 156, 214);
                    edge.Attr.LineWidth = 2;
                    edge.Attr.ArrowheadAtTarget = ArrowStyle.Generalization;
                }

                //Interface Implementation
                foreach (var iface in cls.Interfaces)
                {
                    if (knownClassNames.Contains(iface))
                    {
                        Edge edge = graph.AddEdge(cls.Name, iface);
                        edge.Attr.Color = new Color(78, 201, 176);
                        edge.Attr.LineWidth = 1;
                        edge.Attr.AddStyle(Style.Dashed);
                        edge.Attr.ArrowheadAtTarget = ArrowStyle.Generalization;
                    }
                }

                //Field/Property References
                var referencedTypes = cls.Fields.Select(f => f.Type)
                    .Concat(cls.Properties.Select(p => p.Type))
                    .Where(t => knownClassNames.Contains(t) && t != cls.Name)
                    .Distinct();

                foreach (var targetType in referencedTypes)
                {
                    Edge edge = graph.AddEdge(cls.Name, targetType);
                    edge.Attr.Color = new Color(150, 150, 150);
                    edge.Attr.LineWidth = 1;
                    edge.Attr.ArrowheadAtTarget = ArrowStyle.Normal;
                }
            }

            return graph;
        }

        private string CreateNodeLabel(ClassModel cls, DiagramModes mode) 
        {
            string label = default;

            switch (mode)
            {
                case DiagramModes.Summary:
                    label = CreateSummaryLabel(cls);
                    break;

                case DiagramModes.Detailed:
                    label = CreateDetailedLabel(cls);
                    break;

                default:
                    label = "Unkown Mode";
                    break;

            }

            return label;

        }

        private string CreateSummaryLabel(ClassModel cls)
        {
            var lines = new List<string>
            {
                $"{cls.AccessModifier} {cls.Name}"
            };

            if (!string.IsNullOrEmpty(cls.ParentClass))
                        lines.Add($"Base: {cls.ParentClass}");

            if (cls.Interfaces.Any())
            {
                lines.Add($"Interfaces: {string.Join(", ", cls.Interfaces)}");
            }

            lines.Add("");
            lines.Add($"Props: {cls.PropertyCount} | Fields: {cls.FieldCount} | Methods: {cls.MethodCount}");

            return string.Join("\n", lines);
        }

        private string CreateDetailedLabel(ClassModel cls)
        {
            var lines = new List<string>
            {
                $"{cls.AccessModifier} {cls.Name}",
                "──────────────"
            };

            if (!string.IsNullOrEmpty(cls.ParentClass))
                lines.Add($"Base: {cls.ParentClass}");

            if (cls.Interfaces.Any())
                lines.Add($"Interfaces: {string.Join(", ", cls.Interfaces)}");

            var properties = cls.Properties
                .Select(m => m.Name);

            if (properties.Any())
            {
                lines.Add("──────────────");
                lines.AddRange(properties);
            }

            var methods = cls.Methods
                .Where(m => m.Visibility == "public" || m.Visibility == "protected")
                .Select(m => $"{m.ReturnType} {m.Name}()");

            if (methods.Any())
            {
                lines.Add("──────────────");
                lines.AddRange(methods);
            }

            return string.Join("\n", lines);
        }

    }

}