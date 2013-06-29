using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// Zip压缩与解压缩
    /// </summary>
    public class DZips
    {
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }
            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    using (FileStream StreamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.SetLevel(compressionLevel);
                        byte[] buffer = new byte[blockSize];
                        int sizeRead = 0;
                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                        StreamToZip.Close();
                    }
                    ZipStream.Finish();
                    ZipStream.Close();
                }
                ZipFile.Close();
            }
        }
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        public static void ZipFile(string fileToZip, string zipedFile)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }
            using (FileStream fs = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                using (FileStream ZipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.SetLevel(5);

                        ZipStream.Write(buffer, 0, buffer.Length);
                        ZipStream.Finish();
                        ZipStream.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile)
        {
            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    ZipSetp(strDirectory, s, "");
                }
            }
        }
        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strDirectory);
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                try
                {
                    if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    {
                        string pPath = parentPath;
                        pPath += file.Substring(file.LastIndexOf("\\") + 1);
                        pPath += "\\";
                        ZipSetp(file, s, pPath);
                    }
                    else // 否则直接压缩文件
                    {
                        //打开压缩文件
                        using (FileStream fs = File.OpenRead(file))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                            ZipEntry entry = new ZipEntry(fileName);
                            entry.DateTime = DateTime.Now;
                            entry.Size = fs.Length;
                            fs.Close();
                            crc.Reset();
                            crc.Update(buffer);
                            entry.Crc = crc.Value;
                            s.PutNextEntry(entry);
                            s.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch 
                {
                    //不处理异常
                }
            }
        }
        /// <summary>
        /// 解压单个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public static void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;
                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";
                    string fileName = Path.GetFileName(pathToZip);
                    Directory.CreateDirectory(strDirectory + directoryName);
                    if (fileName != "")
                    {
                        if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
        }
        /// <summary>
        /// 判断压缩文件是否可用
        /// </summary>
        /// <param name="TargetFile">文件路径</param>
        /// <returns></returns>
        public static bool ValidZipFile(string TargetFile)
        {
            try
            {
                ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (fileName != String.Empty)
                    {
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size <= 0) break;
                        }

                    }
                }
                s.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 复制文件夹到另一文件夹
        /// </summary>
        /// <param name="srcPath">源路径</param>
        /// <param name="aimPath">赋值的路径</param>
        public static void CopyDirs(string srcPath, string aimPath)
        {
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(aimPath))
            {
                Directory.CreateDirectory(aimPath);
            }
            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                {
                    CopyDirs(file, aimPath + Path.GetFileName(file));
                }
                else
                {
                    File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
        }

    }
}