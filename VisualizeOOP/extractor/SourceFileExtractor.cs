using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static ClassExtractor.ClassNode;

namespace ClassExtractor
{
    public class SourceFolderExtractor
    {
        public string FolderPath { get; set; }
        public ConcurrentBag<ClassNode> FoundClasses = new ConcurrentBag<ClassNode>();
        public IEnumerable<string> SourceFiles { get; set; }

        public SourceFolderExtractor(string cFolderPath)
        {
            FolderPath = cFolderPath;
            SourceFiles = Directory.EnumerateFiles(FolderPath, "*.cs", SearchOption.AllDirectories);
        }

        public async Task SearchFolderForSourceFiles()
        {
            //Cancelation Token for stopping discovery
            using var cts = new CancellationTokenSource();

            
            await Parallel.ForEachAsync(SourceFiles, cts.Token, async (filePath, token) =>
            {
                //Read from the source file 
                string readCode = await File.ReadAllTextAsync(filePath, token);

                //Converts to roslyn tree
                SyntaxTree tree = CSharpSyntaxTree.ParseText(readCode, cancellationToken: token);

                //Start point of the file
                CompilationUnitSyntax root = tree.GetCompilationUnitRoot(token);

                //Extract all the classes within the file
                var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                //Go through each file
                foreach (var cls in classDeclarations)
                {
                    //Read all the Details from the class
                    string clsName = ExtractClassName(cls);
                    string clsNamespace = ExtractNamespace(cls);
                    string visibility = ExtractVisibility(cls);
                    ClassInheritanceExtraction inheritanceSituation = ExtractInheritance(cls);
                    List<string> methods = ExtractMethods(cls);
                    List<string> properties = ExtractProperties(cls);

                    //Create a new node for the class
                    ClassNode node = new(
                        Name: clsName,
                        Namespace: clsNamespace,
                        AccessModifier: visibility,
                        ParentClass: inheritanceSituation.BaseClass,
                        Interfaces: inheritanceSituation.Interfaces,
                        Methods: methods,
                        Attributes: properties,
                        FilePath: filePath
                        );

                    //Add node to the list of classes
                    FoundClasses.Add(node);
                }


            });
        }

        private string ExtractClassName(ClassDeclarationSyntax cls)
        {
            return cls.Identifier.Text;
        }

        private string ExtractNamespace(ClassDeclarationSyntax cls)
        {
            return cls.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault()?.Name.ToString() ?? "Global";
        }

        private string ExtractVisibility(ClassDeclarationSyntax cls)
        {
            //Gets the visibility of the class
            string visibility =  cls.Modifiers.FirstOrDefault(m => m.IsKind(SyntaxKind.PublicKeyword) || 
                                                m.IsKind(SyntaxKind.PrivateKeyword) ||
                                                m.IsKind(SyntaxKind.InternalKeyword) ||
                                                m.IsKind(SyntaxKind.ProtectedKeyword))
                                                .Text;

            //Defaults to internal if no modifiers are found
            if (String.IsNullOrEmpty(visibility))
            {
                visibility = "internal";
            }

            return visibility;
        }

        private ClassInheritanceExtraction ExtractInheritance(ClassDeclarationSyntax cls)
        {
            //Create new record to return at end
            string baseClass = null;
            List<string> interfaces = new();

            //Check if the cls has a base list
            if (cls.BaseList != null)
            {
                //Loop through each item and determine if they are an interface or the base
                foreach (var baseType in cls.BaseList.Types)
                {
                    string typeName = baseType.Type.ToString();

                    //Check for interface
                    if (typeName.StartsWith("I") && typeName.Length > 1 && char.IsUpper(typeName[1]))
                    {
                        interfaces.Add(typeName);
                    }

                    else if (baseClass == null)
                    {
                        baseClass = typeName;
                    }
                }
            }

            //Create new record for returnal
            ClassInheritanceExtraction inheritanceSituation = new ClassInheritanceExtraction(baseClass, interfaces);

            //Return result
            return inheritanceSituation;
        }

        private List<string> ExtractMethods(ClassDeclarationSyntax cls)
        {
            return cls.Members
                .OfType<MethodDeclarationSyntax>()
                .Select(m => m.Identifier.Text)
                .ToList();
        }

        private List<string> ExtractProperties(ClassDeclarationSyntax cls)
        {
            return cls.Members
                .OfType<FieldDeclarationSyntax>()
                .SelectMany(f => f.Declaration.Variables.Select(v => v.Identifier.Text))
                .ToList();
        }
    }

    internal record ClassInheritanceExtraction(
        string BaseClass,
        List<string> Interfaces
        );
}