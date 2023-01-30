using BlazorApp1.Shared.Models;

namespace BlazorApp1.Client.Services
{
    public interface IAppStateService
    {
        List<Folder> ChildFolders { get; set;}
        List<Item> Items { get; set; }
        Item ActiveItem { get; set; }
        Folder ActiveFolder { get; set; }
        string FilePath { get; set; }

        List<Folder> AllFolders { get; set; } 
        List<Item> AllItems { get; set; }

        Dictionary<string, bool> openFolders { get; set; }

        bool showItem { get; set; }
        bool showFolder { get; set; }

        event Action OnchangeFolder;
        event Action OnchangeItem;
        event Action OnChangePath;

        public Task GetAllItems();
        public Task GetAllFolders();

        public void AddRootFolder();

        public void AddFolder(Folder parentfolder,string foldername);
        public void AddChildItem(Folder folder);
        public void DeleteFolder(Folder folder);
        public void DeleteItem(Item item);

        public List<Folder> GetChildFolders(string parentid);
        public List<Item> GetChildItems(string parentid);
        void SetActiveFolder(Folder folder);
        void SetActiveItem(Item item,string path);


        string GetFolderPath(Folder folder);

        string GetItemPath(Item item);

        Task<ItemTag> CreateTag(ItemTag tag);

        Task DeleteTag(ItemTag tag);

        Task<List<ItemTag>> GetAllTags();

        Task<Item> UpdateItem(Item item);

        Task<List<HistoryModel>> GetHistoryList();
         Task HistoryAdd(string history, string path);


    }
}