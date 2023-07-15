using Microsoft.AspNetCore.Mvc;
using WebWithFileApiExample.Constants;
using WebWithFileApiExample.Interfaces;

namespace WebWithFileApiExample.Helpers
{
    /// <summary>
    /// Функционал работы с файлом в рамках запроса
    /// </summary>
    public class FileHelper : IFileHelper
    {
        private readonly IProcessHelper _processHelper;

        public FileHelper(IProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }

        /// <summary>
        /// Получает <see cref="Stream"/> с файлом по его идентификатору
        /// </summary>
        /// <param name="id">идентификатор файла</param>
        /// <returns><see cref="Stream"/></returns>
        public async Task<Stream> GetFileById(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException($"{nameof(id)} parameter is empty");
            }
            var filePath = Path.Combine(PathConstants.TempPackagePath, $"{id}.gz");

            return await Task.Run<Stream>(() => File.OpenRead(filePath));
        }

        /// <summary>
        /// Записывает файл в хранилище
        /// по данным из <see cref="IFormFile"/>
        /// </summary>
        /// <param name="formFile"><see cref="IFormFile"/> полученный в запросе</param>
        /// <returns>Идентификатор файла</returns>
        public async Task<Guid> WriteFile(IFormFile formFile)
        {
            if (formFile is null)
            {
                throw new ArgumentNullException($"{nameof(formFile)} is null");
            }
            var fileId = Guid.NewGuid();
            var filePath = Path.Combine(PathConstants.TempPackagePath, $"{fileId}.gz");
            if (!Directory.Exists(PathConstants.TempPackagePath)) {
                Directory.CreateDirectory(PathConstants.TempPackagePath);
            }
            using (var fileWriteStream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                await formFile.CopyToAsync(fileWriteStream);
            }

            _processHelper.ProcessPackage(fileId);

            return fileId;
        }
    }
}
