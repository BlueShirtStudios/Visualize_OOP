using System;
using System.Threading.Tasks;
using ClassExtractor;

public class NodeEngine
{
    private readonly SourceFolderExtractor _extractor = default;
    private RelationshipMapper _mapper = default;

    public NodeEngine(string cSearchableFolder)
    {
        _extractor = new SourceFolderExtractor(cSearchableFolder);
        _mapper = new();
    }
    public async Task RunAsync()
    {
        //Must await the asynchronous file search inside an async method
        await _extractor.SearchFolderForSourceFiles();

        //Process results -- Test code
        Console.WriteLine($"Discovered {_extractor.FoundClasses.Count} classes:");
        Console.WriteLine("----------------------------");
        Console.WriteLine("");
        foreach (var cls in _extractor.FoundClasses)
        {
            Console.WriteLine($"- {cls.Name} ({cls.AccessModifier})");
            Console.WriteLine($"- Fields: {string.Join(",", cls.Fields.Select(a => $"{a}"))}");
            Console.WriteLine($"- Properties: {string.Join(",", cls.Properties.Select(a => $"{a}"))}");
            Console.WriteLine($"- Methods: {string.Join(",", cls.Methods.Select(a => $"{a}"))}");
            Console.WriteLine($"- Base Class: {cls.ParentClass}");
            Console.WriteLine($"- Interfaces: {string.Join(",", cls.Interfaces.Select(a => $"{a}"))}");
            Console.WriteLine($"- Filepath: {cls.FilePath}");
            Console.WriteLine("----------------------------");
            Console.WriteLine("");
        }
    }

    public void EstablishRelationshipsBetweenNodes()
    {
        //Sets our found concurrent bag for the mapper
        _mapper.FoundClasses = _extractor.FoundClasses;

        //Builds the dictionary containing each node with its related nodes
        _mapper.MapRelationShips();

        foreach (var cls in _mapper.ClassRelationships)
        {
            Console.WriteLine($"{cls.Key.Name}: {string.Join(", ", cls.Value.Select(n => $"{n}"))}");
        }
    }
}