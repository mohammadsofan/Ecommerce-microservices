using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.IServices;
using InvoiceService.Domain.Entities;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvoiceService.Application.Services
{
    public class PdfInvoiceDocumentGenerator : IInvoiceDocumentGenerator
    {
        public string ContentType => "application/pdf";
        public string FileExtension => ".pdf";
        private readonly ILogger<PdfInvoiceDocumentGenerator> _logger;
        public PdfInvoiceDocumentGenerator(
            ILogger<PdfInvoiceDocumentGenerator> logger
            )
        {
            _logger = logger;
        }

        public byte[] GenerateInvoiceDocument(Invoice invoice)
        {
            try
            {
                var document = Document.Create(container => ComposePdf(container, invoice));
                var pdfBytes = document.GeneratePdf();

                _logger.LogInformation("Generating PDF document for Invoice ID: {InvoiceId}", invoice.Id);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF document for Invoice ID: {InvoiceId}", invoice.Id);
                throw;
            }
        }
        public void ComposePdf(IDocumentContainer container, Invoice invoice)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));

                page.Header().Element(c => ComposeHeader(c, invoice));
                page.Content().Element(c => ComposeContent(c, invoice));

                page.Footer().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span($"{DateTime.Now:yyyy-MM-dd}").SemiBold();
                    });

                    row.RelativeItem().AlignRight().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });
        }
        void ComposeContent(IContainer container, Invoice invoice)
        {
            container.Column(column =>
            {
                column.Spacing(10);

                column.Item().Element(c => ComposeCustomerInfo(c, invoice));
                column.Item().Element(c => ComposeItemsTable(c, invoice));
            });
        }
        void ComposeHeader(IContainer container, Invoice invoice)
        {
            container.Row(row =>
          {
              row.RelativeItem().Column(column =>
              {
                  column
                      .Item().Text($"Invoice #{invoice.Id}")
                      .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                  column.Item().Text(text =>
                  {
                      text.Span("Issue date: ").SemiBold();
                      text.Span($"{invoice.CreatedAt:d}");
                  });

                  column.Item().Text(text =>
                  {
                      text.Span("Due date: ").SemiBold();
                      text.Span($"{invoice.DueDate:d}");
                  });
              });

              //row.ConstantItem(175).Image(LogoImage);
          });
        }
        void ComposeCustomerInfo(IContainer container, Invoice invoice)
        {
            container.PaddingVertical(10).Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Bill To").FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);
                    column.Item().Text(invoice.CustomerInfo.FullName);
                    column.Item().Text(invoice.CustomerInfo.Email);
                    column.Item().Text(invoice.CustomerInfo.PhoneNumber);
                });

                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Ship To").FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);
                    column.Item().Text(invoice.ShippingAddress.Street);
                    column.Item().Text($"{invoice.ShippingAddress.City}, {invoice.ShippingAddress.State} {invoice.ShippingAddress.PostalCode}");
                    column.Item().Text(invoice.ShippingAddress.Country);
                });
            });
        }
        void ComposeItemsTable(IContainer container, Invoice invoice)
        {
            container.Table(table =>
            {
                // Define columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4); // Item Name
                    columns.RelativeColumn(2); // Quantity
                    columns.RelativeColumn(2); // Unit Price
                    columns.RelativeColumn(2); // Total Price
                });

                // Table Header
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Item").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Quantity").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit Price").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Total Price").FontSize(12).SemiBold();

                    static IContainer CellStyle(IContainer cell)
                    {
                        return cell.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });
                static IContainer CellStyleItem(IContainer cell)
                {
                    return cell.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                }

                // Table Rows
                foreach (var item in invoice.Items)
                {
                    table.Cell().Element(CellStyleItem).Text(item.Name);
                    table.Cell().Element(CellStyleItem).AlignRight().Text(item.Quantity.ToString());
                    table.Cell().Element(CellStyleItem).AlignRight().Text($"{item.UnitPrice:C}");
                    table.Cell().Element(CellStyleItem).AlignRight().Text($"{item.TotalPrice:C}");
                }

                // Totals Row
                table.Cell().ColumnSpan(3).Element(CellStyle).AlignRight().Text("Total").FontSize(14).SemiBold();
                table.Cell().Element(CellStyle).AlignRight().Text($"{invoice.Totals.Total:C}").FontSize(14).SemiBold();

                static IContainer CellStyle(IContainer cell)
                {
                    return cell.PaddingVertical(5);
                }
            });
        }
    }
}