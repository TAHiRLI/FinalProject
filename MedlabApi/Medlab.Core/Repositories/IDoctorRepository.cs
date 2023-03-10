using Medlab.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Repositories
{
    public interface IDoctorRepository : IEntityRepository<Doctor>
    {
        Doctor? GetDoctor(int id);

        IEnumerable<object> GetTopDoctor();

    }
}
