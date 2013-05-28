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
    public class ManagedDirectX7 : IGraphicsEngine
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

        /// <summary>
        ///  Provides access to the native DirectX7 object
        /// </summary>
        public DirectX7 DirectX
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
    }
}