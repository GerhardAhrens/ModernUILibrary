//-----------------------------------------------------------------------
// <copyright file="FileAsyncCopy.cs" company="Lifeprojects.de">
//     Class: FileAsyncCopy
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.01.2023</date>
//
// <summary>
// Klasse zum asychronen kopieren von Dateien
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System.ComponentModel;
    using System.IO;

    public class FileAsyncCopy
    {
        private readonly string _source;
        private readonly string _target;
        private readonly BackgroundWorker _worker;

        public FileAsyncCopy(string source, string target)
        {
            if (File.Exists(source) == false)
            {
                throw new FileNotFoundException(string.Format(@"Source file was not found. FileName: {0}", source));
            }

            _source = source;
            _target = target;
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = false;
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += DoWork;
        }


        private void DoWork(object sender, DoWorkEventArgs e)
        {
            int bufferSize = 1024 * 512;
            using (FileStream inStream = new FileStream(_source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream fileStream = new FileStream(_target, FileMode.OpenOrCreate, FileAccess.Write))
            {
                int bytesRead = -1;
                var totalReads = 0;
                var totalBytes = inStream.Length;
                byte[] bytes = new byte[bufferSize];
                int prevPercent = 0;

                string[] userState = new string[] { _source, _target };

                e.Result = new ResultBackgroundWorker {
                    SourceFile = _source,
                    TargetFile = _target,
                    Length = inStream.Length,
                    UserState = e.Argument };

                while ((bytesRead = inStream.Read(bytes, 0, bufferSize)) > 0)
                {
                    fileStream.Write(bytes, 0, bytesRead);
                    totalReads += bytesRead;
                    int percent = System.Convert.ToInt32(((decimal)totalReads / (decimal)totalBytes) * 100);
                    if (percent != prevPercent)
                    {
                        _worker.ReportProgress(percent, userState);
                        prevPercent = percent;
                    }

                    if (inStream.Length < 30000000)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }

            }
        }

        public event ProgressChangedEventHandler ProgressChanged
        {
            add { _worker.ProgressChanged += value; }
            remove { _worker.ProgressChanged -= value; }
        }

        public event RunWorkerCompletedEventHandler Completed
        {
            add { _worker.RunWorkerCompleted += value; }
            remove { _worker.RunWorkerCompleted -= value; }
        }

        public void StartAsync()
        {
            _worker.RunWorkerAsync();
        }
    }

    public class ResultBackgroundWorker
    {
        public string SourceFile { get; set; }

        public string TargetFile { get; set; }

        public long Length { get; set; }

        public object UserState { get; set; }
    }
}
