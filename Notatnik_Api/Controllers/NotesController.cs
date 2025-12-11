using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notatnik_Api.Data;
using Notatnik_Api.DTOs;
using Notatnik_Api.Models;
using System.Security.Claims;

namespace Notatnik_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notes")]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetNotes()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var notes = await _context.Notes
                .Where(n => n.UserId == userId)
                .Select(n => new NoteDTO
                {
                    Id = n.Id,
                    Content = n.Content
                })
                .ToListAsync();
            return Ok(notes);

        }
        [HttpGet("id")]
        public async Task<ActionResult<NoteDTO>> GetNoteById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var note = await _context.Notes
                .Where(n => n.UserId == userId && n.Id == id)
                .Select(n => new NoteDTO
                {
                    Id = n.Id,
                    Content = n.Content
                })
                .FirstOrDefaultAsync();
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }
        [HttpPost]
        public async Task<ActionResult<NoteDTO>> CreateNote(CreateNote dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var note = new Note
            {
                Content = dto.Content,
                UserId = userId
            };
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return Ok(new NoteDTO
            {
                Id = note.Id,
                Content = note.Content
            });

        }
    }
}
