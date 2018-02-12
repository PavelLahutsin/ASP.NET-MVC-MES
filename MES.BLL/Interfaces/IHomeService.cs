using System.Threading.Tasks;

namespace MES.BLL.Interfaces
{
    public interface IHomeService
    {
        Task<string> ProductInfo();
    }
}