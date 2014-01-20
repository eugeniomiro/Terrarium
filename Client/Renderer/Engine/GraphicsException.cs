using System;

namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphicsException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public GraphicsException(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public GraphicsException(String msg, Exception innerException)
        : base(msg, innerException) { }
    }
}
