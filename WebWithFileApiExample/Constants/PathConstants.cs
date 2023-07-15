namespace WebWithFileApiExample.Constants
{
    /// <summary>
    /// Список констант путей к директориям и файлам
    /// </summary>
    public static class PathConstants
    {
        /// <summary>
        /// Путь до папки для хранения файлов
        /// </summary>
        public static readonly string TempPackagePath
            = Path.Combine(Path.GetTempPath(), "PackageFolder");
    }
}
