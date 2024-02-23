namespace LSMTreeExample.API.Business.Interfaces
{
    public interface ILSMTreeService
    {
        void Put(int key, string value);       
        string Get(int key);
        void Delete(int key);
    }
}
