using StarkBank;
using StarkChallenge.Interfaces;
using StarkChallenge.Models;
using System.Collections.Generic;

public class InvoiceService : IInvoiceService
{
    public void CreateInvoices(List<Client> clients)
    {
        var invoices = clients.Select(client =>
          new Invoice(
              amount: client.Valor,
              name: client.Nome,
              taxID: client.Cpf
          )
        ).ToList();

        Invoice.Create(invoices);
    }
}