using StarkBank;
using StarkChallenge.DTOs;
using StarkChallenge.Interfaces;
using StarkChallenge.Interfaces.IRepositorys;
using StarkChallenge.Interfaces.IServices;
using StarkChallenge.Models;

namespace StarkChallenge.Services
{
    public class TransferService : ITransferService
    {
        private readonly IInvoiceIDRepository _invoiceIDRepository;
        private readonly IConfiguration _configuration;

        public TransferService(IConfiguration configuration, IInvoiceIDRepository invoiceIDRepository)
        {
            _configuration = configuration;
            _invoiceIDRepository = invoiceIDRepository;
        }   

        public async Task ValidateTransferProcess(ResponseInvoiceDTO p_responseInvoiceDTO)
        {
            if (p_responseInvoiceDTO.Event.Log.Type == "credited" && !_invoiceIDRepository.AlreadyExists(p_responseInvoiceDTO.Event.Log.Invoice.Id))
            {
                TransferInfo infoObject = _configuration.GetSection("StarkBank").GetSection("TransferTarget").Get<TransferInfo>() ?? new TransferInfo();
                Transfer.Create(
                    new List<Transfer>
                    {
                        new Transfer(
                            amount: p_responseInvoiceDTO.Event.Log.Invoice.Amount,
                            name: infoObject.Name,
                            taxID: infoObject.TaxID,
                            bankCode: infoObject.BankCode,
                            branchCode: infoObject.BranchCode,
                            accountNumber: infoObject.AccountNumber
                        )
                    }
                );
                _invoiceIDRepository.RecordID(p_responseInvoiceDTO.Event.Log.Invoice.Id);
            }
        }
    }
}
