using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InvoiceService.Application.Dtos.Invoice;
using InvoiceService.Application.IServices;
using InvoiceService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<InvoiceDetailDto>> GetInvoiceById(string invoiceId)
        {
            var invoice = await _mediator.Send(new GetInvoiceByIdQuery(invoiceId));
            return Ok(invoice);
        }
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<InvoiceDetailDto>>> GetUserInvoices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var invoices = await _mediator.Send(new GetUserInvoicesQuery(userId!));
            return Ok(invoices);
        }
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<InvoiceDetailDto>>> FilterInvoices([FromBody] FilterInvoicesDto filter)
        {
            var invoices = await _mediator.Send(new FilterInvoicesQuery(filter));
            return Ok(invoices);
        }
        [HttpGet("{invoiceId}/download")]
        public async Task<ActionResult<DownloadInvoiceDocumentResDto>> DownloadInvoiceDocument(string invoiceId)
        {
            var document = await _mediator.Send(new DownloadInvoiceDocumentQuery(invoiceId));
            return Ok(document);
        }

    }
}