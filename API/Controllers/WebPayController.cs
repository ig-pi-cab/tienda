using Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using Transbank.Common;
using Transbank.Webpay.Common;
using Transbank.Webpay.WebpayPlus;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebPayController : BaseController
    {
        private readonly Transaction _transaction;

        public WebPayController(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
            : base(urlHelperFactory, actionContextAccessor)
        {
            _transaction = new Transaction(new Options(IntegrationCommerceCodes.WEBPAY_PLUS, IntegrationApiKeys.WEBPAY, WebpayIntegrationType.Test));
        }

        [HttpPost("create")]
        public ActionResult Create(int monto)
        {
            var buyOrder = "buyOrder_" + GetRandomNumber();
            var sessionId = "sessionId_" + GetRandomNumber();
            var amount = monto;
            var returnUrl = CreateUrl("webpay_plus", "Commit");
            var response = _transaction.Create(buyOrder, sessionId, amount, returnUrl);

            return Ok(response);
        }

        [HttpPost("commit")]
        public ActionResult Commit(string token_ws)
        {
            var response = _transaction.Commit(token_ws);


            return Ok(response);
        }

        [HttpPost("refund")]
        public ActionResult Refund([FromForm] string token_ws, [FromForm] decimal amount)
        {
            var response = _transaction.Refund(token_ws, amount);

            return Ok(response);
        }

        [HttpPost("status")]
        public ActionResult Status([FromForm] string token_ws)
        {
            var response = _transaction.Status(token_ws);

            return Ok(response);
        }
    }
}
