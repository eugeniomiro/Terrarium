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
        private     readonly TerrariumTextSurfaceManager    _tfm = new TerrariumTextSurfaceManager();
        private     readonly TerrariumSpriteSurfaceManager  _tsm = new TerrariumSpriteSurfaceManager();

        private     IGraphicsSurface                        _backgroundSurface;
        private     IGraphicsSurface                        _stagingSurface;
        /// <summary>
        ///  The primary screen surface for the picture box.
        /// </summary>
        protected   IGraphicsSurface                        ScreenSurface;

        /// <summary>
        ///  The back buffer surface used with the picture box.
        /// </summary>
        protected   IGraphicsSurface                        BackBufferSurface;

        private     Rectangle                               _bezelRect;
        private     bool                                    _bltUsingBezel;
        private     Rectangle                               _clipRect;

        // Cursor
        private     int                                     _cursor;
        private     Point                                   _cursorPos;
        private     bool                                    doubleBuffer = true;
        private     bool                                    _drawBackgroundGrid;
        private     bool                                    _drawing;
        private     bool                                    _enabledCursor;
        private     bool                                    _fullscreen;
        private     Hashtable                               _hackTable = new Hashtable();
        private     Bitmap                                  _miniMap;
        private     bool                                    _paintPlants;
        private     bool                                    _resizing;

        // Scrolling
        private     int                                     _scrollDown;
        private     int                                     _scrollLeft;
        private     int                                     _scrollRight;
        private     int                                     _scrollUp;
        private     bool                                    _skipframe = true;
        private     Profiler                                _tddGameViewProf;
        private     bool                                    _updateMiniMap;

        private     int                                     _updateTicker;
        private     bool                                    _updateTickerChanged;
        private     bool                                    _viewchanged = true;
        private     World                                   _wld;
        private     WorldVector                             _wv;

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
        public TerrariumGameView() {
            DrawCursor = true;
            VideoMemory = true;
            DrawText = true;
            DrawScreen = true;
            InitializeComponent();
        }

        /// <summary>
        ///  Provides access to the game profiler.
        /// </summary>
        public Profiler Profiler
        {
            get { return _tddGameViewProf ?? (_tddGameViewProf = new Profiler()); }
        }

        /// <summary>
        ///  Returns the amount of time required to render a scene.
        /// </summary>
        public long RenderTime { get; private set; }

        /// <summary>
        ///  Returns the number of samples (frames) obtained.
        /// </summary>
        public int Samples { get; private set; }

        /// <summary>
        ///  Pauses the TerrariumGameView and stops rendering.  A call to 
        ///  RenderFrame will automatically unpause the animation.
        /// </summary>
        public bool Paused { private get; set; }

        /// <summary>
        ///  Provides access to the bitmap representing the minimap for the
        ///  currently loaded world.
        /// </summary>
        public Bitmap MiniMap
        {
            get
            {
                if (_miniMap != null) return _miniMap;
                if (_wld != null)
                {
                    //this.miniMap = new Bitmap(wld.MiniMap);
                    _miniMap = new Bitmap(_wld.MiniMap, (ActualSize.Right - ActualSize.Left)/4,
                        (ActualSize.Bottom - ActualSize.Top)/4);
                    var graphics = Graphics.FromImage(_miniMap);
                    graphics.Clear(Color.Transparent);
                    graphics.Dispose();
                }
                else
                {
                    return null;
                }

                return _miniMap;
            }
        }

        /// <summary>
        ///  Enables the textual display of a message over top of
        ///  the Terrarium view.  This property accepts newlines and
        ///  centers the text on the screen.
        /// </summary>
        public string TerrariumMessage { get; set; }

        /// <summary>
        ///  Enables the drawing of organism destination lines
        ///  within the Terrarium Client
        /// </summary>
        public bool DrawDestinationLines { get; set; }

        /// <summary>
        ///  Controls the rendering of the entire game view.
        /// </summary>
        public bool DrawScreen { get; set; }

        /// <summary>
        ///  Controls the rendering of organism bounding boxes useful
        ///  for movement debugging.
        /// </summary>
        public bool DrawBoundingBox { get; set; }

        /// <summary>
        ///  Controls rendering of a special background that contains
        ///  a grid overlay that mimics the Terrarium application's
        ///  cell grid.
        /// </summary>
        public bool DrawBackgroundGrid
        {
            get { return _drawBackgroundGrid; }
            set
            {
                _viewchanged = true;
                _drawBackgroundGrid = value;
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
                if (Paused || !DrawScreen)
                {
                    return Cursors.Default;
                }

                if (_bltUsingBezel)
                {
                    if (!ComputeCursorRectangle().Contains(_cursorPos))
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
        public bool DrawText { get; set; }

        /// <summary>
        ///  Determines if the primary surfaces are in video memory
        ///  or not.
        /// </summary>
        public bool VideoMemory { get; private set; }

        /// <summary>
        ///  Returns the size of the viewport window.
        /// </summary>
        public Rectangle ViewSize { get; private set; }

        /// <summary>
        ///  Returns the full size of the world.
        /// </summary>
        public Rectangle ActualSize { get; private set; }

        /// <summary>
        /// Determines whether to 
        /// </summary>
        public bool DrawCursor { get; set; }

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
            RenderTime = 0;
            Samples = 0;
        }

        /// <summary>
        ///  Overrides OnMouseMove to enable custom cursor rendering.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            _cursorPos = new Point(e.X, e.Y);
        }

        private Rectangle ComputeCursorRectangle()
        {
            var left = 0;
            var top = 0;
            var right = Width;
            var bottom = Height;

            if (_bltUsingBezel)
            {
                if (_clipRect.Right == ActualSize.Right)
                {
                    left = (right - _clipRect.Right)/2;
                    right = left + _clipRect.Right;
                }
                if (_clipRect.Bottom == ActualSize.Bottom)
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
            if (!_enabledCursor)
            {
                _scrollUp = 0;
                _scrollDown = 0;
                _scrollLeft = 0;
                _scrollRight = 0;
                return;
            }

            var cursorRect = ComputeCursorRectangle();
            var left = cursorRect.Left;
            var top = cursorRect.Top;
            var right = cursorRect.Right;
            var bottom = cursorRect.Bottom;

            if (_cursorPos.Y <= bottom && _cursorPos.Y > (bottom - 30))
            {
                _scrollDown++;
                if (_scrollDown > 4)
                {
                    ScrollDown(15);
                }
            }
            else
            {
                _scrollDown = 0;
            }

            if (_cursorPos.Y >= top && _cursorPos.Y < (top + 30))
            {
                _scrollUp++;
                if (_scrollUp > 4)
                {
                    ScrollUp(15);
                }
            }
            else
            {
                _scrollUp = 0;
            }

            if (_cursorPos.X <= right && _cursorPos.X > (right - 30))
            {
                _scrollRight++;
                if (_scrollRight > 4)
                {
                    ScrollRight(15);
                }
            }
            else
            {
                _scrollRight = 0;
            }

            if (_cursorPos.X >= left && _cursorPos.X < (left + 30))
            {
                _scrollLeft++;
                if (_scrollLeft > 4)
                {
                    ScrollLeft(15);
                }
            }
            else
            {
                _scrollLeft = 0;
            }

            if (_scrollUp == 0 && _scrollDown == 0 && _scrollLeft == 0 && _scrollRight == 0)
            {
                _cursor = 8;
            }
            else if (_scrollUp > 0 && _scrollDown == 0 && _scrollLeft == 0 && _scrollRight == 0)
            {
                _cursor = 0;
            }
            else if (_scrollUp == 0 && _scrollDown > 0 && _scrollLeft == 0 && _scrollRight == 0)
            {
                _cursor = 4;
            }
            else if (_scrollUp == 0 && _scrollDown == 0 && _scrollLeft > 0 && _scrollRight == 0)
            {
                _cursor = 6;
            }
            else if (_scrollUp == 0 && _scrollDown == 0 && _scrollLeft == 0 && _scrollRight > 0)
            {
                _cursor = 2;
            }
            else if (_scrollUp > 0 && _scrollDown == 0 && _scrollLeft > 0 && _scrollRight == 0)
            {
                _cursor = 7;
            }
            else if (_scrollUp > 0 && _scrollDown == 0 && _scrollLeft == 0 && _scrollRight > 0)
            {
                _cursor = 1;
            }
            else if (_scrollUp == 0 && _scrollDown > 0 && _scrollLeft == 0 && _scrollRight > 0)
            {
                _cursor = 3;
            }
            else if (_scrollUp == 0 && _scrollDown > 0 && _scrollLeft > 0 && _scrollRight == 0)
            {
                _cursor = 5;
            }
            else
            {
                _cursor = 8;
            }
        }

        /// <summary>
        ///  Overrides the OnMouseLeave event to provide custom cursor manipulation
        /// </summary>
        /// <param name="e">Null</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _enabledCursor = false;
        }

        /// <summary>
        ///  Overrides the OnMouseEnter event to provide custom cursor manipulation
        /// </summary>
        /// <param name="e">Null</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _enabledCursor = true;
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
                _fullscreen = fullscreen;

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
            catch (GraphicsException de)
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
            ScreenSurface = GraphicsEngine.Current.CreatePrimarySurface(Handle, false, false);
            
            if (ScreenSurface != null)
            {
                Trace.WriteLine("Primary Surface InVideo? " + ScreenSurface.InVideo);

                var workSurfaceWidth = Math.Min(Width, ActualSize.Right);
                var workSurfaceHeight = Math.Min(Height, ActualSize.Bottom);

                BackBufferSurface = GraphicsEngine.Current.CreateWorkSurface(workSurfaceWidth, workSurfaceHeight);
                
                Trace.WriteLine("Back Buffer Surface InVideo? " + BackBufferSurface.InVideo);

                _backgroundSurface = GraphicsEngine.Current.CreateWorkSurface(workSurfaceWidth, workSurfaceHeight);

                Trace.WriteLine("Background Surface InVideo? " + _backgroundSurface.InVideo);

                _stagingSurface = GraphicsEngine.Current.CreateWorkSurface(workSurfaceWidth, workSurfaceHeight);
                Trace.WriteLine("Staging Surface InVideo? " + _stagingSurface.InVideo);
            }

            if (ScreenSurface != null && (!ScreenSurface.InVideo || !BackBufferSurface.InVideo || !_backgroundSurface.InVideo ||
                                          !_stagingSurface.InVideo))
            {
                VideoMemory = false;
                DrawText = true; // For now, turn this to false for a perf increase on slower machines
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
                ScreenSurface = GraphicsEngine.Current.CreatePrimarySurface(Handle, true, false);

                if (ScreenSurface != null)
                {
                    BackBufferSurface = GraphicsEngine.Current.CreateWorkSurface(640, 480);
                }

                ClearBackground();
            }
            else
            {
                ScreenSurface = GraphicsEngine.Current.CreatePrimarySurface(Handle, true, true);

                if (ScreenSurface != null)
                {
                    BackBufferSurface = ScreenSurface.GetBackBufferSurface();
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

            bool selectThisAnimalOnly = (ModifierKeys & Keys.Shift) != Keys.Shift;

            SelectAnimalFromPoint(new Point(e.X + ViewSize.Left, e.Y + ViewSize.Top), selectThisAnimalOnly);
        }

        /// <summary>
        ///  Attempts to select a creature given the world offset point.
        /// </summary>
        /// <param name="p">Point to check for creature intersection.</param>
        /// <param name="selectThisAnimalOnly">Determines if this creature should be added to the selection</param>
        public void SelectAnimalFromPoint(Point p, bool selectThisAnimalOnly)
        {
            if (_wv != null)
            {
                foreach (OrganismState orgState in _wv.State.Organisms)
                {
                    if (orgState.RenderInfo == null) continue;
                    var tsSprite = (TerrariumSprite) orgState.RenderInfo;
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
            _tsm.Remove("background");
            _tsm.Add("background", 1, 1);

            if (DrawBackgroundGrid)
            {
                var workSurface = _tsm["background"].GetDefaultSurface();
                var surface = workSurface.SpriteSurface;

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
            _tsm.Remove(name);
            _tsm.Add(name, 10, 40);
        }

        /// <summary>
        ///  Add a complex sprite surface given the number of frames.
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        /// <param name="xFrames">The number of frames width-wise.</param>
        /// <param name="yFrames">The number of frames height-wise.</param>
        public void AddComplexSpriteSurface(string name, int xFrames, int yFrames)
        {
            _tsm.Remove(name);
            _tsm.Add(name, xFrames, yFrames);
        }

        /// <summary>
        ///  Add a complex sprite surface that takes advantage of sized sprites
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        /// <param name="xFrames">The number of frames width wise.</param>
        /// <param name="yFrames">The number of frames height wise.</param>
        public void AddComplexSizedSpriteSurface(string name, int xFrames, int yFrames)
        {
            _tsm.Remove(name);
            _tsm.AddSizedSurface(name, xFrames, yFrames);
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
            _wld = new World();
            ActualSize = _wld.CreateWorld(xPixels, yPixels);

            ResetTerrarium();
            _viewchanged = true;
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
            ScreenSurface = null;
            BackBufferSurface = null;
            _backgroundSurface = null;
            _stagingSurface = null;

            _tsm.Clear();
            _tfm.Clear();

            GC.Collect(2);
            GC.WaitForPendingFinalizers();

            if (_fullscreen)
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
                _resizing = true;
                _viewchanged = true;

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
                _viewchanged = true;

                // Re-center the screen
                var centerX = ViewSize.Left + ((ViewSize.Right - ViewSize.Left)/2);
                var centerY = ViewSize.Top + ((ViewSize.Bottom - ViewSize.Top)/2);
                CenterTo(centerX, centerY);

                _resizing = false;

                return true;
            }
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);
                _resizing = false;
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
            if (_resizing)
            {
                return;
            }

            // If we are still drawing then skip this frame.
            // However, only skip one frame to prevent hangs.
            if (_drawing)
            {
                _drawing = false;
                return;
            }

            try
            {
                Paused = false;
                _drawing = true;
                _skipframe = !_skipframe;
                if (_fullscreen)
                {
                    // No full screen mode
                }
                else
                {
                    if (ScreenSurface == null || ScreenSurface.IsLost() != 0 ||
                        BackBufferSurface == null || BackBufferSurface.IsLost() != 0)
                    {
                        if (ResizeViewer() == false)
                        {
                            return;
                        }
                    }

                    if (_updateTickerChanged && (_updateTicker % 10) == 0)
                    {
                        _updateTickerChanged = false;
                        _updateMiniMap = true;
                        if (_wld != null && _miniMap == null)
                        {
                            //this.miniMap = new Bitmap(wld.MiniMap);
                            _miniMap = new Bitmap(_wld.MiniMap, (ActualSize.Right - ActualSize.Left) / 4,
                                                 (ActualSize.Bottom - ActualSize.Top) / 4);
                        }
                        if (_miniMap != null)
                        {
                            var graphics = Graphics.FromImage(_miniMap);
                            graphics.Clear(Color.Transparent);
                            graphics.Dispose();
                        }
                    }

                    CheckScroll();
#if TRACE
                    Profiler.Start("TerrariumDirectDrawGameView.RenderFrame()::Primary Surface Blit");
#endif
                    if (DrawScreen)
                    {
                        if (VideoMemory || !_skipframe)
                        {
                            PaintBackground();
                            PaintSprites(BackBufferSurface, false);
                            PaintMessage();
                            PaintCursor();

                            // Set up a sample destination rectangle
                            var destRect = GraphicsEngine.Current.GetWindowRect(Handle);

                            // Grab the Source Rectangle for the Bezel
                            if (BackBufferSurface != null) {
                                Rectangle srcRect = BackBufferSurface.Rect;
                                var destWidth = destRect.Right - destRect.Left;
                                var destHeight = destRect.Bottom - destRect.Top;
                                var srcWidth = srcRect.Right;
                                var srcHeight = srcRect.Bottom;

                                _bltUsingBezel = false;
                                if (srcWidth < destWidth)
                                {
                                    int left = (destWidth - srcWidth) / 2;
                                    int right = destRect.Left + srcWidth;

                                    destRect = Rectangle.FromLTRB(destRect.Left + left, destRect.Top, right, destRect.Bottom);
                                    _bltUsingBezel = true;
                                }
                                if (srcHeight < destHeight)
                                {
                                    int top = (destHeight - srcHeight) / 2;
                                    int bottom = destRect.Top + srcHeight;

                                    destRect = Rectangle.FromLTRB(destRect.Left, destRect.Top + top, destRect.Right, bottom);
                                    _bltUsingBezel = true;
                                }

                                if (_bltUsingBezel)
                                {
                                    if (destRect.Top != _bezelRect.Top ||
                                        destRect.Left != _bezelRect.Left ||
                                        destRect.Bottom != _bezelRect.Bottom ||
                                        destRect.Right != _bezelRect.Right)
                                    {
                                        if (ScreenSurface != null) 
                                            ScreenSurface.BltColorFill(ref destRect, 0);
                                    }
                                    _bezelRect = destRect;
                                }

                                if (ScreenSurface != null)
                                    ScreenSurface.Blt(ref destRect, BackBufferSurface, ref srcRect, BltFlags.BltWait);
                            }
                        }
                    }
#if TRACE
                    Profiler.End("TerrariumDirectDrawGameView.RenderFrame()::Primary Surface Blit");
#endif
                    if (_updateMiniMap)
                    {
                        _updateMiniMap = false;
                        OnMiniMapUpdated(new MiniMapUpdatedEventArgs(_miniMap));
                    }
                }
            }
            finally
            {
                _drawing = false;
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
        private void TeleporterZIndex()
        {
            var zIndices = new int[_hackTable.Count];
            var index = 0;

            var spriteList = _hackTable.GetEnumerator();
            while (spriteList.MoveNext())
            {
                var tsSprite = (TerrariumSprite) spriteList.Value;
                tsSprite.AdvanceFrame();
                zIndices[index++] = (int) tsSprite.YPosition;
            }
        }

        /// <summary>
        ///  Renders any teleporters that exist between the given z
        ///  ordered locations.
        /// </summary>
        /// <param name="lowZ">The minimum z index.</param>
        /// <param name="highZ">The maximum z index.</param>
        private void RenderTeleporter(int lowZ, int highZ)
        {
            var spriteList = _hackTable.GetEnumerator();
            var workSurface = _tsm["teleporter"].GetDefaultSurface();
            if (workSurface != null)
            {
                while (spriteList.MoveNext())
                {
                    var tsSprite = (TerrariumSprite) spriteList.Value;

                    if (!VideoMemory && _skipframe)
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
                    var dest = Rectangle.FromLTRB((int) tsSprite.XPosition - ViewSize.Left, 
                                                  (int) tsSprite.YPosition - ViewSize.Top,
                                                  (int) tsSprite.XPosition - ViewSize.Left + radius,
                                                  (int) tsSprite.YPosition - ViewSize.Top + radius);
                    if (_updateMiniMap)
                    {
                        var miniMapX = (int) (tsSprite.XPosition*_miniMap.Width)/ActualSize.Right;
                        miniMapX = (miniMapX > (_miniMap.Width - 1)) ? (_miniMap.Width - 1) : miniMapX;
                        var miniMapY = (int) (tsSprite.YPosition*_miniMap.Height)/ActualSize.Bottom;
                        miniMapY = (miniMapY > (_miniMap.Height - 1)) ? (_miniMap.Height - 1) : miniMapY;
                        //this.miniMap.SetPixel(miniMapX, miniMapY, Color.Blue);
                        var miniMapGraphics = Graphics.FromImage(_miniMap);
                        miniMapGraphics.FillRectangle(Brushes.SkyBlue, miniMapX, miniMapY, 12, 12);
                        miniMapGraphics.Dispose();
                    }

                    IClippedRect ddClipRect = workSurface.GrabSprite((int) tsSprite.CurFrame, 0, dest, _clipRect);
                    if (!ddClipRect.Invisible) {
                        Rectangle r = ddClipRect.Source;
                        BackBufferSurface.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top,
                                                  workSurface.SpriteSurface, ref r, BltFastFlags.SrcColorKey);
                        ddClipRect.Source = r;
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
            if (TerrariumMessage == null)
            {
                return;
            }

            var dcHandle = BackBufferSurface.GetDC();

            using (var graphics = Graphics.FromHdc(dcHandle)) {
                using (var font = new Font("Verdana", 6.75f, FontStyle.Bold)) {
                    var rectangle = new Rectangle(4, 4, ClientRectangle.Width - 8, ClientRectangle.Height - 8);

                    var stringFormat = new StringFormat {
                        Alignment = StringAlignment.Near,
                        FormatFlags = StringFormatFlags.NoClip,
                        LineAlignment = StringAlignment.Near,
                        Trimming = StringTrimming.EllipsisCharacter
                    };

                    graphics.DrawString(TerrariumMessage, font, Brushes.Black, rectangle, stringFormat);
                    rectangle.Offset(-1, -1);
                    graphics.DrawString(TerrariumMessage, font, Brushes.WhiteSmoke, rectangle, stringFormat);
                }
            }

            BackBufferSurface.ReleaseDC(dcHandle);
        }

        /// <summary>
        ///  Paints a custom cursor rather than the default windows cursors.
        ///  Can be used to enable cursor animation, but in the current
        ///  revision simply paints a custom cursor based on mouse location.
        /// </summary>
        private void PaintCursor()
        {
            if (!_enabledCursor || Cursor != null || !DrawCursor)
            {
                return;
            }

            ISpriteSurface workSurface = _tsm["cursor"].GetDefaultSurface();
            if (workSurface != null)
            {
                Rectangle dest;

                var xOffset = _cursorPos.X;
                var yOffset = _cursorPos.Y;

                if (_bltUsingBezel)
                {
                    var cursorRectangle = ComputeCursorRectangle();
                    xOffset -= cursorRectangle.Left;
                    yOffset -= cursorRectangle.Top;
                }

                switch (_cursor)
                {
                    case 1:
                    case 2:
                        dest = Rectangle.FromLTRB(xOffset - 16, yOffset, xOffset, yOffset + 16);
                        break;
                    case 3:
                    case 4:
                        dest = Rectangle.FromLTRB(xOffset - 16, yOffset - 16, xOffset, yOffset);
                        break;
                    case 5:
                    case 6:
                        dest = Rectangle.FromLTRB(xOffset, yOffset - 16, xOffset + 16, yOffset);
                        break;
                    default:
                        dest = Rectangle.FromLTRB(xOffset, yOffset, xOffset + 16, yOffset + 16);
                        break;
                }

                IClippedRect ddClipRect = workSurface.GrabSprite(0, _cursor, dest, _clipRect);
                if (!ddClipRect.Invisible) {
                    Rectangle d = ddClipRect.Destination;
                    Rectangle s = ddClipRect.Source;

                    BackBufferSurface.Blt(ref d, workSurface.SpriteSurface, ref s, BltFlags.KeySrc);
                    ddClipRect.Source = s;
                    ddClipRect.Destination = d;
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
        /// <param name="plantsOnly">True to render plants, false to render animals.</param>
        private void PaintSprites(IGraphicsSurface surf, bool plantsOnly)
        {
#if TRACE
            Profiler.Start("TerrariumDirectDrawGameView.PaintSprites()");
#endif
            if (_wv == null)
            {
                return;
            }

            if (_tfm.Count > 100)
            {
                _tfm.Clear();
            }

            TeleporterZIndex();
            var lastTeleporterZIndex = 0;

            foreach (OrganismState orgState in _wv.State.ZOrderedOrganisms)
            {
                if (orgState.RenderInfo == null) continue;

                var tsSprite = (TerrariumSprite) orgState.RenderInfo;

                if (!plantsOnly && orgState.Species is PlantSpecies || 
                     plantsOnly && !(orgState.Species is PlantSpecies))
                {
                    continue;
                }

                if (orgState.Species is AnimalSpecies)
                {
                    tsSprite.AdvanceFrame();
                    orgState.RenderInfo = tsSprite;
                }

                if (!VideoMemory && _skipframe)
                {
                    continue;
                }

                TerrariumSpriteSurface workTss = null;

                if (workTss == null && tsSprite.SpriteKey != null)
                {
                    workTss = _tsm[tsSprite.SpriteKey, orgState.Radius, (orgState.Species is AnimalSpecies)];
                }

                if (workTss == null && tsSprite.SkinFamily != null)
                {
                    workTss = _tsm[tsSprite.SkinFamily, orgState.Radius, (orgState.Species is AnimalSpecies)];
                }

                if (workTss == null)
                {
                    if (orgState.Species is AnimalSpecies)
                    {
                        workTss = _tsm["ant", orgState.Radius, (orgState.Species is AnimalSpecies)];
                    }
                    else
                    {
                        workTss = _tsm["plant", orgState.Radius, (orgState.Species is AnimalSpecies)];
                    }
                }

                if (workTss != null)
                {
                    var direction   = orgState.ActualDirection;
                    var radius      = orgState.Radius;
                    var framedir    = 1;
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

                    var dest = new Rectangle((int) tsSprite.XPosition - (ViewSize.Left + (radius >> 1)),
                        (int) tsSprite.YPosition - (ViewSize.Top + (radius >> 1)),
                        (int) tsSprite.XPosition - (ViewSize.Left + (radius >> 1)) + radius,
                        (int) tsSprite.YPosition - (ViewSize.Top + (radius >> 1)) + radius);
                        
                    if (_updateMiniMap)
                    {
                        var miniMapX = (int) (tsSprite.XPosition*_miniMap.Width)/ActualSize.Right;
                        miniMapX = (miniMapX > (_miniMap.Width - 1)) ? (_miniMap.Width - 1) : miniMapX;
                        var miniMapY = (int) (tsSprite.YPosition*_miniMap.Height)/ActualSize.Bottom;
                        miniMapY = (miniMapY > (_miniMap.Height - 1)) ? (_miniMap.Height - 1) : miniMapY;

                        Color brushColor;

                        if (orgState.Species is PlantSpecies)
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

                        var miniMapGraphics = Graphics.FromImage(_miniMap);
                        miniMapGraphics.FillRectangle(orgBrush, miniMapX, miniMapY, 12, 12);
                        miniMapGraphics.Dispose();
                        orgBrush.Dispose();

                        //this.miniMap.SetPixel(
                        //    miniMapX,
                        //    miniMapY,
                        //    (orgState.Species is PlantSpecies) ? Color.Green : (!orgState.IsAlive) ? Color.Black : (((Species) orgState.Species).MarkingColor == KnownColor.Green || ((Species) orgState.Species).MarkingColor == KnownColor.Black) ? Color.Red : Color.FromKnownColor(((Species) orgState.Species).MarkingColor));
                    }

                    IClippedRect ddClipRect;
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
                        if (!plantsOnly)
                        {
                            RenderTeleporter(lastTeleporterZIndex, (int) tsSprite.YPosition);
                            lastTeleporterZIndex = (int) tsSprite.YPosition;
                        }
                        Rectangle s = ddClipRect.Source;

                        surf.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top, workSurface.SpriteSurface, ref s, BltFastFlags.SrcColorKey);
                        ddClipRect.Source = s;

                        if (DrawText && !plantsOnly)
                        {
                            var textSurface = _tfm[((Species) orgState.Species).Name];
                            if (textSurface != null)
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
                            if (DrawDestinationLines) {
                                if (orgState.CurrentMoveToAction != null) {
                                    var start = orgState.Position;
                                    var end = orgState.CurrentMoveToAction.MovementVector.Destination;
                                    surf.SetForeColor(0);
                                    surf.DrawLine(start.X - ViewSize.Left, start.Y - ViewSize.Top, end.X - ViewSize.Left,
                                        end.Y - ViewSize.Top);
                                }
                            }

                            if (DrawBoundingBox) {
                                var boundingBox = GetBoundsOfState(orgState);
                                surf.SetForeColor(0);
                                surf.DrawBox(boundingBox.Left - ViewSize.Left,
                                             boundingBox.Top - ViewSize.Top,
                                             (boundingBox.Right + 1) - ViewSize.Left,
                                             (boundingBox.Bottom + 1) - ViewSize.Top);
                            }

                            if (tsSprite.Selected)
                            {
                                surf.SetForeColor(0);
                                surf.DrawBox(ddClipRect.Destination.Left, ddClipRect.Destination.Top,
                                    ddClipRect.Destination.Right, ddClipRect.Destination.Bottom);

                                // red  Maybe we want some cool graphic here though
                                var lineheight =
                                    (int)
                                        ((ddClipRect.Destination.Bottom - ddClipRect.Destination.Top)*
                                         orgState.PercentEnergy);
                                surf.SetForeColor(63488);
                                surf.DrawLine(ddClipRect.Destination.Left - 1, ddClipRect.Destination.Top,
                                    ddClipRect.Destination.Left - 1,
                                    ddClipRect.Destination.Top + lineheight);

                                //green  Maybe we want some cool graphic here though (or an actual bar?)
                                lineheight =
                                    (int)
                                        ((ddClipRect.Destination.Bottom - ddClipRect.Destination.Top)*
                                         orgState.PercentInjured);
                                surf.SetForeColor(2016);
                                surf.DrawLine(ddClipRect.Destination.Right + 1, ddClipRect.Destination.Top,
                                    ddClipRect.Destination.Right + 1,
                                    ddClipRect.Destination.Top + lineheight);
                            }
                        }
                    }
                }
            }

            RenderTeleporter(lastTeleporterZIndex, ActualSize.Bottom);

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
        private static Rectangle GetBoundsOfState(OrganismState orgState)
        {
            var origin      = orgState.Position;
            var cellRadius  = orgState.CellRadius;

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
            var tsZone = new TerrariumSprite {
                CurFrame = 0,
                CurFrameDelta = 1,
                SpriteKey = "teleporter",
                XPosition = zone.Rectangle.X,
                YPosition = zone.Rectangle.Y
            };
            _hackTable.Add(zone.ID, tsZone);
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
            var tsSprite = new TerrariumSprite {CurFrame = 0, CurFrameDelta = 1};

            var species         = orgState.Species;
            var animalSpecies   = species as AnimalSpecies;
            if (animalSpecies != null)
            {
                tsSprite.SpriteKey = animalSpecies.Skin;
                tsSprite.SkinFamily = animalSpecies.SkinFamily.ToString();
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
            _wv = worldVector;
            _paintPlants = true;

            _updateTicker++;
            _updateTickerChanged = true;

            var zones = _wv.State.Teleporter.GetTeleportZones();
            foreach (var zone in zones)
            {
                if (_hackTable.ContainsKey(zone.ID))
                {
                    var tsZone = (TerrariumSprite) _hackTable[zone.ID];
                    tsZone.XDelta = (zone.Rectangle.X - tsZone.XPosition)/10;
                    tsZone.YDelta = (zone.Rectangle.Y - tsZone.YPosition)/10;
                    _hackTable[zone.ID] = tsZone;
                }
                else
                {
                    InitTeleporter(zone);
                }
            }

            foreach (OrganismState orgState in _wv.State.Organisms)
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
            _hackTable = new Hashtable();

            // Lets reset the view coordinates
            if (_fullscreen)
            {
                ViewSize = Rectangle.FromLTRB(0, 0, 500, 400);
            }
            else
            {
                int right   = Width - 1;
                int bottom  = Height - 1;

                if (right > ActualSize.Right)
                {
                    right = ActualSize.Right;
                }
                if (right > ActualSize.Bottom)
                {
                    right = ActualSize.Bottom;
                }

                ViewSize = Rectangle.FromLTRB(0, 0, right, bottom);

            }
            _clipRect = ViewSize;
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
            BackBufferSurface.BltColorFill(ref clearRect, 0);

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
            if (!VideoMemory && _skipframe)
            {
#if TRACE
                Profiler.End("TerrariumDirectDrawGameView.PaintBackground()");
#endif
                return;
            }

            if (_viewchanged)
            {
                var workSurface = _tsm["background"].GetDefaultSurface();

                if (workSurface != null)
                {
                    var xTileStart = ViewSize.Left/_wld.TileYSize;
                    var xTileEnd = (ViewSize.Right/_wld.TileYSize) + 1;

                    if ((ViewSize.Right%_wld.TileYSize) != 0)
                    {
                        xTileEnd++;
                    }

                    var yTileStart = ViewSize.Top/_wld.TileYSize;
                    var yTileEnd = (ViewSize.Bottom/_wld.TileYSize) + 2;

                    for (var j = yTileStart; j < yTileEnd && j < _wld.Map.GetLength(1); j++)
                    {
                        for (var i = xTileStart; i < xTileEnd; i++)
                        {
                            var tInfo = _wld.Map[i, j];
                            Rectangle dest = Rectangle.FromLTRB(tInfo.XOffset - ViewSize.Left,
                                                                tInfo.YOffset - ViewSize.Top,
                                                                tInfo.XOffset - ViewSize.Left + World.TileXSize,
                                                                tInfo.YOffset - ViewSize.Top + _wld.TileYSize);

                            const int iTile = 0;
                            const int iTrans = 0;

                            var ddClipRect = workSurface.GrabSprite(iTile, iTrans, dest, _clipRect);
                            if (!ddClipRect.Invisible)
                            {
                                Rectangle s = ddClipRect.Source;
                                _backgroundSurface.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top,
                                                           workSurface.SpriteSurface, ref s, BltFastFlags.SrcColorKey | BltFastFlags.FastWait);
                                ddClipRect.Source = s;
                            }
                        }
                    }
                }
                _viewchanged = false;
                _paintPlants = true;
            }

            Rectangle srcRect;
            Rectangle destRect;

            if (_paintPlants)
            {
                srcRect     = _backgroundSurface.Rect;
                destRect    = _stagingSurface.Rect;

                _stagingSurface.Blt(ref destRect, _backgroundSurface, ref srcRect, BltFlags.BltWait);

                PaintSprites(_stagingSurface, true);
                _paintPlants = false;
            }


            srcRect = _stagingSurface.Rect;
            destRect = BackBufferSurface.Rect;
            BackBufferSurface.Blt(ref destRect, _stagingSurface, ref srcRect, BltFlags.BltWait);
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
            if ((ViewSize.Top - pixels) < ActualSize.Top)
            {
                pixels = ViewSize.Top - ActualSize.Top;
            }

            ViewSize = Rectangle.FromLTRB(ViewSize.Left, ViewSize.Top - pixels, ViewSize.Right, ViewSize.Bottom - pixels);

            if (pixels != 0)
            {
                _viewchanged = true;
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
            if ((ViewSize.Bottom + pixels) > ActualSize.Bottom)
            {
                pixels = ActualSize.Bottom - ViewSize.Bottom;
            }

            ViewSize = Rectangle.FromLTRB(ViewSize.Left, ViewSize.Top + pixels, ViewSize.Right, ViewSize.Bottom + pixels);

            if (pixels != 0)
            {
                _viewchanged = true;
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
            if ((ViewSize.Left - pixels) < ActualSize.Left)
            {
                pixels = ViewSize.Left - ActualSize.Left;
            }
            ViewSize = Rectangle.FromLTRB(ViewSize.Left - pixels, ViewSize.Top, ViewSize.Right - pixels, ViewSize.Bottom);

            if (pixels != 0)
            {
                _viewchanged = true;
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
            if ((ViewSize.Right + pixels) > ActualSize.Right)
            {
                pixels = ActualSize.Right - ViewSize.Right;
            }

            ViewSize = Rectangle.FromLTRB(ViewSize.Left + pixels, ViewSize.Top, ViewSize.Right + pixels, ViewSize.Bottom);

            if (pixels != 0)
            {
                _viewchanged = true;
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
            var viewportOffsetXSize = (ViewSize.Right - ViewSize.Left)/2;
            var viewportOffsetYSize = (ViewSize.Bottom - ViewSize.Top)/2;

            if (xOffset < (ViewSize.Left + viewportOffsetXSize))
            {
                ScrollLeft((ViewSize.Left + viewportOffsetXSize) - xOffset);
            }
            else
            {
                ScrollRight(xOffset - (ViewSize.Left + viewportOffsetXSize));
            }

            if (yOffset < (ViewSize.Top + viewportOffsetYSize))
            {
                ScrollUp((ViewSize.Top + viewportOffsetYSize) - yOffset);
            }
            else
            {
                ScrollDown(yOffset - (ViewSize.Top + viewportOffsetYSize));
            }
        }
    }
}