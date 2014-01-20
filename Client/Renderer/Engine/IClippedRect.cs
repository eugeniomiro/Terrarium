using System.Drawing;

namespace Terrarium.Renderer.Engine 
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClippedRect 
    {
        /// <summary>
        ///  The destination rectangle
        /// </summary>
        Rectangle Destination { get; set; }

        /// <summary>
        ///  The source rectangle
        /// </summary>
        Rectangle Source { get; set; }

        /// <summary>
        ///  Has the sprite been clipped outside of the view
        /// </summary>
        bool Invisible { get; set; }

        /// <summary>
        ///  Has the sprite been clipped along the top
        /// </summary>
        bool ClipTop { get; set; }

        /// <summary>
        ///  Has the sprite been clipped along the bottom
        /// </summary>
        bool ClipBottom { get; set; }

        /// <summary>
        ///  Has the sprite been clipped along the left
        /// </summary>
        bool ClipLeft { get; set; }

        /// <summary>
        ///  Has the sprite been clipped along the right.
        /// </summary>
        bool ClipRight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="bounds"></param>
        /// <param name="factor"></param>
        void ClipRectBounds(Rectangle dest, Rectangle bounds, int factor = 0);
    }
}