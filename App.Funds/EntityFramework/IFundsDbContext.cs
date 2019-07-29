using App.Core.Common.EntityFramework;
using App.Funds.ContractPayments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Funds.EntityFramework
{
    public interface IFundsDbContext:  IAppCoreDbContext
    {
        DbSet<ContractPayment> ContractPayments { get; set; }
    }
}
