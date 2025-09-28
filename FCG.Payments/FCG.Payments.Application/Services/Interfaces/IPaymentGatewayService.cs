using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<bool> SendPaymentRequest();
    }
}
