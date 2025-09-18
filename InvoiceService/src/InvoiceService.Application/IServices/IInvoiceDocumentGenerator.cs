using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Domain.Entities;

namespace InvoiceService.Application.IServices
{
    public interface IInvoiceDocumentGenerator
    {
        byte[] GenerateInvoiceDocument(Invoice invoice);
        string ContentType { get; }
        string FileExtension { get; }
    }
}