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
    public class ContactMessageRepository:EntityRepository<ContactMessage> , IContactMessageRepository
    {
        private readonly MedlabDbContext _context;

        public ContactMessageRepository(MedlabDbContext context):base(context)
        {
            _context = context;
        }
    }

}
