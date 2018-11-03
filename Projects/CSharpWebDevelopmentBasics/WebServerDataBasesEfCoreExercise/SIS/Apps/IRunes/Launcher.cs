﻿using IRunes.Controllers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System;

namespace IRunes
{
    public class Launcher
    {
        public static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = ConfigureRouting();

            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }

        private static ServerRoutingTable ConfigureRouting()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            //Get
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Home/Index"] = request => new RedirectResult("/");
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Login"] = request => new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Register"] = request => new UsersController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/All"] = request => new AlbumsController().All(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Create"] = request => new AlbumsController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Details"] = request => new AlbumsController().Details(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Create"] = request => new TracksController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Details"] = request => new TracksController().Details(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Logout"] = request => new UsersController().Logout(request);

            //POST
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Login"] = request => new UsersController().PostLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Register"] = request => new UsersController().PostRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Albums/Create"] = request => new AlbumsController().PostCreate(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Tracks/Create"] = request => new TracksController().PostCreate(request);
          //  serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Logout"] = request => new UsersController().LogoutPost(request);



            return serverRoutingTable;
        }
    }
}
