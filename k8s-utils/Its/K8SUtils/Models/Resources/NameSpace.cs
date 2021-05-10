using System;

namespace Its.K8SUtils.Models.Resources
{
    public class NameSpace
    {
        public string Kind { get; set; }
        public string Name { get; set; }
        public Resource[] Resources { get; set; }

        public NameSpace()
        {
            Resources = Array.Empty<Resource>();
        }         
    } 
}

