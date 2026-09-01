namespace ClassExtractor
{   
    public record ClassNode(
    
        string Name,
        string Namespace,
        string AccessModifier,
        string? ParentClass,
        List<string> Interfaces,
        List<string> Methods,
        List<MemberDetails> Fields,
        List<MemberDetails> Properties,
        string FilePath
    );
}