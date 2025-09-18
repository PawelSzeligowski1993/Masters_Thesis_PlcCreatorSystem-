using PlcCreatorSystem_API.Data;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Repository.IRepository;

namespace PlcCreatorSystem_API.Repository
{
    public class Project_Repository : Repository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _db;
        public Project_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Project> UpdateAsync(Project entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Projects.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
