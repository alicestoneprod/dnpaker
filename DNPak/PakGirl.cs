namespace DnPaker
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using zlib;

    internal class PakGirl
    {
        private static Encoding DEncoding = Encoding.Default;
        public static char[] chars = DEncoding.GetChars(new byte[1]);

        private static string clear(string str)
        {
            int index = str.IndexOf(Convert.ToChar((byte) 0));
            return ((index <= 0) ? str : str.Remove(index));
        }

        private static byte[] compressFile(string inFile, out int fileLength)
        {
            byte[] buffer2;
            MemoryStream stream = new MemoryStream();
            ZOutputStream output = new ZOutputStream(stream, 1);
            FileStream input = new FileStream(inFile, FileMode.Open);
            fileLength = (int) input.Length;
            try
            {
                CopyStream(input, output);
                output.Close();
                input.Close();
                byte[] buffer = stream.ToArray();
                stream.Close();
                buffer2 = buffer;
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
                buffer2 = null;
            }
            finally
            {
                if (!ReferenceEquals(input, null))
                {
                    input.Close();
                }
            }
            return buffer2;
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[0x7d0];
            while (true)
            {
                int count = input.Read(buffer, 0, 0x7d0);
                if (count <= 0)
                {
                    output.Flush();
                    return;
                }
                output.Write(buffer, 0, count);
            }
        }

        private static void decompressFile(byte[] inBytes, string outFile)
{
    MemoryStream input = new MemoryStream(inBytes);
    string directoryName = Path.GetDirectoryName(outFile);
    if (!Directory.Exists(directoryName))
    {
        Directory.CreateDirectory(directoryName);
    }

    ZOutputStream output = null;
    FileStream stream2 = null;

    try
    {
        output = new ZOutputStream(new FileStream(outFile, FileMode.Create));
        CopyStream(input, output);
    }
    catch (Exception exception1)
    {
        MessageBox.Show(exception1.Message + "\r\n又是你：" + outFile);
    }
    finally
    {
        if (output != null)
        {
            output.Close();
        }

        if (stream2 != null)
        {
            stream2.Close();
        }

        input.Close();
    }
}


        public static int File2Pak(string FolderPath, BackgroundWorker backgroundWorker1)
        {
            string str;
            string directoryName;
            int length;
            if (Path.GetFileName(FolderPath) != "resource")
            {
                if ((Directory.GetDirectories(FolderPath, "resource").Length != 1) && (Directory.GetDirectories(FolderPath, "mapdata").Length != 1))
                {
                    MessageBox.Show("【Resource-12345/resource/[files]】");
                    return 0;
                }
                else
                {
                    directoryName = FolderPath;
                    str = FolderPath + ".pak";
                }
            }
            else
            {
                directoryName = Path.GetDirectoryName(FolderPath);
                DateTime now = DateTime.Now;
                object[] objArray = new object[] { directoryName, @"\Resource-", now.DayOfYear, now.Hour, now.Minute, now.Second, ".pak" };
                str = string.Concat(objArray);
            }
            string[] strArray = Directory.GetFiles(FolderPath, "*", SearchOption.AllDirectories);
            backgroundWorker1.ReportProgress(2, "Файлы для упаковки：" + strArray.Length);
            PaKInfo info = new PaKInfo {
                FilePaths = new string[strArray.Length],
                FileOutSizes = new int[strArray.Length],
                FileZipSizes = new int[strArray.Length],
                FileOffsets = new int[strArray.Length]
            };
            FileStream output = File.Create(str);
            BinaryWriter writer = new BinaryWriter(output, DEncoding);
            writer.Write(WriteString("EyedentityGames Packing File 0.1"));
            writer.Seek(0xe0, SeekOrigin.Current);
            writer.Write(11);
            writer.Write(strArray.Length);
            writer.Write(0);
            writer.Seek(0x2f4, SeekOrigin.Current);
            backgroundWorker1.ReportProgress(5, "Упаковка...");
            int index = 0;
            while (true)
            {
                bool flag = index < strArray.Length;
                if (!flag)
                {
                    writer.Seek(0x108, SeekOrigin.Begin);
                    writer.Write(writer.BaseStream.Length);
                    writer.Seek(0, SeekOrigin.End);
                    index = 0;
                    while (true)
                    {
                        flag = index < strArray.Length;
                        if (!flag)
                        {
                            info.FileZipSizes = null;
                            info.FileOutSizes = null;
                            info.FilePaths = null;
                            info.FileOffsets = null;
                            info = null;
                            output.Flush();
                            output.Close();
                            writer.Close();
                            backgroundWorker1.ReportProgress(100, "Общее количество файлов" + strArray.Length);
                            length = strArray.Length;
                            break;
                        }
                        long position = writer.BaseStream.Position;
                        writer.Write(WriteString(info.FilePaths[index]));
                        writer.Seek(0x100 - info.FilePaths[index].Length, SeekOrigin.Current);
                        writer.Write(info.FileZipSizes[index]);
                        writer.Write(info.FileOutSizes[index]);
                        writer.Write(info.FileZipSizes[index]);
                        writer.Write(info.FileOffsets[index]);
                        writer.Write(WriteNull(0x2c));
                        backgroundWorker1.ReportProgress(((index * 10) / strArray.Length) + 0x55, string.Concat(new object[] { "Упорядочивание файлов", index, "@", strArray.Length }));
                        index++;
                    }
                    break;
                }
                info.FileOffsets[index] = (int) output.Position;
                byte[] buffer = compressFile(strArray[index], out info.FileOutSizes[index]);
                writer.Write(buffer);
                info.FileZipSizes[index] = buffer.Length;
                info.FilePaths[index] = strArray[index].Replace(directoryName, "");
                backgroundWorker1.ReportProgress(((index * 80) / strArray.Length) + 5, string.Concat(new object[] { "Упаковка", index, "@", strArray.Length, "\r\n", strArray[index] }));
                index++;
            }
            return length;
        }

        public static int Pak2File(string PakPath, BackgroundWorker backgroundWorker1)
        {
            int num9;
            try
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(PakPath);
                string str2 = Path.GetDirectoryName(PakPath) + @"\" + fileNameWithoutExtension;
                FileStream input = new FileStream(PakPath, FileMode.Open);
                if (input.Length < 0x400L)
                {
                    throw new Exception("Ошибка размера файла");
                }
                BinaryReader reader = new BinaryReader(input);
                backgroundWorker1.ReportProgress(2, "Загрузка:" + fileNameWithoutExtension);
                byte[] buffer = new byte[0x100];
                input.Seek(0L, SeekOrigin.Begin);
                input.Read(buffer, 0, 0x100);
                if (clear(DEncoding.GetString(buffer).ToString()) != "EyedentityGames Packing File 0.1")
                {
                    throw new Exception("Неправильный тип файла!");
                }
                reader.BaseStream.Seek(4L, SeekOrigin.Current);
                int num = reader.ReadInt32();
                input.Seek((long) reader.ReadInt32(), SeekOrigin.Begin);
                backgroundWorker1.ReportProgress(5, "Найденный файл：" + num);
                long position = input.Position;
                int num7 = 0;
                while (true)
                {
                    string str4;
                    if (num7 >= num)
                    {
                        buffer = null;
                        str4 = null;
                        reader.Close();
                        input.Close();
                        input.Dispose();
                        input = null;
                        backgroundWorker1.ReportProgress(100, "Распаковка завершена. Всего было извлечено файлов:" + num + "шт");
                        num9 = num;
                        break;
                    }
                    input.Seek(position, SeekOrigin.Begin);
                    input.Read(buffer, 0, 0x100);
                    str4 = clear(DEncoding.GetString(buffer));
                    int count = reader.ReadInt32();
                    int num4 = reader.ReadInt32();
                    input.Seek(4L, SeekOrigin.Current);
                    int num5 = reader.ReadInt32();
                    input.Seek((long) 0x2c, SeekOrigin.Current);
                    position = input.Position;
                    byte[] buffer2 = new byte[count];
                    input.Seek((long) num5, SeekOrigin.Begin);
                    input.Read(buffer2, 0, count);
                    string outFile = str2 + "/" + str4;
                    decompressFile(buffer2, outFile);
                    int percentProgress = 5 + ((num7 * 90) / num);
                    backgroundWorker1.ReportProgress(percentProgress, string.Concat(new object[] { "Распаковка: ", num7, "/", num, "\r\n", str4 }));
                    num7++;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(PakPath + ":" + exception.Message);
                backgroundWorker1.ReportProgress(100, "Ошибка распаковки, " + exception.Message + "：\r\n" + PakPath);
                backgroundWorker1.Dispose();
                backgroundWorker1.CancelAsync();
                num9 = 0;
            }
            return num9;
        }

        private static byte[] WriteNull(int length)
        {
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer.SetValue((byte) 0, i);
            }
            return buffer;
        }

        private static byte[] WriteString(string Num) => 
            DEncoding.GetBytes(Num);
    }
}

