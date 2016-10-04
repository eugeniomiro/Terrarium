//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using OrganismBase;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Terrarium.Game;
using Terrarium.Renderer.DirectX;
using Terrarium.Renderer.Engine;
using Terrarium.Tools;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Encapsulates all of the drawing code for the Terrarium Game
    ///  View.  This class makes heavy use of the DirectDraw APIs in
    ///  order to provide high speed animation.
    /// </summary>
    public class TerrariumGameView : PictureBox
    {
        private     readonly TerrariumTextSurfaceManager    tfm = new TerrariumTextSurfaceManager();
        private     readonly TerrariumSpriteSurfaceManager  tsm = new TerrariumSpriteSurfaceManager();
        private     Rectangle                               _actualsize;

        private     IGraphicsSurface                        _backgroundSurface;
        private     IGraphicsSurface                        _stagingSurface;
        /// <summary>
        ///  The primary screen surface for the picture box.
        /// </summary>
        protected   IGraphicsSurface                        _screenSurface;

        /// <summary>
        ///  The back buffer surface used with the picture box.
        /// </summary>
        protected   IGraphicsSurface                        _backBufferSurface;

        private     Rectangle                               _bezelRect;
        private     bool                                    bltUsingBezel;
        private     Rectangle                               _clipRect;

        // Cursor
        private int cursor;
        private Point cursorPos;
        private bool doubleBuffer = true;
        private bool drawBackgroundGrid;
        private bool drawBoundingBox;
        private bool drawCursor = true;
        private bool drawDestinationLines;
        private bool drawing;
        private bool drawScreen = true;
        private bool drawtext = true;
        private bool enabledCursor;
        private bool fullscreen;
        private Hashtable hackTable = new Hashtable();
        private Bitmap miniMap;
        private bool paintPlants;
        private bool paused;
        private Int64 renderTime;
        private bool resizing;
        private Int32 samples;

        // Scrolling
        private int scrollDown;
        private int scrollLeft;
        private int scrollRight;
        private int scrollUp;
        private bool skipframe = true;
        private Profiler tddGameViewProf;
        private string textMessage;
        private bool updateMiniMap;

        private int updateTicker;
        private bool updateTickerChanged;
        private bool videomemory = true;
        private bool viewchanged = true;
        private Rectangle           _viewsize;
        private World               wld;
        private WorldVector         wv;

        /// <summary>
        ///  Overrides the Painting logic because painting will be handled
        ///  using timers and DirectX.  If the control is in design mode, then
        ///  clear it because DirectX isn't available yet.
        /// </summary>
        /// <param name="e">Graphics context objects</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.Clear(BackColor);
            }
        }

        /// <summary>
        ///  Don't paint the background when a background erase is requested.
        ///  Hurts performance and causes flicker.
        /// </summary>
        /// <param name="e">Graphics context objects</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        ///  Creates a new instance of the game view and initializes any properties.
        /// </summary>
        public TerrariumGameView()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Provides access to the game profiler.
        /// </summary>
        public Profiler Profiler
        {
            get
            {
                if (tddGameViewProf == null)
                {
                    tddGameViewProf = new Profiler();
                }

                return tddGameViewProf;
            }
        }

        /// <summary>
        ///  Returns the amount of time required to render a scene.
        /// </summary>
        public Int64 RenderTime
        {
            get { return renderTime; }
        }

        /// <summary>
        ///  Returns the number of samples (frames) obtained.
        /// </summary>
        public Int64 Samples
        {
            get { return samples; }
        }

        /// <summary>
        ///  Pauses the TerrariumGameView and stops rendering.  A call to 
        ///  RenderFrame will automatically unpause the animation.
        /// </summary>
        public bool Paused
        {
            set { paused = value; }
        }

        /// <summary>
        ///  Provides access to the bitmap representing the minimap for the
        ///  currently loaded world.
        /// </summary>
        public Bitmap MiniMap
        {
            get
            {
                if (miniMap == null)
                {
                    if (wld != null)
                    {
                        //this.miniMap = new Bitmap(wld.MiniMap);
                        miniMap = new Bitmap(wld.MiniMap, (_actualsize.Right - _actualsize.Left)/4,
                                             (_actualsize.Bottom - _actualsize.Top)/4);
                        var graphics = Graphics.FromImage(miniMap);
                        graphics.Clear(Color.Transparent);
                        graphics.Dispose();
                    }
                    else
                    {
                        return null;
                    }
                }

                return miniMap;
            }
        }

        /// <summary>
        ///  Enables the textual display of a message over top of
        ///  the Terrarium view.  This property accepts newlines and
        ///  centers the text on the screen.
        /// </summary>
        public string TerrariumMessage
        {
            get { return textMessage; }
            set { textMessage = value; }
        }

        /// <summary>
        ///  Enables the drawing of organism destination lines
        ///  within the Terrarium Client
        /// </summary>
        public bool DrawDestinationLines
        {
            get { return drawDestinationLines; }
            set { drawDestinationLines = value; }
        }

        /// <summary>
        ///  Controls the rendering of the entire game view.
        /// </summary>
        public bool DrawScreen
        {
            get { return drawScreen; }
            set { drawScreen = value; }
        }

        /// <summary>
        ///  Controls the rendering of organism bounding boxes useful
        ///  for movement debugging.
        /// </summary>
        public bool DrawBoundingBox
        {
            get { return drawBoundingBox; }
            set { drawBoundingBox = value; }
        }

        /// <summary>
        ///  Controls rendering of a special background that contains
        ///  a grid overlay that mimics the Terrarium application's
        ///  cell grid.
        /// </summary>
        public bool DrawBackgroundGrid
        {
            get { return drawBackgroundGrid; }
            set
            {
                viewchanged = true;
                drawBackgroundGrid = value;
                AddBackgroundSlide();
            }
        }

        /// <summary>
        ///  Overrides the cursor property so that
        ///  custom cursors can be implement.
        /// </summary>
        public override Cursor Cursor
        {
            get
            {
                if (paused || !drawScreen)
                {
                    return Cursors.Default;
                }

                if (bltUsingBezel)
                {
                    if (!ComputeCursorRectangle().Contains(cursorPos))
                    {
                        return Cursors.Default;
                    }
                }

                return null;
            }
        }

        /// <summary>
        ///  Determines if sprite labels should be drawn.
        /// </summary>
        public bool DrawText
        {
            get { return drawtext; }
            set { drawtext = value; }
        }

        /// <summary>
        ///  Determines if the primary surfaces are in video memory
        ///  or not.
        /// </summary>
        public bool VideoMemory
        {
            get { return videomemory; }
        }

        /// <summary>
        ///  Returns the size of the viewport window.
        /// </summary>
        public Rectangle ViewSize
        {
            get { return _viewsize; }
        }

        /// <summary>
        ///  Returns the full size of the world.
        /// </summary>
        public Rectangle ActualSize
        {
            get { return _actualsize; }
        }

        /// <summary>
        /// Determines whether to 
        /// </summary>
        public bool DrawCursor
        {
            get { return drawCursor; }
            set { drawCursor = value; }
        }

        /// <summary>
        ///  Clients can connect to this event and be notified of
        ///  click events that correspond to creatures within the view.
        /// </summary>
        public event OrganismClickedEventHandler OrganismClicked;

        /// <summary>
        ///  Clients can connect to this event and be notified of
        ///  mini map changes that occur during map transitions.
        /// </summary>
        public event MiniMapUpdatedEventHandler MiniMapUpdated;

        /// <summary>
        ///  Initialize Component
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        ///  Clear the profiler
        /// </summary>
        public void ClearProfiler()
        {
            renderTime = 0;
            samples = 0;
        }

        /// <summary>
        ///  Overrides OnMouseMove to enable custom cursor rendering.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            cursorPos = new Point(e.X, e.Y);
        }

        private Rectangle ComputeCursorRectangle()
        {
            var left = 0;
            var top = 0;
            var right = Width;
            var bottom = Height;

            if (bltUsingBezel)
            {
                if (_clipRect.Right == _actualsize.Right)
                {
                    left = (right - _clipRect.Right)/2;
                    right = left + _clipRect.Right;
                }
                if (_clipRect.Bottom == _actualsize.Bottom)
                {
                    top = (bottom - _clipRect.Bottom)/2;
                    bottom = top + _clipRect.Bottom;
                }
            }

            return Rectangle.FromLTRB(left, top, right, bottom);
        }


        /// <summary>
        ///  Computes whether scrolling should occur based on mouse location and
        ///  hovering.
        /// </summary>
        private void CheckScroll()
        {
            if (!enabledCursor)
            {
                scrollUp = 0;
                scrollDown = 0;
                scrollLeft = 0;
                scrollRight = 0;
                return;
            }

            var cursorRect = ComputeCursorRectangle();
            var left = cursorRect.Left;
            var top = cursorRect.Top;
            var right = cursorRect.Right;
            var bottom = cursorRect.Bottom;

            if (cursorPos.Y <= bottom && cursorPos.Y > (bottom - 30))
            {
                scrollDown++;
                if (scrollDown > 4)
                {
                    ScrollDown(15);
                }
            }
            else
            {
                scrollDown = 0;
            }

            if (cursorPos.Y >= top && cursorPos.Y < (top + 30))
            {
                scrollUp++;
                if (scrollUp > 4)
                {
                    ScrollUp(15);
                }
            }
            else
            {
                scrollUp = 0;
            }

            if (cursorPos.X <= right && cursorPos.X > (right - 30))
            {
                scrollRight++;
                if (scrollRight > 4)
                {
                    ScrollRight(15);
                }
            }
            else
            {
                scrollRight = 0;
            }

            if (cursorPos.X >= left && cursorPos.X < (left + 30))
            {
                scrollLeft++;
                if (scrollLeft > 4)
                {
                    ScrollLeft(15);
                }
            }
            else
            {
                scrollLeft = 0;
            }

            if (scrollUp == 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight == 0)
            {
                cursor = 8;
            }
            else if (scrollUp > 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight == 0)
            {
                cursor = 0;
            }
            else if (scrollUp == 0 && scrollDown > 0 && scrollLeft == 0 && scrollRight == 0)
            {
                cursor = 4;
            }
            else if (scrollUp == 0 && scrollDown == 0 && scrollLeft > 0 && scrollRight == 0)
            {
                cursor = 6;
            }
            else if (scrollUp == 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight > 0)
            {
                cursor = 2;
            }
            else if (scrollUp > 0 && scrollDown == 0 && scrollLeft > 0 && scrollRight == 0)
            {
                cursor = 7;
            }
            else if (scrollUp > 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight > 0)
            {
                cursor = 1;
            }
            else if (scrollUp == 0 && scrollDown > 0 && scrollLeft == 0 && scrollRight > 0)
            {
                cursor = 3;
            }
            else if (scrollUp == 0 && scrollDown > 0 && scrollLeft > 0 && scrollRight == 0)
            {
                cursor = 5;
            }
            else
            {
                cursor = 8;
            }
        }

        /// <summary>
        ///  Overrides the OnMouseLeave event to provide custom cursor manipulation
        /// </summary>
        /// <param name="e">Null</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            enabledCursor = false;
        }

        /// <summary>
        ///  Overrides the OnMouseEnter event to provide custom cursor manipulation
        /// </summary>
        /// <param name="e">Null</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            enabledCursor = true;
        }

        /// <summary>
        ///  Initializes DirectDraw rendering APIs.  The function can be called to initialize
        ///  both Windowed and FullScreen mode, but FullScreen mode isn't fully
        ///  implemented.
        /// </summary>
        /// <param name="fullscreen">If true then fullscreen will be enabled.</param>
        /// <returns>True if DirectDraw is initialized.</returns>
        public bool InitializeGraphicEngine(bool fullscreen)
        {
            try
            {
                this.fullscreen = fullscreen;

                if (fullscreen)
                {
                    GraphicsEngine.Current.SetFullScreenMode(Parent.Handle);
                    CreateFullScreenSurfaces();
                }
                else
                {
                    Parent.Show();
                    GraphicsEngine.Current.SetWindow(Parent.Handle);                    
                    CreateWindowedSurfaces();
                }

                return true;
            }
            catch (DirectXException de)
            {
                ErrorLog.LogHandledException(de);
            }
            return false;
        }

        /// <summary>
        ///  Method used to create the necessary surfaces required for windowed
        ///  mode.
        /// </summary>
        /// <returns>True if the surfaces are created, otherwise false.</returns>
        public bool CreateWindowedSurfaces()
        {
            _screenSurface = GraphicsEngine.Current.CreatePrimarySurface(Handle, false, false);
            
            if (_screenSurface != null)
            {
                Trace.WriteLine("Primary Surface InVideo? " + _screenSurface.InVideo);

                var workSurfaceWidth = Math.Min(Width, _actualsize.Right);
                var workSurfaceHeight = Math.Min(Height, _actualsize.Bottom);

                _backBufferSurface = GraphicsEngine.Current.CreateWorkSurface(workSurfaceWidth, workSurfaceHeight);
                
                Trace.WriteLine("Back Buffer Surface InVideo? " + _backBufferSurface.InVideo);

                _backgroundSurface = GraphicsEngine.Current.CreateWorkSurface(workSurfaceWidth, workSurfaceHeight);

                Trace.WriteLine("Background Surface InVideo? " + _backgroundSurface.InVideo);

                _stagingSurface = GraphicsEngine.Current.CreateWorkSurface(workSurfaceWidth, workSurfaceHeight);
                Trace.WriteLine("Staging Surface InVideo? " + _stagingSurface.InVideo);
            }

            if (!_screenSurface.InVideo || !_backBufferSurface.InVideo || !_backgroundSurface.InVideo ||
                !_stagingSurface.InVideo)
            {
                videomemory = false;
                drawtext = true; // For now, turn this to false for a perf increase on slower machines
            }

            ResetTerrarium();
            ClearBackground();

            return true;
        }

        /// <summary>
        ///  Creates the surfaces required for full screen operation.
        /// </summary>
        /// <returns>True if the surfaces are initialized, false otherwise.</returns>
        public bool CreateFullScreenSurfaces()
        {
            if (doubleBuffer)
            {
                _screenSurface = GraphicsEngine.Current.CreatePrimarySurface(Handle, true, false);

                if (_screenSurface != null)
                {
                    _backBufferSurface = GraphicsEngine.Current.CreateWorkSurface(640, 480);
                }

                ClearBackground();
            }
            else
            {
                _screenSurface = GraphicsEngine.Current.CreatePrimarySurface(Handle, true, true);

                if (_screenSurface != null)
                {
                    _backBufferSurface = _screenSurface.GetBackBufferSurface();
                }
            }

            ResetTerrarium();

            return true;
        }

        /// <summary>
        ///  Overrides OnMouseUp in order to enable creature selection.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            var selectThisAnimalOnly = true;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                selectThisAnimalOnly = false;
            }

            SelectAnimalFromPoint(new Point(e.X + _viewsize.Left, e.Y + _viewsize.Top), selectThisAnimalOnly);
        }

        /// <summary>
        ///  Attempts to select a creature given the world offset point.
        /// </summary>
        /// <param name="p">Point to check for creature intersection.</param>
        /// <param name="selectThisAnimalOnly">Determines if this creature should be added to the selection</param>
        public void SelectAnimalFromPoint(Point p, bool selectThisAnimalOnly)
        {
            if (wv != null)
            {
                foreach (OrganismState orgState in wv.State.Organisms)
                {
                    if (orgState.RenderInfo != null)
                    {
                        var tsSprite = (TerrariumSprite) orgState.RenderInfo;
                        var radius = tsSprite.FrameWidth;
                        var rec = new Rectangle((int) tsSprite.XPosition - (tsSprite.FrameWidth >> 1),
                                                (int) tsSprite.YPosition - (tsSprite.FrameWidth >> 1),
                                                tsSprite.FrameWidth, tsSprite.FrameWidth);
                        if (rec.Contains(p))
                        {
                            tsSprite.Selected = !tsSprite.Selected;
                            OnOrganismClicked(new OrganismClickedEventArgs(orgState));
                        }
                        else if (selectThisAnimalOnly && tsSprite.Selected)
                        {
                            tsSprite.Selected = false;
                            OnOrganismClicked(new OrganismClickedEventArgs(orgState));
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Helper function for firing the MiniMapUpdated event
        ///  whenever a new mini-map becomes available.
        /// </summary>
        /// <param name="e">Mini-map event arguments containing the newest map.</param>
        private void OnMiniMapUpdated(MiniMapUpdatedEventArgs e)
        {
            if (MiniMapUpdated != null)
            {
                MiniMapUpdated(this, e);
            }
        }

        /// <summary>
        ///  Helper function for firing the OrganismClicked event
        ///  whenever an organism is selected within the game
        ///  view.
        /// </summary>
        /// <param name="e">Event arguments detailing which organism was clicked.</param>
        private void OnOrganismClicked(OrganismClickedEventArgs e)
        {
            if (OrganismClicked != null)
            {
                OrganismClicked(this, e);
            }
        }

        /// <summary>
        ///  Used to change the background slide of the Terrarium.
        ///  Only a single background slide can be added with this
        ///  revision.
        /// </summary>
        public void AddBackgroundSlide()
        {
            tsm.Remove("background");
            tsm.Add("background", 1, 1);

            if (DrawBackgroundGrid)
            {
                var workSurface = tsm["background"].GetDefaultSurface();
                var surface = workSurface.SpriteSurface.Surface;

                for (var h = 0; h < workSurface.SpriteSurface.Rect.Bottom; h += 8)
                {
                    surface.SetForeColor(Color.Gray.ToArgb());
                    surface.SetFillColor(Color.Gray.ToArgb());
                    surface.DrawLine(0, h, workSurface.SpriteSurface.Rect.Right, h);
                    surface.DrawLine(0, h - 1, workSurface.SpriteSurface.Rect.Right, h - 1);
                }
                for (var w = 0; w < workSurface.SpriteSurface.Rect.Right; w += 8)
                {
                    surface.SetForeColor(Color.Gray.ToArgb());
                    surface.SetFillColor(Color.Gray.ToArgb());
                    surface.DrawLine(w, 0, w, workSurface.SpriteSurface.Rect.Bottom);
                    surface.DrawLine(w - 1, 0, w - 1, workSurface.SpriteSurface.Rect.Bottom);
                }
            }
        }

        /// <summary>
        ///  Adds a generic 10frame by 40frame sprite surface that
        ///  is compatible with creature animation.
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        public void AddSpriteSurface(string name)
        {
            tsm.Remove(name);
            tsm.Add(name, 10, 40);
        }

        /// <summary>
        ///  Add a complex sprite surface given the number of frames.
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        /// <param name="xFrames">The number of frames width-wise.</param>
        /// <param name="yFrames">The number of frames height-wise.</param>
        public void AddComplexSpriteSurface(string name, int xFrames, int yFrames)
        {
            tsm.Remove(name);
            tsm.Add(name, xFrames, yFrames);
        }

        /// <summary>
        ///  Add a complex sprite surface that takes advantage of sized sprites
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        /// <param name="xFrames">The number of frames width wise.</param>
        /// <param name="yFrames">The number of frames height wise.</param>
        public void AddComplexSizedSpriteSurface(string name, int xFrames, int yFrames)
        {
            tsm.Remove(name);
            tsm.AddSizedSurface(name, xFrames, yFrames);
        }

        /// <summary>
        ///  Tell the game view to create a new world and reset the rendering
        ///  surfaces.
        /// </summary>
        /// <param name="xPixels">The number of world pixels</param>
        /// <param name="yPixels">The number of world pixels</param>
        /// <returns></returns>
        public Rectangle CreateWorld(int xPixels, int yPixels)
        {
            wld = new World();
            _actualsize = wld.CreateWorld(xPixels, yPixels);

            ResetTerrarium();
            viewchanged = true;
            return ActualSize;
        }

        /// <summary>
        ///  Reinitialize all surfaces after they have been lost.
        ///  This method invokes the garbage collector to make sure
        ///  that any COM references have been cleaned up, else surface
        ///  renewal won't work correctly.
        /// </summary>
        private void ReInitSurfaces()
        {
            _screenSurface = null;
            _backBufferSurface = null;
            _backgroundSurface = null;
            _stagingSurface = null;

            tsm.Clear();
            tfm.Clear();

            GC.Collect(2);
            GC.WaitForPendingFinalizers();

            if (fullscreen)
            {
                CreateFullScreenSurfaces();
            }
            else
            {
                CreateWindowedSurfaces();
            }
        }

        /// <summary>
        /// Handles logic for resizing the game view
        /// </summary>
        public bool ResizeViewer()
        {
            try
            {
                resizing = true;
                viewchanged = true;

                // Reset and re-initialize all surfaces to the new size
                ResetTerrarium();
                ReInitSurfaces();

                // Add default surfaces
                AddBackgroundSlide();
                AddComplexSpriteSurface("cursor", 1, 9);
                AddComplexSpriteSurface("teleporter", 16, 1);
                AddComplexSizedSpriteSurface("plant", 1, 1);

                // Mark viewport as having changed.  This forces us to redraw
                // background surfaces that we have cached
                viewchanged = true;

                // Re-center the screen
                var centerX = _viewsize.Left + ((_viewsize.Right - _viewsize.Left)/2);
                var centerY = _viewsize.Top + ((_viewsize.Bottom - _viewsize.Top)/2);
                CenterTo(centerX, centerY);

                resizing = false;

                return true;
            }
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);
                resizing = false;
                return false;
            }
        }

        /// <summary>
        ///  Renders a new frame of animation.  This is the entry point for drawing
        ///  code and is required every time a new frame is to be drawn.
        /// </summary>
        public void RenderFrame()
        {
#if TRACE
            Profiler.Start("TerrariumDirectDrawGameView.RenderFrame()");
#else
            TimeMonitor tm = new TimeMonitor();
            tm.Start();
#endif
            // Don't draw while we are resizing the viewer
            // This might happen on a secondary thread so
            // we need some minimal protection.
            if (resizing)
            {
                return;
            }

            // If we are still drawing then skip this frame.
            // However, only skip one frame to prevent hangs.
            if (drawing)
            {
                drawing = false;
                return;
            }

            try
            {
                paused = false;
                drawing = true;
                skipframe = !skipframe;
                if (fullscreen)
                {
                    // No full screen mode
                }
                else
                {
                    if (_screenSurface == null || _screenSurface.IsLost() != 0 ||
                        _backBufferSurface == null || _backBufferSurface.IsLost() != 0)
                    {
                        if (ResizeViewer() == false)
                        {
                            return;
                        }
                    }

                    if (updateTickerChanged && (updateTicker % 10) == 0)
                    {
                        updateTickerChanged = false;
                        updateMiniMap = true;
                        if (wld != null && miniMap == null)
                        {
                            //this.miniMap = new Bitmap(wld.MiniMap);
                            miniMap = new Bitmap(wld.MiniMap, (_actualsize.Right - _actualsize.Left) / 4,
                                                 (_actualsize.Bottom - _actualsize.Top) / 4);
                        }
                        if (miniMap != null)
                        {
                            var graphics = Graphics.FromImage(miniMap);
                            graphics.Clear(Color.Transparent);
                            graphics.Dispose();
                        }
                    }

                    CheckScroll();
#if TRACE
                    Profiler.Start("TerrariumDirectDrawGameView.RenderFrame()::Primary Surface Blit");
#endif
                    if (drawScreen)
                    {
                        if (videomemory || !skipframe)
                        {
                            PaintBackground();
                            PaintSprites(_backBufferSurface, false);
                            PaintMessage();
                            PaintCursor();

                            // Grab the Window RECT
                            var windowRect = new Rectangle();
                            GraphicsEngine.Current.GetWindowRect(Handle, ref windowRect);

                            // Set up a sample destination rectangle
                            var destRect = windowRect;

                            // Grab the Source Rectangle for the Bezel
                            Rectangle srcRect = _backBufferSurface.Rect;
                            var destWidth = destRect.Right - destRect.Left;
                            var destHeight = destRect.Bottom - destRect.Top;
                            var srcWidth = srcRect.Right;
                            var srcHeight = srcRect.Bottom;

                            bltUsingBezel = false;
                            if (srcWidth < destWidth)
                            {
                                int left = (destWidth - srcWidth) / 2;
                                int right = destRect.Left + srcWidth;

                                destRect = Rectangle.FromLTRB(destRect.Left + left, destRect.Top, right, destRect.Bottom);
                                bltUsingBezel = true;
                            }
                            if (srcHeight < destHeight)
                            {
                                int top = (destHeight - srcHeight) / 2;
                                int bottom = destRect.Top + srcHeight;

                                destRect = Rectangle.FromLTRB(destRect.Left, destRect.Top + top, destRect.Right, bottom);
                                bltUsingBezel = true;
                            }

                            if (bltUsingBezel)
                            {
                                if (destRect.Top != _bezelRect.Top ||
                                    destRect.Left != _bezelRect.Left ||
                                    destRect.Bottom != _bezelRect.Bottom ||
                                    destRect.Right != _bezelRect.Right)
                                {
                                    _screenSurface.BltColorFill(ref windowRect, 0);
                                }
                                _bezelRect = destRect;
                            }

                            _screenSurface.Blt(ref destRect, _backBufferSurface, ref srcRect, BltFlags.BltWait);
                        }
                    }
#if TRACE
                    Profiler.End("TerrariumDirectDrawGameView.RenderFrame()::Primary Surface Blit");
#endif
                    if (updateMiniMap)
                    {
                        updateMiniMap = false;
                        OnMiniMapUpdated(new MiniMapUpdatedEventArgs(miniMap));
                    }
                }
            }
            finally
            {
                drawing = false;
            }
#if TRACE
            Profiler.End("TerrariumDirectDrawGameView.RenderFrame()");
#else
            renderTime += tm.EndGetMicroseconds();
            samples++;
#endif
        }

        /// <summary>
        ///  ZIndexes the teleporters so they can be properly rendered
        ///  on the screen.
        /// </summary>
        /// <returns>The z-indices for the teleporters</returns>
        private int[] TeleporterZIndex()
        {
            var zIndices = new int[hackTable.Count];
            var index = 0;

            var spriteList = hackTable.GetEnumerator();
            while (spriteList.MoveNext())
            {
                var tsSprite = (TerrariumSprite) spriteList.Value;
                tsSprite.AdvanceFrame();
                zIndices[index++] = (int) tsSprite.YPosition;
            }

            return zIndices;
        }

        /// <summary>
        ///  Renders any teleporters that exist between the given z
        ///  ordered locations.
        /// </summary>
        /// <param name="lowZ">The minimum z index.</param>
        /// <param name="highZ">The maximum z index.</param>
        private void RenderTeleporter(int lowZ, int highZ)
        {
            var spriteList = hackTable.GetEnumerator();
            var workSurface = tsm["teleporter"].GetDefaultSurface();
            if (workSurface != null)
            {
                while (spriteList.MoveNext())
                {
                    var tsSprite = (TerrariumSprite) spriteList.Value;

                    if (!videomemory && skipframe)
                    {
                        continue;
                    }

                    if (tsSprite.SpriteKey != "teleporter")
                    {
                        continue;
                    }

                    if (tsSprite.YPosition <= lowZ || tsSprite.YPosition > highZ)
                    {
                        continue;
                    }

                    var radius = 48; // This is actually diameter.  True diameter is 50
                    // but the size of the sprites are 48.
                    var dest = Rectangle.FromLTRB((int) tsSprite.XPosition - _viewsize.Left, 
                                                  (int) tsSprite.YPosition - _viewsize.Top,
                                                  (int) tsSprite.XPosition - _viewsize.Left + radius,
                                                  (int) tsSprite.YPosition - _viewsize.Top + radius);
                    if (updateMiniMap)
                    {
                        var miniMapX = (int) (tsSprite.XPosition*miniMap.Width)/_actualsize.Right;
                        miniMapX = (miniMapX > (miniMap.Width - 1)) ? (miniMap.Width - 1) : miniMapX;
                        var miniMapY = (int) (tsSprite.YPosition*miniMap.Height)/_actualsize.Bottom;
                        miniMapY = (miniMapY > (miniMap.Height - 1)) ? (miniMap.Height - 1) : miniMapY;
                        //this.miniMap.SetPixel(miniMapX, miniMapY, Color.Blue);
                        var miniMapGraphics = Graphics.FromImage(miniMap);
                        miniMapGraphics.FillRectangle(Brushes.SkyBlue, miniMapX, miniMapY, 12, 12);
                        miniMapGraphics.Dispose();
                    }

                    DirectDrawClippedRect ddClipRect = workSurface.GrabSprite((int) tsSprite.CurFrame, 0, dest, _clipRect);
                    if (!ddClipRect.Invisible)
                    {
                        _backBufferSurface.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top, workSurface.SpriteSurface, ref ddClipRect.Source, BltFastFlags.SrcColorKey);
                    }
                }
            }
        }

        /// <summary>
        ///  Controls the rendering of textual messages to the Terrarium
        ///  client screen.  Since DrawText is invoked each time, this method
        ///  is slow.
        /// </summary>
        private void PaintMessage()
        {
            if (textMessage == null)
            {
                return;
            }

            var lines = textMessage.Split('\n');
            var yOffset = (_clipRect.Bottom - ((lines.Length - 1)*12))/2;

            var dcHandle = _backBufferSurface.GetDC();

            var graphics = Graphics.FromHdc(dcHandle);

            var font = new Font("Verdana", 6.75f, FontStyle.Bold);

            var rectangle = new Rectangle(4, 4, ClientRectangle.Width - 8, ClientRectangle.Height - 8);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.FormatFlags = StringFormatFlags.NoClip;
            stringFormat.LineAlignment = StringAlignment.Near;
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;

            graphics.DrawString(textMessage, font, Brushes.Black, rectangle, stringFormat);
            rectangle.Offset(-1, -1);
            graphics.DrawString(textMessage, font, Brushes.WhiteSmoke, rectangle, stringFormat);

            font.Dispose();

            graphics.Dispose();

            _backBufferSurface.ReleaseDC(dcHandle);
        }

        /// <summary>
        ///  Paints a custom cursor rather than the default windows cursors.
        ///  Can be used to enable cursor animation, but in the current
        ///  revision simply paints a custom cursor based on mouse location.
        /// </summary>
        private void PaintCursor()
        {
            if (!enabledCursor || Cursor != null || !drawCursor)
            {
                return;
            }

            DirectDrawSpriteSurface workSurface;
            workSurface = tsm["cursor"].GetDefaultSurface();
            if (workSurface != null)
            {
                var dest = new Rectangle();

                var xOffset = cursorPos.X;
                var yOffset = cursorPos.Y;

                if (bltUsingBezel)
                {
                    var cursorRectangle = ComputeCursorRectangle();
                    xOffset -= cursorRectangle.Left;
                    yOffset -= cursorRectangle.Top;
                }

                switch (cursor)
                {
                    case 1:
                        dest = Rectangle.FromLTRB(xOffset - 16, yOffset, xOffset, yOffset + 16);
                        break;
                    case 2:
                        goto case 1;
                    case 3:
                        dest = Rectangle.FromLTRB(xOffset - 16, yOffset - 16, xOffset, yOffset);
                        break;
                    case 4:
                        goto case 3;
                    case 5:
                        dest = Rectangle.FromLTRB(xOffset, yOffset - 16, xOffset + 16, yOffset);
                        break;
                    case 6:
                        goto case 5;
                    default:
                        dest = Rectangle.FromLTRB(xOffset, yOffset, xOffset + 16, yOffset + 16);
                        break;
                }

                DirectDrawClippedRect ddClipRect;
                ddClipRect = workSurface.GrabSprite(0, cursor, dest, _clipRect);
                if (!ddClipRect.Invisible)
                {
                    _backBufferSurface.Blt(ref ddClipRect.Destination, workSurface.SpriteSurface, ref ddClipRect.Source, BltFlags.KeySrc);
                }
            }
        }

        /// <summary>
        ///  Paint sprites on the given surface.  This method is the meat
        ///  of the graphics engine.  Normally, creatures are painted to
        ///  the work surface using this method.  To increase performance plants
        ///  are rendered to the background surface only once every 10 frames.
        ///  Lots of work happening in this function so either read through the
        ///  code or examine the Terrarium Graphics Engine whitepaper for more
        ///  information.
        /// </summary>
        /// <param name="surf">The surface to render to.</param>
        /// <param name="PlantsOnly">True to render plants, false to render animals.</param>
        private void PaintSprites(IGraphicsSurface surf, bool PlantsOnly)
        {
#if TRACE
            Profiler.Start("TerrariumDirectDrawGameView.PaintSprites()");
#endif
            if (wv == null)
            {
                return;
            }

            if (tfm.Count > 100)
            {
                tfm.Clear();
            }

            var TeleporterZIndices = TeleporterZIndex();
            var lastTeleporterZIndex = 0;

            foreach (OrganismState orgState in wv.State.ZOrderedOrganisms)
            {
                if (orgState.RenderInfo != null)
                {
                    var tsSprite = (TerrariumSprite) orgState.RenderInfo;

                    if ((PlantsOnly && !(orgState.Species is PlantSpecies)) ||
                        (!PlantsOnly && orgState.Species is PlantSpecies))
                    {
                        continue;
                    }

                    if (orgState.Species is AnimalSpecies)
                    {
                        tsSprite.AdvanceFrame();
                        orgState.RenderInfo = tsSprite;
                    }

                    if (!videomemory && skipframe)
                    {
                        continue;
                    }

                    TerrariumSpriteSurface workTss = null;

                    if (workTss == null && tsSprite.SpriteKey != null)
                    {
                        workTss = tsm[tsSprite.SpriteKey, orgState.Radius, (orgState.Species is AnimalSpecies)];
                    }

                    if (workTss == null && tsSprite.SkinFamily != null)
                    {
                        workTss = tsm[tsSprite.SkinFamily, orgState.Radius, (orgState.Species is AnimalSpecies)];
                    }

                    if (workTss == null)
                    {
                        if (orgState.Species is AnimalSpecies)
                        {
                            workTss = tsm["ant", orgState.Radius, (orgState.Species is AnimalSpecies)];
                        }
                        else
                        {
                            workTss = tsm["plant", orgState.Radius, (orgState.Species is AnimalSpecies)];
                        }
                    }

                    if (workTss != null)
                    {
                        var direction = orgState.ActualDirection;
                        var radius = orgState.Radius;
                        var framedir = 1;
                        var workSurface = workTss.GetClosestSurface((radius*2));
                        radius = workSurface.FrameWidth;

                        if (direction >= 68 && direction < 113)
                        {
                            framedir = 1;
                        }
                        else if (direction >= 113 && direction < 158)
                        {
                            framedir = 2;
                        }
                        else if (direction >= 158 && direction < 203)
                        {
                            framedir = 3;
                        }
                        else if (direction >= 203 && direction < 248)
                        {
                            framedir = 4;
                        }
                        else if (direction >= 248 && direction < 293)
                        {
                            framedir = 5;
                        }
                        else if (direction >= 293 && direction < 338)
                        {
                            framedir = 6;
                        }
                        else if (direction >= 338 && direction < 23)
                        {
                            framedir = 7;
                        }
                        else
                        {
                            framedir = 8;
                        }

                        var dest = new Rectangle((int) tsSprite.XPosition - (_viewsize.Left + (radius >> 1)),
                                                 (int) tsSprite.YPosition - (_viewsize.Top + (radius >> 1)),
                                                 (int) tsSprite.XPosition - (_viewsize.Left + (radius >> 1)) + radius,
                                                 (int) tsSprite.YPosition - (_viewsize.Top + (radius >> 1)) + radius);
                        
                        if (updateMiniMap)
                        {
                            var miniMapX = (int) (tsSprite.XPosition*miniMap.Width)/_actualsize.Right;
                            miniMapX = (miniMapX > (miniMap.Width - 1)) ? (miniMap.Width - 1) : miniMapX;
                            var miniMapY = (int) (tsSprite.YPosition*miniMap.Height)/_actualsize.Bottom;
                            miniMapY = (miniMapY > (miniMap.Height - 1)) ? (miniMap.Height - 1) : miniMapY;

                            var brushColor = Color.Fuchsia;

                            if (orgState.Species.GetType() == typeof (PlantSpecies))
                            {
                                brushColor = Color.Lime;
                            }
                            else if (orgState.IsAlive == false)
                            {
                                brushColor = Color.Black;
                            }
                            else
                            {
                                var orgSpecies = (Species) orgState.Species;
                                if (orgSpecies.MarkingColor == KnownColor.Green ||
                                    orgSpecies.MarkingColor == KnownColor.Black)
                                {
                                    brushColor = Color.Red;
                                }
                                else
                                {
                                    brushColor = Color.FromKnownColor(orgSpecies.MarkingColor);
                                }
                            }

                            Brush orgBrush = new SolidBrush(brushColor);

                            var miniMapGraphics = Graphics.FromImage(miniMap);
                            miniMapGraphics.FillRectangle(orgBrush, miniMapX, miniMapY, 12, 12);
                            miniMapGraphics.Dispose();
                            orgBrush.Dispose();

                            //this.miniMap.SetPixel(
                            //    miniMapX,
                            //    miniMapY,
                            //    (orgState.Species is PlantSpecies) ? Color.Green : (!orgState.IsAlive) ? Color.Black : (((Species) orgState.Species).MarkingColor == KnownColor.Green || ((Species) orgState.Species).MarkingColor == KnownColor.Black) ? Color.Red : Color.FromKnownColor(((Species) orgState.Species).MarkingColor));
                        }

                        DirectDrawClippedRect ddClipRect;
                        if (orgState.Species is PlantSpecies)
                        {
                            ddClipRect = workSurface.GrabSprite((int) tsSprite.CurFrame, 0, dest, _clipRect);
                        }
                        else
                        {
                            if (tsSprite.PreviousAction == DisplayAction.NoAction ||
                                tsSprite.PreviousAction == DisplayAction.Reproduced ||
                                tsSprite.PreviousAction == DisplayAction.Teleported ||
                                tsSprite.PreviousAction == DisplayAction.Dead)
                            {
                                if (tsSprite.PreviousAction == DisplayAction.Dead)
                                {
                                    ddClipRect = workSurface.GrabSprite(
                                        9,
                                        ((int) DisplayAction.Died) + framedir,
                                        dest,
                                        _clipRect);
                                }
                                else
                                {
                                    ddClipRect = workSurface.GrabSprite(0, ((int) DisplayAction.Moved) + framedir, dest,
                                                                        _clipRect);
                                }
                            }
                            else
                            {
                                ddClipRect = workSurface.GrabSprite((int) tsSprite.CurFrame,
                                                                    ((int) tsSprite.PreviousAction) + framedir, dest,
                                                                    _clipRect);
                            }
                        }

                        if (!ddClipRect.Invisible)
                        {
                            if (!PlantsOnly)
                            {
                                RenderTeleporter(lastTeleporterZIndex, (int) tsSprite.YPosition);
                                lastTeleporterZIndex = (int) tsSprite.YPosition;
                            }

                            surf.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top, workSurface.SpriteSurface, ref ddClipRect.Source, BltFastFlags.SrcColorKey);

                            if (drawtext && !PlantsOnly)
                            {
                                var textSurface = tfm[((Species) orgState.Species).Name];
                                if (textSurface != null && textSurface.Surface != null)
                                {
                                    surf.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top - 14, textSurface, 
                                                 ref TerrariumTextSurfaceManager.StandardFontRect, BltFastFlags.SrcColorKey);
                                }
                            }

                            if (!ddClipRect.ClipLeft &&
                                !ddClipRect.ClipRight &&
                                !ddClipRect.ClipTop &&
                                !ddClipRect.ClipBottom)
                            {
                                if (drawDestinationLines)
                                {
                                    if (orgState.CurrentMoveToAction != null)
                                    {
                                        var start = orgState.Position;
                                        var end = orgState.CurrentMoveToAction.MovementVector.Destination;
                                        ((DirectDrawSurface)surf).Surface.SetForeColor(0);
                                        ((DirectDrawSurface)surf).Surface.DrawLine(start.X - _viewsize.Left, start.Y - _viewsize.Top,
                                                             end.X - _viewsize.Left, end.Y - _viewsize.Top);
                                    }
                                }

                                if (drawBoundingBox)
                                {
                                    var boundingBox = GetBoundsOfState(orgState);
                                    ((DirectDrawSurface)surf).Surface.SetForeColor(0);
                                    ((DirectDrawSurface)surf).Surface.DrawBox(
                                        boundingBox.Left - _viewsize.Left,
                                        boundingBox.Top - _viewsize.Top,
                                        (boundingBox.Right + 1) - _viewsize.Left,
                                        (boundingBox.Bottom + 1) - _viewsize.Top
                                        );
                                }

                                if (tsSprite.Selected)
                                {
                                    var boundingBox = GetBoundsOfState(orgState);
                                    ((DirectDrawSurface)surf).Surface.SetForeColor(0);
                                    ((DirectDrawSurface)surf).Surface.DrawBox(
                                        boundingBox.Left - _viewsize.Left,
                                        boundingBox.Top - _viewsize.Top,
                                        (boundingBox.Right + 1) - _viewsize.Left,
                                        (boundingBox.Bottom + 1) - _viewsize.Top
                                        );

                                    // line to left of selection box showing how much energy the animal has
                                    var lineheight =
                                        (int)
                                        ((boundingBox.Bottom - boundingBox.Top) *
                                         orgState.PercentEnergy);
                                    ((DirectDrawSurface)surf).Surface.SetForeColor(63488);
                                    ((DirectDrawSurface)surf).Surface.DrawLine(boundingBox.Left - _viewsize.Left, boundingBox.Top - _viewsize.Top,
                                                         boundingBox.Left - _viewsize.Left,
                                                         ddClipRect.Destination.Top + lineheight);

                                    //green  Maybe we want some cool graphic here though (or an actual bar?)
                                    lineheight =
                                        (int)
                                        ((boundingBox.Bottom - boundingBox.Top)*
                                         orgState.PercentInjured);
                                    ((DirectDrawSurface)surf).Surface.SetForeColor(2016);
                                    ((DirectDrawSurface)surf).Surface.DrawLine(boundingBox.Right - _viewsize.Right, boundingBox.Top - _viewsize.Top,
                                                        boundingBox.Right - _viewsize.Right,
                                                         boundingBox.Top - _viewsize.Top + lineheight);
                                }
                            }
                        }
                    }
                }
            }

            RenderTeleporter(lastTeleporterZIndex, _actualsize.Bottom);

#if TRACE
            Profiler.End("TerrariumDirectDrawGameView.PaintSprites()");
#endif
        }

        /// <summary>
        ///  Uses the bounding box computation methods to compute a
        ///  box that can be printed within the graphics engine.  This
        ///  is used for debugging creature pathing.
        /// </summary>
        /// <param name="orgState">The state of the creature to compute a bounding box for.</param>
        /// <returns>A bounding box.</returns>
        private Rectangle GetBoundsOfState(OrganismState orgState)
        {
            var origin = orgState.Position;
            var cellRadius = orgState.CellRadius;

            var p1 = new Point(
                (origin.X >> 3)*8,
                (origin.Y >> 3)*8
                );

            var bounds = new Rectangle(
                p1.X - (cellRadius*8),
                p1.Y - (cellRadius*8),
                (((cellRadius*2 + 1)*8) - 1),
                (((cellRadius*2 + 1)*8) - 1)
                );

            return bounds;
        }

        /// <summary>
        ///  Sets up the hack table with teleporter information.
        ///  The hack table is used to quickly implement new types
        ///  of sprites or implement sprites linked to immutable
        ///  objects (like the teleporter).
        /// </summary>
        /// <param name="zone">The teleporter zone to initialize.</param>
        private void InitTeleporter(TeleportZone zone)
        {
            var tsZone = new TerrariumSprite();
            tsZone.CurFrame = 0;
            tsZone.CurFrameDelta = 1;
            tsZone.SpriteKey = "teleporter";
            tsZone.XPosition = zone.Rectangle.X;
            tsZone.YPosition = zone.Rectangle.Y;
            hackTable.Add(zone.ID, tsZone);
        }

        /// <summary>
        ///  Initializes a new organism state by computed and
        ///  attaching a TerrariumSprite class that can be used
        ///  to control on screen movement, animation skins, and
        ///  selection.
        /// </summary>
        /// <param name="orgState">The organism state to attach the sprite animation information to.</param>
        private void InitOrganism(OrganismState orgState)
        {
            var tsSprite = new TerrariumSprite();
            tsSprite.CurFrame = 0;
            tsSprite.CurFrameDelta = 1;

            var species = orgState.Species;
            if (species is AnimalSpecies)
            {
                tsSprite.SpriteKey = ((AnimalSpecies) species).Skin;
                tsSprite.SkinFamily = ((AnimalSpecies) species).SkinFamily.ToString();
                tsSprite.IsPlant = false;

                if (tsSprite.SpriteKey == null && tsSprite.SkinFamily == null)
                {
                    tsSprite.SpriteKey = AnimalSkinFamily.Spider.ToString(); // This will be our default
                }
            }
            else
            {
                tsSprite.SpriteKey = ((PlantSpecies) species).Skin;
                tsSprite.SkinFamily = ((PlantSpecies) species).SkinFamily.ToString();
                tsSprite.IsPlant = true;

                if (tsSprite.SpriteKey == null && tsSprite.SkinFamily == null)
                {
                    tsSprite.SpriteKey = PlantSkinFamily.Plant.ToString(); // This will be our default
                }
            }

            tsSprite.XPosition = orgState.Position.X;
            tsSprite.YPosition = orgState.Position.Y;
            tsSprite.PreviousAction = orgState.PreviousDisplayAction;
            orgState.RenderInfo = tsSprite;
        }

        /// <summary>
        ///  Updates the sprites controlled by the game view by providing a new
        ///  world vector from the game engine.
        /// </summary>
        /// <param name="worldVector">The new world vector of organisms.</param>
        public void UpdateWorld(WorldVector worldVector)
        {
#if TRACE
            Profiler.Start("TerrariumDirectDrawGameView.UpdateWorld()");
#endif
            wv = worldVector;
            paintPlants = true;

            updateTicker++;
            updateTickerChanged = true;

            var zones = wv.State.Teleporter.GetTeleportZones();
            foreach (var zone in zones)
            {
                if (hackTable.ContainsKey(zone.ID))
                {
                    var tsZone = (TerrariumSprite) hackTable[zone.ID];
                    tsZone.XDelta = (zone.Rectangle.X - tsZone.XPosition)/10;
                    tsZone.YDelta = (zone.Rectangle.Y - tsZone.YPosition)/10;
                    hackTable[zone.ID] = tsZone;
                }
                else
                {
                    InitTeleporter(zone);
                }
            }

            foreach (OrganismState orgState in wv.State.Organisms)
            {
                if (orgState.RenderInfo != null)
                {
                    var tsSprite = (TerrariumSprite) orgState.RenderInfo;

                    if (orgState is AnimalState)
                    {
                        if (tsSprite.PreviousAction != orgState.PreviousDisplayAction)
                        {
                            tsSprite.CurFrame = 0;
                            tsSprite.PreviousAction = orgState.PreviousDisplayAction;
                        }
                        tsSprite.XDelta = (orgState.Position.X - tsSprite.XPosition)/10;
                        tsSprite.YDelta = (orgState.Position.Y - tsSprite.YPosition)/10;
                    }
                    else
                    {
                        tsSprite.CurFrame = 0;
                        tsSprite.PreviousAction = orgState.PreviousDisplayAction;
                        tsSprite.XPosition = orgState.Position.X;
                        tsSprite.YPosition = orgState.Position.Y;
                    }
                    orgState.RenderInfo = tsSprite;
                }
                else
                {
                    InitOrganism(orgState);
                }
            }
#if TRACE
            Profiler.End("TerrariumDirectDrawGameView.UpdateWorld()");
#endif
        }

        /// <summary>
        ///  Resets the Terrarium and prepares it for a new world, without
        ///  having to reboot the entire game.  
        /// </summary>
        private void ResetTerrarium()
        {
            // Lets reset the hackTable
            hackTable = new Hashtable();

            // Lets reset the view coordinates
            if (fullscreen)
            {
                _viewsize = Rectangle.FromLTRB(0, 0, 500, 400);
            }
            else
            {
                int right   = Width - 1;
                int bottom  = Height - 1;

                if (right > _actualsize.Right)
                {
                    right = _actualsize.Right;
                }
                if (right > _actualsize.Bottom)
                {
                    right = _actualsize.Bottom;
                }

                _viewsize = Rectangle.FromLTRB(0, 0, right, bottom);

            }
            _clipRect = _viewsize;
        }

        /// <summary>
        ///  Clear any background surfaces to black.  This helps
        ///  find rendering artifacts where portions of a scene
        ///  aren't updated.  Since the background is cleared to
        ///  black, the portions not updated are clearly visible.
        /// </summary>
        private void ClearBackground()
        {
            var clearRect = new Rectangle();
            _backBufferSurface.BltColorFill(ref clearRect, 0);

            clearRect = new Rectangle();
            _backgroundSurface.BltColorFill(ref clearRect, 0);

            clearRect = new Rectangle();
            _stagingSurface.BltColorFill(ref clearRect, 0);
        }

        /// <summary>
        ///  Renders the Terrarium background image.  Also renders
        ///  plants on the background for static plant drawing to
        ///  enable higher performance.
        /// </summary>
        private void PaintBackground()
        {
#if TRACE
            Profiler.Start("TerrariumDirectDrawGameView.PaintBackground()");
#endif
            if (!videomemory && skipframe)
            {
#if TRACE
                Profiler.End("TerrariumDirectDrawGameView.PaintBackground()");
#endif
                return;
            }

            if (viewchanged)
            {
                DirectDrawSpriteSurface workSurface = tsm["background"].GetDefaultSurface();

                if (workSurface != null)
                {
                    var xTileStart = _viewsize.Left/wld.TileYSize;
                    var xTileEnd = (_viewsize.Right/wld.TileYSize) + 1;

                    if ((_viewsize.Right%wld.TileYSize) != 0)
                    {
                        xTileEnd++;
                    }

                    var yTileStart = _viewsize.Top/wld.TileYSize;
                    var yTileEnd = (_viewsize.Bottom/wld.TileYSize) + 2;

                    for (var j = yTileStart; j < yTileEnd && j < wld.Map.GetLength(1); j++)
                    {
                        for (var i = xTileStart; i < xTileEnd; i++)
                        {
                            var tInfo = wld.Map[i, j];
                            Rectangle dest = Rectangle.FromLTRB(tInfo.XOffset - _viewsize.Left,
                                                                tInfo.YOffset - _viewsize.Top,
                                                                tInfo.XOffset - _viewsize.Left + World.TileXSize,
                                                                tInfo.YOffset - _viewsize.Top + wld.TileYSize);

                            var iTrans =
                                (tInfo.Tile < 8)
                                    ?
                                        (tInfo.Transition*2)
                                    :
                                        (tInfo.Transition*2) + 1;
                            var iTile =
                                (tInfo.Tile < 8)
                                    ?
                                        tInfo.Tile
                                    :
                                        tInfo.Tile - 8;

                            iTile = 0;
                            iTrans = 0;

                            var ddClipRect = workSurface.GrabSprite(iTile, iTrans, dest, _clipRect);
                            if (!ddClipRect.Invisible)
                            {
                                _backgroundSurface.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top,
                                                           workSurface.SpriteSurface, ref ddClipRect.Source, BltFastFlags.SrcColorKey | BltFastFlags.FastWait);
                            }
                        }
                    }
                }
                viewchanged = false;
                paintPlants = true;
            }

            Rectangle srcRect;
            Rectangle destRect;

            if (paintPlants)
            {
                srcRect     = _backgroundSurface.Rect;
                destRect    = _stagingSurface.Rect;

                _stagingSurface.Blt(ref destRect, _backgroundSurface, ref srcRect, BltFlags.BltWait);

                PaintSprites(_stagingSurface, true);
                paintPlants = false;
            }


            srcRect = _stagingSurface.Rect;
            destRect = _backBufferSurface.Rect;
            _backBufferSurface.Blt(ref destRect, _stagingSurface, ref srcRect, BltFlags.BltWait);
#if TRACE
            Profiler.End("TerrariumDirectDrawGameView.PaintBackground()");
#endif
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll up</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollUp(int pixels)
        {
            if ((_viewsize.Top - pixels) < _actualsize.Top)
            {
                pixels = _viewsize.Top - _actualsize.Top;
            }

            _viewsize = Rectangle.FromLTRB(_viewsize.Left, _viewsize.Top - pixels, _viewsize.Right, _viewsize.Bottom - pixels);

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll down</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollDown(int pixels)
        {
            if ((_viewsize.Bottom + pixels) > _actualsize.Bottom)
            {
                pixels = _actualsize.Bottom - _viewsize.Bottom;
            }

            _viewsize = Rectangle.FromLTRB(_viewsize.Left, _viewsize.Top + pixels, _viewsize.Right, _viewsize.Bottom + pixels);

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll left</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollLeft(int pixels)
        {
            if ((_viewsize.Left - pixels) < _actualsize.Left)
            {
                pixels = _viewsize.Left - _actualsize.Left;
            }
            _viewsize = Rectangle.FromLTRB(_viewsize.Left - pixels, _viewsize.Top, _viewsize.Right - pixels, _viewsize.Bottom);

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll right</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollRight(int pixels)
        {
            if ((_viewsize.Right + pixels) > _actualsize.Right)
            {
                pixels = _actualsize.Right - _viewsize.Right;
            }

            _viewsize = Rectangle.FromLTRB(_viewsize.Left + pixels, _viewsize.Top, _viewsize.Right + pixels, _viewsize.Bottom);

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.  Attempts
        ///  to center the view to the defined point.
        /// </summary>
        /// <param name="xOffset">X location to center to.</param>
        /// <param name="yOffset">Y location to center to.</param>
        public void CenterTo(int xOffset, int yOffset)
        {
            var ViewportOffsetXSize = (_viewsize.Right - _viewsize.Left)/2;
            var ViewportOffsetYSize = (_viewsize.Bottom - _viewsize.Top)/2;

            if (xOffset < (_viewsize.Left + ViewportOffsetXSize))
            {
                ScrollLeft((_viewsize.Left + ViewportOffsetXSize) - xOffset);
            }
            else
            {
                ScrollRight(xOffset - (_viewsize.Left + ViewportOffsetXSize));
            }

            if (yOffset < (_viewsize.Top + ViewportOffsetYSize))
            {
                ScrollUp((_viewsize.Top + ViewportOffsetYSize) - yOffset);
            }
            else
            {
                ScrollDown(yOffset - (_viewsize.Top + ViewportOffsetYSize));
            }
        }
    }
}