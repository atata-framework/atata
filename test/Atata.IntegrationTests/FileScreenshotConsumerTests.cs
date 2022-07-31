using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Atata.IntegrationTests
{
    [TestFixture]
    public class FileScreenshotConsumerTests : UITestFixtureBase
    {
        internal const string DefaultDateTimeFormat = "yyyyMMddTHHmmss";

        private ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> _consumerBuilder;

        private List<string> _directoriesToDelete;

        [SetUp]
        public void SetUp()
        {
            _consumerBuilder = ConfigureBaseAtataContext().
                ScreenshotConsumers.AddFile();

            _directoriesToDelete = new List<string>();
        }

        [Test]
        public void FileScreenshotConsumer_DirectoryPath_Relative()
        {
            _consumerBuilder.
                WithDirectoryPath(@$"TestLogs\{{build-start:{DefaultDateTimeFormat}}}\{{test-name-sanitized}}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_DirectoryPath_Relative));

            _directoriesToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_DirectoryPath_Absolute()
        {
            _consumerBuilder.
                WithDirectoryPath(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        @$"{{build-start:{DefaultDateTimeFormat}}}\{{test-name-sanitized}}")).
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                AtataContext.BuildStart.Value.ToString(DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_DirectoryPath_Absolute));

            _directoriesToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_DirectoryPathBuilder()
        {
            _consumerBuilder.
                WithDirectoryPath(() => $@"TestLogs\{AtataContext.BuildStart.Value.ToString(DefaultDateTimeFormat)}\{AtataContext.Current.TestName}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_DirectoryPathBuilder));

            _directoriesToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FileName()
        {
            _consumerBuilder
                .WithFileName(@"{screenshot-number:d3} {screenshot-title:* - }{screenshot-pageobjectname}")
                .Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();
            AtataContext.Current.Log.Screenshot("Some title");

            string directoryPath = AtataContext.Current.Artifacts.FullName;

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

            string directoryPath = AtataContext.Current.Artifacts.FullName;

            _directoriesToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01 - Basic Controls page.png"));
            FileAssert.Exists(Path.Combine(directoryPath, "02 - Basic Controls page - Some title.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FilePath()
        {
            _consumerBuilder.
                WithFilePath(@$"TestLogs\{{build-start:{DefaultDateTimeFormat}}}\Test {{test-name-sanitized}}\{{screenshot-number:d2}}{{screenshot-title: - *}}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();
            AtataContext.Current.Log.Screenshot("Some title");

            string directoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(DefaultDateTimeFormat),
                $"Test {nameof(FileScreenshotConsumer_FilePath)}");

            _directoriesToDelete.Add(directoryPath);

            FileAssert.Exists(Path.Combine(directoryPath, "01.png"));
            FileAssert.Exists(Path.Combine(directoryPath, "02 - Some title.png"));
        }

        public override void TearDown()
        {
            base.TearDown();

            foreach (string directoryPath in _directoriesToDelete)
                Directory.Delete(directoryPath, true);
        }
    }
}
