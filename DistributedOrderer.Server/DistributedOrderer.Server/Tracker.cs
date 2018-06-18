using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedOrderer.Common;

namespace DistributedOrderer.Server
{
    public class Tracker
    {
        public List<JobAction> Jobs { get; private set; }
        public List<JobAction> Results { get; private set; }

        public Tracker()
        {
            Jobs = new List<JobAction>();
            Results = new List<JobAction>();
            CreateJobs(50);
        }


        public JobAction GetJob()
        {
            var job = Jobs.First();
            if (Jobs.Count > 0)
                Jobs.RemoveAt(0);
            return job;
        }

        public bool StillHasJobs()
        {
            return Jobs.Count > 0;
        }

        public void ShowResult()
        {
            var result = new List<string>();
            for (int i = 0; i < Results.Count; i++)
            {
                Console.WriteLine(GenerateResultString(i, Results[i].Content));
            }
        }

        private string GenerateResultString(int jobNumber, List<int> content)
        {
            var first50 = "";
            content.GetRange(0, 50).ForEach(item => { first50 += $"{item},"; });

            var last50 = "";
            content.Skip(Math.Max(0, content.Count() - 50)).ToList().ForEach(item => { last50 += $"{item},"; });
            last50 = last50.Substring(0, last50.Length - 1);

            return $"Job {jobNumber}: [{first50}, ..., ..., ..., {last50}]";
        }

        public void AddResult(JobAction jobAction)
        {
            Results.Add(jobAction);
        }

        private void CreateJobs(int numberOfJobs)
        {
            for (int i = 0; i < numberOfJobs; i++)
            {
                Jobs.Add(new OrderAction(Generate10ThousandRandomNumbers()));
            }
        }

        private List<int> Generate10ThousandRandomNumbers()
        {
            var rand = new Random();
            var rtnlist = new List<int>();

            for (int i = 0; i < 10000; i++)
                rtnlist.Add(rand.Next(1000));

            return rtnlist;
        }
    }
}
