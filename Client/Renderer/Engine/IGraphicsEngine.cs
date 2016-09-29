//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Drawing;
namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGraphicsEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="windowRect"></param>
        void GetWindowRect(IntPtr handle, ref Rectangle windowRect);
        /// <summary>
        /// 
        /// </summary>
        void SetFullScreenMode(IntPtr handle);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        void SetWindow(IntPtr handle);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        IGraphicsSurface CreateSurface(int width, int height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fullScreen"></param>
        /// <param name="doubleBuffer"></param>
        /// <returns></returns>
        IGraphicsSurface CreatePrimarySurface(IntPtr handle, Boolean fullScreen, Boolean doubleBuffer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        IGraphicsSurface CreateWorkSurface(int width, int height);
    }
}