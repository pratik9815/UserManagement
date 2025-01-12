using Microsoft.Extensions.Configuration;
using System.Text.Json;
using UserManagement.Application.IServices;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;
public class SenderService : ISenderService
{
    private readonly IConfiguration _configuration;

    public SenderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMail(EmailPayLoad payload)
    {
        string data = JsonSerializer.Serialize(payload);
        //Message 
    }
}
