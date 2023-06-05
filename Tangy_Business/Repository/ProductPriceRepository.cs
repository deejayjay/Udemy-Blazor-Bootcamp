using AutoMapper;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        #region Dependency Injection

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        #endregion

        public async Task<ProductPriceDTO> Create(ProductPriceDTO objDTO)
        {
            // Convert ProductPriceDTO to ProductPrice using AutoMapper
            var obj = _mapper.Map<ProductPriceDTO, ProductPrice>(objDTO);

            var addedObj = await _db.ProductPrices!.AddAsync(obj);
            await _db.SaveChangesAsync();

            // Convert ProductPrice to ProductPriceDTO using AutoMapper & return it
            return _mapper.Map<ProductPrice, ProductPriceDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var objFromDb = await _db.ProductPrices!.FindAsync(id);

            if (objFromDb is not null)
            {
                _db.ProductPrices.Remove(objFromDb);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductPriceDTO> Get(int id)
        {
            var objFromDb = await _db.ProductPrices!.FindAsync(id);

            if (objFromDb is not null)
            {
                return _mapper.Map<ProductPrice, ProductPriceDTO>(objFromDb);
            }
            return new ProductPriceDTO();
        }

        public Task<IEnumerable<ProductPriceDTO>> GetAll(int? id = null)
        {
            if (id is not null && id > 0)
            {
                return Task.FromResult(
                    _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(
                        _db.ProductPrices!.Where(pp => pp.ProductId == id)));
            }
            return Task.FromResult(_mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices!));
        }

        public async Task<ProductPriceDTO> Update(ProductPriceDTO objDTO)
        {
            var objFromDb = await _db.ProductPrices!.FindAsync(objDTO.Id);

            if (objFromDb is not null)
            {
                objFromDb.Size = objDTO.Size;
                objFromDb.Price = objDTO.Price;
                objFromDb.ProductId = objDTO.ProductId;

                _db.ProductPrices.Update(objFromDb);
                await _db.SaveChangesAsync();

                return _mapper.Map<ProductPrice, ProductPriceDTO>(objFromDb);
            }

            return objDTO;
        }
    }
}
