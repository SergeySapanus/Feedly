using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using MyFeedlyServer.Models;
using console = System.Console;

namespace MyFeedlyClient.Console
{
    class Client
    {
        public HttpClient HttpClient { get; } = new HttpClient();
        public string JWT { get; private set; }
        public int AuthorizedUserId { get; private set; }

        public Client()
        {
            HttpClient.BaseAddress = new Uri("http://localhost:5000/");
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Auth

        public void Login()
        {
            this.RemoveJWTAuth();

            var model = new UserCreateOrUpdateModel();

            console.Write("Enter user name: ");
            model.Name = console.ReadLine();

            console.Write("Enter password: ");
            model.Password = console.ReadLine();

            var response = HttpClient.PostAsJsonAsync("api/auth/login", model).Result;
            console.WriteLine(response.StatusCode);

            var result = response.Content.ReadAsAsync<AuthGetModel>().Result;
            if (!ReferenceEquals(result, null))
            {
                JWT = result.Token;
                AuthorizedUserId = result.AuthorizedUserId;
            }

            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        #endregion

        #region User

        public void GetAllUsers()
        {
            this.RemoveJWTAuth();

            var response = HttpClient.GetAsync("api/user/all").Result;

            console.WriteLine(response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            console.WriteLine(result);

            CheckAuth(response);
        }

        public void GetUser()
        {
            this.AddJWTAuth();

            var response = HttpClient.GetAsync("api/user").Result;

            console.WriteLine(response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            console.WriteLine(result);

            CheckAuth(response);
        }

        public void GetUserWithCollections()
        {
            this.AddJWTAuth();

            var response = HttpClient.GetAsync("/api/user/collection").Result;

            console.WriteLine(response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            console.WriteLine(result);

            CheckAuth(response);
        }

        public void CreateUser()
        {
            this.RemoveJWTAuth();

            var model = new UserCreateOrUpdateModel();

            console.Write("Enter user name: ");
            model.Name = console.ReadLine();

            console.Write("Enter password: ");
            model.Password = console.ReadLine();

            var response = HttpClient.PostAsJsonAsync("api/user", model).Result;
            console.WriteLine(response.StatusCode);

            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        public void UpdateUser()
        {
            this.AddJWTAuth();

            var model = new UserCreateOrUpdateModel();

            console.Write("Enter user name: ");
            model.Name = console.ReadLine();

            console.Write("Enter password: ");
            model.Password = console.ReadLine();

            var response = HttpClient.PutAsJsonAsync("api/user", model).Result;
            console.WriteLine(response.StatusCode);

            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        public void DeleteUser()
        {
            this.AddJWTAuth();

            var response = HttpClient.DeleteAsync("api/user").Result;
            console.WriteLine(response.StatusCode);

            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        #endregion

        #region Feed

        public void GetAllFeeds()
        {
            this.RemoveJWTAuth();

            var response = HttpClient.GetAsync("api/feed").Result;

            console.WriteLine(response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            console.WriteLine(result);

            CheckAuth(response);
        }

        public void GetFeedById()
        {
            this.RemoveJWTAuth();

            console.Write("Enter feed id: ");

            int id;
            while (!int.TryParse(console.ReadLine(), out id))
            {

            }

            var response = HttpClient.GetAsync($"api/feed/{id}").Result;

            console.WriteLine(response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            console.WriteLine(result);

            CheckAuth(response);
        }

        public void CreateFeed()
        {
            this.AddJWTAuth();

            var model = new FeedCreateOrUpdateModel();

            console.Write("Enter collection id: ");

            int id;
            while (!int.TryParse(console.ReadLine(), out id))
            {

            }

            model.CollectionId = id;

            console.Write("Enter feed uri: ");
            model.Uri = console.ReadLine();

            var response = HttpClient.PostAsJsonAsync("api/feed", model).Result;
            console.WriteLine(response.StatusCode);
            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        #endregion

        #region Collection

        public void CreateCollection()
        {
            this.AddJWTAuth();

            var model = new CollectionCreateOrUpdateModel();

            console.Write("Enter collection name: ");
            model.Name = console.ReadLine();
            model.UserId = AuthorizedUserId;

            var response = HttpClient.PostAsJsonAsync("api/collection", model).Result;
            console.WriteLine(response.StatusCode);
            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        public void GetCollectionById()
        {
            this.AddJWTAuth();

            console.Write("Enter collection id: ");

            int id;
            while (!int.TryParse(console.ReadLine(), out id))
            {

            }

            var response = HttpClient.GetAsync($"api/collection/{id}").Result;
            console.WriteLine(response.StatusCode);
            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        public void GetNewsByCollectionId()
        {
            this.AddJWTAuth();

            console.Write("Enter collection id: ");

            int id;
            while (!int.TryParse(console.ReadLine(), out id))
            {

            }

            var response = HttpClient.GetAsync($"api/collection/{id}/news").Result;
            console.WriteLine(response.StatusCode);
            console.WriteLine(response.Content.ReadAsStringAsync().Result);

            CheckAuth(response);
        }

        #endregion

        public void Exit()
        {
            Environment.Exit(0);
        }

        private void CheckAuth(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                JWT = null;
        }
    }
}