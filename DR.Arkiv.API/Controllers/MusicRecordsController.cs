using DR.Arkiv.API.Models;
using DR.Arkiv.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DR.Arkiv.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicRecordsController : ControllerBase
{
    private readonly IMusicRecordRepository _repo;

    public MusicRecordsController(IMusicRecordRepository repo)
    {
        _repo = repo;
    }

    // US1: See all music records
    // US2: Filter by title / artist
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<MusicRecord>> GetAll(
        [FromQuery] string? title,
        [FromQuery] string? artist)
    {
        return Ok(_repo.GetAll(title, artist));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MusicRecord> GetById(int id)
    {
        var record = _repo.GetById(id);
        return record is null ? NotFound() : Ok(record);
    }

    // US4: Add a music record (protected)
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<MusicRecord> Create([FromBody] MusicRecord record)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = _repo.Add(record);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // US6: Update a music record (protected)
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<MusicRecord> Update(int id, [FromBody] MusicRecord record)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = _repo.Update(id, record);
        return updated is null ? NotFound() : Ok(updated);
    }

    // US5: Delete a music record (protected)
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult Delete(int id)
    {
        return _repo.Delete(id) ? NoContent() : NotFound();
    }
}
