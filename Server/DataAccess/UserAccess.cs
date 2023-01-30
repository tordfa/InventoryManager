using BlazorApp1.Shared.Models;
using MongoDB.Driver;

namespace BlazorApp1.Server.DataAccess

{
    public class UserAccess
    {
        private DBContext db = new DBContext();
        public UserCredentialsModel GetUserByUserName(string username)
        {
            FilterDefinition<UserCredentialsModel> filter = Builders<UserCredentialsModel>.Filter.Eq("UserName", username);
            var usercredentials = db.UserCollection.Find(filter).FirstOrDefault();
            return usercredentials;
        }

        public List<Folder> GetFoldersByUserName(string username)
        {
            var user = GetUserByUserName(username);
            FilterDefinition<AccountModel> filter = Builders<AccountModel>.Filter.Eq("Id", user.InventoryId);
            var account = db.AccountCollection.Find(filter).FirstOrDefault();
            return account.folders;
        }

        public List<Item> GetItemsByUserName(string username)
        {
            var user = GetUserByUserName(username);
            FilterDefinition<AccountModel> filter = Builders<AccountModel>.Filter.Eq("Id", user.InventoryId);
            var account = db.AccountCollection.Find(filter).FirstOrDefault();
            return account.items;
        }

        public Folder CreateNewFolder(Folder folder, string username)
        {
            try
            {
                var inventoryid = GetInventoryIdByUsername(username);
                var filter = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                var update = Builders<AccountModel>.Update.Push("folders", folder);
                db.AccountCollection.UpdateOne(filter, update);
                // if parentfolder then add child to parent
                if (folder.ParentFolderId != "")
                {
                    var filter2 = Builders<AccountModel>.Filter.Eq(x => x.Id, inventoryid)
                                & Builders<AccountModel>.Filter.ElemMatch(x => x.folders, Builders<Folder>.Filter.Eq(x => x.Id, folder.ParentFolderId));
                    var update2 = Builders<AccountModel>.Update.Push(x => x.folders[-1].ChildrenFolders, folder.Id);
                    db.AccountCollection.UpdateOne(filter2, update2);

                }
                return folder;

            }
            catch { throw; }
        }

        public Item CreateNewItem(Item item, string username)
        {
            try
            {
                //Insert
                var inventoryid = GetInventoryIdByUsername(username);
                var filter = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                var update = Builders<AccountModel>.Update.Push("items", item);
                db.AccountCollection.UpdateOne(filter, update);
                // Add itemid to parentfolder:
                var filter2 = Builders<AccountModel>.Filter.Eq(x => x.Id, inventoryid)
                            & Builders<AccountModel>.Filter.ElemMatch(x => x.folders, Builders<Folder>.Filter.Eq(x => x.Id, item.ParentFolder));
                var update2 = Builders<AccountModel>.Update.Push(x => x.folders[-1].Items, item.Id);
                db.AccountCollection.UpdateOne(filter2, update2);

                return item;
            }
            catch { throw; }
        }

        public void DeleteFolder(Folder folder, string username)
        {
            try
            {
                var inventoryid = GetInventoryIdByUsername(username);
                //Delete id from parent folder
                if (folder.ParentFolderId != "")
                {
                    var filter2 = Builders<AccountModel>.Filter.Eq(x => x.Id, inventoryid)
                                & Builders<AccountModel>.Filter.ElemMatch(x => x.folders, Builders<Folder>.Filter.Eq(x => x.Id, folder.ParentFolderId)); ;
                    var update2 = Builders<AccountModel>.Update.Pull(x => x.folders[-1].ChildrenFolders, folder.Id);
                    db.AccountCollection.UpdateOne(filter2, update2);

                }
                DeleteAllChildren(folder.Id, inventoryid);

                //Delete folder from folders
                var filter1 = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                var update1 = Builders<AccountModel>.Update.PullFilter("folders", Builders<Folder>.Filter.Eq(x => x.Id, folder.Id));
                db.AccountCollection.UpdateOne(filter1, update1);
            }
            catch { throw; }
        }

        // Summary:
        // Deletes all nested folders and items recursively.
        //
        private void DeleteAllChildren(string folderid, string inventoryid)
        {
            var folder = GetFolderById(folderid, inventoryid);
            if (folder.ChildrenFolders.Count > 0)
            {
                foreach (string childfolderid in folder.ChildrenFolders)
                {

                    DeleteAllChildren(childfolderid, inventoryid);
                    var filter1 = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                    var update1 = Builders<AccountModel>.Update.PullFilter("folders", Builders<Folder>.Filter.Eq(x => x.Id, childfolderid));
                    db.AccountCollection.UpdateOne(filter1, update1);

                }
            }
            if (folder.Items.Count > 0)
            {
                foreach (string itemid in folder.Items)
                {
                    var filter1 = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                    var update1 = Builders<AccountModel>.Update.PullFilter("items", Builders<Item>.Filter.Eq(x => x.Id, itemid));
                    db.AccountCollection.UpdateOne(filter1, update1);
                }
            }

        }

        public void DeleteItem(Item item, string username)
        {
            var inventoryid = GetInventoryIdByUsername(username);
            // Delete item from parentfolder:
            var filter = Builders<AccountModel>.Filter.Eq(x => x.Id, inventoryid)
            & Builders<AccountModel>.Filter.ElemMatch(x => x.folders, Builders<Folder>.Filter.Eq(x => x.Id, item.ParentFolder)); ;
            var update = Builders<AccountModel>.Update.Pull(x => x.folders[-1].Items, item.Id);
            db.AccountCollection.UpdateOne(filter, update);
            //Delete from Item collection:
            var filter2 = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
            var update2 = Builders<AccountModel>.Update.PullFilter("items", Builders<Item>.Filter.Eq(x => x.Id, item.Id));
            db.AccountCollection.UpdateOne(filter2, update2);

        }

        public Item UpdateItem(Item item, string username)
        {
            var inventoryid = GetInventoryIdByUsername(username);
            var filter = Builders<AccountModel>.Filter.Eq(x => x.Id, inventoryid)
            & Builders<AccountModel>.Filter.ElemMatch(x => x.items, Builders<Item>.Filter.Eq(x => x.Id, item.Id));
            var update = Builders<AccountModel>.Update.Set(x => x.items[-1], item);
            db.AccountCollection.UpdateOne(filter, update);
            return item;
        }


        public List<ItemTag> GetAllTags(string username)
        {
            try
            {
                var user = GetUserByUserName(username);
                FilterDefinition<AccountModel> filter = Builders<AccountModel>.Filter.Eq("Id", user.InventoryId);
                var account = db.AccountCollection.Find(filter).FirstOrDefault();
                return account.tags;
            }
            catch { throw; }
        }

        public ItemTag CreateTag(ItemTag tag, string username)
        {
            try
            {
                //Insert
                var inventoryid = GetInventoryIdByUsername(username);
                var filter = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                var update = Builders<AccountModel>.Update.Push("tags", tag);
                db.AccountCollection.UpdateOne(filter, update);

                return tag;
            }
            catch { throw; }
        }

        public void DeleteTag(ItemTag tag, string username)
        {
            try
            {
                var inventoryid = GetInventoryIdByUsername(username);
                //Delete tag from tags
                var filter1 = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                var update1 = Builders<AccountModel>.Update.PullFilter("tags", Builders<ItemTag>.Filter.Eq(x => x.Id, tag.Id));
                db.AccountCollection.UpdateOne(filter1, update1);

                //Delete tag from items
                // DOES NOT WORK!
                var filter2 = Builders<AccountModel>.Filter.Eq(x => x.Id, inventoryid)
                            & Builders<AccountModel>.Filter.ElemMatch(x => x.items, x => x.Tags.Contains(tag));
                var update2 = Builders<AccountModel>.Update.PullFilter(x => x.items[-1].Tags, Builders<ItemTag>.Filter.Eq(x => x.Id, tag.Id));
                db.AccountCollection.UpdateMany(filter2, update2);
            }
            catch { throw; }
        }

        // REFACTOR TO ONLY GET 1 Folder from db.
        public Folder GetFolderById(string folderid, string inventoryid)
        {
            FilterDefinition<AccountModel> filter = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
            var account = db.AccountCollection.Find(filter).FirstOrDefault();
            return account.folders.Find(x => x.Id == folderid);
        }

        private string GetInventoryIdByUsername(string username)
        {
            var user = GetUserByUserName(username);
            return user.InventoryId;
        }

        public List<HistoryModel> GetHistoryList(string username)
        {
            try
            {
                var user = GetUserByUserName(username);
                FilterDefinition<AccountModel> filter = Builders<AccountModel>.Filter.Eq("Id", user.InventoryId);
                var account = db.AccountCollection.Find(filter).FirstOrDefault();
                return account.history;
            }
            catch { throw; }
        }

        public void HistoryAdd(HistoryModel historymodel, string username)
        {
            try
            {
                //Insert
                var inventoryid = GetInventoryIdByUsername(username);
                var filter = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                var update = Builders<AccountModel>.Update.Push("history", historymodel);
                db.AccountCollection.UpdateOne(filter, update);
            }
            catch { throw; }
        }

        public void RemoveHistory(string historyid, string username)
        {
            try
            {
                var inventoryid = GetInventoryIdByUsername(username);
                //Delete from Item collection:
                var filter2 = Builders<AccountModel>.Filter.Eq("Id", inventoryid);
                var update2 = Builders<AccountModel>.Update.PullFilter("history", Builders<Item>.Filter.Eq(x => x.Id, historyid));
                db.AccountCollection.UpdateOne(filter2, update2);
            }
            catch { throw; }


        }




    }
}
