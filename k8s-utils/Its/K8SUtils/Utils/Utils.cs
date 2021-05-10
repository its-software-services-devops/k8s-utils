using Serilog;
using System.Diagnostics;

namespace Its.K8SUtils.Utils
{
    public static class Utils
    {
        public static string Exec(string cmd, string argv)
        {
            string output = "";
            string cmdWithArg = string.Format("{0} {1}", cmd, argv);
            
            Log.Information("Executing command [{0}]...", cmdWithArg);

            using(Process pProcess = new Process())
            {
                pProcess.StartInfo.FileName = cmd;
                pProcess.StartInfo.Arguments = argv;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
                pProcess.Start();
                output = pProcess.StandardOutput.ReadToEnd();
                pProcess.WaitForExit();

                //If want to use Environment.Exit(1), we will need DI to do the unit testing
                //We may create another static class like IascEnvironment.ExitIfError(pProcess.ExitCode)

                if (pProcess.ExitCode != 0)
                {
                    Log.Error("Command executed with error, exit code [{0}]", pProcess.ExitCode);
                }
            }

            return output;            
        } 
    }
}
