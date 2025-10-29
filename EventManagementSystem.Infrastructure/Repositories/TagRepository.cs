using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await _context.Tags
            .ToListAsync();
    }

    public async Task<Tag?> GetTagAsync(Guid tagId)
    {
        if (tagId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(tagId));
        }

        return await _context.Tags
            .FirstOrDefaultAsync(t => t.Id == tagId);
    }

    public void AddTag(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        tag.Id = Guid.NewGuid();

        _context.Tags.Add(tag);
    }

    public void UpdateTag(Tag tag)
    {
    }

    public void DeleteTag(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        _context.Tags.Remove(tag);
    }

    public async Task<bool> TagNameExistsAsync(string name)
    {
        return await _context.Tags.AnyAsync(t => t.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
