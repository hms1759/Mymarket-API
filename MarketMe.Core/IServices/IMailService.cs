using MarketMe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.IServices
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}
