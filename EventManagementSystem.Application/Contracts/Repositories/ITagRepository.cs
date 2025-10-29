using EventManagementSystem.Domain.Entities;

namespace EventManagementSystem.Application.Contracts.Repositories;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetTagsAsync();
    Task<Tag?> GetTagAsync(Guid tagId);
    void AddTag(Tag tag);
    void UpdateTag(Tag tag);
    void DeleteTag(Tag tag);
    Task<bool> TagNameExistsAsync(string name);
    Task<bool> SaveAsync();
}
