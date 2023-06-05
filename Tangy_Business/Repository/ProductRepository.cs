using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class ProductRepository : IProductRepository
    {
        #region Dependency Injection

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        #endregion

        public async Task<ProductDTO> Create(ProductDTO objDTO)
        {
            // Convert ProductDTO to Product
            var obj = _mapper.Map<ProductDTO, Product>(objDTO);

            // Create a new Product
            var addedObj = await _db.Products!.AddAsync(obj);
            await _db.SaveChangesAsync();

            // Convert Product to ProductDTO & return it
            return _mapper.Map<Product, ProductDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var objFromDb = await _db.Products!.FindAsync(id);

            if (objFromDb is not null)
            {
                _db.Products!.Remove(objFromDb);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDTO> Get(int id)
        {
            var objFromDb = await _db.Products!
                                    .Include(p => p.Category)
                                    .FirstOrDefaultAsync(p => p.Id == id);

            if (objFromDb is not null)
            {
                return _mapper.Map<Product, ProductDTO>(objFromDb);
            }
            return new ProductDTO();
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            return await Task
                        .FromResult(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(_db.Products!
                        .Include(p => p.Category)));
        }

        public async Task<ProductDTO> Update(ProductDTO objDTO)
        {
            var objFromDb = await _db.Products!.FindAsync(objDTO.Id);

            if (objFromDb is not null)
            {
                objFromDb.Name = objDTO.Name;
                objFromDb.Description = objDTO.Description;
                objFromDb.ShopFavorites = objDTO.ShopFavorites;
                objFromDb.CustomerFavorites = objDTO.CustomerFavorites;
                objFromDb.Color = objDTO.Color;
                objFromDb.ImageUrl = objDTO.ImageUrl;
                objFromDb.CategoryId = objDTO.CategoryId;

                _db.Products.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Product, ProductDTO>(objFromDb);
            }
            return objDTO;
        }
    }
}