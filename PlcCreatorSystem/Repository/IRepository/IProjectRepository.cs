using PlcCreatorSystem_API.Models;

namespace PlcCreatorSystem_API.Repository.IRepository
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project> UpdateAsync(Project entity);
    }
}
