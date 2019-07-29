using App.Contract.ConstructionUnits;
using App.Contract.ConstructionUnits.Dto;
using App.Contract.ContractDeposits;
using App.Contract.ContractDeposits.Dto;
using App.Contract.Contracts;
using App.Contract.Contracts.Dto;
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract
{
    public class ContractProfile: PfProfile
    {
        public ContractProfile()
        {
            //保证金
            base.CreateMap<AddContractDepositInput, ContractDeposit>();
            base.CreateMap<UpdateContractDepositInput, ContractDeposit>();
            base.CreateMap<ContractDeposit, GetContractDepositOutput>();

            //合同
            CreateMap<AddAttachmentFileMetaInput, ContractAttachment>();
            CreateMap<UpdateAttachmentFileMetaInput, ContractAttachment>();
            CreateMap<ContractAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
            base.CreateMap<AddContractInput, Contractt>();
            base.CreateMap<UpdateContractInput, Contractt>();
            base.CreateMap<Contractt, GetContractOutput>()
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.Contractor, expr => expr.MapFrom(u => u.Contractor.Name));// .ForMember(u => u.ContractionMethod, expr => expr.MapFrom(u => u.ContractionMethod.Name))
            base.CreateMap<Contractt, GetContractListOutput>()
                .ForMember(u => u.Contractor, expr => expr.MapFrom(u => u.Contractor.Name))
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.PerformanceDatePeriods, expr=>expr.MapFrom(u=>$"{(u.PerformanceStartDate ==null?String.Empty:u.PerformanceStartDate.Value.ToString("yyyy-MM-dd"))}-{(u.PerformanceEndDate ==null?String.Empty:u.PerformanceEndDate.Value.ToString("yyyy-MM-dd"))}"));
            CreateMap<Contractt, ExportContractOutput>()
                 .ForMember(u => u.Contractor, expr => expr.MapFrom(u => u.Contractor.Name))
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.PerformanceDatePeriods, expr => expr.MapFrom(u => $"{(u.PerformanceStartDate == null ? String.Empty : u.PerformanceStartDate.Value.ToString("yyyy-MM-dd"))}-{(u.PerformanceEndDate == null ? String.Empty : u.PerformanceEndDate.Value.ToString("yyyy-MM-dd"))}")); 
            CreateMap<Contractt, GetContractCountOutput>();
            //参建单位
            base.CreateMap<AddConstructionUnitInput, ConstructionUnit>();
            base.CreateMap<UpdateConstructionUnitInput, ConstructionUnit>();
            base.CreateMap<ConstructionUnit, GetConstructionUnitOutput>();
            base.CreateMap<ConstructionUnit, GetConstructionUnitListOutput>();
            CreateMap<ConstructionUnit, ExportConstructionUnitOutput>();

        }
    }
}
