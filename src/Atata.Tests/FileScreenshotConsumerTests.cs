using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class FileScreenshotConsumerTests
    {
        private AtataContextBuilder<FileScreenshotConsumer> consumer;
        private List<string> foldersToDelete;

        [SetUp]
        public void SetUp()
        {
            consumer = AtataContext.Configure().
                UseChrome().
                UseBaseUrl(AppConfig.BaseUrl).
                UseNUnitTestName().
                AddNUnitTestContextLogging().
                AddScreenshotFileSaving();

            foldersToDelete = new List<string>();
        }

        [Test]
        public void FileScreenshotConsumer_FolderPath_Relative()
        {
            consumer.
                WithFolderPath(@"TestLogs\{build-start}\{test-name}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FolderPath_Relative));

            foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FolderPath_Absolute()
        {
            consumer.
                WithFolderPath(@"C:\TestLogs\{build-start}\{test-name}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string folderPath = Path.Combine(
                @"C:\TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FolderPath_Absolute));

            foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FolderPathBuilder()
        {
            consumer.
                WithFolderPath(() => $@"TestLogs\{AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat)}\{AtataContext.Current.TestName}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                nameof(FileScreenshotConsumer_FolderPathBuilder));

            foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01 - Basic Controls page.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FileName()
        {
            consumer.
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

            foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "001 Basic Controls.png"));
            FileAssert.Exists(Path.Combine(folderPath, "002 Some title - Basic Controls.png"));
        }

        [Test]
        public void FileScreenshotConsumer_FilePath()
        {
            consumer.
                WithFilePath(@"TestLogs\{build-start}\Test {test-name}\{screenshot-number:d2}{screenshot-title: - *}").
                Build();

            Go.To<BasicControlsPage>();

            AtataContext.Current.Log.Screenshot();
            AtataContext.Current.Log.Screenshot("Some title");

            string folderPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestLogs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                $"Test {nameof(FileScreenshotConsumer_FilePath)}");

            foldersToDelete.Add(folderPath);

            FileAssert.Exists(Path.Combine(folderPath, "01.png"));
            FileAssert.Exists(Path.Combine(folderPath, "02 - Some title.png"));
        }

        [TearDown]
        public void TearDown()
        {
            AtataContext.Current?.CleanUp();

            foreach (string folderPath in foldersToDelete)
                Directory.Delete(folderPath, true);
        }
    }
}
