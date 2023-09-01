using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class CSGOSqlRepository : ICSGORepository
    {
        private readonly CSGODbContext _CSGODbContext;

        public CSGOSqlRepository(CSGODbContext CSGODbContext)
        {
            _CSGODbContext = CSGODbContext;
        }

        public CSGOEntity GetSingle(int id)
        {
            return _CSGODbContext.CSGOItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(CSGOEntity item)
        {
            _CSGODbContext.CSGOItems.Add(item);
        }

        public void Delete(int id)
        {
            CSGOEntity CSGOItem = GetSingle(id);
            _CSGODbContext.CSGOItems.Remove(CSGOItem);
        }

        public CSGOEntity Update(int id, CSGOEntity item)
        {
            _CSGODbContext.CSGOItems.Update(item);
            return item;
        }

        public IQueryable<CSGOEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<CSGOEntity> _allItems = _CSGODbContext.CSGOItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Damage.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _CSGODbContext.CSGOItems.Count();
        }

        public bool Save()
        {
            return (_CSGODbContext.SaveChanges() >= 0);
        }

        public ICollection<CSGOEntity> GetRandomCSGO()
        {
            List<CSGOEntity> toReturn = new List<CSGOEntity>();

            toReturn.Add(GetRandomItem("Pistol"));
            toReturn.Add(GetRandomItem("Rifle"));
            toReturn.Add(GetRandomItem("SMG"));

            return toReturn;
        }

        private CSGOEntity GetRandomItem(string type)
        {
            return _CSGODbContext.CSGOItems
                .Where(x => x.Type == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
