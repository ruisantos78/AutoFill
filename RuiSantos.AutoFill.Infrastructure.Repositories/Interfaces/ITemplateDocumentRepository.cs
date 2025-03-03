using System.Linq.Expressions;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Interface for TemplateDocument repository.
/// </summary>
public interface ITemplateDocumentRepository
{
    /// <summary>
    /// Gets a TemplateDocument by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the TemplateDocument.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TemplateDocument if found; otherwise, null.</returns>
    Task<TemplateDocument?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all TemplateDocuments optionally filtered by a predicate.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TemplateDocuments.</returns>
    Task<IEnumerable<TemplateDocument>> GetAllAsync(Expression<Func<TemplateDocument, bool>>? filter = null);

    /// <summary>
    /// Creates a new TemplateDocument.
    /// </summary>
    /// <param name="document">The TemplateDocument to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the created TemplateDocument.</returns>
    Task<Guid> CreateAsync(TemplateDocument document);

    /// <summary>
    /// Updates an existing TemplateDocument.
    /// </summary>
    /// <param name="document">The TemplateDocument to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(TemplateDocument document);

    /// <summary>
    /// Deletes an existing TemplateDocument.
    /// </summary>
    /// <param name="document">The TemplateDocument to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(TemplateDocument document);
}