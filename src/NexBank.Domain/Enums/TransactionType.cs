using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexBank.Domain.Enums;

namespace NexBank.Domain.Enums;


public enum TransactionType
{
    Credit = 1 , // Para girişi
    Debit= 2 // Para çıkışı
}
