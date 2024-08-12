using DistribucionEnergia.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribucionEnergia.Core.Application.Contracts
{
    public interface ISegment
    {
        Task<List<Segment>> GetAll();       
    }
}
