using Moq;
using StarkChallenge.Models;
using StarkChallenge.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarkChallenge.Tests.Process

{
    public class InvoiceProcessTests
    {
        [Fact]
        public void ProcessInvoices_DeveChamarService_ComClientesValidos()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client
                {
                    Nome = "João",
                    Cpf = "12345678900",
                    Valor = 1000
                }
            };

            var invoiceServiceMock = new Mock<IInvoiceService>();
            var processor = new InvoiceProcessor(invoiceServiceMock.Object);

            // Act
            processor.ProcessInvoices(clients);

            // Assert
            invoiceServiceMock.Verify(
                x => x.CreateInvoices(It.Is<List<Client>>(c =>
                    c.Count == 1 &&
                    c[0].Nome == "João" &&
                    c[0].Valor == 1000
                )),
                Times.Once
            );
        }

        [Fact]
        public void ProcessInvoices_NaoDeveCriarInvoices_QuandoListaVazia()
        {
            // Arrange
            var invoiceServiceMock = new Mock<IInvoiceService>();
            var processor = new InvoiceProcessor(invoiceServiceMock.Object);

            // Act
            processor.ProcessInvoices(new List<Client>());

            // Assert
            invoiceServiceMock.Verify(
                x => x.CreateInvoices(It.IsAny<List<Client>>()),
                Times.Never
            );
        }
    }
}
