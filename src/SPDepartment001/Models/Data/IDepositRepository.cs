using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public interface IDepositRepository
    {
        IEnumerable<Deposit> Deposits { get; }
        void SaveDeposit(Deposit deposit);
        Deposit DeleteDeposit(Guid depositId);
    }
}
