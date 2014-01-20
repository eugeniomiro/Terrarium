//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using Terrarium.Configuration;
using Terrarium.Renderer.DirectX7;
using Terrarium.Renderer.Engine;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Manages Terrarium Sprite Surfaces.  Sprite Surfaces
    ///  are linked to one or more actual surfaces.  Each surface
    ///  can be used to represent a creature's size.  Each sprite
    ///  surface is uniquely identified by name, and is capable of
    ///  returning the appropriate internal sprite sheet based on
    ///  the look-up method.
    /// </summary>
    internal class TerrariumSpriteSurface
    {
        /// <summary>
        ///  A collection of surfaces attached to this sprite surface.
        /// </summary>
        private readonly DirectDrawSpriteSurface[] _surfaces;

        /// <summary>
        ///  Determines if this instance supports sized surfaces.
        /// </summary>
        private bool _sizedSurfaces;

        /// <summary>
        ///  Creates a new sprite surface, initializing the size
        ///  array to an initial 49 slots.
        /// </summary>
        internal TerrariumSpriteSurface()
        {
            _surfaces = new DirectDrawSpriteSurface[49];
        }

        /// <summary>
        ///  Attaches a single, unsized surface to this instance.
        ///  If sizedSurface was previously set, it is now unset.
        /// </summary>
        /// <param name="ddss">The surface to attach.</param>
        internal void AttachSurface(ISpriteSurface ddss)
        {
            _sizedSurfaces = false;
            _surfaces[0] = ddss as DirectDrawSpriteSurface;
        }

        /// <summary>
        ///  Attaches a sized surface to this instance.  No bounds
        ///  checking is performed.  The size should be between 0 and 48.
        ///  The sizedSurfaces field is set to true.
        /// </summary>
        /// <param name="ddss">The sprite surface to attach to this instance.</param>
        /// <param name="size">The size of this sprite surface.</param>
        internal void AttachSurface(ISpriteSurface ddss, int size)
        {
            _sizedSurfaces = true;
            _surfaces[size] = ddss as DirectDrawSpriteSurface;
        }

        /// <summary>
        ///  Gets the default surface.  This only works for unsized surfaces.
        /// </summary>
        /// <returns>The default, non-sized surface.</returns>
        internal ISpriteSurface GetDefaultSurface()
        {
            return _surfaces[0];
        }

        /// <summary>
        ///  Attempts to look-up the surface closest to the given size.
        /// </summary>
        /// <param name="size">The ideal size of the surface requrested.</param>
        /// <returns>A surface that closesly resembles the ideal surface in size.</returns>
        internal ISpriteSurface GetClosestSurface(int size)
        {
            ISpriteSurface close = null;

            if (!_sizedSurfaces) {
                return _surfaces[0];
            }

            for (var i = 1; i <= 48; i++) {
                if (_surfaces[i] == null) continue;
                close = _surfaces[i];

                if (GameConfig.UseLargeGraphics) continue;
                if (i >= size)
                {
                    break;
                }
            }

            return close;
        }
    }
}