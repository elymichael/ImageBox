namespace ImageBox
{
    using SQLite; 
    using System.Collections.Generic;    
    using System.Threading.Tasks;

    public class MyDatabase
    {
        private SQLiteAsyncConnection _myDatabaseConnection;

        public MyDatabase(string path)
        {
            _myDatabaseConnection = new SQLiteAsyncConnection(path);

            _myDatabaseConnection.CreateTableAsync<ImageInfo>();
            _myDatabaseConnection.CreateTableAsync<FolderInfo>();
        }
        public async Task AddImageItem(ImageInfo item)
        {
            if (_myDatabaseConnection.Table<ImageInfo>() == null)
            {
                await _myDatabaseConnection.CreateTableAsync<ImageInfo>();
            }
            await _myDatabaseConnection.InsertAsync(item);
        }
        public async Task DeleteImageItem(ImageInfo item)
        {
            if (_myDatabaseConnection.Table<ImageInfo>() != null)
            {
                ImageInfo iiToDelete = await _myDatabaseConnection
                  .Table<ImageInfo>().FirstOrDefaultAsync(x => x.Name == item.Name);

                await _myDatabaseConnection.DeleteAsync(iiToDelete);
            }
        }
        public async Task<ImageInfo> GetImage(string name)
        {
            if (_myDatabaseConnection.Table<ImageInfo>() != null)
            {
                return await _myDatabaseConnection
                .Table<ImageInfo>().FirstOrDefaultAsync(x => x.Name == name);
            }
            return null;
        }
        public async Task AddFolderItem(string folderName)
        {
            FolderInfo item = new FolderInfo { Name = folderName };

            if (_myDatabaseConnection.Table<FolderInfo>() == null)
            {
                await _myDatabaseConnection.CreateTableAsync<FolderInfo>();
            }

            FolderInfo fi = await _myDatabaseConnection.Table<FolderInfo>()
                .FirstOrDefaultAsync(x => x.Name == folderName);

            if (fi == null)
            {
                await _myDatabaseConnection.InsertAsync(item);
            }
        }
        public async Task<List<FolderInfo>> GetFolders()
        {
            if (_myDatabaseConnection.Table<FolderInfo>() == null)
            {
                await _myDatabaseConnection.CreateTableAsync<FolderInfo>();
            }

            return await _myDatabaseConnection.Table<FolderInfo>()
                .OrderByDescending(x => x.Name)
                .ToListAsync(); ;
        }
    }
}
