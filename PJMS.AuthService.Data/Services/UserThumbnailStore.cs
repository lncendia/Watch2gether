using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using PJMS.AuthService.Abstractions.AppThumbnailStore;
using PJMS.AuthService.Abstractions.Exceptions;
using RestSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PJMS.AuthService.Data.Services;

/// <inheritdoc cref="IThumbnailStore"/>
/// <summary>
/// Хранилище миниатюр.
/// </summary>
public class UserThumbnailStore : IThumbnailStore, IDisposable
{
    /// <summary>
    /// Корневой путь.
    /// </summary>
    private readonly string _rootPath;

    /// <summary>
    /// Путь.
    /// </summary>
    private readonly string _path;

    /// <summary>
    /// Клиент REST.
    /// </summary>
    private readonly RestClient _client;

    /// <summary>
    /// Конструктор класса UserThumbnailStore.
    /// </summary>
    /// <param name="rootPath">Корневой путь.</param>
    /// <param name="path">Путь.</param>
    /// <param name="client">HTTP-клиент (необязательный).</param>
    public UserThumbnailStore(string rootPath, string path, HttpClient? client = null)
    {
        _rootPath = rootPath;
        _path = path;
        _client = client == null ? new RestClient() : new RestClient(client, true);
        _client = new RestClient();
    }

    /// <inheritdoc cref="IThumbnailStore.DeleteAsync"/>
    /// <summary>
    /// Удаляет миниатюру по URI.
    /// </summary>
    public Task DeleteAsync(Uri uri)
    {
        File.Delete(Path.Combine(_rootPath, uri.ToString()));
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IThumbnailStore.SaveAsync(System.Guid, System.Uri)"/>
    /// <summary>
    /// Сохраняет миниатюру из URL.
    /// </summary>
    public async Task<Uri> SaveAsync(Guid id, Uri url)
    {
        try
        {
            // Загрузка потока данных из указанного URL с использованием REST-клиента.
            var stream = await _client.DownloadStreamAsync(new RestRequest(url));

            // Сохранение потока и возврат url на файл
            return await SaveStreamAsync(id, stream ?? throw new NullReferenceException());
        }
        catch (Exception ex)
        {
            // Если произошла ошибка, выбрасываем исключение ThumbnailSaveException с передачей внутреннего исключения.
            throw new ThumbnailSaveException(ex);
        }
    }

    /// <inheritdoc cref="IThumbnailStore.SaveAsync(System.Guid, System.IO.Stream)"/>
    /// <summary>
    /// Сохраняет миниатюру из потока данных.
    /// </summary>
    public async Task<Uri> SaveAsync(Guid id, Stream stream)
    {
        try
        {
            // Сохранение потока и возврат url на файл
            return await SaveStreamAsync(id, stream);
        }
        catch (Exception ex)
        {
            // Если произошла ошибка, выбрасываем исключение ThumbnailSaveException с передачей внутреннего исключения.
            throw new ThumbnailSaveException(ex);
        }
    }

    /// <summary>
    /// Асинхронно сохраняет поток данных в виде изображения.
    /// </summary>
    /// <param name="stream">Поток данных.</param>
    /// <param name="id">Идентификатор фото.</param>
    /// <returns>Относительный путь к сохраненному файлу.</returns>
    private async Task<Uri> SaveStreamAsync(Guid id, Stream stream)
    {
        // Загрузка изображения из потока данных.
        using var image = await Image.LoadAsync(stream);

        // Изменение размера изображения на 128x128 пикселей.
        image.Mutate(x => x.Resize(128, 128));

        // Генерация уникального имени файла с расширением .jpg.
        var fileName = $"{id}.jpg";

        // Получаем путь к директории для сохранения.
        var directoryPath = Path.Combine(_rootPath, _path);

        // Проверка существования директории по указанному пути.
        if (!Directory.Exists(directoryPath))
        {
            // Создание директории, если она не существует.
            Directory.CreateDirectory(directoryPath);
        }

        // Сохранение изображения по указанному пути.
        await image.SaveAsync(Path.Combine(directoryPath, fileName));

        // Возвращение относительного пути к сохраненному файлу.
        return new Uri(Path.Combine(_path, fileName), UriKind.Relative);
    }

    /// <summary>
    /// Освобождает ресурсы.
    /// </summary>
    public void Dispose()
    {
        // Предотвращаем вызов GC
        GC.SuppressFinalize(this);
        
        // Уничтожение Rest Client
        _client.Dispose();
    }
}