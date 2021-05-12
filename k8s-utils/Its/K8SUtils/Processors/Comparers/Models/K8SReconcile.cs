using System.Collections.Generic;

namespace Its.K8SUtils.Processors.Comparers.Models
{
    public class K8SReconcile
    {
        public List<K8SReconcileItem> SummaryGlobalRes { get; set; }
        public List<K8SReconcileItem> SummaryNsRes { get; set; }

        public List<K8SReconcileItem> DetailGlobalRes { get; set; }
        public List<K8SReconcileItem> DetailNsRes { get; set; }

        public K8SReconcile()
        {
            SummaryGlobalRes = new List<K8SReconcileItem>();
            SummaryNsRes = new List<K8SReconcileItem>();

            DetailGlobalRes = new List<K8SReconcileItem>();
            DetailNsRes = new List<K8SReconcileItem>();
        }
    }
}