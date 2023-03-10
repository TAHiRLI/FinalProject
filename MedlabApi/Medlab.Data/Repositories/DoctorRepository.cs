using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Microsoft.EntityFrameworkCore;
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

        public Doctor? GetDoctor(int id)
        {
            Doctor? doctor = _context.Doctors
                .Include(x => x.AppUser)
                .Include(x=> x.Blogs)
                .Include(x => x.DoctorAppointments)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefault(x => x.Id == id);
            return doctor;
        }

        public IEnumerable<object> GetTopDoctor()
        {
            var result = _context.Doctors
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Fullname,
                    CountOfAppointments= c.DoctorAppointments.Count(x=> x.IsApproved == true),
                    TotalPaid = _context.DoctorAppointments
                        .Where( a=> a.Doctor.Id == c.Id)
                        .Sum(a => a.TotalPaid)
                })
                .OrderByDescending(x => x.TotalPaid);

            return result;
        }
    }
}
