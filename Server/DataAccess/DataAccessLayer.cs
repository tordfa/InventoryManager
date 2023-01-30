using BlazorApp1.Shared.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BlazorApp1.Server.DataAccess
{
    public class DataAccessLayer
    {
        private DBContext db = new DBContext();
        //To Get all employees details
        public List<Item> GetAllItems()
        {
            try { return db.ItemRecord.Find(_ => true).ToList(); }
            catch { throw; }
        }

        public Item GetItem(string itemid)
        {
            try
            {
                var itemfilter = Builders<Item>.Filter.Eq("Id", itemid);
                return db.ItemRecord.Find(itemfilter).FirstOrDefault();
            }
            catch { throw; }
        }
        //To Add new employee record
        public void AddItem(Item item)
        {
            try { db.ItemRecord.InsertOne(item); }
            catch { throw; }
        }

        public List<Folder> GetAllFolders()
        {
            try
            {
                var filter = Builders<Folder>.Filter.Eq("ParentFolderId", "");
                return db.FolderRecord.Find(filter).ToList();
            }
            catch { throw; }
        }

        public List<Folder> GetAllFolders2()
        {
            try
            {
                return db.FolderRecord.Find(_=> true).ToList();
            }
            catch { throw; }
        }

        public Folder GetFolder(string id)
        {
            try
            {
                FilterDefinition<Folder> folderData = Builders<Folder>.Filter.Eq("Id", id);
                return db.FolderRecord.Find(folderData).FirstOrDefault();
            }
            catch { throw; }
        }

        public IEnumerable<Folder> GetFolderChildren(string id)
        {
            try
            {
                FilterDefinition<Folder> filterParentFolder = Builders<Folder>.Filter.Eq("Id", id);
                List<string> childids = db.FolderRecord.Find(filterParentFolder).FirstOrDefault().ChildrenFolders;
                List<Folder> childFolders = new List<Folder>();
                if (childids.Count > 0)
                {
                    foreach (var childid in childids)
                    {
                        childFolders.Add(GetFolder(childid));
                    }
                }

                return childFolders;

            }
            catch { throw; }
        }

        public Folder AddFolder(Folder folder)
        {
            try
            {
                db.FolderRecord.InsertOne(folder);
                // if parentfolder then add child to parent
                if (folder.ParentFolderId != "")
                {
                    var filter = Builders<Folder>.Filter.Eq("Id", folder.ParentFolderId);
                    var update = Builders<Folder>.Update.Push("ChildrenFolders", folder.Id);
                    db.FolderRecord.UpdateOne(filter, update);
                }
                return folder;

            }
            catch { throw; }
        }



        public void DeleteFolder(string id, string parentid)
        {
            try
            {
                //Delete id from parent folder
                if (parentid != "")
                {
                    var filter2 = Builders<Folder>.Filter.Eq("Id", parentid);
                    var update = Builders<Folder>.Update.Pull("ChildrenFolders", id);
                    db.FolderRecord.UpdateOne(filter2, update);
                }
                DeleteAllChildren(id);
                var filter1 = Builders<Folder>.Filter.Eq("Id", id);
                db.FolderRecord.DeleteOne(filter1);
            }
            catch { throw; }
        }

        private void DeleteAllChildren(string id)
        {
            Folder folder = GetFolder(id);
            if (folder.ChildrenFolders.Count > 0)
            {
                foreach (string childfolderid in folder.ChildrenFolders)
                {
                    DeleteAllChildren(childfolderid);
                    var filter = Builders<Folder>.Filter.Eq("Id", childfolderid);
                    db.FolderRecord.DeleteOne(filter);
                    
                }
            }
            if (folder.Items.Count > 0)
            {
                foreach (string itemid in folder.Items)
                {
                    var itemfilter = Builders<Item>.Filter.Eq("Id", itemid);
                    db.ItemRecord.DeleteOne(itemfilter);
                }
            }
               
        }

        public IEnumerable<Item> GetFolderItems(string folderid)
        {
            try
            {
                FilterDefinition<Folder> filterParentFolder = Builders<Folder>.Filter.Eq("Id", folderid);
                List<string> itemids = db.FolderRecord.Find(filterParentFolder).FirstOrDefault().Items;
                List<Item> items = new List<Item>();
                if (itemids.Count > 0)
                {
                    foreach (var itemid in itemids)
                    {
                        items.Add(GetItem(itemid));
                    }
                }

                return items;

            }
            catch { throw; }
        }

        public Item CreateNewItem(Item item)
        {
            try 
            {

                //Insert
                db.ItemRecord.InsertOne(item);
                // Add itemid to parentfolder:
                var folderfilter = Builders<Folder>.Filter.Eq("Id", item.ParentFolder);
                var update = Builders<Folder>.Update.Push("Items", item.Id);
                db.FolderRecord.UpdateOne(folderfilter, update);

                return item;
            }
            catch { throw; }
        }
        public void DeleteItem(string id)
        {
            var itemfilter = Builders<Item>.Filter.Eq("Id", id);
            var item = db.ItemRecord.Find(itemfilter).FirstOrDefault();
            // Delete item from parentfolder:
            var folderfilter = Builders<Folder>.Filter.Eq("Id", item.ParentFolder);
            var update = Builders<Folder>.Update.Pull("Items", id);
            db.FolderRecord.UpdateOne(folderfilter, update);
            //Delete from Item collection:
            db.ItemRecord.DeleteOne(itemfilter);

        }

        public List<HistoryModel> GetHistoryList()
        {
            try
            {
                return db.HistoryCollection.Find(_=> true).ToList();
            }
            catch { throw; }
        }

        public void HistoryAdd(HistoryModel historymodel)
        {
            try
            {
                db.HistoryCollection.InsertOne(historymodel);
            }
            catch { throw; }
        }

        public void RemoveHistory(string historyid)
        {
            try
            {
                var historyfilter = Builders<HistoryModel>.Filter.Eq("Id", historyid);

                db.HistoryCollection.DeleteOne(historyfilter);
            }
            catch { throw; }


        }

        public ItemTag CreateTag(ItemTag tag) 
        {
            try
            {
                db.TagCollection.InsertOne(tag);
                return tag;
            }
            catch { throw; }
        }

        public List<ItemTag> GetAllTags()
        {
            try
            {
                return db.TagCollection.Find(_ => true).ToList();
            }
            catch { throw; }
        }

        public Item UpdateItem(Item item)
        { 
            var itemfilter = Builders<Item>.Filter.Eq("Id", item.Id);
            return db.ItemRecord.FindOneAndReplace<Item>(itemfilter, item);
        }


    }
}
