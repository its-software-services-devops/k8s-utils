using Its.K8SUtils.Options;

namespace Its.K8SUtils.Actions
{
    public enum ActionType
    {
        Export,
        Info,
        Compare,
        Snapshot,
    }

    public static class UtilsAction
    {
        private static IAction exportAction = new ActionExport();
        private static IAction infoAction = new ActionInfo();
        private static IAction compareAction = new ActionCompare();
        private static IAction snapshotAction = new ActionSnapshot();

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
            else if (type == ActionType.Compare)
            {
                compareAction = action;
            }
            else if (type == ActionType.Snapshot)
            {
                snapshotAction = action;
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

        public static void RunCompareAction(BaseOptions o)
        {
            compareAction.Run(o);
        }

        public static void RunSnapshotAction(BaseOptions o)
        {
            snapshotAction.Run(o);
        }        
    }
}
