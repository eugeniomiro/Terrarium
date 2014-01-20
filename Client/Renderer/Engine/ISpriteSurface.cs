using System.Drawing;

namespace Terrarium.Renderer.Engine
{
    interface ISpriteSurface
    {
        int FrameHeight { get; }
        int FrameWidth { get; }
        Rectangle GrabSprite(int xFrame, int yFrame);
        IClippedRect GrabSprite(int xFrame, int yFrame, System.Drawing.Rectangle dest, System.Drawing.Rectangle bounds);
        IClippedRect GrabSprite(int xFrame, int yFrame, System.Drawing.Rectangle dest, System.Drawing.Rectangle bounds, int factor);
        string SpriteName { get; }
        IGraphicsSurface SpriteSurface { get; }
    }
}
