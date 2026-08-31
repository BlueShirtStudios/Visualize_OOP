using ClassExtractor;
using System;
using System.Collections;
using System.Collections.Concurrent;

namespace ClassExtractor
{
    public class RelationshipMapper
    {
        Dictionary<ClassNode, List<ClassNode>> classRelationships = new();
        ConcurrentBag<ClassNode> foundClassess = new();

        public Dictionary<ClassNode, List<ClassNode>> ClassRelationships 
        { 
            get { return this.classRelationships; }
        }

        public ConcurrentBag<ClassNode> FoundClasses
        {
            get { return this.foundClassess; }
            set { this.foundClassess = value; }
        }

        public void MapRelationShips()
        {
            //Go through each class to determine their relationships
            foreach (var cls in FoundClasses)
            {
                //Get our target's relationships
                List<string> clsRelationships = GetClassRelationships(cls);

                //Fetch the node value of the sting names and assign it to the node's key
                if (!classRelationships.TryAdd(cls, FetchNodes(clsRelationships)))
                {
                    //If the addition fails - skips the issue class
                    continue;
                }

            }
        }

        private List<string> GetClassRelationships(ClassNode node)
        {
            List<string> classRelationships = new();

            //Add parent
            classRelationships.Add(node.ParentClass);

            //Add the interfaces of the class
            foreach (var nInterface in node.Interfaces)
            {
                classRelationships.Add(nInterface);
            }

            return classRelationships;
        }

        private List<ClassNode> FetchNodes(List<string> targets)
        {
            List<ClassNode> nodeValues = new();

            //Go through each node found
            foreach (var node in FoundClasses)
            {
                //Go through each presented target's name
                foreach (var tarName in targets)
                {
                    //Checks if current node is same as taget's name
                    if (node.Name == tarName)
                    {
                        //If it is a match
                        nodeValues.Add(node);

                    }
                }
            }

            //After finding all string name node pair, return it back
            return nodeValues;
        }

        
    }
}
