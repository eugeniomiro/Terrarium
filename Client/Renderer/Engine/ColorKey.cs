using System;
using DxVBLib;

namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorKey
    {
        internal ColorKey(DDCOLORKEY key)
        {
            High = key.high;
            Low = key.low;
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 High;
        /// <summary>
        /// 
        /// </summary>
        public Int32 Low;
    }
}
