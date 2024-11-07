using System.Data;
using Application.Infrastructure.IRepository;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Repository;

public class UnitOfwork: IUnitOfWork
{
    private readonly ApplicationContext _dbContext;
    private bool _disposed;
    private readonly IServiceScopeFactory _scopeFactory;

    public UnitOfwork(ApplicationContext dbContext, IServiceScopeFactory scopeFactory)
    {
        _dbContext = dbContext;
        _scopeFactory = scopeFactory;
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Rollback()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries())
            entry.State = entry.State switch
            {
                EntityState.Added => EntityState.Detached,
                _ => entry.State
            };
    }

    public IDbTransaction BeginTransaction()
    {
        var transaction = _dbContext.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }

    private IAudioRepository? _audioRepository;
    private IScriptRepository? _scriptRepository;
    
    public IAudioRepository AudioRepository => _audioRepository ??= new AudioRepository(_dbContext);
    public IScriptRepository ScriptRepository => _scriptRepository ??= new ScriptRepository(_dbContext);
}