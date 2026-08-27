using ClassVisibility = GlobalResources.Visibility;
using System.IO;
using System.Collections.Concurrent;
using static ClassExtractor.ClassNode;

namespace ClassExtractor
{
    public class SourceFolderReader
    {
        public string FolderPath { get; set; }
    }
}