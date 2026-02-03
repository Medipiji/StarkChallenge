using StarkChallenge.DTOs;

namespace StarkChallenge.Interfaces.IServices
{
    public interface ITransferService
    {
        public Task ValidateTransferProcess(ResponseInvoiceDTO p_responseInvoiceDTO);
    }
}
