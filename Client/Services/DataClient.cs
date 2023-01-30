using BlazorApp1.Client.Authentication;
using BlazorApp1.Shared.Models;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlazorApp1.Client.Services
{
    public class DataClient : IDataClient
    {
        private AuthenticationStateProvider _authenticationStateProvider;
        private readonly HttpClient _httpClient;
        public DataClient(AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _httpClient = httpClient;
        }
        public async Task<Folder> addChildFolder(Folder parentFolder, string foldername)
        {


            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                Folder newchildfolder = new Folder();
                newchildfolder.Name = foldername;
                newchildfolder.ParentFolderId = parentFolder.Id;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var result = await _httpClient.PostAsJsonAsync("api/folders/create", newchildfolder);
                return await result.Content.ReadFromJsonAsync<Folder>();
            }
            return null;
        }

        public async Task<Item> createNewItem(Folder parentfolder)
        {

            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                Item newitem = new Item();
                newitem.Name = "New Item";
                newitem.ParentFolder = parentfolder.Id;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var result = await _httpClient.PostAsJsonAsync<Item>("api/items/create/", newitem);
                return await result.Content.ReadFromJsonAsync<Item>();
            }
            return null;
        }

        public async Task<Folder> createNewRootFolder(Folder folder)
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var result = await _httpClient.PostAsJsonAsync("api/folders/create", folder);
                return await result.Content.ReadFromJsonAsync<Folder>();
            }
            return null;
            
        }

        public async Task<ItemTag> CreateTag(ItemTag tag)
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var result = await _httpClient.PostAsJsonAsync<ItemTag>("api/tags/create/", tag);
                return await result.Content.ReadFromJsonAsync<ItemTag>();
            }
            return null;

        }

        public async Task DeleteFolder(Folder folder)
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                await _httpClient.PostAsJsonAsync("api/folders/delete/",folder);
            }
            
        }

        public async Task DeleteItem(Item item)
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                await _httpClient.PostAsJsonAsync("api/items/delete/", item);
            }
        }

        public async Task<List<Folder>> GetAllFolders()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var folders = await _httpClient.GetFromJsonAsync<List<Folder>>("api/folders/all/");
                return folders;
            }
            return null;
        }

        public async Task<List<Item>> GetAllItems()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var items = await _httpClient.GetFromJsonAsync<List<Item>>("api/items/all/");
                return items;
            }
            return null;
        }

        public async Task<List<ItemTag>> GetAllTags()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var tags = await _httpClient.GetFromJsonAsync<List<ItemTag>>("api/tags/all/");
                return tags;
            }
            return null;
        }

        public Task<List<Folder>> getChildrenFolders(string parentid)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HistoryModel>> GetHistoryList()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var historylist = await _httpClient.GetFromJsonAsync<List<HistoryModel>>("api/history/all");
                return historylist;
            }
            return null;

        }

        public Task<List<Item>> getItemsFromFolder(string folderid)
        {
            throw new NotImplementedException();
        }

        public Task<List<Folder>> GetRootFolders()
        {
            throw new NotImplementedException();
        }

        public async Task HistoryAdd(string history, string path)
        {
            HistoryModel historymodel = new HistoryModel();
            historymodel.history = history + path;
            await _httpClient.PostAsJsonAsync<HistoryModel>("api/history/add/", historymodel);
        }

        public async Task RemoveHistory(string id)
        {
            await _httpClient.DeleteAsync("api/history/remove/" + id);
        }

        public async Task<Item> UpdateItem(Item item)
        {
            var result = await _httpClient.PostAsJsonAsync<Item>("api/items/update", item);
            return await result.Content.ReadFromJsonAsync<Item>();
        }

        public async Task DeleteTag(ItemTag tag)
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            var token = await customAuthStateProvider.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                await _httpClient.PostAsJsonAsync("api/tags/delete/", tag);
            }
        }
    }
}
