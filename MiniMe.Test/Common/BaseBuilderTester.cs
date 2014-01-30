using System.Collections;
using System.Web;
using MiniMe.Common;
using NUnit.Framework;
using Rhino.Mocks;

namespace MiniMe.Test.Common
{
    public abstract class BaseBuilderTester
    {
        protected HttpContextBase HttpContext;
        protected HttpRequestBase HttpRequest;
        protected HttpServerUtilityBase Server;
        protected IFileSystem FileSystem;
        protected IDictionary Dictionary;
        protected IMini Mini;
        protected ICache Cache;

        [SetUp]
        public void Setup()
        {
            HttpContext = MockRepository.GenerateStub<HttpContextBase>();
            HttpRequest = MockRepository.GenerateStub<HttpRequestBase>();
            Server = MockRepository.GenerateMock<HttpServerUtilityBase>();
            FileSystem = MockRepository.GenerateMock<IFileSystem>();
            Dictionary = MockRepository.GenerateStub<IDictionary>();
            Cache = MockRepository.GenerateStub<ICache>();
            Mini = MockRepository.GenerateStub<IMini>();
            
            HttpContext.Stub(c => c.Request).Return(HttpRequest);
            HttpContext.Stub(c => c.Items).Return(Dictionary);

            HttpContext.Stub(c => c.Server).Return(Server);
        }
    }
}
