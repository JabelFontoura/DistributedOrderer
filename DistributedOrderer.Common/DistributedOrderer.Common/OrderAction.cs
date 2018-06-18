using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedOrderer.Common
{
    [Serializable]
    public class OrderAction : JobAction
    {
        public OrderAction(List<int> content) : base(content) { }

        public OrderAction() { }
        public override void Run()
        {
            var repos = 0;
            for (var i = 0; i < Content.Count - 1; i++)
            {
                for (var j = 0; j < Content.Count - (i + 1); j++)
                {
                    if (Content[j] > Content[j + 1])
                    {
                        repos = Content[j];
                        Content[j] = Content[j + 1];
                        Content[j + 1] = repos;
                    }
                }
            }
        }
    }
}
