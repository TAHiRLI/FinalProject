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
    public class DoctorRepository:EntityRepository<Doctor> , IDoctorRepository
    {
        private readonly MedlabDbContext _context;

        public DoctorRepository(MedlabDbContext context):base(context)
        {
            this._context = context;
        }
    }
}
