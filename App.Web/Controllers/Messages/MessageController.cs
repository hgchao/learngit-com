using App.Core.Authorization.Accounts;
using App.Core.Common.Entities;
using App.Core.Messaging.MessageRecords;
using App.Core.Messaging.MessageRecords.Dto;
using App.Core.Messaging.Messages;
using Microsoft.AspNetCore.Mvc;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.Messages
{
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/messages")]
    public class MessageController: ControllerBase
    {
        private IMessageRecordService _messageService;
        private IAuthInfoProvider _authInfoProvider;
        public MessageController(IMessageRecordService messageService,
            IAuthInfoProvider authInfoProvider)
        {
            _messageService = messageService;
            _authInfoProvider = authInfoProvider;
        }

        /// <summary>
        /// 分页获取消息列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <param name="readState">已读状态</param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetMessageRecordOutput> GetMessages(int pageIndex, int pageSize, MessageType? type, bool? readState)
        {
            return _messageService.Get(_authInfoProvider.GetCurrent().User.Id, pageIndex, pageSize, type, readState);
        }

        /// <summary>
        /// 未读计数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("unread-count")]
        public object Count(MessageType? type)
        {
            return new { count =_messageService.UnreadCount(_authInfoProvider.GetCurrent().User.Id, type) };
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:Int}")]
        public IActionResult Read(int id)
        {
            _messageService.Read(_authInfoProvider.GetCurrent().User.Id, id);
            return Created("", null);
        }
    }
}
