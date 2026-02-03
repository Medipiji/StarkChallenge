using StarkChallenge.Models;

namespace StarkChallenge.Interfaces.IProcess
{
    public interface IInvoiceProcess
    {
        public void ProcessInvoices(List<Client> clients);
    }
}
