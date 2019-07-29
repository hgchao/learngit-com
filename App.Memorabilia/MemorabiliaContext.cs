using System;
using System.Collections.Generic;
using System.Text;

namespace App.Memorabilia
{
  public  class MemorabiliaContext
    {
        public static MemorabiliaContext Instance;
        static MemorabiliaContext()
        {
            Instance = new MemorabiliaContext();
        }
        private MemorabiliaContext() { }
        public const string FormNameOfProjectMemorabilia = "大事记";
    }
}
