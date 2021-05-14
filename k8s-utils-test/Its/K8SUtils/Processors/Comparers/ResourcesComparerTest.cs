using Moq;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using Its.K8SUtils.Options;
using Its.K8SUtils.Templates;

namespace Its.K8SUtils.Processors.Comparers
{
    public class ResourcesComparerTest
    {
        private const string content1 = @"
GB;APIService;v1.
GB;APIService;v1.acme.cert-manager.io
GB;APIService;v1.admissionregistration.k8s.io
NS;alpha;ConfigMap;istio-ca-root-cert
NS;alpha;ConfigMap;kube-root-ca.crt
NS;alpha;Role;alpha-developer
NS;alpha;Secret;default-token-cvwsf
NS;alpha;ServiceAccount;default
NS;anthos-identity-service;ConfigMap;ais-config
NS;anthos-identity-service;ConfigMap;istio-ca-root-cert
NS;anthos-identity-service;ConfigMap;kube-root-ca.crt
NS;anthos-identity-service;Deployment;ais
NS;anthos-identity-service;Endpoints;ais
NS;anthos-identity-service;EndpointSlice;ais-jdj6v
NS;anthos-identity-service;Pod;ais-78fb8d6449-l46zr
NS;anthos-identity-service;PodMetrics;ais-78fb8d6449-l46zr
NS;anthos-identity-service;Event;aaaaaaaaaaaaaaaaa-bbbbbbbbbbbbb
";

        private const string content2 = @"
GB;APIService;v1.
GB;APIService;v1.acme.cert-manager.io
GB;APIService;v1.admissionregistration.k8s.io
NS;alpha;ConfigMap;istio-ca-root-cert
NS;alpha;ConfigMap;kube-root-ca.crt
NS;alpha;Role;alpha-developer
NS;alpha;Secret;default-token-cvwsf
NS;alpha;ServiceAccount;default
NS;anthos-identity-service;ConfigMap;ais-config
NS;anthos-identity-service;ConfigMap;ais-configXXX
NS;anthos-identity-service;ConfigMap;istio-ca-root-cert
NS;anthos-identity-service;ConfigMap;kube-root-ca.crt
NS;anthos-identity-service;Deployment;ais
NS;anthos-identity-service;Endpoints;ais
NS;anthos-identity-service;EndpointSlice;ais-jdj6v
NS;anthos-identity-service;Pod;ais-78fb8d6449-l46zr
NS;anthos-identity-service;PodMetrics;ais-78fb8d6449-l46zr
";

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(content1, content2, false)]
        [TestCase(content2, content1, false)]
        [TestCase(content1, content2, true)]
        [TestCase(content2, content1, true)]        
        public void DoTest(string content1, string content2, bool setExclude)
        {
            var tplEngineMoq = new Mock<ITemplateEngine>();
            var tplEngine = tplEngineMoq.Object;

            string path1 = Path.GetTempFileName();
            string path2 = Path.GetTempFileName();
            string outputPath = Path.GetTempFileName();

            File.WriteAllText(path1, content1);
            File.WriteAllText(path2, content2);

            var option = new CompareOptions();
            option.BasedFilePath = path1;
            option.NewFilePath = path2;
            option.OutputFilePath = outputPath;

            var comparer = new ResourcesComparer();
            if (setExclude)
            {
                comparer.SetExcludedList(new List<string>());
            }
            comparer.SetOptions(option);
            comparer.SetTemplateEngine(tplEngine);
            comparer.Do();
        }           
    }
}