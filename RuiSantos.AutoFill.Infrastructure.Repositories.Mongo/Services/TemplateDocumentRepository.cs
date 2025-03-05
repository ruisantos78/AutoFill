using System.Linq.Expressions;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Infrastructure.Repositories.Extensions;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.Mappings;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.Services;

/// <summary>
/// Repository for managing TemplateDocument entities in LiteDb.
/// </summary>
internal class TemplateDocumentRepository(IDataContext context) : ITemplateDocumentRepository
{
    /// <summary>
    /// Retrieves a TemplateDocument by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the TemplateDocument.</param>
    /// <returns>The TemplateDocument if found; otherwise, null.</returns>
    public async Task<TemplateDocument?> GetByIdAsync(Guid id)
    {
        var result = await context.FindAsync<TemplateDocumentMapper>(id);
        return result?.ToDomain();
    }

    /// <summary>
    /// Retrieves all TemplateDocuments that match the specified filter.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply.</param>
    /// <returns>A collection of TemplateDocuments.</returns>
    public async Task<IEnumerable<TemplateDocument>> GetAllAsync(Expression<Func<TemplateDocument, bool>>? filter = null)
    {
        var mapperFilter = filter?.Convert<TemplateDocument, TemplateDocumentMapper>();
        var results = await context.FindAllAsync(mapperFilter);
        return results.Select(x => x.ToDomain());
    }

    /// <summary>
    /// Creates a new TemplateDocument.
    /// </summary>
    /// <param name="document">The TemplateDocument to create.</param>
    /// <returns>The unique identifier of the created TemplateDocument.</returns>
    public async Task<Guid> CreateAsync(TemplateDocument document)
    {
        var mapper = new TemplateDocumentMapper
        {
            Id = document.Id,
            Name = document.Name,
            Content = document.Content,
            Fields = document.Fields.Select(f => new TemplateFieldsMapper
            {
                Name = f.FieldName,
                Value = f.Value,
                Label = f.Label
            }).ToList()
        };

        await context.CreateAsync(mapper);
        return mapper.Id;
    }

    /// <summary>
    /// Updates an existing TemplateDocument.
    /// </summary>
    /// <param name="document">The TemplateDocument to update.</param>
    public async Task UpdateAsync(TemplateDocument document)
    {
        var mapper = new TemplateDocumentMapper
        {
            Id = document.Id,
            Name = document.Name,
            Content = document.Content,
            Fields = document.Fields.Select(f => new TemplateFieldsMapper
            {
                Name = f.FieldName,
                Value = f.Value,
                Label = f.Label
            }).ToList()
        };

        await context.UpdateAsync(mapper);
    }

    /// <summary>
    /// Deletes a TemplateDocument.
    /// </summary>
    /// <param name="document">The TemplateDocument to delete.</param>
    public async Task DeleteAsync(TemplateDocument document)
    {
        await context.DeleteAsync<TemplateDocumentMapper>(document.Id);
    }
}