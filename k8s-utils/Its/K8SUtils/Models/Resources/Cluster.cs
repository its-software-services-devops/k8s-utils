using System;

namespace Its.K8SUtils.Models.Resources
{
    public class Cluster
    {
        public string Name { get; set; }
        public Resource[] GlobalResources { get; set; }
        public NameSpace[] NameSpaces { get; set; }

        public Cluster()
        {
            GlobalResources = Array.Empty<Resource>();
            NameSpaces = Array.Empty<NameSpace>();
        }         
    } 
}

