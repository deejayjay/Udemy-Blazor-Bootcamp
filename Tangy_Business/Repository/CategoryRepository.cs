using AutoMapper;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        #region Dependency Injection
        
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public CategoryRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        } 

        #endregion

        public async Task<CategoryDTO> Create(CategoryDTO objDTO)
        {
            // Converts CategoryDTO to Category using AutoMapper
            var obj = _mapper.Map<CategoryDTO, Category>(objDTO);
            obj.CreatedDate = DateTime.Now;
            
            var addedObj = await _db.Categories!.AddAsync(obj);
            await _db.SaveChangesAsync();

            // Converts Category to CategoryDTO using AutoMapper
            return _mapper.Map<Category, CategoryDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Categories!.FindAsync(id);

            if (obj is not null)
            {
                _db.Categories!.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CategoryDTO> Get(int id)
        {
            var obj = await _db.Categories!.FindAsync(id);
            
            if (obj is not null) 
            {
                return _mapper.Map<Category, CategoryDTO>(obj);
            }
            return new CategoryDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            return await Task.FromResult(_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_db.Categories!));
        }

        public async Task<CategoryDTO> Update(CategoryDTO objDTO)
        {
            var objFromDb = await _db.Categories!.FindAsync(objDTO.Id);

            if (objFromDb is not null)
            {
                objFromDb.Name = objDTO.Name;
                _db.Categories.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Category, CategoryDTO>(objFromDb);
            }

            return objDTO;
        }
    }
}
