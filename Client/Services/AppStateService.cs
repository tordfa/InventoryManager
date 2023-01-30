using BlazorApp1.Shared.Models;
using BlazorApp1.Client.Services;

namespace BlazorApp1.Client.Services
{
    public class AppStateService : IAppStateService
    {
        private readonly IDataClient _clientController;
        public List<Folder> AllFolders { get; set; } = new List<Folder>();
        public List<Item> AllItems { get; set; } = new List<Item>();
        public Item ActiveItem { get; set; } = new Item();
        public Folder ActiveFolder { get; set; } = new Folder();
        public List<Folder> ChildFolders { get; set; } = new List<Folder>();
        public List<Item> Items { get; set; } = new List<Item>();
        public Dictionary<string, bool> openFolders { get; set; } = new Dictionary<string, bool>();
        public string FilePath { get; set; }
        public bool showItem { get; set; }
        public bool showFolder { get; set; }

        public event Action OnchangeFolder;
        public event Action OnchangeItem;
        public event Action OnChangePath;

        public AppStateService(IDataClient clientController)
        {
            _clientController = clientController;

        }

        public async Task GetAllItems()
        {
            var items = await _clientController.GetAllItems();
            AllItems = items;
        }
        public async Task GetAllFolders()
        {
            var folders = await _clientController.GetAllFolders();
            AllFolders = folders;
        }
        public List<Folder> GetChildFolders(string parentid)
        {
            List<Folder> childfolders = new List<Folder>();
            foreach (var folder in AllFolders)
            {
                if (folder.ParentFolderId == parentid)
                {
                    childfolders.Add(folder);
                }
            }
            return childfolders;
        }
        public List<Item> GetChildItems(string parentid)
        {
            List<Item> childitems = new List<Item>();
            foreach (var item in AllItems)
            {
                if (item.ParentFolder == parentid)
                {
                    childitems.Add(item);
                }
            }
            return childitems;
        }

        public async void AddRootFolder() 
        {
                Folder newFolder = new Folder();
                newFolder.Name = "ParentFolder";
                newFolder.ParentFolderId = "";
                newFolder = await _clientController.createNewRootFolder(newFolder);
                AllFolders.Add(newFolder);
                HistoryAdd("Added folder: " + newFolder.Name + " to ", "Root");
                NotifyStateChangedSetFolder();
            
        }
        public async void AddFolder(Folder folder, string foldername)
        {
            var newfolder = await _clientController.addChildFolder(folder,foldername);
            AllFolders.Add(newfolder);
            var parentfolder = AllFolders.Find(x => x.Id == folder.Id);
            parentfolder.ChildrenFolders.Add(newfolder.Id);
            var path = GetFolderPath(newfolder);
            HistoryAdd("Added folder: " + newfolder.Name + " to ", path);
            SetActiveFolder(newfolder);
            NotifyStateChangedSetFolder();
        }
        public async void DeleteFolder(Folder folder)
        {
            await _clientController.DeleteFolder(folder);
            var path = GetFolderPath(folder);
            HistoryAdd("Deleted folder: " + folder.Name + " from ", path);
            AllFolders.Remove(folder);
            // Delete from parentfolder
            if (folder.ParentFolderId != "")
            {
                var parentfolder = AllFolders.Find(x => x.Id == folder.ParentFolderId);
                parentfolder.ChildrenFolders.Remove(folder.Id);
                SetActiveFolder(parentfolder);
            }
            //Delete all children folders and items
            DeleteAllchildren(folder);
            NotifyStateChangedSetFolder();
        }
        public async void AddChildItem(Folder folder)
        {
            var newitem = await _clientController.createNewItem(folder);
            AllItems.Add(newitem);
            var parentfolder = AllFolders.Find(x => x.Id == folder.Id);
            parentfolder.Items.Add(newitem.Id);
            var path = GetItemPath(newitem);
            HistoryAdd("Added Item: " + newitem.Name + " to ", path);
            SetActiveItem(newitem, path);

        }
        private void DeleteAllchildren(Folder folder)
        {
            if (folder.ChildrenFolders.Count > 0)
            {
                foreach (var childfolderid in folder.ChildrenFolders)
                {
                    var childfolder = AllFolders.Find(x => x.Id == childfolderid);
                    DeleteAllchildren(childfolder);
                    AllFolders.Remove(childfolder);
                }  
            }
            if (folder.Items.Count > 0)
            {
                foreach (var childitemid in folder.Items)
                {
                    var childitem = AllItems.Find(x => x.Id == childitemid);
                    AllItems.Remove(childitem);
                }
            }
        }
        public async void DeleteItem(Item item)
        {
            await _clientController.DeleteItem(item);
            var path = GetItemPath(item);
            HistoryAdd("Deleted Item: " + item.Name + " from ", path);
            AllItems.Remove(item);
            var parentfolder = AllFolders.Find(x => x.Id == item.ParentFolder);
            parentfolder.Items.Remove(item.Id);
            NotifyStateChangedSetItem();
            SetActiveFolder(parentfolder);
        }
        public async Task<Item> UpdateItem(Item item)
        {
            var olditem = AllItems.Find(x => x.Id == item.Id);
            olditem = item;
            var path = GetItemPath(item);
            HistoryAdd("Updated Item: " + item.Name + " from ", path);
            NotifyStateChangedSetItem();
            return await _clientController.UpdateItem(item);
        }

        public async Task<ItemTag> CreateTag(ItemTag tag)
        {
            return await _clientController.CreateTag(tag);

        }
        public async Task<List<ItemTag>> GetAllTags()
        {
            return await _clientController.GetAllTags();
        }

        public async Task HistoryAdd(string history, string path)
        {
            await _clientController.HistoryAdd(history, path);
        }
        public void SetActiveItem(Item item, string path)
        {
            ActiveFolder = null ;
            ActiveItem = item;
            FilePath = path;
            NotifyStateChangedSetItem();
        }
        public void SetActiveFolder(Folder folder)
        {
            Console.WriteLine(GetFolderPath(folder));
            FilePath = GetFolderPath(folder);
            ActiveFolder = folder;
            ActiveItem = null;
            NotifyStateChangedSetPath();
            Items = GetChildItems(folder.Id);
            ChildFolders = GetChildFolders(folder.Id);
            NotifyStateChangedSetFolder();
        }

        public string GetFolderPath(Folder folder)
        {
            string path = folder.Name ;
            if (folder.ParentFolderId != "")
            {
                var parentfolder = AllFolders.Find(x => x.Id == folder.ParentFolderId);
                path = GetFolderPath(parentfolder) +" > "+ path;
            }
            return path;

        }
        public string GetItemPath(Item item)
        {
            var parentfolder = AllFolders.Find(x => x.Id == item.ParentFolder);
            return GetFolderPath(parentfolder) + " > " + item.Name;
        }

        private void NotifyStateChangedSetFolder()
        {
            OnchangeFolder?.Invoke();

        }
        private void NotifyStateChangedSetItem()
        {
            OnchangeItem?.Invoke();
        }
        private void NotifyStateChangedSetPath()
        {
            OnChangePath?.Invoke();
        }

        public async Task<List<HistoryModel>> GetHistoryList()
        {
            return await _clientController.GetHistoryList();
        }

        public Task DeleteTag(ItemTag tag)
        {
           return _clientController.DeleteTag(tag);
        }
    }
}
