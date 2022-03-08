using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class FileScreenshotConsumerTests : UITestFixtureBase
    {
        private ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> _consumerBuilder;

        private List<string> _directorysToDelete;

        [SetUp]
        public void SetUp()
        {
            _consumerBuilder = ConfigureBaseAtataContext().
                ScreenshotConsumers.AddFile();

            _directorysToDelete = new List<string>();
        }

        [Test]
        public void FileScreenshotConsumer_DirectoryPath_Relative()
        {
            _consumerBuilder.
                WithDirectoryPath(@"TestLogs\{build-start}\{test-name-sanitized}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_DirectoryPath_Relative));

            _directorysToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_DirectoryPath_Absolute()
        {
            _consumerBuilder.
                WithDirectoryPath(@"C:\TestLogs\{build-start}\{test-name-sanitized}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string directoryPath = Path.Combine(
                @"C:\TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_DirectoryPath_Absolute));

            _directorysToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_DirectoryPathBuilder()
        {
            _consumerBuilder.
                WithDirectoryPath(() => $@"TestLogs\{AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat)}\{AtataContext.Current.TestName}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_DirectoryPathBuilder));

            _directorysToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FileName()
        {
            _consumerBuilder.
                WithFileName(@"{screenshot-number:d3} {screenshot-title:* - }{screenshot-pageobjectname}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();
            AtataContext.Current.Log.Screenshot("Some title");

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FileName));

            _directorysToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "001 Basic Controls.png"));
            FileAssert.Exists(Path.Combine(directoryPath, "002 Some title - Basic Controls.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FileName_Sanitizing()
        {
            _consumerBuilder.
                UseTestName("FileScreenshotConsumer_File\"Name\\_/Sanitizing").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();
            AtataContext.Current.Log.Screenshot("Some\\ /:title");

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FileName_Sanitizing));

            _directorysToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
            FileAssert.Exists(Path.Combine(directoryPath, "02 - Basic Controls page - Some title.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FilePath()
        {
            _consumerBuilder.
                WithFilePath(@"TestLogs\{build-start}\Test {test-name-sanitized}\{screenshot-number:d2}{screenshot-title: - *}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();
            AtataContext.Current.Log.Screenshot("Some title");

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                $"Test {nameof(FileScreenshotConsumer_FilePath)}");

            _directorysToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01.png"));
            FileAssert.Exists(Path.Combine(directoryPath, "02 - Some title.png"));
        }

        public override void TearDown()
        {
            base.TearDown();

            foreach (string directoryPath in _directorysToDelete)
                Directory.Delete(directoryPath, true);
        }
    }
}
