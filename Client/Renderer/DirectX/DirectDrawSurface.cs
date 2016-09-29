//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using DxVBLib;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Terrarium.Renderer.Engine;

namespace Terrarium.Renderer.DirectX
{
    /// <summary>
    ///  Managed Wrapper for a DirectX7 DirectDraw Surface.
    /// </summary>
    public class DirectDrawSurface : IGraphicsSurface
    {
        /// <summary>
        ///  Defines a default surface description
        /// </summary>
        internal static DDSURFACEDESC2 DefaultSurfaceDescription;

        /// <summary>
        ///  Defines a surface description used for image surfaces
        /// </summary>
        internal static DDSURFACEDESC2 ImageSurfaceDescription;

        /// <summary>
        ///  Defines a surface description for a system memory surface.
        /// </summary>
        internal static DDSURFACEDESC2 SystemMemorySurfaceDescription;

        /// <summary>
        ///  Pointer to the surface description used to create this surface.
        /// </summary>
        private DDSURFACEDESC2 descriptor;

        /// <summary>
        ///  File based image used to initialize this surface.
        /// </summary>
        private String image;

        /// <summary>
        ///  The size of the surface.
        /// </summary>
        private RECT rect;

        /// <summary>
        ///  Pointer to the real DirectDrawSurface7 class
        /// </summary>
        private DirectDrawSurface7 surface;

        /// <summary>
        ///  Determines if transparency is enabled for this surface.
        /// </summary>
        private bool transparencyEnabled;

        /// <summary>
        ///  The transparency key for this surface
        /// </summary>
        private DDCOLORKEY transparencyKey;

        /// <summary>
        ///  Static constructor used to intialize static surface description fields.
        /// </summary>
        static DirectDrawSurface()
        {
            // Setup default Surface Description
            DefaultSurfaceDescription = new DDSURFACEDESC2();
            DefaultSurfaceDescription.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
            DefaultSurfaceDescription.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;

            // Setup default Surface Description
            ImageSurfaceDescription = new DDSURFACEDESC2();
            ImageSurfaceDescription.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
            ImageSurfaceDescription.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;

            SystemMemorySurfaceDescription = new DDSURFACEDESC2();
            SystemMemorySurfaceDescription.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
            SystemMemorySurfaceDescription.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN |
                                                           CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
        }

        /// <summary>
        ///  Creates a new DirectDrawSurface given a width and height.
        /// </summary>
        /// <param name="x">The width of the surface.</param>
        /// <param name="y">The height of the surface.</param>
        public DirectDrawSurface(int x, int y)
        {
            descriptor = new DDSURFACEDESC2();
            descriptor.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT |
                                CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
            descriptor.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
            descriptor.lWidth = x;
            descriptor.lHeight = y;
            image = "";

            CreateSurface();
        }

        /// <summary>
        ///  Create a new surface given a surface description.
        /// </summary>
        /// <param name="surfaceDescription">Surface Description</param>
        internal DirectDrawSurface(DDSURFACEDESC2 surfaceDescription)
            : this("", surfaceDescription)
        {
        }

        /// <summary>
        ///  Create a new surface given an image path
        /// </summary>
        /// <param name="imagePath">Path to an image file.</param>
        public DirectDrawSurface(String imagePath) : this(imagePath, DefaultSurfaceDescription)
        {
        }

        /// <summary>
        ///  Create a new surface from an image and a surface description.
        /// </summary>
        /// <param name="imagePath">Path to an image file.</param>
        /// <param name="surfaceDescription">Surface Description.</param>
        internal DirectDrawSurface(String imagePath, DDSURFACEDESC2 surfaceDescription)
        {
            descriptor = surfaceDescription;
            image = imagePath;
            CreateSurface();
        }

        /// <summary>
        ///  Initialize a new surface based on a previously created surface
        /// </summary>
        /// <param name="directDrawSurface">The native DirectDraw surface used as reference.</param>
        internal DirectDrawSurface(DirectDrawSurface7 directDrawSurface)
        {
            directDrawSurface.GetSurfaceDesc(ref descriptor);
            surface = directDrawSurface;
        }

        /// <summary>
        ///  The default transparency color key.  Points to the MagentaColorKey
        /// </summary>
        internal static DDCOLORKEY DefaultColorKey
        {
            get { return MagentaColorKey; }
        }

        /// <summary>
        ///  Creates a transparency key for the color Magenta.
        /// </summary>
        internal static DDCOLORKEY MagentaColorKey
        {
            get
            {
                var ddck = new DDCOLORKEY();
                var ddsd2 = new DDSURFACEDESC2();
                ((DirectX7GraphicsEngine)(GraphicsEngine.Current)).DirectDraw.GetDisplayMode(ref ddsd2);

                if ((ddsd2.ddpfPixelFormat.lFlags & CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8) ==
                    CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8)
                {
                    ddck.low = 253;
                    ddck.high = 253;
                }
                else
                {
                    ddck.low = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lBBitMask;
                    ddck.high = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lBBitMask;
                }

                return ddck;
            }
        }

        /// <summary>
        ///  Creates a transparency key for the color White
        /// </summary>
        internal static DDCOLORKEY WhiteColorKey
        {
            get
            {
                var ddck = new DDCOLORKEY();
                var ddsd2 = new DDSURFACEDESC2();
                ((DirectX7GraphicsEngine)(GraphicsEngine.Current)).DirectDraw.GetDisplayMode(ref ddsd2);

                if ((ddsd2.ddpfPixelFormat.lFlags & CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8) ==
                    CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8)
                {
                    ddck.low = 255;
                    ddck.high = 255;
                }
                else
                {
                    ddck.low = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lGBitMask +
                               ddsd2.ddpfPixelFormat.lBBitMask;
                    ddck.high = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lGBitMask +
                                ddsd2.ddpfPixelFormat.lBBitMask;
                }

                return ddck;
            }
        }

        /// <summary>
        ///  Create a transparency key for the color Lime
        /// </summary>
        internal static DDCOLORKEY LimeColorKey
        {
            get
            {
                var ddck = new DDCOLORKEY();
                var ddsd2 = new DDSURFACEDESC2();
                ((DirectX7GraphicsEngine)(GraphicsEngine.Current)).DirectDraw.GetDisplayMode(ref ddsd2);

                if ((ddsd2.ddpfPixelFormat.lFlags & CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8) ==
                    CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8)
                {
                    ddck.low = 250;
                    ddck.high = 250;
                }
                else
                {
                    ddck.low = ddsd2.ddpfPixelFormat.lGBitMask;
                    ddck.high = ddsd2.ddpfPixelFormat.lGBitMask;
                }

                return ddck;
            }
        }

        /// <summary>
        ///  Determines if the surface is in video memory
        ///  or system memory.
        /// </summary>
        public bool InVideo
        {
            get
            {
                if (surface != null)
                {
                    var ddsc = new DDSCAPS2();
                    surface.GetCaps(ref ddsc);
                    if ((ddsc.lCaps & CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        ///  Path to the image used to initialize this surface if one exists.
        /// </summary>
        public String ImagePath
        {
            get { return image; }
            set
            {
                image = value;
                CreateSurface();
            }
        }

        /// <summary>
        ///  Determines if this is a transparent surface.
        /// </summary>
        public bool TransparentSurface
        {
            get { return transparencyEnabled; }
        }

        /// <summary>
        ///  Sets the transparency key for this surface.
        /// </summary>
        public ColorKey TransparencyKey
        {
            get { return new ColorKey(transparencyKey); }
            set
            {
                transparencyKey.high = value.High;
                transparencyKey.low = value.Low;
                transparencyEnabled = true;
                if (surface != null)
                {
                    surface.SetColorKey(CONST_DDCKEYFLAGS.DDCKEY_SRCBLT, ref transparencyKey);
                }
            }
        }

        /// <summary>
        ///  Modifies the Surface Description
        /// </summary>
        internal DDSURFACEDESC2 Descriptor
        {
            get { return descriptor; }
            set { descriptor = value; }
        }

        /// <summary>
        ///  Retrieves the size of this surface.
        /// </summary>
        public Rectangle Rect
        {
            get { return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom); }
        }

        /// <summary>
        ///  Provides access to the native surface object
        /// </summary>
        public DirectDrawSurface7 Surface
        {
            get { return surface; }
        }

        /// <summary>
        ///  Attempts to generate a transparency key from an r,g,b byte color.
        /// </summary>
        /// <param name="r">Red component</param>
        /// <param name="g">Green component</param>
        /// <param name="b">Blue component</param>
        /// <returns></returns>
        internal static DDCOLORKEY GenerateColorKey(byte r, byte g, byte b)
        {
            // This may not be perfect since we are going to have to average
            // different bit depths together
            var ddck = new DDCOLORKEY();
            var ddsd2 = new DDSURFACEDESC2();
            ((DirectX7GraphicsEngine)(GraphicsEngine.Current)).DirectDraw.GetDisplayMode(ref ddsd2);

            var bBitCount = CountBits(ddsd2.ddpfPixelFormat.lBBitMask);
            var gBitCount = CountBits(ddsd2.ddpfPixelFormat.lGBitMask);
            var rBitCount = CountBits(ddsd2.ddpfPixelFormat.lRBitMask);

            var bBitMask = ddsd2.ddpfPixelFormat.lBBitMask;
            var gBitMask = ddsd2.ddpfPixelFormat.lGBitMask >> bBitCount;
            var rBitMask = ddsd2.ddpfPixelFormat.lRBitMask >> (gBitCount + bBitCount);

            var bValue = (b/255)*bBitMask;
            var gValue = (g/255)*gBitMask;
            var rValue = (r/255)*rBitMask;

            ddck.low = (rValue << (gBitCount + bBitCount)) + (gValue << bBitCount) + bValue;
            ddck.high = ddck.low;

            return ddck;
        }

        /// <summary>
        ///  Helper function for counting bits used when creating transparency keys.
        /// </summary>
        /// <param name="number">The number of bits</param>
        /// <returns>The number specified by the number of bits.</returns>
        private static int CountBits(int number)
        {
            var bits = 0;

            while (number != 0)
            {
                if ((number & 1) == 1)
                {
                    bits++;
                }
                number >>= 1;
            }

            return bits;
        }

        /// <summary>
        ///  Recreate the surface in the instance that the image
        ///  memory is lost do to a video mode switch.
        /// </summary>
        public void RestoreSurface()
        {
            if (surface == null || surface.isLost() != 0)
            {
                CreateSurface();
            }
        }

        /// <summary>
        ///  Helper function used to complete initialization of a surface.
        /// </summary>
        private void CreateSurface()
        {
#if TRACE
            GraphicsEngine.Profiler.Start("CreateSurface");
#endif
            try
            {
                if (string.IsNullOrEmpty(image))
                {
                    surface = ((DirectX7GraphicsEngine)(GraphicsEngine.Current)).DirectDraw.CreateSurface(ref descriptor);
                    if (surface != null)
                    {
                        rect.Bottom = descriptor.lHeight;
                        rect.Right = descriptor.lWidth;
                    }
                }
                else
                {
                    try
                    {
                        Trace.WriteLine(image);
                        try
                        {
                            surface = ((DirectX7GraphicsEngine)(GraphicsEngine.Current)).DirectDraw.CreateSurfaceFromFile(image, ref descriptor);
                        }
                        catch (ArgumentException)
                        {
                            descriptor = SystemMemorySurfaceDescription;
                            surface = ((DirectX7GraphicsEngine)(GraphicsEngine.Current)).DirectDraw.CreateSurfaceFromFile(image, ref descriptor);
                        }
                    }
                    catch (COMException e)
                    {
                        // File Not Found
                        switch ((uint) e.ErrorCode)
                        {
                            case 0x800A0035:
                                Trace.WriteLine(
                                    "Could not find the file '" + image +
                                    "'.  This must be placed in the current directory.", "Picture Not Found");
                                break;
                            case 0x8876024E:
                                Trace.WriteLine(
                                    "The graphics card is in an unsupported mode.  We will try to initalize again later.");
                                throw new DirectXException(
                                    "Error Creating a DirectDraw Surface because of unsupported graphics mode.", e);
                            default:
                                Trace.WriteLine("Unexpected exception: " + e, "Unexpected Exception");
                                break;
                        }
                    }
                    rect.Bottom = descriptor.lHeight;
                    rect.Right = descriptor.lWidth;
                }
            }
            catch (Exception exc)
            {
                throw new DirectXException("Error Creating a DirectDraw Surface", exc);
            }
#if TRACE
            GraphicsEngine.Profiler.End("DirectDrawSurface.CreateSurface");
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="fillvalue"></param>
        public void BltColorFill(ref Rectangle rect, int fillvalue)
        {
            RECT innerRect = new RECT
            {
                Left = rect.Left,
                Top = rect.Top,
                Bottom = rect.Bottom,
                Right = rect.Right
            };
            Surface.BltColorFill(innerRect, fillvalue);
            rect = Rectangle.FromLTRB(innerRect.Left, innerRect.Top, innerRect.Right, innerRect.Bottom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public void SetForeColor(int color)
        {
            Surface.SetFillColor(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IntPtr GetDC()
        {
            return new IntPtr(Surface.GetDC());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public void ReleaseDC(IntPtr handle)
        {
            Surface.ReleaseDC(handle.ToInt32());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int IsLost()
        {
            return Surface.isLost();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IGraphicsSurface GetBackBufferSurface()
        {
            DDSCAPS2 ddsCaps = new DDSCAPS2();
            ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_BACKBUFFER;
            return new DirectDrawSurface(Surface.GetAttachedSurface(ref ddsCaps));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destRect"></param>
        /// <param name="surface"></param>
        /// <param name="srcRect"></param>
        /// <param name="flags"></param>
        public void Blt(ref Rectangle destRect, IGraphicsSurface surface, ref Rectangle srcRect, BltFlags flags)
        {
            RECT innerDestRect = new RECT
            {
                Left = destRect.Left,
                Top = destRect.Top,
                Right = destRect.Right,
                Bottom = destRect.Bottom
            };
            RECT innerSrcRect = new RECT
            {
                Left = srcRect.Left,
                Top = srcRect.Top,
                Right = srcRect.Right,
                Bottom = srcRect.Bottom
            };
            Surface.Blt(ref innerDestRect, ((DirectDrawSurface) surface).Surface, ref innerSrcRect, (CONST_DDBLTFLAGS)((int)flags));
            destRect = Rectangle.FromLTRB(innerDestRect.Left, innerDestRect.Top, innerDestRect.Right, innerDestRect.Bottom);
            srcRect = Rectangle.FromLTRB(innerSrcRect.Left, innerSrcRect.Top, innerSrcRect.Right, innerSrcRect.Bottom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="surface"></param>
        /// <param name="rectangle"></param>
        /// <param name="flags"></param>
        public void BltFast(int x, int y, IGraphicsSurface surface, ref Rectangle rectangle, BltFastFlags flags)
        {
            RECT innerRect = new RECT
            {
                Left = rectangle.Left,
                Top = rectangle.Top,
                Right = rectangle.Right,
                Bottom = rectangle.Bottom
            };
            Surface.BltFast(x, y, ((DirectDrawSurface)surface).Surface, ref innerRect, (CONST_DDBLTFASTFLAGS)((int)flags));
            rectangle = Rectangle.FromLTRB(innerRect.Left, innerRect.Top, innerRect.Right, innerRect.Bottom);
        }
    }
}