using System;
using System.Collections.Generic;
using System.Text;

namespace App.Base
{
    public class CompleteTaskInput<TData>
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public bool PreventCommit { get; set; }
        public TData Data { get; set; }
    }
}
