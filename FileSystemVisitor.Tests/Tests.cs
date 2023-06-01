using NUnit.Framework;
using FileSystem;
using System;
using System.IO;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private FileSystemVisitor visitor;
        private FileSystemVisitor findFileVisitor;
        string testPath;
        [SetUp]
        public void SetUp()
        {
            string runPath = AppDomain.CurrentDomain.BaseDirectory;
            testPath = Path.GetFullPath(Path.Combine(runPath, @"..\..\..\Res"));
            visitor = new FileSystemVisitor(testPath);
            findFileVisitor = new FileSystemVisitor(testPath);
            findFileVisitor.FileFound += (o, e) => { e.FileName.EndsWith(".txt");};
        }

        [Test]
        public void TestCount()
        {
            int count = 0;
            foreach (var item in visitor)
            {
               count++;
            }
            Assert.AreEqual(5, count);
        }
        [Test]
        public void TestStartEvent()
        {
            bool startCalled = false;
            visitor.Started += (object o, EventArgs e) => {
                startCalled = true;
            };
            foreach (var item in visitor)
            {
            }
            Assert.AreEqual(true, startCalled);
        }
        public void TestFileFindedEvent()
        {
            int fileCount = 0;
            findFileVisitor.FileFound += (o, e) =>
            {
                fileCount++;
            };
            foreach (var e in findFileVisitor)
            { }
            Assert.AreEqual(1, fileCount);


        }
    }
}