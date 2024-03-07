namespace Villains.Library.Abstractions;

/// <summary>
/// The service for working with villains.
/// </summary>
public interface IVillainsService
{
    /// <summary>
    /// Returns all the villains.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public IAsyncEnumerable<Villain> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns a villain by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<Result<Villain>> GetAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Checks if a villain exists by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<bool> ExistsAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new villain.
    /// </summary>
    /// <param name="newVillain"></param>
    /// <param name="ct"></param>
    /// <returns>The id of the new villain </returns>
    public Task<string> CreateAsync(NewVillain newVillain, CancellationToken ct = default);

    /// <summary>
    /// Updates a villain.
    /// </summary>
    /// <param name="villain"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<Result> UpdateAsync(EditVillain villain, CancellationToken ct = default);
}