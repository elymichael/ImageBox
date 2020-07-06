namespace ImageBox
{
    using SQLite;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class MyDatabase
    {
        private SQLiteAsyncConnection _myDatabaseConnection;

        public MyDatabase(string path)
        {
            _myDatabaseConnection = new SQLiteAsyncConnection(path);

            _myDatabaseConnection.CreateTableAsync<ImageInfo>();
        }

        public async Task AddItem(ImageInfo item)
        {
            if(_myDatabaseConnection.Table<ImageInfo>() == null)
            {
                await _myDatabaseConnection.CreateTableAsync<ImageInfo>();
            }
            await _myDatabaseConnection.InsertAsync(item);
        }

        public async Task DeleteItem(ImageInfo item)
        {
            if (_myDatabaseConnection.Table<ImageInfo>() != null)
            {
              ImageInfo iiToDelete = await _myDatabaseConnection
                .Table<ImageInfo>().FirstOrDefaultAsync(x => x.Name == item.Name);

                await _myDatabaseConnection.DeleteAsync(iiToDelete);
            }            
        }
        public async Task<ImageInfo> Get(string name)
        {
            if (_myDatabaseConnection.Table<ImageInfo>() != null)
            {
                return await _myDatabaseConnection
                .Table<ImageInfo>().FirstOrDefaultAsync(x => x.Name == name);
            }
            return null;
        }
    }
}
