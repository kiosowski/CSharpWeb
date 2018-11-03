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
    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var albumId = request.QueryData["albumId"].ToString();
            var album = this.Context.Albums.FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                return new BadRequestResult("Album not found", HttpResponseStatusCode.NotFound);
            }

            this.ViewBag["albumId"] = albumId;

            return this.View();
        }

        public IHttpResponse PostCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }
            var albumId = request.QueryData["albumId"].ToString();

            var trackName = request.FormData["name"].ToString().Trim();
            var trackLink = request.FormData["Link"].ToString().Trim();
            var price = decimal.Parse(request.FormData["price"].ToString().Trim());

            var track = new Track
            {
                Name = trackName,
                Link = trackLink,
                Price = price
            };

            var album = this.Context.Albums.FirstOrDefault(x => x.Id == albumId);


            var trackAlbum = new TrackAlbum
            {
                AlbumId=album.Id,
                Album=album,
                TrackId = track.Id,
                Track = track
            };

           
            this.Context.TracksAlbums.Add(trackAlbum);
            this.Context.Tracks.Add(track);
            album.Price = (album.Tracks.Sum(x => x.Track.Price)) * 0.87m ;

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return new BadRequestResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }
            
            var response = new RedirectResult($"/Albums/Details?id={albumId}");

            return response;
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var albumId = request.QueryData["albumId"].ToString();

            var trackId = request.QueryData["trackId"].ToString();

            var track = this.Context.Tracks.FirstOrDefault(x => x.Id == trackId);

            var trackLink = WebUtility.UrlDecode(track.Link);

            this.ViewBag["VideoUrl"] = trackLink;
            this.ViewBag["TrackName"] = track.Name;
            this.ViewBag["TrackPrice"] = track.Price.ToString();
            this.ViewBag["albumId"] = albumId;
            return this.View();
        }
    }
}
