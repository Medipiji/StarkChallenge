using Moq;
using StarkChallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarkChallenge.Tests.Services
{
    public class ClientsServiceTests
    {
        [Fact]
        public void GenerateRandomClients_DeveRetornarListaVazia_QuandoMinMaiorQueMax()
        {
            var service = new ClientsService();

            var result = service.GenerateRandomClients(10, 5);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 5)]
        [InlineData(1, 10)]
        public void GenerateRandomClients_DeveCriarQuantidadeDentroDoRange(int min, int max)
        {
            var service = new ClientsService();

            var result = service.GenerateRandomClients(min, max);

            Assert.InRange(result.Count, min, max);
        }

        [Fact]
        public void GenerateRandomClients_ClientesCriadosDevemSerValidos()
        {
            var service = new ClientsService();

            var result = service.GenerateRandomClients(5, 5);

            Assert.All(result, client =>
            {
                Assert.False(string.IsNullOrWhiteSpace(client.Nome));
                Assert.False(string.IsNullOrWhiteSpace(client.Cpf));
            });
        }
    }
}
