using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class FileScreenshotConsumerTests : UITestFixtureBase
    {
        private AtataContextBuilder<FileScreenshotConsumer> _consumerBuilder;

        private List<string> _foldersToDelete;

        [SetUp]
        public void SetUp()
        {
            _consumerBuilder = ConfigureBaseAtataContext().
                AddScreenshotFileSaving();

            _foldersToDelete = new List<string>();
        }

        [Test]
        public void FileScreenshotConsumer_FolderPath_Relative()
        {
            _consumerBuilder.
                WithFolderPath(@"TestLogs\{build-start}\{test-name-sanitized}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FolderPath_Relative));

            _foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FolderPath_Absolute()
        {
            _consumerBuilder.
                WithFolderPath(@"C:\TestLogs\{build-start}\{test-name-sanitized}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string folderPath = Path.Combine(
                @"C:\TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FolderPath_Absolute));

            _foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FolderPathBuilder()
        {
            _consumerBuilder.
                WithFolderPath(() => $@"TestLogs\{AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat)}\{AtataContext.Current.TestName}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FolderPathBuilder));

            _foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01 - Basic Controls page.png"));
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

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FileName));

            _foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "001 Basic Controls.png"));
            FileAssert.Exists(Path.Combine(folderPath, "002 Some title - Basic Controls.png"));
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

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FileName_Sanitizing));

            _foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01 - Basic Controls page.png"));
            FileAssert.Exists(Path.Combine(folderPath, "02 - Basic Controls page - Some title.png"));
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

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                $"Test {nameof(FileScreenshotConsumer_FilePath)}");

            _foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01.png"));
            FileAssert.Exists(Path.Combine(folderPath, "02 - Some title.png"));
        }

        public override void TearDown()
        {
            base.TearDown();

            foreach (string folderPath in _foldersToDelete)
                Directory.Delete(folderPath, true);
        }
    }
}
