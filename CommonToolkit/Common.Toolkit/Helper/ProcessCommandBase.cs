using System.Diagnostics;
using System.Text;

namespace Common.Toolkit.Helper
{
    public class ProcessCommandBase : IDisposable
    {
        //程序名
        public string programe;
        //参数
        StringBuilder parameter = new();
        Process? process = null;

        public ProcessCommandBase(string programe)
        {
            if (string.IsNullOrEmpty(programe))
            {
                throw new ArgumentException($"“{nameof(programe)}”不能为 null 或空。", nameof(programe));
            }

            this.programe = programe;
        }

        public ProcessCommandBase AddParameter(string para)
        {
            if (para is null)
            {
                return this;
            }

            parameter.Append($" {para} ");

            return this;
        }

        public void Exec(bool waitForExit = false, string? workDirectory = null)
        {
            //var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            process = new Process();
            process.StartInfo.FileName = programe;
            process.StartInfo.Arguments = parameter.ToString();
            if (!string.IsNullOrEmpty(workDirectory))
            {
                process.StartInfo.WorkingDirectory = workDirectory;
            }
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;

            //重定向标准输输出、标准错误流
            process.StartInfo.RedirectStandardError = true;
            //process.StartInfo.RedirectStandardOutput = true;

            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Exited += Process_Exited;
            //process.OutputDataReceived += Process_OutputDataReceived;
            Trace.WriteLine($"Exe:{programe}");
            Trace.WriteLine($"Parameter:{parameter.ToString()}");
            process.Start();
            process.BeginErrorReadLine();
            //process.BeginOutputReadLine();
            if (waitForExit)
            {
                process.WaitForExit();
            }
        }

        public void Process_OutputDataReceived(object sender, DataReceivedEventArgs? e)
        {

        }

        public void Process_Exited(object? sender, EventArgs e)
        {

        }

        public void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        public void ClearParameter()
        {
            parameter.Clear();
        }

        public void Close()
        {
            process?.Close();
            process = null;
        }

        public void Kill()
        {
            process?.Kill();
            process?.Close();
            process = null;
        }

        public void Dispose()
        {
            Kill();
        }
    }
}
