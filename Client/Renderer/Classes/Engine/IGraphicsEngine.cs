//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using DxVBLib;
namespace Terrarium.Renderer.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGraphicsEngine
    {
        /// <summary>
        /// 
        /// </summary>
        DirectX7 DirectX { get; }
        /// <summary>
        /// 
        /// </summary>
        DirectDraw7 DirectDraw { get; }
    }
}