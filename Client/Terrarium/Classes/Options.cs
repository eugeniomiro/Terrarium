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

        [Option('p', "Preview", DefaultValue = 0)]
        public int Preview { get; set; }

        [Option('s', "ScreenSaver", DefaultValue = false)]
        public bool ScrenSaver { get; set; }

        [Option("NoStart", DefaultValue = false)]
        public bool NoStart { get; set; }

        [Option("NoDirectX", DefaultValue = false)]
        public bool NoDirectX { get; set; }

        [Option("LoadTerrarium", DefaultValue = "")]
        public string LoadTerrarium { get; set; }

        [Option("NewTerrarium", DefaultValue = "")]
        public string NetTerrarium { get; set; }

        [Option("BlackListCheck", DefaultValue = false)]
        public bool BlackListCheck { get; set; }

        [Option("SkipSplashScreen", DefaultValue = false)]
        public bool SkipSplashScreen { get; set; }

        [Option("WindowRectangle", DefaultValue = new Int32[] { 0 })]
        public int[] WindowRectangle { get; set; }

        [Option("WindowState", DefaultValue = FormWindowState.Normal)]
        public FormWindowState WindowState { get; set; }
    }
}
