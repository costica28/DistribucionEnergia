using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Infrastructure.Persistence.Persistence;
using DistribucionEnergia.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistribucionEnergia.Infrastructure.Persistence
{
    public static class RegistrationPersistenceService
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<DistribucionEnergiaDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));
            service.AddScoped<ISegment, SegmentRepository>();
            service.AddScoped<ISector, SectorRepository>();
            service.AddScoped<IEnergyInformation, EnergyInformationRepository>();

            return service;
        }
    }
}
