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
    /// <returns>URI сохраненной миниатюры.</returns>
    Task<Uri> SaveAsync(Uri url);

    /// <summary>
    /// Сохраняет миниатюру из потока данных.
    /// </summary>
    /// <param name="stream">Поток данных с миниатюрой.</param>
    /// <returns>URI сохраненной миниатюры.</returns>
    Task<Uri> SaveAsync(Stream stream);

    /// <summary>
    /// Удаляет миниатюру по URI.
    /// </summary>
    /// <param name="uri">URI миниатюры для удаления.</param>
    Task DeleteAsync(Uri uri);
}