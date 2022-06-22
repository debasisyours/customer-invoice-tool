using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Reflection;

namespace CustomerInvoice.Common
{
    public enum LogLevel { 
         None = 0
        ,Error = 1
        ,Information = 2
    };

    public sealed class Logger
    {

        #region Private Constants
        
        private const string LogFileName = "LogFile.txt";

        #endregion

        #region private methods

        private static void WriteLogInternal(string logDetails, LogLevel level)
        {
            string applicationName = "CustomerInvoice";
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string timeStamp = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2} {3}:{4}:{5}",
                                    DateTime.Now.Day.ToString().PadLeft(2, '0'),
                                    DateTime.Now.Month.ToString().PadLeft(2, '0'),
                                    DateTime.Now.Year.ToString().PadLeft(4, '0'),
                                    DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                                    DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                                    DateTime.Now.Second.ToString().PadLeft(2, '0'));

            string folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), applicationName);
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string fileName = Path.Combine(folderName, LogFileName);
            using (FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0} Ver {1}", applicationName, version));
                    writer.WriteLine(timeStamp);
                    switch (level)
                    {
                        case LogLevel.Information:
                            {
                                writer.WriteLine("Information");
                                break;
                            }
                        case LogLevel.Error:
                            {
                                writer.WriteLine("Error");
                                break;
                            }
                    }
                    writer.WriteLine(logDetails);
                    writer.Flush();
                }
            }
        }

        #endregion

        #region public methods

        public static void WriteLog(Exception ex)
        {
            WriteLogInternal(ex.Message, LogLevel.Error);
        }

        public static void WriteLogDetails(Exception ex)
        {
            WriteLogInternal(ex.StackTrace, LogLevel.Error);
        }

        public static void WriteInformation(string information)
        {
            WriteLogInternal(information, LogLevel.Information);
        }

        #endregion
    }
}
