namespace Ryze.System.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();
        Task RollbackAsync();        
    }
}


