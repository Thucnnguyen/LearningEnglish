using Application.Infrastructure.IRepository;
using Infrastructure.Persistence.Context;
using Models;

namespace Infrastructure.Persistence;

public class AudioRepository(ApplicationContext context) : BaseRepository<Audio>(context), IAudioRepository;