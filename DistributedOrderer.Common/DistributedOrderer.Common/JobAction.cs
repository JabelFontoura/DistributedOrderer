using System;
using System.Collections.Generic;

namespace DistributedOrderer.Common
{
    [Serializable]
    public abstract class JobAction
    {
        public List<int> Content { get; set; }
        public bool NoJobsLeft { get; set; }

        public abstract void Run();

        public JobAction() { }

        protected JobAction(List<int> content)
        {
            Content = content;
        }
    }
}
