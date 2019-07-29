using App.Contract.ConstructionUnits;
using App.Contract.ContractDeposits;
using App.Contract.Contracts;
using App.Core.Common.EntityFramework;
using Microsoft.EntityFrameworkCore;


using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract.EntityFramework
{
    public interface IContractDbContext: IAppCoreDbContext
    {
        DbSet<ContractDeposit> ContractDeposits { get; set; }
        DbSet<Contractt> Contracts { get; set; }
        DbSet<ContractAttachment> ContractAttachments { get; set; }
        DbSet<ConstructionUnit> ConstructionUnits { get; set; }
    }
    
}
