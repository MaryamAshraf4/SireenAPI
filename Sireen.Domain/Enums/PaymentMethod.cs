using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Enums
{
    public enum PaymentMethod
    {
        CreditCard = 1,
        DebitCard = 2,
        Cash = 3,
        BankTransfer = 4,
        EWallet = 5,
        POS = 6
    }
}
