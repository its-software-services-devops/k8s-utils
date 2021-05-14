using System;
using System.Collections.Generic;
using Its.K8SUtils.Processors.Comparers.Models;

namespace Its.K8SUtils.Processors.Comparers
{
    public static class Utils
    {

        public static string FlagToString(bool oldFlag, bool newFlag)
        {
            if (oldFlag && newFlag)
            {
                return "BOTH";
            }
            
            if (oldFlag)
            {
                return "OLD";
            }
            
            if (newFlag)
            {
                return "NEW";
            }

            return "ERROR";
        }

        public static string GetStatKey(string line)
        {
            char[] delims = new[] { ';' };
            var fields = line.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            string recType = fields[0];

            string key = String.Format("{0};{1}", recType, fields[1]);
            if (recType.Equals("NS"))
            {
                key = String.Format("{0};{1};{2}", recType, fields[1], fields[2]);
            }

            return key;
        }       

        public static bool IsExcluded(string line, List<string> excludedList)
        {
            char[] delims = new[] { ';' };
            var fields = line.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            bool excluded = false;

            string recType = fields[0];
            if (recType.Equals("NS"))
            {
                string resName = fields[2];
                excluded = excludedList.Contains(resName);
            }

            return excluded;
        } 

        public static K8SReconcile CreateReconcileModel(List<string> rows)
        {
            var model = new K8SReconcile();

            foreach (string row in rows)
            {
                char[] delims = new[] { ';' };
                var fields = row.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                string recType = fields[0];

                if (recType.Equals("00_GB"))
                {
                    var item = new K8SReconcileItem();
                    item.RecType = recType;
                    item.Kind = fields[1];
                    item.OldQuantity = Int32.Parse(fields[2]);
                    item.NewQuantity = Int32.Parse(fields[3]);
                    item.Status = fields[4];

                    model.SummaryGlobalRes.Add(item);
                }
                else if (recType.Equals("00_NS"))
                {
                    var item = new K8SReconcileItem();
                    item.RecType = recType;
                    item.NameSpace = fields[1];
                    item.Kind = fields[2];
                    item.OldQuantity = Int32.Parse(fields[3]);
                    item.NewQuantity = Int32.Parse(fields[4]);
                    item.Status = fields[5];

                    model.SummaryNsRes.Add(item);                    
                }
                else if (recType.Equals("01_GB"))
                {
                    var item = new K8SReconcileItem();
                    item.RecType = recType;
                    item.Kind = fields[1];
                    item.ResourceName = fields[2];
                    item.Status = fields[3];

                    model.DetailGlobalRes.Add(item);                     
                }
                else if (recType.Equals("01_NS"))
                {
                    var item = new K8SReconcileItem();
                    item.RecType = recType;
                    item.NameSpace = fields[1];
                    item.Kind = fields[2];
                    item.ResourceName = fields[3];
                    item.Status = fields[4];

                    model.DetailNsRes.Add(item);                       
                }                                       
            }

            return model;           
        }
    }
}
