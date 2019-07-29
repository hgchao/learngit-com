using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.Problems.Dto
{
    public class CompleteProblemInput
    {
        public int Id { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualCompletionTime { get; set; }
    }
}
