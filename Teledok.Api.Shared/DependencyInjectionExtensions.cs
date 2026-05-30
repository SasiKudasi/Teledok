
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teledok.Domain.Client;
using Teledok.Infrastructure.ClientContext;
using Teledok.Infrastructure.Repositories;

namespace Teledok.Api.Shared;

public static class DependencyInjectionExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PersonContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("default")));
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
    }
}
