
using App.Funds.ContractPayments;
using App.Funds.ContractPayments.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Funds
{
    public class FundsProfile: PfProfile
    {
        public FundsProfile()
        {
            base.CreateMap<AddContractPaymentInput, ContractPayment>();
            base.CreateMap<UpdateContractPaymentInput, ContractPayment>();
            base.CreateMap<ContractPayment, GetContractPaymentOutput>();
        }
    }
}
