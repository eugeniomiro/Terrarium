//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using DxVBLib;
using System;
using Terrarium.Renderer.Engine;
using Terrarium.Tools;

namespace Terrarium.Renderer.DirectX 
{
    /// <summary>
    ///  Provides access to the DirectDraw and DirectX interfaces
    /// </summary>
    public class DirectX7GraphicsEngine : IGraphicsEngine
    {
        /// <summary>
        ///  Holds an instance of the DirectX7 native object
        /// </summary>
        private DirectX7 directX;
        /// <summary>
        ///  Holds an instance of the DirectDraw7 native object
        /// </summary>
        private DirectDraw7 directDraw;

        /// <summary>
        ///  Provides access to the native DirectDraw7 object
        /// </summary>
        public DirectDraw7 DirectDraw
        {
            get
            {
                try
                {
                    if (directDraw == null)
                    {
                        directDraw = DirectX.DirectDrawCreate( "" );
                    }
                
                    return directDraw;
                }
                catch (Exception exc)
                {
                    throw new DirectXException("Error obtaining DirectDraw interface", exc);
                }
            }
        }

        private DirectX7 DirectX
        {
            get
            {
                try
                {
                    if (directX == null)
                    {
                        directX = new DirectX7();
                    }
                    return directX;
                }
                catch (Exception exc)
                {
                    throw new DirectXException("Error obtaining DirectX interface", exc);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="windowRect"></param>
        public void GetWindowRect(IntPtr handle, ref RECT windowRect)
        {
            DirectX.GetWindowRect(handle.ToInt32(), ref windowRect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public void SetFullScreenMode(IntPtr handle)
        {
            DirectDraw.SetCooperativeLevel(
               handle.ToInt32(),
               CONST_DDSCLFLAGS.DDSCL_FULLSCREEN |
               CONST_DDSCLFLAGS.DDSCL_EXCLUSIVE |
               CONST_DDSCLFLAGS.DDSCL_ALLOWREBOOT);

            DirectDraw.SetDisplayMode(640, 480, 16, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public void SetWindow(IntPtr handle)
        {
            DirectDraw.SetCooperativeLevel(
                        handle.ToInt32(),
                        CONST_DDSCLFLAGS.DDSCL_NORMAL);
        }
    }
}