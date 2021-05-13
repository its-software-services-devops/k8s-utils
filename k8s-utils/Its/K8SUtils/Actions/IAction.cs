using System;
using Its.K8SUtils.Options;

namespace Its.K8SUtils.Actions
{
    public interface IAction
    {
        int Run(BaseOptions options);
        int GetLastRunStatus();
    }
}