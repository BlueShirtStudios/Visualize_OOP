using System;
using System.Threading.Tasks;
using ClassExtractor;

public class NodeEngine
{
    private readonly SourceFolderExtractor _extractor = default;

    public NodeEngine(string cSearchableFolder)
    {
        _extractor = new SourceFolderExtractor(cSearchableFolder);
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
        }
    }
}