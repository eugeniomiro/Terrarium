using System;
using System.Drawing;

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
        ColorKey TransparencyKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="fillvalue"></param>
        void BltColorFill(ref Rectangle rect, int fillvalue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        void SetForeColor(int color);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        void SetFillColor(int color);
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
        Rectangle Rect { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IGraphicsSurface GetBackBufferSurface();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destRect"></param>
        /// <param name="surface"></param>
        /// <param name="srcRect"></param>
        /// <param name="flags"></param>
        void Blt(ref Rectangle destRect, IGraphicsSurface surface, ref Rectangle srcRect, BltFlags flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="surface"></param>
        /// <param name="rectangle"></param>
        /// <param name="flags"></param>
        void BltFast(int x, int y, IGraphicsSurface surface, ref Rectangle rectangle, BltFastFlags flags);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        void DrawLine(int x1, int y1, int x2, int y2);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        void DrawBox(int x1, int y1, int x2, int y2);
    }
}
