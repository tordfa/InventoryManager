using BlazorApp1.Server.Authentication;
using BlazorApp1.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlazorApp1.Server.DataAccess;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        UserAccess useraccess = new UserAccess();

        [HttpGet]
        [Route("api/jadda/")]
        [Authorize(Roles = "user")]
        public Folder test()
        {
            var nyfolder = new Folder();
            nyfolder.Name = HttpContext.User.Identity.Name;
            var test = HttpContext.Request.Headers.Authorization.ToString();
            test = test.Replace("bearer", "").Trim();

            nyfolder.Name = test;


            return nyfolder;
        }

        [HttpGet]
        [Route("api/folders/all")]
        [Authorize(Roles = "user")]
        public List<Folder> GetFolders()
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.GetFoldersByUserName(username);
            
        }

        [HttpGet]
        [Route("api/items/all")]
        [Authorize(Roles = "user")]
        public List<Item> GetItems()
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.GetItemsByUserName(username);

        }

        [HttpPost]
        [Route("api/folders/create/")]
        [Authorize(Roles = "user")]
        public Folder CreateNewFolder([FromBody] Folder folder)
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.CreateNewFolder(folder, username);
        }
        [HttpPost]
        [Route("api/items/create/")]
        [Authorize(Roles = "user")]
        public Item CreateNewItem([FromBody] Item newitem)
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.CreateNewItem(newitem,username);
        }

        [HttpPost]
        [Route("api/folders/delete/")]
        [Authorize(Roles = "user")]
        public void DeleteFolder([FromBody] Folder folder) 
        {
            var username = HttpContext.User.Identity.Name;
            useraccess.DeleteFolder(folder,username); 
        }

        [HttpPost]
        [Route("api/items/delete")]
        [Authorize(Roles = "user")]
        public void DeleteItem([FromBody ]Item item) {
            var username = HttpContext.User.Identity.Name;
            useraccess.DeleteItem(item,username); 
        }

        [HttpPost]
        [Route("api/items/update")]
        [Authorize(Roles = "user")]
        public Item UpdateItem([FromBody] Item item)
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.UpdateItem(item,username);
        }

        [HttpGet]
        [Route("api/tags/all")]
        [Authorize(Roles = "user")]
        public IEnumerable<ItemTag> GetAllTags()
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.GetAllTags(username);
        }

        [HttpPost]
        [Route("api/tags/create/")]
        [Authorize(Roles = "user")]
        public ItemTag CreateTag([FromBody] ItemTag tag)
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.CreateTag(tag,username);
        }

        [HttpPost]
        [Route("api/tags/delete/")]
        [Authorize(Roles = "user")]
        public void DeleteTag([FromBody] ItemTag tag)
        {
            var username = HttpContext.User.Identity.Name;
            useraccess.DeleteTag(tag, username);
        }

        [HttpGet]
        [Route("api/history/all")]
        [Authorize(Roles = "user")]
        public IEnumerable<HistoryModel> GetHistoryList()
        {
            var username = HttpContext.User.Identity.Name;
            return useraccess.GetHistoryList(username);
        }

        [HttpPost]
        [Route("api/History/Add/")]
        [Authorize(Roles = "user")]
        public void HistoryAdd([FromBody] HistoryModel historymodel)
        {
            var username = HttpContext.User.Identity.Name;
            useraccess.HistoryAdd(historymodel,username);
        }

        [HttpDelete]
        [Route("api/History/Remove/")]
        [Authorize(Roles = "user")]
        public void RemoveHistory(string historyid) 
        {
            var username = HttpContext.User.Identity.Name;
            useraccess.RemoveHistory(historyid,username);
        }

    }
}
