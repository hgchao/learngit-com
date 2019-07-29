using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.RecordMgmt.Records;
using App.RecordMgmt.Records.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.RecordMgmt
{
   public class RecordMgmtProfile : PfProfile
    {
        public RecordMgmtProfile()
        {
            //CreateMap<Contract, GetContractOutput>();

            CreateMap<UpdateAttachmentFileMetaInput, RecordAttachment>();
            CreateMap<AddAttachmentFileMetaInput, RecordAttachment>();
            CreateMap<RecordAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u=>u.Suffix, expr=>expr.MapFrom(u=>u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            CreateMap<AddRecordInput, Record>();
            CreateMap<Record, GetRecordOutput>();
            CreateMap<Record, GetRecordListOutput>()
                .ForMember(u=>u.RecordType, expr=>expr.MapFrom(u=>u.RecordType.Name));
            CreateMap<UpdateRecordInput, Record>();
        }
    }
}
