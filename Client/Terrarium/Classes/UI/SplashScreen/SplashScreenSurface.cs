//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System.Drawing;

namespace Terrarium.Client.SplashScreen
{
    /// <summary>
    ///  Represents a splash screen surface.  The surface is generated
    ///  from a Graphics context object, and a rectangular region.
    /// </summary>
    internal class SplashScreenSurface
    {
        internal Rectangle bounds;
        internal System.Drawing.Graphics graphics;

        /// <summary>
        ///  Creates a new SplashScreenSurface class.
        /// </summary>
        /// <param name="g">The graphics context used for drawing.</param>
        /// <param name="r">The rectangular drawing region.</param>
        internal SplashScreenSurface(System.Drawing.Graphics g, Rectangle r)
        {
            graphics = g;
            bounds = r;
        }
    }
}