using Ryze.System.Domain.Interfaces.UnitOfWork;
using Ryze.System.Infra.Context;

namespace Ryze.System.Infra.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
