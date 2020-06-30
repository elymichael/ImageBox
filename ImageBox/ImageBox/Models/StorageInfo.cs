using System;
using System.Collections.Generic;
using System.Text;

namespace ImageBox
{
    public class StorageInfo
    {
        public LocalStorageInfo LocalStorage { get; set; }
        public SDStorageInfo SDStorage { get; set; }

        public StorageInfo()
        {
            LocalStorage = new LocalStorageInfo();
            SDStorage = new SDStorageInfo();
        }
    }

    #region StorageBase
    public class LocalStorageInfo: StorageBase
    {

    }

    public class SDStorageInfo: StorageBase
    {
        
    }

    public abstract class StorageBase
    {
        public double TotalSpace { get; set; }
        public double AvailableSpace { get; set; }
        public double FreeSpace { get; set; }
    }
    #endregion

}
