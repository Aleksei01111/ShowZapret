using DB.Context;
using DB.Entities;

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
        _context.Notes.Where(n => n.User.Login == user.Login).ToList();
    
    public void ClearAllNotes()
    {
        if (!_context.Notes.Any())
            return;
        
        _context.Notes.RemoveRange(_context.Notes.ToList());
        _context.SaveChanges();
    }
}