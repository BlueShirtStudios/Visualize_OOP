using System;
using System.Threading.Tasks;
using ClassExtractor;

public class NodeEngine
{
    private readonly SourceFolderExtractor _extractor = default;
    private RelationshipMapper _mapper = default;
    private ClassToJSONConvertor _writer = default;

    public NodeEngine(string cSearchableFolder)
    {
        _extractor = new SourceFolderExtractor(cSearchableFolder);
        _mapper = new();
        _writer = new();
    }
    public async Task RunAsyncReadSourcefiles()
    {
        //Must await the asynchronous file search inside an async method
        await _extractor.SearchFolderForSourceFiles();
        
    }

    public void EstablishRelationshipsBetweenClasses()
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

    public async Task CreateJSONClassFile()
    {
        //Writes all the class to the running directory in a file called classes.json
        await _writer.WriteToJSONFile(_extractor.FoundClasses.ToList());
    }
}