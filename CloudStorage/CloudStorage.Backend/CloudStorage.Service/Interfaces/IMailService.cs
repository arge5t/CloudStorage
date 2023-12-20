using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailConfirm(string activeLink, string email);
    }
}
