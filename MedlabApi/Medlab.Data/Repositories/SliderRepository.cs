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
    public class SliderRepository:EntityRepository<Slider> , ISliderRepository
    {
        private readonly MedlabDbContext _context;

        public SliderRepository(MedlabDbContext context):base(context)
        {
            this._context = context;
        }
    }
}
