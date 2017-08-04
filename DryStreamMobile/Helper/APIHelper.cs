using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using DryStreamMobile.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DryStreamMobile.Helper
{
    public static class APIHelper
    {
        #region UserApi 
        public async static Task<string> UploadCoverGetLink(MediaFile _mediaFile)
        {

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(_mediaFile.GetStream()), "\"file\"", $"\"{ _mediaFile.Path }\"");
        

            using (var client = new HttpClient())
            {
               // var httpClient = new HttpClient();

                var uploadServiceBaseAddress = GlobalMemory.serverAddressIP + "/api/Files/Upload";

                var response = await client.PostAsync(uploadServiceBaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var returnUser = JsonConvert.DeserializeObject<User>(responseBody);

                    return returnUser.CoverLink;
                }
                else
                    return "";
            }
        
        }

        public async static Task<bool> PostUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8, "application/json");
                //HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/MobileUsers",content);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;


            }
        }
        public async static Task<bool> UpdateUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                //HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/UpdateUser", content);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public static async Task<bool> DeleteUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //StringContent content = new StringContent(Conver, Encoding.UTF8, "application/json");
                // HTTP DELETE
                HttpResponseMessage response = await client.DeleteAsync("api/DeleteUser/" + user.UserID);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public async static Task<User> getUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                //HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/LoginUser",content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var returnUser = JsonConvert.DeserializeObject<User>(responseBody);
                    if (returnUser.Password == user.Password)
                    {
                        return returnUser;
                    }
                    else
                        return null;
                }
                else
                    return null;
              
            }
        }

        public static async Task<bool> findLogin(string login)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/MobileUsers/" + login);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public static async Task<bool> findEmail(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/MobileEmail/" + email);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        //nie dziala usuwanie 
        public static async Task<bool> deletePhoto(string link)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP DELETE
                HttpResponseMessage response = await client.DeleteAsync("api/DeletePhoto/" + link);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region API_Music 
        public static async Task<List<Genre>> getGenres()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Genres");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                }
            }

            return null;
        }
        public static async Task<List<SongAlbumArtist>> getSongs()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Songs");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<SongAlbumArtist>>(responseBody);
                }
            }

            return null;
        }
        public static async Task<List<Song>> getSongs(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Songs/"+id);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<Song>>(responseBody);
                }
            }

            return null;
        }

        public static async Task<List<Album>> getAlbums()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Albums");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<Album>>(responseBody);
                }
            }

            return null;
        }

        public static async Task<List<Album>> getAlbums(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Albums/"+id);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<Album>>(responseBody);
                }
            }

            return null;
        }

        public static async Task<List<Artist>> getArtists()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Artists");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<Artist>>(responseBody);
                }
            }

            return null;
        }
        public static async Task<List<Artist>> findArtists(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/findArtists/" + name.Trim());
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<Artist>>(responseBody);
                }
            }

            return null;
        }

        public static async Task<List<SongAlbumArtist>> findSong(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/FindSongs/" + name.Trim());
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<SongAlbumArtist>>(responseBody);
                }
            }

            return null;
        }
        #endregion
        #region API_Playlists

        public async static Task<bool> PostSongToPlaylist(PlaylistSong playlist)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(playlist), Encoding.UTF8, "application/json");
                //HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/PlaylistsSongs", content);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public async static Task<bool> PostPlaylist(Playlist playlist)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(playlist), Encoding.UTF8, "application/json");
                //HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/Playlists", content);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public static async Task<bool> GetPlaylists()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Playlists/" + GlobalMemory._user.UserID);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    GlobalMemory._user.Playlists = JsonConvert.DeserializeObject<List<Playlist>>(responseBody);
                    return true;
                }
            }
            return false;
        }
        public static async Task<List<PlaylistSong>> GetPlaylistsSong(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/PlaylistsSongs/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<PlaylistSong>>(responseBody);
                }
            }
            return null;
        }
        public static async Task<bool> DeletePlaylist(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // HTTP DELETE
                HttpResponseMessage response = await client.DeleteAsync("api/Playlists/" + id);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public static async Task<bool> DeleteSongFromPlaylist(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // HTTP DELETE
                HttpResponseMessage response = await client.DeleteAsync("api/PlaylistsSongs/" + id);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public static async Task<List<SongAlbumArtist>> GetSongFromPlaylists(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/SongsFromPlaylists/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<SongAlbumArtist>>(responseBody);
                }
                return null;
            }
        }
        #endregion

    }
}