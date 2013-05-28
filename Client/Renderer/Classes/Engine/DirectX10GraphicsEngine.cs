//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectX10GraphicsEngine : IGraphicsEngine
    {
        /// <summary>
        /// 
        /// </summary>
        public DxVBLib.DirectX7 DirectX
        {
            get { throw new System.NotImplementedException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public DxVBLib.DirectDraw7 DirectDraw
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}