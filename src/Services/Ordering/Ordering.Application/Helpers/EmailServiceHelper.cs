using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Features.Orders.Commands.CheckOutOrder;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Helpers;

public interface IEmailServiceHelper
{
    Task SendMail(Order newOrder);
}

public class EmailServiceHelper : IEmailServiceHelper
{
    readonly IEmailService _emailService;
    readonly ILogger<EmailServiceHelper> _logger;

    public EmailServiceHelper(IEmailService emailService, ILogger<EmailServiceHelper> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task SendMail(Order newOrder)
    {
        try
        {
            string message = "Order was created";
            var email = new Models.Email
            {
                To = "njezichigozie@yahoo.com",
                Body = message,
                Subject = message
            };
            await _emailService.SendEmailAsync(email);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Order {newOrder.Id} failed due to an error with the mail server: {ex.Message}");
        }
    }
}
