using StarkChallenge.Models;

namespace StarkChallenge.Interfaces.IServices
{
    public interface IClientsService
    {
        public List<Client> GenerateRandomClients(int minClient, int maxClient);
    }
}
