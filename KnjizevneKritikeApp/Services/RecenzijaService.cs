using KnjizevneKritikeApp.Services;
using MongoDB.Driver;

public class RecenzijaService
{
    private readonly IMongoCollection<Recenzija> _recenzije;

    public RecenzijaService(MongoDbService db)
    {
        _recenzije = db.Database.GetCollection<Recenzija>("Recenzije");
    }

    public async Task<List<Recenzija>> GetAllAsync()
        => await _recenzije.Find(_ => true).ToListAsync();

    public async Task<List<Recenzija>> GetByKnjigaIdAsync(string knjigaId)
        => await _recenzije.Find(x => x.KnjigaId == knjigaId).ToListAsync();
}
