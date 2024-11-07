using Application.Infrastructure.IRepository;
using Infrastructure.Persistence.Context;
using Models;

namespace Infrastructure.Persistence;

public class ScriptRepository(ApplicationContext context) : BaseRepository<Script>(context), IScriptRepository
{
    
}