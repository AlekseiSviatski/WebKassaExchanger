using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebKassa
{
    public class INIFile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(String Section, String Key, String Value, String FilePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(String Section, String Key, String Default, StringBuilder retVal, int Size, String FilePath);

        /// <summary>
        /// Считывает значение из .ini файла из заданной секции для заданного ключа.
        /// </summary>
        /// <param name="section">название секции</param>
        /// <param name="key">название параметра</param>
        /// <param name="defaultString">значение по умолчанию для парамента</param>
        /// <param name="fileName">путь к файлу</param>
        /// <returns>значение параметра</returns>
        public static string ReadKey(string section, string key, string defaultString, string fileName)
        {
            StringBuilder StrBu = new StringBuilder();
            GetPrivateProfileString(section, key, defaultString, StrBu, 255, fileName);
            if (StrBu.ToString() == string.Empty)
                return defaultString;
            return StrBu.ToString();
        }

        /// <summary>
        /// Записывает значение в файл.
        /// </summary>
        /// <param name="section">название секции</param>
        /// <param name="key">название параметра</param>
        /// <param name="defaultString">значение по умолчанию для парамента</param>
        /// <param name="fileName">путь к файлу</param>
        /// <returns>статус выполнения функции</returns>
        public static long WriteKey(String section, String key, String value, String filePath)
        {
            long result = 0;
            result = WritePrivateProfileString(section, key, value, filePath);
            return result;
        }
    }
}
