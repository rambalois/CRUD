using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface ICSGORepository
    {
        CSGOEntity GetSingle(int id);
        void Add(CSGOdEntity item);
        void Delete(int id);
        CSGOEntity Update(int id, CSGOEntity item);
        IQueryable<CSGOEntity> GetAll(QueryParameters queryParameters);
        ICollection<FoodEntity> GetRandomCSGO();
        int Count();
        bool Save();
    }
}
