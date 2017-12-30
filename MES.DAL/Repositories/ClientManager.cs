using MES.DAL.Entities;
using MES.DAL.Interfaces;

namespace MES.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public IMesContext Context { get; set; }

        public ClientManager(IMesContext context)
        {
            Context = context;
        }

        public void Create(ClientProfile item)
        {
            Context.ClientProfiles.Add(item);
        }
    }
}
