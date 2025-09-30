using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductImageRepository : RepositoryBase<ProductImage>, IProductImagesRepository
    {
        public ProductImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
