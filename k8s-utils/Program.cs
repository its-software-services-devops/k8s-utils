using System;
using Serilog;
using CommandLine;
using System.Reflection;
using Its.K8SUtils.Options;
using Its.K8SUtils.Actions;

namespace Its.K8SUtils
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyVersion = assembly.GetName().Version;
            Log.Information("Running [k8s-utils] version [{0}]", assemblyVersion);

            Parser.Default.ParseArguments<ExportOptions, CompareOptions, InfoOptions, SnapshotOptions>(args)
                .WithParsed<ExportOptions>(UtilsAction.RunExportAction)
                .WithParsed<CompareOptions>(UtilsAction.RunCompareAction)
                .WithParsed<InfoOptions>(UtilsAction.RunInfoAction)
                .WithParsed<SnapshotOptions>(UtilsAction.RunSnapshotAction);
        }
    }
}
