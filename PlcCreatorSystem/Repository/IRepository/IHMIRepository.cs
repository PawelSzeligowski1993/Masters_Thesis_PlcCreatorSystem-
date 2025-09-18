using PlcCreatorSystem_API.Models;

namespace PlcCreatorSystem_API.Repository.IRepository
{
    public interface IHMIRepository : IRepository<HMI>
    {
        Task<HMI> UpdateAsync(HMI entity);
    }
}
