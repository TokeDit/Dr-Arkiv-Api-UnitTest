using DR.Arkiv.API.Models;

namespace DR.Arkiv.API.Repositories;

public interface IMusicRecordRepository
{
    IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null);
    MusicRecord? GetById(int id);
    MusicRecord Add(MusicRecord record);
    MusicRecord? Update(int id, MusicRecord record);
    bool Delete(int id);
}
