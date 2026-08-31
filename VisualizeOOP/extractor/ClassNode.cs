using ClassVisibility = GlobalResources.Visibility;

namespace ClassExtractor
{   
    public record ClassNode(
    
        string Name,
        string Namespace,
        string AccessModifier,
        string? ParentClass,
        List<string> Interfaces,
        List<string> Methods,
        List<string> Attributes,
        string FilePath
    );
}