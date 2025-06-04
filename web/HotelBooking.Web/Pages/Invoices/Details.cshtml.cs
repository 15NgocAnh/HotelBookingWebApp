using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.Commands.AddPayment;
using HotelBooking.Application.CQRS.Invoice.Commands.AddRoomDamage;
using HotelBooking.Application.CQRS.Invoice.Commands.UpdateInvoiceStatus;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Utils.Enum;
using HotelBooking.Web.Configuration;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using QRCoder;

namespace HotelBooking.Web.Pages.Invoices
{
    public class DetailsModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<DetailsModel> _logger;
        private readonly BankSettings _bankSettings;

        public DetailsModel(
            IApiService apiService, 
            ILogger<DetailsModel> logger,
            IOptions<BankSettings> bankSettings)
        {
            _apiService = apiService;
            _logger = logger;
            _bankSettings = bankSettings.Value;
        }

        public InvoiceDto? Invoice { get; set; }
        public string QrCodeImageBase64 { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var result = await _apiService.GetAsync<InvoiceDto>($"api/invoice/{id}");
                if (result == null || !result.IsSuccess || result.Data == null)
                {
                    TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to fetch invoice details.";
                    return RedirectToPage("./Index");
                }

                Invoice = result.Data;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading invoice details");
                TempData["ErrorMessage"] = "An error occurred while loading the invoice details.";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostMarkAsPaidAsync(int invoiceId)
        {
            try
            {
                var command = new UpdateInvoiceStatusCommand
                {
                    Id = invoiceId,
                    Status = InvoiceStatus.Paid
                };

                var result = await _apiService.PutAsync<Result>($"api/invoice/{invoiceId}/status", command);
                if (result == null || !result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to process mark invoice as paid.";
                    return RedirectToPage(new { id = invoiceId });
                }

                TempData["SuccessMessage"] = "Mark invoice as paid processed successfully!";
                return RedirectToPage(new { id = invoiceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing mark invoice as paid");
                TempData["ErrorMessage"] = "An error occurred while processing mark invoice as paid.";
                return RedirectToPage(new { id = invoiceId });
            }
        }

        public async Task<IActionResult> OnPostCancelAsync(int invoiceId)
        {
            try
            {
                var command = new UpdateInvoiceStatusCommand
                {
                    Id = invoiceId,
                    Status = InvoiceStatus.Cancelled
                };

                var result = await _apiService.PutAsync<Result>($"api/invoice/{invoiceId}/status", command);
                if (result == null || !result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to process mark invoice as cancel.";
                    return RedirectToPage(new { id = invoiceId });
                }

                TempData["SuccessMessage"] = "Mark invoice as cancel processed successfully!";
                return RedirectToPage(new { id = invoiceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing mark invoice as cancel");
                TempData["ErrorMessage"] = "An error occurred while processing mark invoice as cancel.";
                return RedirectToPage(new { id = invoiceId });
            }
        }

        public async Task<IActionResult> OnPostAddPaymentAsync(int invoiceId, decimal amount, PaymentMethod paymentMethod)
        {
            try
            {
                var command = new AddPaymentCommand
                {
                    InvoiceId = invoiceId,
                    Amount = amount,
                    PaymentMethod = paymentMethod,
                };

                var result = await _apiService.PutAsync<Result>($"api/invoice/{invoiceId}/payment", command);
                if (result == null || !result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to process payment.";
                    return RedirectToPage(new { id = invoiceId });
                }

                TempData["SuccessMessage"] = "Payment processed successfully!";
                return RedirectToPage(new { id = invoiceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                TempData["ErrorMessage"] = "An error occurred while processing the payment.";
                return RedirectToPage(new { id = invoiceId });
            }
        }

        public async Task<IActionResult> OnPostAddDamageAsync(int invoiceId, decimal amount, string description)
        {
            try
            {
                var command = new AddRoomDamageCommand
                {
                    InvoiceId = invoiceId,
                    Amount = amount,
                    Description = description
                };

                var result = await _apiService.PutAsync<Result>($"api/invoice/{invoiceId}/damage", command);
                if (result == null || !result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to add room damage.";
                    return RedirectToPage(new { id = invoiceId });
                }

                TempData["SuccessMessage"] = "Room damage added successfully!";
                return RedirectToPage(new { id = invoiceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding room damage");
                TempData["ErrorMessage"] = "An error occurred while adding room damage.";
                return RedirectToPage(new { id = invoiceId });
            }
        }

        public async Task<IActionResult> OnPostPreviewQrAsync(int invoiceId, string invoiceNumber, decimal amount, PaymentMethod paymentMethod)
        {
            if (paymentMethod != PaymentMethod.BankTransfer)
            {
                await OnPostAddPaymentAsync(invoiceId, amount, paymentMethod);
                return RedirectToPage(new { id = invoiceId });
            }

            var description = $"Thanh toan HD {invoiceNumber}";
            var paymentUrl = $"https://img.vietqr.io/image/{_bankSettings.BankBin}-{_bankSettings.AccountNumber}-compact2.jpg" +
                             $"?amount={amount}&addInfo={Uri.EscapeDataString(description)}";

            using var qrGen = new QRCodeGenerator();
            var qrData = qrGen.CreateQrCode(paymentUrl, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            var qrBytes = qrCode.GetGraphic(10);
            var qrBase64 = "data:image/png;base64," + Convert.ToBase64String(qrBytes);

            TempData["QrImage"] = qrBase64;
            TempData["QrAmount"] = amount.ToString();
            TempData["QrInvoice"] = invoiceNumber;
            TempData["QrMode"] = "preview";
            TempData["QrMethod"] = "BankTransfer";

            return RedirectToPage(new { id = invoiceId });
        }

        public async Task<IActionResult> OnGetInvoicePdfAsync(int invoiceId, string invoiceNumber)
        {
            try
            {
                var result = await _apiService.GetAsync<byte[]>($"api/invoice/{invoiceId}/pdf");
                if (result == null || !result.IsSuccess)
                {
                    TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to process export invoice.";
                    return RedirectToPage(new { invoiceId });
                }

                return File(result?.Data, "application/pdf", $"invoice-{invoiceNumber}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                TempData["ErrorMessage"] = "An error occurred while processing export invoice.";
                return RedirectToPage(new { invoiceId });
            }
        }
    }
}
