using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public CategoryDTO Create(CategoryDTO objDTO);
        public CategoryDTO Update(CategoryDTO objDTO);
        public CategoryDTO Get(int id);
        public IEnumerable<CategoryDTO> GetAll();
        public int Delete(int id);
    }
}
