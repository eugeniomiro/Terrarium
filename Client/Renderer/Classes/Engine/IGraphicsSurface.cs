using DxVBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGraphicsSurface
    {
        /// <summary>
        /// 
        /// </summary>
        DDCOLORKEY TransparencyKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="fillvalue"></param>
        void BltColorFill(ref RECT rect, int fillvalue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        void SetForeColor(int color);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IntPtr GetDC();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        void ReleaseDC(IntPtr handle);

        /// <summary>
        /// 
        /// </summary>
        Boolean InVideo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int IsLost();

        /// <summary>
        /// 
        /// </summary>
        RECT Rect { get; }
    }
}
