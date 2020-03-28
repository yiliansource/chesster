using System;
using System.Text;
using System.Diagnostics;

using Chesster.Logging;

namespace Chesster.Chess.Engines
{
    /// <summary>
    /// A wrapper for a chess engine that uses the UCI protocol. Used to evaluate positions.
    /// </summary>
    public abstract class UciEngine : IDisposable
    {
        /// <summary>
        /// The path where the engine is located at.
        /// </summary>
        protected abstract string EnginePath { get; } 

        protected Process _engineProcess;

        public UciEngine()
        {
            Initialize();
        }

        /// <summary>
        /// The standard initialization procedure for a UCI engine.
        /// </summary>
        protected virtual void Initialize()
        {
            // Create the engine process, making sure to redirect the streams
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

            // When an error occurs, make sure to log it
            _engineProcess.ErrorDataReceived += (s, e) => Logger.Error<StockfishEngine>(e.Data);

            // Start the engine process
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

        /// <summary>
        /// Evaluates the given position, from the given perspective, at the given depth.
        /// </summary>
        public abstract Evaluation Evaluate(string fen, bool isWhite, int depth);

        /// <summary>
        /// Sends a command to the engine via the standard input.
        /// </summary>
        protected void SendCommand(string command)
        {
            _engineProcess.StandardInput.WriteLine(command);
        }
        /// <summary>
        /// Reads output from the engine until the specified terminator is found.
        /// </summary>
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