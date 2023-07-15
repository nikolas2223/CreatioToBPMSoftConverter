using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using WebWithFileApiExample.Interfaces;

namespace WebWithFileApiExample.Helpers
{
    public class ProcessHelper : IProcessHelper
    {
        private readonly ILogger<ProcessHelper> _logger;

        protected static ProcessStartInfo StartInfoRoot = new ProcessStartInfo
        {
            WorkingDirectory = Constants.PathConstants.TempPackagePath,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        /// <summary>
        /// Распаковывает пакет из gz
        /// </summary>
        protected ProcessInfo ClioExtractProcessInfo;

        /// <summary>
        /// Упаковывает пакет в gz
        /// </summary>
        protected ProcessInfo ClioCompressProcessInfo;

        /// <summary>
        /// Смена имен классов
        /// </summary>
        protected ProcessInfo ClassChangerClassProcessInfo;

        /// <summary>
        /// Замена подстрок в текстовых файлах и привязках
        /// </summary>
        protected ProcessInfo ClassChangerTextProcessInfo;

        /// <summary>
        /// Обновление дескриптора под новый формат
        /// </summary>
        protected ProcessInfo ClassChangerUpdateDescriptorProcessInfo;

        /// <summary>
        /// Очистка ресурсов пакета (оставить только en, ru)
        /// TODO: В будущем рассмотреть вариант задать этот параметр с клиента
        /// </summary>
        protected ProcessInfo ClassChangerClearResourcesProcessInfo;

        /// <summary>
        /// Важна последовательность добавленных 
        /// в коллекцию инфо для процессов
        /// </summary>
        private IReadOnlyList<ProcessInfo> ProcessInfos;

        public ProcessHelper(string clioPath, string classChangerPath, ILogger<ProcessHelper> logger)
        {
            _logger = logger;

            InitProcessInfo(clioPath, classChangerPath);
            InitProcessInfoCollection();
        }

        /// <summary>
        /// Выполняет обработку пакета
        /// </summary>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <exception cref="ArgumentException">Идентификатор файла не указан</exception>
        public void ProcessPackage(Guid packageId)
        {
            if (packageId == Guid.Empty)
            {
                _logger.LogError($"{nameof(packageId)} not set");
                throw new ArgumentException(nameof(packageId));
            }
            foreach (ProcessInfo info in ProcessInfos)
            {
                StartInfoRoot.Arguments = string.Format(info.ArgumentsString, packageId);
                StartInfoRoot.FileName = info.UtilityPath;
                var proc = new Process() { StartInfo = StartInfoRoot };
                proc.Start();
                proc.WaitForExit();
            }
        }

        protected void InitProcessInfo(string clioPath, string classChangerPath)
        {
            ClioExtractProcessInfo = new ProcessInfo
            {
                UtilityPath = clioPath,
                ArgumentsString = "extract-pkg-zip {0}.gz"
            };
            ClioCompressProcessInfo = new ProcessInfo
            {
                UtilityPath = clioPath,
                ArgumentsString = "generate-pkg-zip {0}",
            };
            ClassChangerClassProcessInfo = new ProcessInfo
            {
                UtilityPath = classChangerPath,
                ArgumentsString = "changeClass -p {0} -o Terrasoft -n BPMSoft -u",
            };
            ClassChangerTextProcessInfo = new ProcessInfo
            {
                UtilityPath = classChangerPath,
                ArgumentsString = "replaceText -p {0} -o Terrasoft -n BPMSoft",
            };
            ClassChangerUpdateDescriptorProcessInfo = new ProcessInfo
            {
                UtilityPath = classChangerPath,
                ArgumentsString = "changePackageDescriptor -p {0}",
            };
            ClassChangerClearResourcesProcessInfo = new ProcessInfo
            {
                UtilityPath = classChangerPath,
                ArgumentsString = "clearResources -p {0} -w ru-RU,en-US",
            };
        }

        protected void InitProcessInfoCollection()
        {
            ProcessInfos = new List<ProcessInfo>()
            {
                ClioExtractProcessInfo,
                ClassChangerClassProcessInfo,
                ClassChangerTextProcessInfo,
                ClassChangerUpdateDescriptorProcessInfo,
                ClassChangerClearResourcesProcessInfo,
                ClioCompressProcessInfo,
            };
        }

        protected class ProcessInfo
        {
            public string UtilityPath { get; set; }
            public string ArgumentsString { get; set; }
        }
    }
}
