using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp1.Server.DataAccess;
using BlazorApp1.Shared.Models;
using Microsoft.AspNetCore.Mvc;
namespace BlazorApp1.Server.Controllers
{
    public class FolderController : Controller
    {
        DataAccessLayer objfolder = new DataAccessLayer();

        [HttpGet]
        [Route("api/Folder/{id}")]
        public Folder folder(string id)
        {
            return objfolder.GetFolder(id);
        }

        [HttpGet]
        [Route("api/Folder/parent/{id}")]
        public IEnumerable<Folder> folderchildren(string id)
        {
            return objfolder.GetFolderChildren(id);
        }



        [HttpGet]
        [Route("api/Folder/Index")]
        public IEnumerable<Folder> Index()
        {
            return objfolder.GetAllFolders();
        }

        [HttpGet]
        [Route("api/Folders/")]
        public IEnumerable<Folder> GetAllFolders()
        {
            return objfolder.GetAllFolders2();
        }

        [HttpPost]
        [Route("api/Folder/Create")]
        public Folder Create([FromBody] Folder folder)
        {
            return objfolder.AddFolder(folder);
        }



        [HttpDelete]
        [Route("api/Folder/Delete2/{id}/{parentid?}")]
        public void DeleteFolder(string id,string parentid) { objfolder.DeleteFolder(id,parentid); }

        [HttpGet]
        [Route("api/Folder/Items/{folderid}")]
        public IEnumerable<Item> FolderItems(string folderid)
        {
            return objfolder.GetFolderItems(folderid);
        }

        [HttpPost]
        [Route("api/Items/Create2/")]
        public Item CreateNewItem([FromBody] Item newitem)
        {
            return objfolder.CreateNewItem(newitem);
        }

        [HttpDelete]
        [Route("api/Folder/Item/Delete/{itemid}")]
        public void DeleteItem(string itemid) { objfolder.DeleteItem(itemid); }


        [HttpGet]
        [Route("api/History/")]
        public IEnumerable<HistoryModel> GetHistoryList()
        {
            return objfolder.GetHistoryList();
        }

        [HttpPost]
        [Route("api/History/Add2/")]
        public void HistoryAdd([FromBody] HistoryModel historymodel)
        {
            objfolder.HistoryAdd(historymodel);
        }

        [HttpDelete]
        [Route("api/History/Remove2/")]
        public void RemoveHistory(string historyid) { objfolder.RemoveHistory(historyid); }

        [HttpGet]
        [Route("api/Items/")]
        public IEnumerable<Item> GetAllItems()
        {
            return objfolder.GetAllItems();
        }

        [HttpPost]
        [Route("api/Items/Update2")]
        public Item UpdateItem([FromBody] Item item)
        {
            return objfolder.UpdateItem(item);
        }

        [HttpPost]
        [Route("api/Tag/Create")]
        public ItemTag CreateTag([FromBody] ItemTag tag)
        {
            return objfolder.CreateTag(tag);
        }

        [HttpGet]
        [Route("api/Tag/")]
        public IEnumerable<ItemTag> GetAllTags()
        {
            return objfolder.GetAllTags();
        }

    }
}
