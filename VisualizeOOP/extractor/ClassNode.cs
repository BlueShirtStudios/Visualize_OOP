using ClassVisibility = GlobalResources.Visibility;

namespace ClassExtractor
{
    public class ClassNode
    {
        public string ClassName { get; set; }
        public string ClassNameSpace { get; set; }
        public ClassVisibility VisibleState { get; set; }
        public List<string> Modifiers = new();
        public Dictionary<string, string> Attributes = new();
        public Dictionary<string, string> Methods = new();

        public ClassNode(string cName)
        {
            ClassName = cName;
            ClassNameSpace = "";
        }
    }
}