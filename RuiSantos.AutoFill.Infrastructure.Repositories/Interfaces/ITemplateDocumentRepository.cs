using System.Linq.Expressions;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

public interface ITemplateDocumentRepository
{
    Task<TemplateDocument?> GetByIdAsync(Guid id);
    Task<IEnumerable<TemplateDocument>> GetAllAsync(Expression<Func<TemplateDocument, bool>>? filter = null);
    Task<Guid> CreateAsync(TemplateDocument document);
    Task UpdateAsync(TemplateDocument document);
    Task DeleteAsync(TemplateDocument document);
}