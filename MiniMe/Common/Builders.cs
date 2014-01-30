namespace MiniMe.Common
{
    public interface ISetupReRenderBuilder
    {
        IReRenderBuilder ReRenders { get; }
    }

    public interface ISetupForceRenderBuilder : IAddSourceFileBuilder
    {
        /// <summary>
        /// Instructs MiniMe to always generate output file even if running on local machine or in Debug Mode.
        /// </summary>
        IAddSourceFileBuilder ForceRender();
        /// <summary>
        /// Instructs MiniMe to always output filereferences even if running externally machine or in Non-debug Mode.
        /// </summary>
        IAddSourceFileBuilder ForceDebug();
    }

    public interface IAddSourceFileBuilder
    {
        /// <summary>
        /// Adds a source file to the Minify collection.
        /// </summary>
        /// <param name="relativeFilePath">Relative path to source file.</param>
        ICanRenderBuilder Add(string relativeFilePath);
        ICanRenderBuilder Add(string relativeFilePath, int groupIndex);
        ICanGroupBuilder With { get; }
    }

    public interface IReRenderBuilder
    {
        IReRenderWhenBuilder When { get; }

        /// <summary>
        /// Instructs MiniMe to never regenerate output file unless application is restarted.
        /// </summary>
        ISetupForceRenderBuilder Never();

        /// <summary>
        /// Disables caching and instructs MiniMe to always regenerate output file for every request. Not recommended in production.
        /// </summary>
        ISetupForceRenderBuilder Always();
    }

    public interface IReRenderWhenBuilder
    {
        /// <summary>
        /// Instructs MiniMe to regenerate output file when any source files are changed. Creates a ASP.NET CacheDependency.
        /// </summary>
        ISetupForceRenderBuilder AnySourceFileChanges();
    }

    public interface ICanGroupBuilder
    {
        ISetupForceRenderBuilder GroupIndex(int groupIndex);
    }

    public interface ICanRenderBuilder : IAddSourceFileBuilder
    {
        /// <summary>
        /// Renders source files to the specified output path.
        /// </summary>
        /// <param name="relativeFilePath">The path where output file is written to. Use # as wildcard for token/version of generated file.</param>
        string Render(string relativeFilePath);
    }


    public interface IAddCdnBuilder
    {
        /// <summary>
        /// Adds a source file to the Minify collection.
        /// </summary>
        ICanRenderCdnBuilder Add(string filePath, string cdnFilePath);
        ICanRenderCdnBuilder Add(string filePath, string cdnFilePath, int groupIndex);
    }

    public interface ICanRenderCdnBuilder : IAddCdnBuilder
    {
        /// <summary>
        /// Renders cdn files references.
        /// </summary>
        string Render();
    }
}