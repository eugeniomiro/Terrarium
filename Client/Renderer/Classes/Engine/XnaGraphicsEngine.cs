//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class XnaGraphicsEngine : IGraphicsEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="windowRect"></param>
        public void GetWindowRect(System.IntPtr handle, ref DxVBLib.RECT windowRect)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public void SetFullScreenMode(System.IntPtr handle)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public void SetWindow(System.IntPtr handle)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public IGraphicsSurface CreateSurface(int width, int height)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IGraphicsSurface CreatePrimarySurface()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public IGraphicsSurface CreatePrimarySurface(System.IntPtr handle)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public IGraphicsSurface CreateWorkSurface(int width, int height)
        {
            throw new System.NotImplementedException();
        }
    }
}