//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectX9GraphicsEngine : IGraphicsEngine
    {
        /// <summary>
        /// 
        /// </summary>
        public DxVBLib.DirectDraw7 DirectDraw
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="windowRect"></param>
        public void GetWindowRect(System.IntPtr handle, ref DxVBLib.RECT windowRect)
        {
            throw new System.NotImplementedException();
        }
    }
}