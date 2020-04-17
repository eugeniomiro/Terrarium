using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Terrarium.Client
{
    class Options
    {
        [Option('c', "modal", DefaultValue = false)]
        public bool Modal { get; set; }

        [Option('p', "preview", DefaultValue = 0)]
        public int Preview { get; set; }

        [Option('s', "screensaver", DefaultValue = false)]
        public bool ScreenSaver { get; set; }

        [Option("nostart", DefaultValue = false)]
        public bool NoStart { get; set; }

        [Option("nodirectx", DefaultValue = false)]
        public bool NoDirectX { get; set; }

        [Option("loadterrarium", DefaultValue = "")]
        public string LoadTerrarium { get; set; }

        [Option("newterrarium", DefaultValue = "")]
        public string NewTerrarium { get; set; }

        [Option("blacklistcheck", DefaultValue = false)]
        public bool BlackListCheck { get; set; }

        [Option("skipsplashscreen", DefaultValue = false)]
        public bool SkipSplashScreen { get; set; }

        [OptionArray("windowrectangle", DefaultValue = new Int32[] { 0 })]
        public int[] WindowRectangle { get; set; }

        [Option("windowstate", DefaultValue = FormWindowState.Normal)]
        public FormWindowState WindowState { get; set; }
    }
}
