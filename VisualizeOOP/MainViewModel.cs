using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Msagl.Drawing;
using ClassExtractor;

namespace VisualizeOOP
{
    public class MainViewModel
    {
        private readonly GraphBuilderService _graphBuilder = new();

        public ObservableCollection<ClassModel> Classes { get; set; } = new();
        public Graph CurrentGraph { get; private set; } = new("Empty");

        public async Task ProcessAndLoadGraphAsync(string targetFolderPath)
        {
            //Run the analyzer to create the JSON files
            var engine = new OOPAnalyzerEngine(targetFolderPath);
            await engine.RunAsyncReadSourcefiles();
            engine.EstablishRelationshipsBetweenClasses();
            await engine.CreateJSONClassFile();

            //Build expected path and read the generated classes from the json file
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "classes.json");
            await LoadClassesFromJsonAsync(jsonPath);
        }

        public async Task LoadClassesFromJsonAsync(string jsonPath)
        {
            //Check if that file existed 
            if (!File.Exists(jsonPath)) return;

            //Read from the JSON file
            string jsonContent = await File.ReadAllTextAsync(jsonPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            //Deserialize the JSON to a list of ClassModel Objects
            List<ClassModel>? loadedClasses = JsonSerializer.Deserialize<List<ClassModel>>(jsonContent, options);

            //Checks if there were successfully classes extracted
            if (loadedClasses != null)
            {
                //If there were
                //Clear class tracker
                Classes.Clear();

                //Add the new classes to the list of read classes
                foreach (var cls in loadedClasses)
                {
                    Classes.Add(cls);
                }

                //Build the graph with the newly read classes
                CurrentGraph = _graphBuilder.BuildUmlGraph(loadedClasses);
            }
        }
    }
}