using System.IO;
using Moq;
using NUnit.Framework;
using Its.K8SUtils.Utils;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Processors.Snapshoters
{
    public class ResourcesSnapshoterTest
    {
        private const string content1 = @"
apiVersion: v1
items:
  - apiVersion: v1
    data:
      root-cert.pem: |
      -----BEGIN CERTIFICATE-----
      -----END CERTIFICATE-----
    kind: ConfigMap
    metadata:
      creationTimestamp: 2020-10-16T14:15:47Z
      labels:
      istio.io/config: true
      name: istio-ca-root-cert
      namespace: alpha
      resourceVersion: 152259292
      selfLink: /api/v1/namespaces/alpha/configmaps/istio-ca-root-cert
      uid: 4720dcae-8dbe-4b1f-a895-589d6d113458
  - apiVersion: v1
    data:
      ca.crt: |
      -----BEGIN CERTIFICATE-----
      JsmGOtOZN2LfmEc2lXST3Q==
      -----END CERTIFICATE-----
    kind: ConfigMap
    metadata:
      creationTimestamp: 2021-05-07T13:11:28Z
      name: kube-root-ca.crt
      namespace: alpha
      resourceVersion: 183059693
      selfLink: /api/v1/namespaces/alpha/configmaps/kube-root-ca.crt
      uid: c5bbbe8b-5eb3-4067-81b1-963f4a43d13d
kind: List
metadata:
  resourceVersion: 
  selfLink:
";

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(content1)]
        public void DoTest(string content)
        {
            SnapshotOptions opt = new SnapshotOptions();
            opt.ExportOutputDir = "."; // Path.GetTempPath();

            var mck = new Mock<IExecutor>();
            mck.Setup(x=>x.Run("kubectl", It.IsAny<string>())).Returns("");
            mck.Setup(x=>x.Run("yq", It.IsAny<string>())).Returns(content);

            var snptr = new ResourcesSnapshoter();
            snptr.SetOptions(opt);
            snptr.SetExecutor(mck.Object);

            snptr.Do();

            Assert.AreEqual(true, File.Exists(snptr.GetSnapshotPath()));
        }           
    }
}