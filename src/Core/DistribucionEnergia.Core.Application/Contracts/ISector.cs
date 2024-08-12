using DistribucionEnergia.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribucionEnergia.Core.Application.Contracts
{
    public interface ISector
    {
        Task<List<Sector>> GetAll();
    }
}
