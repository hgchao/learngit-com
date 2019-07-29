using System;
using System.Collections.Generic;
using System.Text;

namespace App.Funds
{
  public  class FundsContext
    {
        public readonly static FundsContext Instance;
        static FundsContext()
        {
            Instance = new FundsContext();
        }

        public const string FormNameOfProjectContractPayment = "合同支付";
    }
}
