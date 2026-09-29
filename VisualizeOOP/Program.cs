using System;
using ClassExtractor;

//Demo Use
var engine = new NodeEngine(@"C:\Path\To\Folder");
await engine.RunAsyncReadSourcefiles();
engine.EstablishRelationshipsBetweenClasses();
await engine.CreateJSONClassFile();