using System.Collections.Generic;

namespace VisualizeOOP
{
    public class ClassModel
    {
        public string Name { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public string AccessModifier { get; set; } = string.Empty;
        public string? ParentClass { get; set; }
        public List<string> Interfaces { get; set; } = new();
        public List<MethodDetailsModel> Methods { get; set; } = new();
        public List<MemberDetailsModel> Fields { get; set; } = new();
        public List<MemberDetailsModel> Properties { get; set; } = new();
        public string FilePath { get; set; } = string.Empty;

        //Derived helper properties for rendering UI labels
        public int MethodCount => Methods?.Count ?? 0;
        public int PropertyCount => Properties?.Count ?? 0;
        public int FieldCount => Fields?.Count ?? 0;
    }

    public class MemberDetailsModel
    {
        public string Visibility { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class MethodDetailsModel
    {
        public string Visibility { get; set; } = string.Empty;
        public string ReturnType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}