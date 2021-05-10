using Its.K8SUtils.Options;

namespace Its.K8SUtils.Actions
{
    public enum ActionType
    {
        Export,
        Info,
    }

    public static class UtilsAction
    {
        private static IAction exportAction = new ActionExport();
        private static IAction infoAction = new ActionInfo();

        public static void SetAction(ActionType type, IAction action)
        {
            if (type == ActionType.Export)
            {
                exportAction = action;
            }
            else if (type == ActionType.Info)
            {
                infoAction = action;
            }             
        }

        public static void RunExportAction(BaseOptions o)
        {
            exportAction.Run(o);      
        }

        public static void RunInfoAction(BaseOptions o)
        {
            infoAction.Run(o);
        }        
    }
}
