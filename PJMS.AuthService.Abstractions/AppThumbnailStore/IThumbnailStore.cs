namespace PJMS.AuthService.Abstractions.AppThumbnailStore;

/// <summary>
/// Интерфейс для хранилища миниатюр.
/// </summary>
public interface IThumbnailStore
{
    /// <summary>
    /// Сохраняет миниатюру из URL.
    /// </summary>
    /// <param name="url">URL для загрузки миниатюры.</param>
    /// <param name="id">Идентификатор фото.</param>
    /// <returns>URI сохраненной миниатюры.</returns>
    Task<Uri> SaveAsync(Uri url, Guid id);

    /// <summary>
    /// Сохраняет миниатюру из потока данных.
    /// </summary>
    /// <param name="stream">Поток данных с миниатюрой.</param>
    /// <param name="id">Идентификатор фото.</param>
    /// <returns>URI сохраненной миниатюры.</returns>
    Task<Uri> SaveAsync(Stream stream, Guid id);

    /// <summary>
    /// Удаляет миниатюру по URI.
    /// </summary>
    /// <param name="uri">URI миниатюры для удаления.</param>
    Task DeleteAsync(Uri uri);
}