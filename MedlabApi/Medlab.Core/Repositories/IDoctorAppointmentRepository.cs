using Medlab.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Repositories
{
    public interface IDoctorAppointmentRepository:IEntityRepository<DoctorAppointment>
    {
        List<DoctorAppointment> GetAppointmentsIncludingUsers(Expression<Func<DoctorAppointment, bool>> exp);
    }
}
