namespace Its.K8SUtils.Utils
{
    public class Executor : IExecutor
    {
        public string Run(string cmd, string argv)
        {
            return Utils.Exec(cmd, argv);
        }
    } 
}
