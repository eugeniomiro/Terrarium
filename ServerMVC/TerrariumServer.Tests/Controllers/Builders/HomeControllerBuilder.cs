using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariumServer.Controllers;

namespace TerrariumServer.Tests.Controllers.Builders
{
    internal class HomeControllerBuilder
    {
        internal HomeController Build()
        {
            return new HomeController();
        }
    }
}
