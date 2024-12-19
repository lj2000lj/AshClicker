using System;
using System.IO;
using System.Reflection;

namespace AshClicker
{
    public static class TempDllManager
    {
        /// <summary>
        /// 提取嵌入的 DLL 文件到临时目录，并返回临时文件路径。
        /// </summary>
        /// <returns>提取的临时 DLL 文件路径</returns>
        public static string ExtractTempDll()
        {
            // 嵌入资源文件路径
            const string resourcePath = "AshClicker.Assets.Dll.interception.dll";

            // 生成临时文件路径
            var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.dll");

            try
            {
                // 获取嵌入资源流
                using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
                if (resourceStream == null)
                {
                    throw new InvalidOperationException($"Embedded resource '{resourcePath}' not found.");
                }

                // 创建临时文件并写入资源数据
                using var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
                resourceStream.CopyTo(fileStream);

                return tempFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to extract DLL: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 删除临时 DLL 文件。
        /// </summary>
        /// <param name="tempFilePath">要删除的临时 DLL 文件路径</param>
        public static void CleanupTempDll(string tempFilePath)
        {
            try
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete temporary DLL file '{tempFilePath}': {ex.Message}");
            }
        }
    }
}