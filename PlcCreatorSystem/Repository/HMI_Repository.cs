using PlcCreatorSystem_API.Data;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Repository.IRepository;

namespace PlcCreatorSystem_API.Repository
{
    public class HMI_Repository : Repository<HMI>, IHMIRepository
    {
        private readonly ApplicationDbContext _db;
        public HMI_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<HMI> UpdateAsync(HMI entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.HMIs.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
