using System;
using ClassExtractor;

var engine = new NodeEngine(@"C:\Danie\BlueShirtsStudio\VisualizeOOP\VisualizeOOP_Commandline");
await engine.RunAsync();
engine.EstablishRelationshipsBetweenNodes();