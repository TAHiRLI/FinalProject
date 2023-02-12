using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class ServiceRepository:EntityRepository<Service> , IServiceRepository
    {
        private readonly MedlabDbContext _context;

        public ServiceRepository(MedlabDbContext context):base(context)
        {
            this._context = context;
        }
    }
}
