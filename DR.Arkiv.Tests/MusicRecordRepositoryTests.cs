using DR.Arkiv.API.Data;
using DR.Arkiv.API.Models;
using DR.Arkiv.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DR.Arkiv.Tests;

public class MusicRecordRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly MusicRecordRepository _repo;

    public MusicRecordRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();
        _repo = new MusicRecordRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    // ── GetAll ────────────────────────────────────────────────────────────────

    [Fact]
    public void GetAll_ReturnsSeededRecords()
    {
        var records = _repo.GetAll();
        Assert.NotEmpty(records);
    }

    [Fact]
    public void GetAll_FilterByTitle_ReturnsMatchingRecords()
    {
        var records = _repo.GetAll(title: "Bohemian");
        Assert.All(records, r => Assert.Contains("bohemian", r.Title, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetAll_FilterByArtist_ReturnsMatchingRecords()
    {
        var records = _repo.GetAll(artist: "Queen");
        Assert.All(records, r => Assert.Contains("queen", r.Artist, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetAll_FilterByNonExistentTitle_ReturnsEmpty()
    {
        var records = _repo.GetAll(title: "zzz_does_not_exist");
        Assert.Empty(records);
    }

    // ── GetById ───────────────────────────────────────────────────────────────

    [Fact]
    public void GetById_ExistingId_ReturnsRecord()
    {
        var record = _repo.GetById(1);
        Assert.NotNull(record);
        Assert.Equal(1, record.Id);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        var record = _repo.GetById(9999);
        Assert.Null(record);
    }

    // ── Add ───────────────────────────────────────────────────────────────────

    [Fact]
    public void Add_ValidRecord_AssignsIdAndPersists()
    {
        var newRecord = new MusicRecord
        {
            Title = "Test Song",
            Artist = "Test Artist",
            Duration = 200,
            PublicationYear = 2000
        };

        var added = _repo.Add(newRecord);

        Assert.True(added.Id > 0);
        Assert.NotNull(_repo.GetById(added.Id));
    }

    // ── Update ────────────────────────────────────────────────────────────────

    [Fact]
    public void Update_ExistingRecord_ChangesValues()
    {
        var updated = _repo.Update(1, new MusicRecord
        {
            Title = "Updated Title",
            Artist = "Updated Artist",
            Duration = 999,
            PublicationYear = 2024
        });

        Assert.NotNull(updated);
        Assert.Equal("Updated Title", updated!.Title);
        Assert.Equal(999, updated.Duration);
    }

    [Fact]
    public void Update_NonExistingRecord_ReturnsNull()
    {
        var result = _repo.Update(9999, new MusicRecord
        {
            Title = "X", Artist = "X", Duration = 1, PublicationYear = 2000
        });
        Assert.Null(result);
    }

    // ── Delete ────────────────────────────────────────────────────────────────

    [Fact]
    public void Delete_ExistingRecord_ReturnsTrueAndRemoves()
    {
        var newRecord = _repo.Add(new MusicRecord
        {
            Title = "To Delete", Artist = "Artist", Duration = 100, PublicationYear = 2000
        });

        var result = _repo.Delete(newRecord.Id);

        Assert.True(result);
        Assert.Null(_repo.GetById(newRecord.Id));
    }

    [Fact]
    public void Delete_NonExistingRecord_ReturnsFalse()
    {
        Assert.False(_repo.Delete(9999));
    }
}
