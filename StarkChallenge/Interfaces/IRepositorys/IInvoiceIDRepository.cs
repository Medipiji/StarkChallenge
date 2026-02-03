namespace StarkChallenge.Interfaces.IRepositorys
{
    public interface IInvoiceIDRepository
    {
        public bool RecordID(string value);
        public List<string> ReadIDsList();
        public bool AlreadyExists(string valueID);
        public bool DeleteDocument(string search);
    }
}
