using System.Linq.Expressions;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Extensions;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Mappings;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Services;

internal class TemplateDocumentRepository(IDataContext context) : ITemplateDocumentRepository
{
    public async Task<TemplateDocument?> GetByIdAsync(Guid id)
    {
        var result = await context.FindAsync<TemplateDocumentMapper>(id);
        return result?.ToDomain();
    }

    public async Task<IEnumerable<TemplateDocument>> GetAllAsync(Expression<Func<TemplateDocument, bool>>? filter = null)
    {
        var mapperFilter = filter?.Convert<TemplateDocument, TemplateDocumentMapper>();
        var results = await context.FindAllAsync(mapperFilter);
        return results.Select(x => x.ToDomain());
    }

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

    public async Task DeleteAsync(TemplateDocument document)
    {
        await context.DeleteAsync<TemplateDocumentMapper>(document.Id);
    }
}