using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Memorabilia.MemorabiliaRecords;
using App.Memorabilia.MemorabiliaRecords.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Memorabilia
{
    public class MemorabiliaProfile: PfProfile
    {
        public MemorabiliaProfile()
        {
            CreateMap<AddAttachmentFileMetaInput, MemorabiliaAttachment>();
            CreateMap<UpdateAttachmentFileMetaInput, MemorabiliaAttachment>();
            CreateMap<MemorabiliaAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
            base.CreateMap<AddMemorabiliaRecordInput, MemorabiliaRecord>();
            base.CreateMap<UpdateMemorabiliaRecordInput, MemorabiliaRecord>();
            base.CreateMap<MemorabiliaRecord, ExportMemorabiliaRecordOutput>();
            base.CreateMap<MemorabiliaRecord, GetMemorabiliaRecordListOutput>()
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            base.CreateMap<MemorabiliaRecord, GetMemorabiliaRecordOutput>()
                //.ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
        }
    }
}
