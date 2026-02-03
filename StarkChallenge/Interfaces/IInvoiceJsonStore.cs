namespace StarkChallenge.Interfaces
{
    public interface IInvoiceJsonStore
    {
        public bool AlreadyTransferred(string invoiceId);
        public void MarkAsTransferred(string invoiceId);
    }
}
