using System;
using System.Text;
using System.Diagnostics;

using Chesster.Logging;

namespace Chesster.Chess.Engines
{
    public abstract class UciEngine : IDisposable
    {
        public abstract string EnginePath { get; } 

        protected Process _engineProcess;

        public virtual void Initialize()
        {
            _engineProcess = new Process
            {
                StartInfo =
                {
                    FileName = EnginePath,

                    CreateNoWindow = true,
                    UseShellExecute = false,

                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            _engineProcess.ErrorDataReceived += (s, e) => Logger.Error<StockfishEngine>(e.Data);

            _engineProcess.Start();
            _engineProcess.BeginErrorReadLine();

            Logger.Debug<UciEngine>("Started UCI process.");
        }
        public virtual void Dispose()
        {
            Logger.Debug<UciEngine>("Killed UCI process.");

            _engineProcess.Kill();
            _engineProcess.Dispose();
        }

        public abstract Evaluation Evaluate(string fen, bool isWhite, int depth);

        protected void SendCommand(string command)
        {
            _engineProcess.StandardInput.WriteLine(command);
        }
        protected string ReadResult(string until)
        {
            StringBuilder result = new StringBuilder();
            string line;
            do
            {
                line = _engineProcess.StandardOutput.ReadLine();
                result.AppendLine(line);
            } while (!line.StartsWith(until));
            return result.ToString();
        }
    }
}