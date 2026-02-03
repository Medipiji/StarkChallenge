using StarkBank;
using StarkChallenge.Interfaces.IProcess;
using StarkChallenge.Models;
using System.Collections.Generic;
using System.Linq;

namespace StarkChallenge.Process
{
    public class InvoiceProcessor : IInvoiceProcess
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceProcessor(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public void ProcessInvoices(List<Client> clients)
        {
            if (clients == null || !clients.Any()) return;
            _invoiceService.CreateInvoices(clients);
        }
    }
}