using Medlab.Core.Entities;

namespace MedlabApi.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(AppUser user, IList<string> roles, IConfiguration config);
    }
}
