using MoneyAPP.Models;

namespace MoneyAPP.Services
{
    internal interface IRecordService
    {
        List<RecordModel> GetRecords();
        int AddTodo(RecordModel addRecord);
        int UpdateTodo(RecordModel addRecord);
    }
}


