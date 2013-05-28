//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using DxVBLib;
using System;
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
        void GetWindowRect(IntPtr handle, ref RECT windowRect);
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
        /// <returns></returns>
        IGraphicsSurface CreatePrimarySurface(IntPtr handle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        IGraphicsSurface CreateWorkSurface(int width, int height);
    }
}