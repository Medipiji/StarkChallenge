using StarkBank;
using StarkChallenge.Models;
using System.Collections.Generic;

public interface IInvoiceService
{
    public void CreateInvoices(List<Client> clients);
}