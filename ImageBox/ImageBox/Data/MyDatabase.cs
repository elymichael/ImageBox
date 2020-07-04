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
            await _myDatabaseConnection.InsertAsync(item);
        }

        public async Task<ImageInfo> Get(string name)
        {
            return await _myDatabaseConnection
                .Table<ImageInfo>().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
