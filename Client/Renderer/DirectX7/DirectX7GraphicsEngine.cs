//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using Terrarium.Renderer.Engine;

namespace Terrarium.Renderer.DirectX7
{
    /// <summary>
    ///  Provides access to the DirectDraw and DirectX interfaces
    /// </summary>
    public class DirectX7GraphicsEngine : IGraphicsEngine
    {
        /// <summary>
        ///  Holds an instance of the DirectX7 native object
        /// </summary>
        private DxVBLib.DirectX7    _directX;
        /// <summary>
        ///  Holds an instance of the DirectDraw7 native object
        /// </summary>
        private DxVBLib.DirectDraw7 _directDraw;

        /// <summary>
        ///  Provides access to the native DirectDraw7 object
        /// </summary>
        internal DxVBLib.DirectDraw7  DirectDraw
        {
            get
            {
                try
                {
                    return _directDraw ?? (_directDraw = DirectX.DirectDrawCreate(""));
                }
                catch (Exception exc)
                {
                    throw new GraphicsException("Error obtaining DirectDraw interface", exc);
                }
            }
        }

        private DxVBLib.DirectX7 DirectX
        {
            get
            {
                try
                {
                    return _directX ?? (_directX = new DxVBLib.DirectX7());
                }
                catch (Exception exc)
                {
                    throw new GraphicsException("Error obtaining DirectX interface", exc);
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public Rectangle GetWindowRect(IntPtr handle) 
        {
            var innerWindowRect = new DxVBLib.RECT();
            DirectX.GetWindowRect(handle.ToInt32(), ref innerWindowRect);
            var windowRect = Rectangle.FromLTRB(innerWindowRect.Left, innerWindowRect.Top, innerWindowRect.Right, innerWindowRect.Bottom);
            return windowRect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public void SetFullScreenMode(IntPtr handle)
        {
            try {
                DirectDraw.SetCooperativeLevel(handle.ToInt32(),
                                               DxVBLib.CONST_DDSCLFLAGS.DDSCL_FULLSCREEN |
                                               DxVBLib.CONST_DDSCLFLAGS.DDSCL_EXCLUSIVE |
                                               DxVBLib.CONST_DDSCLFLAGS.DDSCL_ALLOWREBOOT);

                DirectDraw.SetDisplayMode(640, 480, 16, 0, 0);

            } catch (GraphicsException e) {
                throw new GraphicsException("setting up Full screen", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public void SetWindow(IntPtr handle)
        {
            DirectDraw.SetCooperativeLevel(
                        handle.ToInt32(),
                        DxVBLib.CONST_DDSCLFLAGS.DDSCL_NORMAL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public IGraphicsSurface CreateSurface(int width, int height)
        {
            return new DirectDrawSurface(width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fullScreen"></param>
        /// <param name="doubleBuffer"></param>
        /// <returns></returns>
        public IGraphicsSurface CreatePrimarySurface(IntPtr handle, Boolean fullScreen, Boolean doubleBuffer)
        {
            var tempDescr   = new DxVBLib.DDSURFACEDESC2 {
                lFlags = DxVBLib.CONST_DDSURFACEDESCFLAGS.DDSD_CAPS,
                ddsCaps = { lCaps = DxVBLib.CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE }
            };

            if (doubleBuffer) {
                tempDescr.lFlags |= DxVBLib.CONST_DDSURFACEDESCFLAGS.DDSD_BACKBUFFERCOUNT;
                tempDescr.lBackBufferCount = 1;
                tempDescr.ddsCaps.lCaps |= DxVBLib.CONST_DDSURFACECAPSFLAGS.DDSCAPS_COMPLEX |
                                           DxVBLib.CONST_DDSURFACECAPSFLAGS.DDSCAPS_FLIP;
            }

            var surface = new DirectDrawSurface(tempDescr);

            if (fullScreen) {
                return surface;
            }

            var clipper = DirectDraw.CreateClipper(0);
            clipper.SetHWnd(handle.ToInt32());
            surface.SetClipper(clipper);

            return surface;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public IGraphicsSurface CreateWorkSurface(int width, int height)
        {
            var tempDescr = new DxVBLib.DDSURFACEDESC2 {
                lFlags = DxVBLib.CONST_DDSURFACEDESCFLAGS.DDSD_CAPS |
                         DxVBLib.CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT |
                         DxVBLib.CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH,
                ddsCaps = { lCaps = DxVBLib.CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN },
                lWidth = width,
                lHeight = height
            };

            return new DirectDrawSurface(tempDescr);
        }
    }
}