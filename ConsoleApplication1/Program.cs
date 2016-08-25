using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static AutorizationCodeAuth auth;
        static void Main(string[] args)
        {
            //LastWay();
            OldWay();
        }

        private static void LastWay()
        {
            //Create the auth object
            var auth = new ClientCredentialsAuth()
            {
                //Your client Id
                ClientId = @"6371ccb6dc9e44069bca6763bcb539b6",
                //Your client secret UNSECURE!!
                ClientSecret = @"38fdd4f3987347f090ec7f184e8d72cc",
                //How many permissions we need?
                Scope = Scope.UserReadPrivate | Scope.PlaylistModifyPublic | Scope.PlaylistModifyPrivate,
            };
            //With this token object, we now can make calls
            Token token = auth.DoAuth();
            Console.WriteLine(token.AccessToken);
        }

        private static void OldWay()
        {
            //Create the auth object
            auth = new AutorizationCodeAuth()
            {
                //Your client Id
                ClientId = "4ba8934628a54571b57ed84de51d1825",
                //Set this to localhost if you want to use the built-in HTTP Server
                RedirectUri = "http://localhost:8000",
                //How many permissions we need?
                Scope = Scope.UserReadPrivate | Scope.PlaylistModifyPublic | Scope.PlaylistModifyPrivate,
                ShowDialog = true
            };
            //This will be called, if the user cancled/accept the auth-request
            auth.OnResponseReceivedEvent += auth_OnResponseReceivedEvent;
            //a local HTTP Server will be started (Needed for the response)
            auth.StartHttpServer(8000);
            //This will open the spotify auth-page. The user can decline/accept the request

            try
            {
                auth.DoAuth();
                Thread.Sleep(60000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            auth.StopHttpServer();
            Console.WriteLine("Too long, didnt respond, exiting now...");
        }

        private static void auth_OnResponseReceivedEvent(AutorizationCodeAuthResponse response)
        {
            //Stop the HTTP Server, done.
            auth.StopHttpServer();

            //NEVER DO THIS! You would need to provide the ClientSecret.
            //You would need to do it e.g via a PHP-Script.
            Token token = auth.ExchangeAuthCode(response.Code, "94486431428d4c0b95ea40897c73b13f");
            Console.WriteLine(token.AccessToken);
            Console.ReadKey();
            //var spotify = new SpotifyWebApiClass()
            //{
            //    TokenType = token.TokenType,
            //    AccessToken = token.AccessToken
            //};

            //With the token object, you can now make API calls
        }
    }
}
