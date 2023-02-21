using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class DoctorAppointmentRepository:EntityRepository<DoctorAppointment> , IDoctorAppointmentRepository
    {
        private readonly MedlabDbContext _context;

        public DoctorAppointmentRepository(MedlabDbContext context):base(context)
        {
            _context = context;
        }

        public List<DoctorAppointment> GetAppointmentsIncludingUsers(Expression<Func<DoctorAppointment, bool>> exp)
        {
            return _context.DoctorAppointments
                .Include(x => x.AppUser)
                .Include(x => x.Doctor)
                .ThenInclude(x => x.AppUser)
                .Where(exp)
                .ToList();


        }
    }
}
