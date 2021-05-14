using System;
using System.IO;
using System.Collections.Generic;
using Its.K8SUtils.Options;
using Its.K8SUtils.Templates;
using Its.K8SUtils.Processors.Comparers.Models;
using Serilog;

namespace Its.K8SUtils.Processors.Comparers
{
    class MapGrouping
    {        
        public string FileName = "";
        public Dictionary<string, string> EntireFieldsMap = new Dictionary<string, string>();

        public Dictionary<string, int> FieldsStatMap = new Dictionary<string, int>();

        public void IncreaseStatCount(string key)
        {
            int current = FieldsStatMap.GetValueOrDefault(key, 0);
            current++;

            FieldsStatMap[key] = current;
        }
    }

    class DupGrouping
    {
        public Dictionary<string, string> EntireFieldsMap = new Dictionary<string, string>();
        public Dictionary<string, string> StatFieldsMap = new Dictionary<string, string>();
    }

    public class ResourcesComparer : BaseProcessor
    {        
        private ITemplateEngine template = new RazorTemplateEngine();

        private List<string> excludedList = new List<string>() { "Event", "ReplicaSet" };

        private List<string> allUnionRows = new List<string>();
        private List<string> allUnionStatKeys = new List<string>();

        private MapGrouping oldGroup = new MapGrouping();
        private MapGrouping newGroup = new MapGrouping();
    
        public override string Do()
        {
            var opt = options as CompareOptions;

            oldGroup.FileName = opt.BasedFilePath;
            newGroup.FileName = opt.NewFilePath;

            PopulateRows();

            var derivedRows = AnalyzeRows();
            WriteFile(derivedRows);
            
            return "";
        }

        public void SetTemplateEngine(ITemplateEngine engine)
        {
            template = engine;
        }

        public void SetExcludedList(List<string> execList)
        {
            excludedList = execList;
        }

        private List<string> AnalyzeRows()
        {
            var rows = new List<string>();

            foreach (string key in allUnionStatKeys)
            {
                int oldValue = 0;
                if (oldGroup.FieldsStatMap.ContainsKey(key))
                {
                    oldValue = oldGroup.FieldsStatMap[key];
                }

                int newValue = 0;
                if (newGroup.FieldsStatMap.ContainsKey(key))
                {
                    newValue = newGroup.FieldsStatMap[key];
                }

                string flag = "EQUAL";
                if (oldValue != newValue)
                {
                    flag = "DIFF";
                }
                rows.Add(String.Format("00_{0};{1};{2};{3}", key, oldValue, newValue, flag));
            }

            foreach (string row in allUnionRows)
            {
                bool foundInOld = false;
                bool foundInNew = false;

                if (oldGroup.EntireFieldsMap.ContainsKey(row))
                {
                    foundInOld = true;
                }

                if (newGroup.EntireFieldsMap.ContainsKey(row))
                {
                    foundInNew = true;
                } 

                string flag = Utils.FlagToString(foundInOld, foundInNew);
                rows.Add(String.Format("01_{0};{1}", row, flag));
            }

            rows.Sort();

            return rows;
        }

        private void WriteFile(List<string> rows)
        {
            string name = "k8s-compare-report";
            var model = Utils.CreateReconcileModel(rows);

            template.RegisterTemplateFile(name, "resources/k8s-compare-report.html");
            
            string content = template.RenderTemplate<K8SReconcile>(name, model);

            var opt = options as CompareOptions;
            File.WriteAllText(opt.OutputFilePath, content);

            Log.Information("Wrote HTML report to file [{0}]", opt.OutputFilePath);
        }

        private void PopulateRows()
        {
            var dupGroup = new DupGrouping();

            allUnionRows.Clear();
            allUnionStatKeys.Clear();

            RowsFromFile(oldGroup, dupGroup);
            RowsFromFile(newGroup, dupGroup);
        }

        private void RowsFromFile(MapGrouping groupMap, DupGrouping dupGroup)
        {
            using(StreamReader file = new StreamReader(groupMap.FileName)) 
            {  
                string line = "";                
                while ((line = file.ReadLine()) != null) 
                {
                    if (line.Equals(""))
                    {
                        continue;
                    }

                    if (Utils.IsExcluded(line, excludedList))
                    {
                        continue;
                    }

                    if (!dupGroup.EntireFieldsMap.ContainsKey(line))
                    {
                        allUnionRows.Add(line);
                        dupGroup.EntireFieldsMap.Add(line, "dummy");
                    }
                    groupMap.EntireFieldsMap[line] = "dummy";


                    string statKey = Utils.GetStatKey(line);
                    groupMap.IncreaseStatCount(statKey);

                    if (!dupGroup.StatFieldsMap.ContainsKey(statKey))
                    {
                        allUnionStatKeys.Add(statKey);
                        dupGroup.StatFieldsMap.Add(statKey, "dummy");
                    }     
                }

                file.Close();  
            }
        }
    }
}