using PlcCreatorSystem_API.Models;

namespace PlcCreatorSystem_API.Repository.IRepository
{
    public interface IPLCRepository : IRepository<PLC>
    {
        Task<PLC> UpdateAsync(PLC entity);
    }
}
