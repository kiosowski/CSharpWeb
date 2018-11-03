using IRunes.Models;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace IRunes.Controllers
{
    public class AlbumsController : BaseController
    {
        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var albums = this.Context.Albums;

            var listOfAlbums = string.Empty;

            if (albums.Any())
            {
                foreach (var album in albums)
                {
                    // var albumHtml = $@"<a href=""/Albums/Details/Album.Id"">" + album.Name + "</a><br>" + Environment.NewLine;
                    var albumHtml = $@"<a href=""/Albums/Details?id=" + album.Id.ToString() + @""">" + album.Name + "</a></br></br>";
                    listOfAlbums += albumHtml;
                }

                this.ViewBag["albumsList"] = listOfAlbums;
            }
            else if (albums.Count() <= 0)
            {
                this.ViewBag["albumsList"] = $@"<h1>There are currently no albums</h1>";
            }

            return this.View();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/");
            }
            return this.View();
        }

        public IHttpResponse PostCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var albumName = request.FormData["name"].ToString().Trim();
            var cover = request.FormData["cover"].ToString().Trim();

            var album = new Album
            {
                Name = albumName,
                Cover = cover
            };

            this.Context.Albums.Add(album);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return new BadRequestResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }

            var response = new RedirectResult("/Albums/All");

            return response;
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var id = request.QueryData["id"].ToString();
            var album = this.Context.Albums.FirstOrDefault(x => x.Id == id);

            if (album == null)
            {
                return new BadRequestResult("Album not found", HttpResponseStatusCode.NotFound);
            }

            var coverUrl = WebUtility.UrlDecode(album.Cover);
            var cover = $@"<img src=""" + coverUrl + @"""style=""width:130px;height:130px;"">";

            this.ViewBag["CoverPicture"] = cover;
            this.ViewBag["albumName"] = album.Name;
            this.ViewBag["albumPrice"] = album.Price.ToString();
            this.ViewBag["CreateTrack"] = $@"<a href=""/Tracks/Create?albumId=" + album.Id.ToString() + @""">" + "Create Track" + "</a>";

            var tracks = album.Tracks;

            var listOfTracks = string.Empty;

            if (tracks.Any())
            {
                listOfTracks += "<ul><ol>";
                foreach (var track in tracks)
                {

                    var trackHtml = $@"<b><li><a href=""/Tracks/Details?albumId=" + album.Id.ToString() + "&trackId=" + track.Track.Id + @""">" + track.Track.Name + "</a></li></b>";
                    listOfTracks += trackHtml;
                }

                listOfTracks += "</ol></ul>";
                this.ViewBag["listOfTracks"] = listOfTracks;
            }
            else
            {
                this.ViewBag["listOfTracks"] = $@"<h1>There are currently no tracks.</h1>";
            }

            return this.View();
        }
    }
}
