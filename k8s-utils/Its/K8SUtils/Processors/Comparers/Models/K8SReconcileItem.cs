
namespace Its.K8SUtils.Processors.Comparers.Models
{
    public class K8SReconcileItem
    {
        public string RecType { get; set; }
        public string Kind { get; set; }
        public string NameSpace { get; set; }
        public string ResourceName { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public string Status { get; set; }
    }
}