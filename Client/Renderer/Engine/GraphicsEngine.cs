using Terrarium.Renderer.DirectX7;
using Terrarium.Tools;

namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphicsEngine
    {
#if TRACE
        /// <summary>
        ///  Holds an instance of the DirectDrawProfiler timing object.
        /// </summary>
        private static Profiler _profiler;

        /// <summary>
        ///  Provides access to the DirectDrawProfiler timing object.
        /// </summary>
        public static Profiler Profiler
        {
            get { return _profiler ?? (_profiler = new Profiler()); }
        }
#endif
        private static IGraphicsEngine _internalGraphicsEngine = null;

        /// <summary>
        /// Currently configured GraphicsEngine
        /// </summary>
        public static IGraphicsEngine Current {
            get { return _internalGraphicsEngine ?? (_internalGraphicsEngine = new DirectX7GraphicsEngine()); }
        }
    }
}
