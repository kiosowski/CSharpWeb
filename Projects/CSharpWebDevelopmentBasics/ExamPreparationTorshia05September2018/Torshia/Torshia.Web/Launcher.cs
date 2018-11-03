using SIS.Framework;
using System;
using Torshia.Data;
using Torshia.Models;
using Torshia.Models.Enums;

namespace Torshia.Web
{
    class Launcher
    {
        public static void Main(string[] args)
        {
            
            WebHost.Start(new StartUp());
        }
    }
}
