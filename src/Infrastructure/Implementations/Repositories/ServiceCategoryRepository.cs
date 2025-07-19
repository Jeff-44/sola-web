using ApplicationCore.Models;
using ApplicationCore.Interfaces.IRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Repositories
{
    public class ServiceCategoryRepository: GenericRepository<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(SolaContext context) : base(context)
        {
        }
    }
}
