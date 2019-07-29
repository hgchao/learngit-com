using App.Core.Common.Entities;
using App.Funds.ContractPayments;
using App.Funds.ContractPayments.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pm.Web.Controllers.Funds
{
    /// <summary>
    /// 资金管理-合同支付
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/contractpayments")]
    public class ContractPaymentController: ControllerBase
    {
        private IContractPaymentService _paymentService;
        public ContractPaymentController(IContractPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// 首页统计合同支付金额
        /// </summary>
        /// <returns></returns>
        [HttpGet("totalmoney")]
        public GetContractPaymentMoneyOutput GetTotalMoney(DateTime? paymentTimeDateL, DateTime? paymentTimeDateR)
        {
            var payment = _paymentService.GetTotalMoney(paymentTimeDateL, paymentTimeDateR);
            return payment;
        }

        /// <summary>
        /// 获取合同支付详情
        /// </summary>
        /// <param name="id">合同支付id</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetContractPaymentOutput Get(int id)
        {
            var unit = _paymentService.Get(id);
            return unit;
        }

        /// <summary>
        /// 分页获取合同支付列表
        /// </summary>
        /// <param name="contractId">合同id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="sortField">需要排序的字段(标题:Title)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet()]
        public PaginationData<GetContractPaymentOutput> Get(int? contractId,
            int pageIndex, int pageSize,
            string keyword,
            string sortField,
             string sortState)
        {
            var datas = _paymentService.Get(contractId, pageIndex, pageSize, keyword, sortField, sortState);
            return datas;
        }


        /// <summary>
        /// 删除合同支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _paymentService.Delete(id);
            return NoContent();
        }

    }
}
