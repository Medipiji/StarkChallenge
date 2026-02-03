using Bogus;
using Bogus.Extensions.Brazil;
using StarkChallenge.Interfaces.IServices;
using StarkChallenge.Models;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace StarkChallenge.Services
{
    public class ClientsService : IClientsService
    {
        public List<Client> GenerateRandomClients(int minClient, int maxClient)
        {
            List<Client> listclient = new List<Client> ();
            if (minClient > maxClient) return listclient;


            int numberOfclients = GenerateNumberClients(minClient, maxClient);

            if(numberOfclients > 0)
            {
                for (int i = 1; i <= numberOfclients; i++)
                {
                    listclient.Add(GenerateClientInfo());
                }
            }

            return listclient;
        }
        protected int GenerateNumberClients(int minClient, int maxClient)
        {
            return Random.Shared.Next(minClient, maxClient + 1);
        }
        protected Client GenerateClientInfo()
        {
            var mock = new Faker("pt_BR");
            return new Client
            {
                Cpf = mock.Person.Cpf(),
                Nome = mock.Name.FullName(),
                Valor = Convert.ToUInt32(mock.Finance.Amount(100, 10000))
            };
        }
    }
}
