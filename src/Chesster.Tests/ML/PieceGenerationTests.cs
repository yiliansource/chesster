using System;
using System.IO;
using System.Linq;

using Xunit;

namespace Chesster.ML.Tests
{
    public sealed class PieceGenerationTests //: IDisposable
    {
        // TODO: Travis test fails, due to a FileNotFoundException.
        //       Will look into this, but the test will remain disabled for now.

        //[Fact]
        //public void TestPieceGeneration()
        //{
        //    Logger.RegisterOutput<ConsoleLogOutput>();
        //    Logger.RegisterOutput<FileLogOutput>();
        //    Logger.Settings.MinimumLevel = LogLevel.Debug;
        //
        //    var generator = new Training.PieceImageGenerator(64,
        //       IO.TrainingDataPath,
        //       IO.GetFiles(IO.SpritesheetsPath, "*.png|*.jpg"),
        //       IO.GetFiles(IO.BackgroundsPath, "*.png|*.jpg"));
        //
        //    generator.Generate();
        //
        //    Assert.Equal(generator.ResultingFileCount, Directory.EnumerateFiles(IO.TrainingDataPath, "*.png", SearchOption.AllDirectories).Count());
        //    Assert.Equal(Enum.GetValues(typeof(Piece)).Length, Directory.EnumerateDirectories(IO.TrainingDataPath).Count());
        //}
        //public void Dispose()
        //{
        //    Directory.Delete(IO.TrainingDataPath, true);
        //}
    }
}
