using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class FileScreenshotConsumerTests : UITestFixtureBase
    {
        private AtataContextBuilder<FileScreenshotConsumer> consumerBuilder;

        private List<string> foldersToDelete;

        [SetUp]
        public void SetUp()
        {
            consumerBuilder = ConfigureBaseAtataContext().
                AddScreenshotFileSaving();

            foldersToDelete = new List<string>();
        }

        [Test]
        public void FileScreenshotConsumer_FolderPath_Relative()
        {
            consumerBuilder.
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
            consumerBuilder.
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
            consumerBuilder.
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
            consumerBuilder.
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
            consumerBuilder.
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

        public override void TearDown()
        {
            base.TearDown();

            foreach (string folderPath in foldersToDelete)
                Directory.Delete(folderPath, true);
        }
    }
}
