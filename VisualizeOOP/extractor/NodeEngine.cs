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

        //Process results
        Console.WriteLine($"Discovered {_extractor.FoundClasses.Count} classes:");
        foreach (var cls in _extractor.FoundClasses)
        {
            Console.WriteLine($"- {cls.Name} ({cls.AccessModifier})");
            Console.WriteLine($"- Attributes: {string.Join(",", cls.Attributes.Select(a => $"{a}"))}");
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