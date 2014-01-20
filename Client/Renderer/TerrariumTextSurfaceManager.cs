//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using Terrarium.Renderer.DirectX7;
using Terrarium.Renderer.Engine;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Manages text based surfaces.  Each surface is keyed
    ///  to the original text that creates it.  For performance
    ///  lengthy strings are concatenated.
    /// </summary>
    internal class TerrariumTextSurfaceManager
    {
        /// <summary>
        ///  Represents the rect each piece of text is drawn
        ///  within.  Each text surface is made exactly this
        ///  size.
        /// </summary>
        internal static Rectangle StandardFontRect;

        /// <summary>
        ///  The sprites associated with each bit of text.
        /// </summary>
        private Dictionary<String, IGraphicsSurface> _sprites;

        /// <summary>
        ///  Initialize the standard font rectangle.
        /// </summary>
        static TerrariumTextSurfaceManager()
        {
            StandardFontRect = Rectangle.FromLTRB(0, 0, 100, 15);
        }

        /// <summary>
        ///  Initialize a new text surface manager and any internal
        ///  fields.
        /// </summary>
        internal TerrariumTextSurfaceManager()
        {
            Clear();
        }

        /// <summary>
        ///  Returns the number of text surfaces currently cached.
        /// </summary>
        internal int Count
        {
            get { return _sprites.Count; }
        }

        /// <summary>
        ///  Gets the text surface associated with the given key.  If
        ///  the surface doesn't exist, it is created.
        /// </summary>
        internal DirectDrawSurface this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                if (!_sprites.ContainsKey(key))
                {
                    Add(key);
                }

                return (DirectDrawSurface) _sprites[key];
            }
        }

        /// <summary>
        ///  Adds a new string to the text surface manager.  This creates
        ///  the associated text surface so that text can be rendered with
        ///  a fast Blt rather than with a DrawText call.  Note that caching
        ///  could be done in a much more efficient manner and some text
        ///  surfaces will have identical contents.
        /// </summary>
        /// <param name="key">The string to add.</param>
        internal void Add(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var text = key;
            if (text.Length > 16)
            {
                text = text.Substring(0, 16);
                text += "...";
            }

            // Set up the surface
            IGraphicsSurface ddSurface = GraphicsEngine.Current.CreateSurface(StandardFontRect.Right, StandardFontRect.Bottom);
            ddSurface.TransparencyKey = new ColorKey(DirectDrawSurface.MagentaColorKey);

            var rect = new Rectangle();
            // Color in the back and add the text
            ddSurface.BltColorFill(ref rect, DirectDrawSurface.MagentaColorKey.low);
            ddSurface.SetForeColor(0);

            using (var font = new Font("Verdana", 6.75f, FontStyle.Regular))
            {
                IntPtr dcHandle = ddSurface.GetDC();

                using (var graphics = Graphics.FromHdc(dcHandle))
                {
                    graphics.DrawString(text, font, Brushes.Black, 1, 1);
                    graphics.DrawString(text, font, Brushes.WhiteSmoke, 0, 0);
                }
                ddSurface.ReleaseDC(dcHandle);

            }

            _sprites.Add(key, ddSurface);
        }

        /// <summary>
        ///  Clears out any existing text surfaces and reinitializes the
        ///  hash table for storing keyed surfaces.
        /// </summary>
        internal void Clear() {
            _sprites = new Dictionary<string, IGraphicsSurface>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        ///  Removes the surface associated with the given key.
        /// </summary>
        /// <param name="key">The key of the surface to remove.</param>
        /// <returns>The DirectDrawSurface being removed.</returns>
        internal IGraphicsSurface Remove(string key)
        {
            IGraphicsSurface ddSurface = null;

            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            if (_sprites.ContainsKey(key))
            {
                ddSurface = (IGraphicsSurface) _sprites[key];
                _sprites.Remove(key);
            }

            return ddSurface;
        }
    }
}