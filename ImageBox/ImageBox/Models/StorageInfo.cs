using System;
using System.Collections.Generic;
using System.Text;

namespace ImageBox
{
    public class StorageInfo
    {
        public LocalStorageInfo localStorage { get; set; }
        public SDStorageInfo sDStorage { get; set; }

        public StorageInfo()
        {
            localStorage = new LocalStorageInfo();
            sDStorage = new SDStorageInfo();
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
