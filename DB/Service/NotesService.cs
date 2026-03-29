using DB.Context;
using DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DB.Service;

public class NotesService
{
    private ShowZapretDbContext _context = new();

    public void SaveNote(Note note)
    {
        note.User = _context.Users.First(u => u.Login == note.User.Login);
        _context.Notes.Add(note);
        _context.SaveChanges();
    }
    
    public List<Note> GetNotesForUser(User user) => 
        _context.Notes.Include(n => n.User)
            .Where(n => n.User.Login == user.Login).ToList();
    
    public List<Note> GetAllNotes() => _context.Notes.
        Include(n => n.User).ToList();
    
    public void ClearAllNotes()
    {
        if (!_context.Notes.Any())
            return;
        
        _context.Notes.RemoveRange(_context.Notes.ToList());
        _context.SaveChanges();
    }
}