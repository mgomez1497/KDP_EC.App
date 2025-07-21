using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Database
{
    public class SqLiteConnection
    {
        protected static SQLiteAsyncConnection _db;

        protected async Task InitAsync()
        {
            if (_db != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "KDP_EC.db3");
            _db = new SQLiteAsyncConnection(dbPath);
            
        }
        
    }
}
