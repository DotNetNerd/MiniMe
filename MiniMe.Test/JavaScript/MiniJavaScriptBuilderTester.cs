using System.Collections.Generic;
using System.IO;
using MiniMe.JavaScript;
using MiniMe.Test.Common;
using NUnit.Framework;
using Rhino.Mocks;

namespace MiniMe.Test.JavaScript
{
    [TestFixture]
    public class MiniJavaScriptBuilderTester : BaseBuilderTester
    {
        [Test]
        public void Render_OutputsTag_ReturnesCorrectTag()
        {
            const string jsPath = "/Content/test.js",
                cssUnversionedRelativePath = "/content/site_#.js",
                tag = "<script src=\"/Content/test.js\"></script>\r\n";
           
            HttpRequest.Stub(r => r.IsLocal).Return(false);
            HttpContext.Stub(c => c.IsDebuggingEnabled).Return(true);
            
            
            var subject = new MiniJavaScriptBuilder(HttpContext, new Mini(), FileSystem, new CacheMock());
            var result = subject.Add(jsPath).Render(cssUnversionedRelativePath);
            Assert.That(result, Is.EqualTo(tag));
        }

        [Test]
        public void Render_WritesCombinedFile_FileIsWritten()
        {
            const string jsPath = "/Content/test.js",
                jsUnversionedRelativePath = "/Content/site_#.js",
				jsVersionedRelativePath = "/Content/site_92223734.js",
				jsVersionedFullPath = "c:\\Content/site_92223734.js",
                fileContent = "Hmm";
           
            HttpRequest.Stub(r => r.IsLocal).Return(false);
            HttpContext.Stub(c => c.IsDebuggingEnabled).Return(false);
            
            Server.Stub(s => s.MapPath(jsVersionedRelativePath)).Return(jsVersionedFullPath);

            FileSystem.Stub(f => f.GetExistingFiles(new List<string>()))
                .Return(new FileInfo[] { new FileInfo(jsPath) }).IgnoreArguments();

            FileSystem.Stub(f => f.ReadAllText("")).Return(fileContent).IgnoreArguments();

            var subject = new MiniJavaScriptBuilder(HttpContext, new Mini(FileSystem), FileSystem, new CacheMock());
            subject.ReRenders.Always().Add(jsPath).Render(jsUnversionedRelativePath);
            FileSystem.AssertWasCalled(f => f.WriteAllText(jsVersionedFullPath, fileContent + ";\r\n"));
        }

        [Test]
        public void Render_SameInputDifferentOutput_RendersBothOutputs()
        {
            const string jsPath = "/Content/test.js",
                outputFile1 = "/Content/#/js1.js",
                outputFile2 = "/Content/#/js2.js",
				jsVersionedRelativePath1 = "/Content/92223734/js1.js",
				jsVersionedFullPath1 = "c:\\Content/92223734/js1.js",
				jsVersionedRelativePath2 = "/Content/92223734/js2.js",
				jsVersionedFullPath2 = "c:\\Content/92223734/js2.js",
                fileContent = "Hmm";

            HttpRequest.Stub(r => r.IsLocal).Return(false);
            HttpContext.Stub(c => c.IsDebuggingEnabled).Return(false);

            Server.Stub(s => s.MapPath(jsVersionedRelativePath1)).Return(jsVersionedFullPath1);
            Server.Stub(s => s.MapPath(jsVersionedRelativePath2)).Return(jsVersionedFullPath2);

            FileSystem.Stub(f => f.GetExistingFiles(new List<string>()))
                .Return(new FileInfo[] { new FileInfo(jsPath) }).IgnoreArguments();

            FileSystem.Stub(f => f.ReadAllText("")).Return(fileContent).IgnoreArguments();

            var subject = new MiniJavaScriptBuilder(HttpContext, new Mini(FileSystem), FileSystem, new CacheMock());
            subject.ReRenders.Never().Add(jsPath).Render(outputFile1);
            subject.ReRenders.Never().Add(jsPath).Render(outputFile2);


            FileSystem.AssertWasCalled(f => f.WriteAllText(jsVersionedFullPath1, fileContent + ";\r\n"));
            FileSystem.AssertWasCalled(f => f.WriteAllText(jsVersionedFullPath2, fileContent + ";\r\n"));
        }
    }
}