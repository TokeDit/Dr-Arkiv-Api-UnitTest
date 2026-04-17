using DR.Arkiv.API.Data;
using DR.Arkiv.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DR.Arkiv.API.Repositories;

public class MusicRecordRepository : IMusicRecordRepository
{
    private readonly AppDbContext _context;

    public MusicRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null)
    {
        IQueryable<MusicRecord> query = _context.MusicRecords;

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(r => r.Title.Contains(title));

        if (!string.IsNullOrWhiteSpace(artist))
            query = query.Where(r => r.Artist.Contains(artist));

        return query.ToList();
    }

    public MusicRecord? GetById(int id) =>
        _context.MusicRecords.Find(id);

    public MusicRecord Add(MusicRecord record)
    {
        _context.MusicRecords.Add(record);
        _context.SaveChanges();
        return record;
    }

    public MusicRecord? Update(int id, MusicRecord record)
    {
        var existing = _context.MusicRecords.Find(id);
        if (existing is null) return null;

        existing.Title = record.Title;
        existing.Artist = record.Artist;
        existing.Duration = record.Duration;
        existing.PublicationYear = record.PublicationYear;
        _context.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var existing = _context.MusicRecords.Find(id);
        if (existing is null) return false;

        _context.MusicRecords.Remove(existing);
        _context.SaveChanges();
        return true;
    }
}
