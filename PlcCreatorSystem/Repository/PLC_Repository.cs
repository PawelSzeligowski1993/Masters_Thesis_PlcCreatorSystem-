using PlcCreatorSystem_API.Data;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Repository.IRepository;

namespace PlcCreatorSystem_API.Repository
{
    public class PLC_Repository : Repository<PLC>, IPLCRepository
    {
        private readonly ApplicationDbContext _db;
        public PLC_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<PLC> UpdateAsync(PLC entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.PLCs.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
