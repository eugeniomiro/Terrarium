//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System.Drawing;
using System;

namespace Terrarium.Renderer.DirectX 
{
    /// <summary>
    ///  Represents a sprite clipping structure that can
    ///  be used to draw sprites between surfaces will
    ///  full edge clipping.
    ///  These are public members instead of property accessors because they sometimes
    ///  need to be passed as ref or out arguments and these aren't supported on accessors.
    /// </summary>
    public struct DirectDrawClippedRect
    {
        /// <summary>
        ///  The destination rectangle
        /// </summary>
        public Rectangle Destination;
        /// <summary>
        ///  The source rectangle
        /// </summary>
        public Rectangle Source;
        /// <summary>
        ///  Has the sprite been clipped outside of the view
        /// </summary>
        public bool Invisible;
        /// <summary>
        ///  Has the sprite been clipped along the top
        /// </summary>
        public bool ClipTop;
        /// <summary>
        ///  Has the sprite been clipped along the bottom
        /// </summary>
        public bool ClipBottom;
        /// <summary>
        ///  Has the sprite been clipped along the left
        /// </summary>
        public bool ClipLeft;
        /// <summary>
        ///  Has the sprite been clipped along the right.
        /// </summary>
        public bool ClipRight;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="bounds"></param>
        /// <param name="factor"></param>
        public void ClipRectBounds(Rectangle dest, Rectangle bounds, int factor = 0)
        {
            int sourceLeft      = Source.Left;
            int destLeft        = Destination.Left;
            int sourceTop       = Source.Top;
            int destTop         = Destination.Top;
            int sourceRight     = Source.Right;
            int destRight       = Destination.Right;
            int sourceBottom    = Source.Bottom;
            int destBottom      = Destination.Bottom;

            if (dest.Left < bounds.Left)
            {
                sourceLeft += bounds.Left - dest.Left << factor;
                destLeft = bounds.Left;
                ClipLeft = true;
            }

            if (dest.Top < bounds.Top)
            {
                sourceTop += (bounds.Top - dest.Top) << factor;
                destTop = bounds.Top;
                ClipTop = true;
            }

            if (dest.Right > bounds.Right)
            {
                sourceRight -= (dest.Right - bounds.Right) << factor;
                destRight = bounds.Right;
                ClipRight = true;
            }

            if (dest.Bottom > bounds.Bottom)
            {
                sourceBottom += (bounds.Bottom - dest.Bottom) << factor;
                destBottom = bounds.Bottom;
                ClipBottom = true;
            }
            Source = Rectangle.FromLTRB(sourceLeft, sourceTop, sourceRight, sourceBottom);
            Destination = Rectangle.FromLTRB(destLeft, destTop, destRight, destBottom);
        }
    }
}