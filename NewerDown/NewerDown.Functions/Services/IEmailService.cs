﻿namespace NewerDown.Functions.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject);
}