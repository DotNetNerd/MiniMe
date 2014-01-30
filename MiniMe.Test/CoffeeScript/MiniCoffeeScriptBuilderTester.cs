using System.Collections.Generic;
using System.IO;
using MiniMe.CoffeeScript;
using MiniMe.Test.Common;
using NUnit.Framework;
using Rhino.Mocks;

namespace MiniMe.Test.CoffeeScript
{
    [TestFixture]
    public class MiniCoffeeScriptBuilderTester : BaseBuilderTester
    {
        [Test]
        public void Render_OutputsTag_ReturnesCorrectTag()
        {
            const string jsPath = "/Content/test.js",
                cssUnversionedRelativePath = "/content/site_#.js",
                tag = "<script src=\"/Content/test.js\"></script>\r\n";
           
            HttpRequest.Stub(r => r.IsLocal).Return(false);
            HttpContext.Stub(c => c.IsDebuggingEnabled).Return(true);
            
            var subject = new MiniCoffeeScriptBuilder(HttpContext, new Mini(), FileSystem);
            var result = subject.Add(jsPath).Render(cssUnversionedRelativePath);
            Assert.That(result, Is.EqualTo(tag));
        }

        [Test]
        public void Render_WritesCombinedFile_FileIsWritten()
        {
            const string jsPath = "/Content/test.js",
                cssUnversionedRelativePath = "/Content/site_#.js",
                jsVersionedRelativePath = "/Content/site_1655544483.js",
                jsVersionedFullPath = "c:\\Content/site_1655544483.js",
                coffeeVersionedFullPath = "c:\\Content/site_1655544483.coffee",
                fileContent = "Hmm";
           
            HttpRequest.Stub(r => r.IsLocal).Return(false);
            HttpContext.Stub(c => c.IsDebuggingEnabled).Return(false);

            Server.Stub(s => s.MapPath(jsVersionedRelativePath)).Return(jsVersionedFullPath);

            FileSystem.Stub(f => f.GetExistingFiles(new List<string>()))
                .Return(new FileInfo[] { new FileInfo(jsPath) }).IgnoreArguments();

            FileSystem.Stub(f => f.ReadAllText("")).Return(fileContent).IgnoreArguments();

            var subject = new MiniCoffeeScriptBuilder(HttpContext, new Mini(), FileSystem);
            subject.ReRenders.Always().Add(jsPath).Render(cssUnversionedRelativePath);
            FileSystem.AssertWasCalled(f => f.WriteAllText(coffeeVersionedFullPath, fileContent + "\r\n"));
        }
    }
}