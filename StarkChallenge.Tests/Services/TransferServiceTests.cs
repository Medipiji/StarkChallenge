using Xunit;
using Moq;
using StarkChallenge.Services;
using StarkChallenge.Interfaces;
using StarkChallenge.Models;
using StarkChallenge.DTOs;
using Microsoft.Extensions.Configuration;
using StarkChallenge.Interfaces.IRepositorys;

namespace StarkChallenge.Tests.Services
{
    public class TransferServiceTests
    {
        [Fact]
        public async Task Nao_transferir_invoice_ja_processada()
        {
            // ARRANGE
            var storeMock = new Mock<IInvoiceIDRepository>();

            storeMock
                .Setup(x => x.AlreadyExists("5357717103312896"))
                .Returns(true);

            var configuration = BuildValidConfiguration();

            var service = new TransferService(
                configuration,
                storeMock.Object
            );

            var dto = BuildValidInvoiceDto();

            // ACT
            await service.ValidateTransferProcess(dto);

            // ASSERT
            storeMock.Verify(
                x => x.RecordID(It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Nao_transferir_quando_evento_nao_for_credited()
        {
            var storeMock = new Mock<IInvoiceIDRepository>();

            storeMock
               .Setup(x => x.AlreadyExists("5357717103312896"))
               .Returns(false);

            var configuration = BuildValidConfiguration();

            var service = new TransferService(
                configuration,
                storeMock.Object
            );

            var dto = BuildValidInvoiceDto(type:"created");
            dto.Event.Log.Type = "created"; // ou qualquer outro

            await service.ValidateTransferProcess(dto);

            storeMock.Verify(
                x => x.RecordID(It.IsAny<string>()),
                Times.Never
            );
        }

        // Suport functions
        private ResponseInvoiceDTO BuildValidInvoiceDto(string type = "credited", string id = "5357717103312896")
        {
            return new ResponseInvoiceDTO
            {
                Event = new()
                {
                    Log = new()
                    {
                        Type = type,
                        Invoice = new()
                        {
                            Id = id,
                            Amount = 1000
                        }
                    }
                }
            };
        }
        private IConfiguration BuildValidConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["StarkBank:TransferTarget:Name"] = "Teste",
                    ["StarkBank:TransferTarget:TaxID"] = "123",
                    ["StarkBank:TransferTarget:BankCode"] = "000",
                    ["StarkBank:TransferTarget:BranchCode"] = "0001",
                    ["StarkBank:TransferTarget:AccountNumber"] = "12345-6"
                })
                .Build();
        }
    }
}
