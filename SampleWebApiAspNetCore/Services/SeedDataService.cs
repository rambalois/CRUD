using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(CSGODbContext context)
        {
            context.CSGOItems.Add(new CSGOEntity() { Damage = 10, Type = "Pistol", Name = "P250", Created = DateTime.Now });
            context.CSGOItems.Add(new CSGOEntity() { Damage = 30, Type = "Rifle", Name = "AK47", Created = DateTime.Now });
            context.CSGOItems.Add(new CSGOEntity() { Damage = 25, Type = "SMG", Name = "Mac-10", Created = DateTime.Now });
            context.CSGOItems.Add(new CSGOEntity() { Damage = 120, Type = "Sniper", Name = "AWP", Created = DateTime.Now });

            context.SaveChanges();
        }
    }
}
