using AutoMapper;
using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Exceptions;
using EventManagementSystem.Application.Helpers;
using EventManagementSystem.Application.Models.Tags;
using EventManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace EventManagementSystem.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public TagService(
        ITagRepository repository,
        IMapper mapper,
        IServiceProvider serviceProvider)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _serviceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<IEnumerable<TagDto>> GetTagsAsync()
    {
        var tags = await _repository.GetTagsAsync();

        return _mapper.Map<IEnumerable<TagDto>>(tags);
    }

    public async Task<TagDto> GetTagAsync(Guid tagId)
    {
        var tag = await _repository.GetTagAsync(tagId)
            ?? throw new NotFoundException($"Tag with id {tagId} not found.");

        return _mapper.Map<TagDto>(tag);
    }

    public async Task<TagDto> CreateTagAsync(TagForCreationDto dto)
    {
        await ValidationHelper.ThrowIfInvalidAsync(dto, _serviceProvider);

        if (await _repository.TagNameExistsAsync(dto.Name))
        {
            throw new CustomValidationException(new Dictionary<string, string[]>
            {
                {
                    nameof(dto.Name),
                    new[] { "Tag name must be unique (case-insensitive)." }
                }
            });
        }

        var entity = _mapper.Map<Tag>(dto);
        _repository.AddTag(entity);
        await _repository.SaveAsync();

        return _mapper.Map<TagDto>(entity);
    }

    public async Task PartiallyUpdateTagAsync(Guid tagId, JsonPatchDocument<TagForUpdateDto> patchDoc)
    {
        var entity = await _repository.GetTagAsync(tagId)
            ?? throw new NotFoundException($"Tag with id {tagId} not found.");

        var dto = _mapper.Map<TagForUpdateDto>(entity);
        var modelState = new ModelStateDictionary();
        patchDoc.ApplyTo(dto, modelState);

        if (!modelState.IsValid)
        {
            var errors = modelState
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage)
                .ToArray());

            throw new CustomValidationException(errors);
        }

        await ValidationHelper.ThrowIfInvalidAsync(dto, _serviceProvider);

        if (await _repository.TagNameExistsAsync(dto.Name))
        {
            throw new CustomValidationException(new Dictionary<string, string[]>
            {
                {
                    nameof(dto.Name),
                    new[] { "Tag name must be unique (case-insensitive)." }
                }
            });
        }

        _mapper.Map(dto, entity);
        _repository.UpdateTag(entity);
        await _repository.SaveAsync();
    }

    public async Task DeleteTagAsync(Guid tagId)
    {
        var entity = await _repository.GetTagAsync(tagId)
            ?? throw new NotFoundException($"Tag with id {tagId} not found.");

        _repository.DeleteTag(entity);
        await _repository.SaveAsync();
    }
}
