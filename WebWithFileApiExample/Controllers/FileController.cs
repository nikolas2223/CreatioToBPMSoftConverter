using Microsoft.AspNetCore.Mvc;
using WebWithFileApiExample.Interfaces;
namespace WebWithFileApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileHelper _fileHelper;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileHelper fileHelper, ILogger<FileController> logger)
        {
            _fileHelper = fileHelper;
            _logger = logger;
        }

        /// <summary>
        /// Позволяет получить файл по идентификатору
        /// </summary>
        /// <param name="id">Идентифтикатор файла</param>
        /// <returns>Обработанный файл обёрнутый в <see cref="FileStreamResult"/></returns>
        // GET: api/<FileController>/09323741-073a-4de7-9336-fb657e7cab20
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation($"Requested file with id: {id}");
            // Пример с возвратом файла https://stackoverflow.com/questions/42460198/return-file-in-asp-net-core-web-api
            return File(await _fileHelper.GetFileById(id), "application/octet-stream");
        }

        /// <summary>
        /// Получает информацию о файле и сохраняет.
        /// IFileHelper в будущем может быть не физическим хранилищем, 
        /// а обёрткой под хранение в s3/памяти/БД/удалённой директории
        /// </summary>
        /// <param name="sendedFile">Отправляемые данные с клиента.
        /// Имя параметра в форме на клиентской стороне должно быть таким 
        /// же как имя принимаемого параметра в методе</param>
        /// <returns>Идентификатор-имя сохранённого файла</returns>
        // POST api/<FileController>/PostFile
        [HttpPost("PostFile")]
        public async Task<ActionResult<Guid>> PostFile(IFormFile sendedFile)
        {
            _logger.LogInformation($"added file with content-type {sendedFile.ContentType}");
            _logger.LogInformation($"added file from local: {HttpContext.Connection.LocalIpAddress}, remote: {HttpContext.Connection.RemoteIpAddress}");

            return await _fileHelper.WriteFile(sendedFile);
        }
    }
}
