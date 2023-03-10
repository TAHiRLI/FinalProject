using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class DoctorAppointmentRepository : EntityRepository<DoctorAppointment>, IDoctorAppointmentRepository
    {
        private readonly MedlabDbContext _context;

        public DoctorAppointmentRepository(MedlabDbContext context) : base(context)
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

        public object GetAppointmentPaymentByMonth()
        {
            var result = _context.DoctorAppointments
                        .Where(x => x.CreatedAt.Year == DateTime.Now.Year)
                        .AsEnumerable()
                        .OrderBy(x => x.CreatedAt)
                        .GroupBy(r => r.CreatedAt.ToString("MMMM"))
                        .Select(g => new { Month = g.Key, TotalPaid = g.Sum(x => x.TotalPaid) })
                        .Cast<object>()
                        .Union(
                            Enumerable.Range(1, 12)
                                .Where(month => !_context.DoctorAppointments.Any(x => x.CreatedAt.Month == month && x.CreatedAt.Year == DateTime.Now.Year))
                                .Select(month => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), TotalPaid = 0 })
                                .Cast<object>()
                        )
                        .OrderBy(x => DateTime.ParseExact(((dynamic)x).Month, "MMMM", CultureInfo.CurrentCulture))
                        .ToDictionary(g => ((dynamic)g).Month, g => ((dynamic)g).TotalPaid);

            return result;
        }
    }
}
