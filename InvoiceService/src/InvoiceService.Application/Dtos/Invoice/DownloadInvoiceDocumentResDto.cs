using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceService.Application.Dtos.Invoice
{
    public class DownloadInvoiceDocumentResDto
    {
        public byte[] DocumentBytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;  
        public string FileExtension { get; set; } = string.Empty;       
    }
}