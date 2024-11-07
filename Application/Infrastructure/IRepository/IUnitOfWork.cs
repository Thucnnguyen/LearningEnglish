using System.Data;

namespace Application.Infrastructure.IRepository;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    
    void Rollback();
    
    IDbTransaction BeginTransaction();
    
    IAudioRepository AudioRepository { get; }
    IScriptRepository ScriptRepository { get; }
}