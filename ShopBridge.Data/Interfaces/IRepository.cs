using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Data.Interfaces
{
    public interface IRepository
    {
        void Save();
        Task SaveAsync();
    }
}
