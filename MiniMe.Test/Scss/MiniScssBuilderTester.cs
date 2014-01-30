using System.Collections.Generic;
using System.IO;
using MiniMe.Test.Common;
using NUnit.Framework;
using MiniMe.Scss;
using Rhino.Mocks;

namespace MiniMe.Test.Scss
{
    [TestFixture]
    public class MiniScssBuilderTester : BaseBuilderTester
    {
        [Test]
        public void Render_OutputsTag_ReturnesCorrectTag()
        {
            const string cssPath = "/Content/test.css",
                cssUnversionedRelativePath = "/content/site_#.css",
                tag = "<link href=\"/Content/test.css\" rel=\"stylesheet\" />\r\n";
           
            HttpRequest.Stub(r => r.IsLocal).Return(false);
            HttpContext.Stub(c => c.IsDebuggingEnabled).Return(true);
            
            var subject = new MiniScssBuilder(HttpContext, new Mini(), FileSystem);
            var result = subject.Add(cssPath).Render(cssUnversionedRelativePath);
            Assert.That(result, Is.EqualTo(tag));
        }

        [Test]
        public void Render_WritesCombinedFile_FileIsWritten()
        {
            const string cssPath = "/Content/test.css",
                cssUnversionedRelativePath = "/Content/site_#.css",
                cssVersionedRelativePath = "/Content/site_1655544483.css",
                cssVersionedFullPath = "c:\\Content/site_1655544483.css",
                scssVersionedFullPath = "c:\\Content/site_1655544483.scss",
                fileContent = "Hmm";
           
            HttpRequest.Stub(r => r.IsLocal).Return(false);
            HttpContext.Stub(c => c.IsDebuggingEnabled).Return(false);

            Server.Stub(s => s.MapPath(cssVersionedRelativePath)).Return(cssVersionedFullPath);

            FileSystem.Stub(f => f.GetExistingFiles(new List<string>()))
                .Return(new FileInfo[] { new FileInfo(cssPath) }).IgnoreArguments();

            FileSystem.Stub(f => f.ReadAllText("")).Return(fileContent).IgnoreArguments();
            
            var subject = new MiniScssBuilder(HttpContext, new Mini(), FileSystem);
            subject.ReRenders.Always().Add(cssPath).Render(cssUnversionedRelativePath);
            FileSystem.AssertWasCalled(f => f.WriteAllText(scssVersionedFullPath, fileContent + "\r\n"));
        }
    }
}