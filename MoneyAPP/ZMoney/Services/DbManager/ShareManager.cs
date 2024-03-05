

namespace ZMoney.Services
{
    public partial class DbManager
    {
        //前情提要 private IDbService _dbService;

        public void Close() 
        {
            _dbService.Close();
        }

        public void Open() 
        {
            _dbService.Open();
        }
    }
}
