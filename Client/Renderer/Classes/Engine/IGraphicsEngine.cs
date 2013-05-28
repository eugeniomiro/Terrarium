//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using DxVBLib;
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
        DirectDraw7 DirectDraw { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="windowRect"></param>
        void GetWindowRect(System.IntPtr handle, ref RECT windowRect);
    }
}