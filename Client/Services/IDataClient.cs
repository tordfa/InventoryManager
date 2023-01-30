namespace BlazorApp1.Client.Services

{
    using BlazorApp1.Shared.Models;

    public interface IDataClient
    {

        Task<List<Folder>> GetAllFolders();
        Task<List<Folder>> GetRootFolders();
        Task<Folder> createNewRootFolder(Folder folder);
        Task<List<Folder>> getChildrenFolders(string parentid);
        Task<List<Item>> getItemsFromFolder(string folderid);

        Task<List<Item>> GetAllItems();

        Task<Folder> addChildFolder(Folder parentFolder,string foldername);

        Task<Item> createNewItem(Folder parentfolder);

        Task<Item> UpdateItem(Item item);

        Task DeleteItem(Item item);

        Task DeleteFolder(Folder folder);

        Task<List<HistoryModel>> GetHistoryList();

        Task HistoryAdd(string history, string path);

        Task RemoveHistory(string id);

        Task<ItemTag> CreateTag(ItemTag tag);

        Task<List<ItemTag>> GetAllTags();
        Task DeleteTag(ItemTag tag);  


    }
}
