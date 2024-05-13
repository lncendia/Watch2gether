namespace PJMS.AuthService.Abstractions.AppThumbnailStore;

/// <summary>
/// Интерфейс для хранилища миниатюр.
/// </summary>
public interface IThumbnailStore
{
    /// <summary>
    /// Сохраняет миниатюру из URL.
    /// </summary>
    /// <param name="id">Идентификатор фотографии.</param>
    /// <param name="url">URL для загрузки миниатюры.</param>
    /// <returns>URI сохраненной миниатюры.</returns>
    Task<Uri> SaveAsync(Guid id, Uri url);

    /// <summary>
    /// Сохраняет миниатюру из потока данных.
    /// </summary>
    /// <param name="id">Идентификатор фотографии.</param>
    /// <param name="stream">Поток данных с миниатюрой.</param>
    /// <returns>URI сохраненной миниатюры.</returns>
    Task<Uri> SaveAsync(Guid id, Stream stream);

    /// <summary>
    /// Удаляет миниатюру по URI.
    /// </summary>
    /// <param name="uri">URI миниатюры для удаления.</param>
    Task DeleteAsync(Uri uri);
}