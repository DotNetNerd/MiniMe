using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace MiniMe.Common
{
    internal abstract class BaseReferenceBuilder : IReRenderBuilder, ICanRenderBuilder, IReRenderWhenBuilder, ISetupForceRenderBuilder, ICanGroupBuilder
    {
        private readonly HttpContextBase _httpContext;
        private readonly ICache _cache;
        private readonly IFileSystem _fileSystem;
        private readonly IMini _mini;

        internal abstract string HttpKey { get; }

        private bool _trackFileChanges = true;
        private bool _disableCaching;
        private bool _forceRender;
        private bool _forceDebug;
        private int _groupIndex;

        protected BaseReferenceBuilder()
            : this(new HttpContextWrapper(HttpContext.Current), new Mini(), new FileSystemWrapper())
        {
        }

        protected BaseReferenceBuilder(IMini mini)
            : this(new HttpContextWrapper(HttpContext.Current), mini, new FileSystemWrapper())
        {
        }

        internal BaseReferenceBuilder(HttpContextBase httpContext, IMini mini, IFileSystem fileSystem)
            : this(httpContext, mini, fileSystem, new CacheWrapper())
        {
        }

        internal BaseReferenceBuilder(HttpContextBase httpContext, IMini mini, IFileSystem fileSystem, ICache cache)
        {
            _httpContext = httpContext;
            _fileSystem = fileSystem;
            _mini = mini;
            _cache = cache;
        }

        protected IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        protected HttpContextBase Context
        {
            get { return _httpContext; }
        }

        protected ICache Cache
        {
            get { return _cache; }
        }

        protected IMini Mini
        {
            get { return _mini; }
        }

        private IList<Item> GetFileList()
        {
            if (Context.Items[HttpKey] == null) Context.Items[HttpKey] = new List<Item>();
            return (List<Item>)Context.Items[HttpKey];
        }

        public ICanRenderBuilder Add(string relativeFilePath)
        {
            return Add(relativeFilePath, _groupIndex);
        }

        public ICanRenderBuilder Add(string relativeFilePath, int groupIndex)
        {
            GetFileList().Add(new Item(relativeFilePath, groupIndex));
            return this;
        }

        public IReRenderBuilder ReRenders
        {
            get { return this; }
        }

        public IReRenderWhenBuilder When
        {
            get { return this; }
        }

        public ICanGroupBuilder With
        {
            get { return this; }
        }

        public ISetupForceRenderBuilder Never()
        {
            _trackFileChanges = false;
            return this;
        }

        public ISetupForceRenderBuilder Always()
        {
            _disableCaching = true;
            return this;
        }

        public ISetupForceRenderBuilder AnySourceFileChanges()
        {
            _trackFileChanges = true;
            return this;
        }

        public IAddSourceFileBuilder ForceRender()
        {
            _forceRender = true;
            _forceDebug = false;
            return this;
        }

        public IAddSourceFileBuilder ForceDebug()
        {
            _forceDebug = true;
            _forceRender = false;
            return this;
        }

        protected bool RunInDebug()
        {
            if (HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["debug"])) return true;
            if (_forceDebug) return true;
            if (_forceRender) return false;

            return Context.Request.IsLocal || Context.IsDebuggingEnabled;
        }

        public string Render(string relativeFilePath)
        {
            var sourceFiles = GetFileList().OrderBy(f => f.Index).Select(f => f.Path).ToList();

            if (sourceFiles.Count == 0) return string.Empty;

            if (RunInDebug())
            {
                var sb = new StringBuilder();
                foreach (var file in sourceFiles)
                    sb.AppendLine(GenerateReferenceMarkup(file));

                GetFileList().Clear();
                return sb.ToString();
            }

            string fileNameCacheHash = relativeFilePath + String.Join(string.Empty, sourceFiles);
            string key = String.Format("MiniMe.{0}_{1}", GetType().Name, fileNameCacheHash.GetHashCode());

            object version = _disableCaching ? null : Cache.Get(key);

            if (version == null)
            {
                lock (RenderLock)
                {
                    version = _disableCaching ? null : Cache.Get(key);

                    if (version == null)
                    {
                        FileInfo[] files = MapToFileInfo(sourceFiles);

                        string minified = Minify(files);

                        version = minified.GetHashCode().ToString().Replace("-", "");

                        relativeFilePath = relativeFilePath.Replace("#", version.ToString());

                        string serverPath = MapRenderedServerPath(relativeFilePath);

                        _fileSystem.CheckCreateDirectory(new FileInfo(serverPath).Directory);
                        _fileSystem.WriteAllText(serverPath, minified);

                        if (!_disableCaching)
                        {
                            Cache.Insert(
                                key,
                                version,
                                _trackFileChanges ? new CacheDependency(files.Select(x => x.FullName).ToArray()) : null,
                                System.Web.Caching.Cache.NoAbsoluteExpiration,
                                System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }
                }
            }

            relativeFilePath = relativeFilePath.Replace("#", version.ToString());

            GetFileList().Clear();

            return GenerateReferenceMarkup(relativeFilePath);
        }

        protected virtual string MapRenderedServerPath(string relativeFilePath)
        {
			return Context.Server.MapPath(relativeFilePath.Split('?')[0]);
        }

        protected virtual FileInfo[] MapToFileInfo(List<string> sourceFiles)
        {
            return _fileSystem.GetExistingFiles(sourceFiles.Select(Context.Server.MapPath));
        }

        protected abstract string GenerateReferenceMarkup(string relativeFilePath);

        //protected abstract string Minify(FileInfo[] files);

        protected virtual string Minify(FileInfo[] files)
        {
            var sb = new StringBuilder();

            foreach (FileInfo file in files)
            {
                sb.Append(FileSystem.ReadAllText(file.FullName));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        protected abstract object RenderLock { get; }

        public ISetupForceRenderBuilder GroupIndex(int groupIndex)
        {
            _groupIndex = groupIndex;
            return this;
        }
    }
}