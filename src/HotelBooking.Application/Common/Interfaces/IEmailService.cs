﻿namespace HotelBooking.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string bodyHtml);
    }
}
