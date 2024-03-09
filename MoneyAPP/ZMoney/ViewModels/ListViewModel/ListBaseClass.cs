using System.Collections;

namespace ZMoney.ViewModels
{
    /// <summary>
    /// 定義種類/帳戶切換器的抽象父類別。
    /// 用途:ListSettingPage選單編輯器的邏輯切換。
    /// </summary>
    public abstract class ListBaseClass
    {
        /// <summary>
        /// 取得資料集合。
        /// </summary>
        /// <returns>可能是CategoryModel或AccountModel的集合</returns>
        public abstract IEnumerable SetPageData();

        /// <summary>
        /// 取得指定Id的名稱及順序。
        /// </summary>
        /// <param name="id">要查詢的id</param>
        /// <param name="sequence">out順序</param>
        /// <returns>名稱</returns>
        public abstract string GetContent(int id, out string sequence);

        /// <summary>
        /// 檢查輸入是否符合標準。
        /// </summary>
        /// <param name="name">輸入的名稱</param>
        /// <param name="sequence">輸入的順序</param>
        /// <param name="sequenceInt">out Int格式的順序</param>
        /// <exception cref="ArgumentException"></exception>
        protected void InputCheck(string name, string sequence, out int sequenceInt)
        {
            //名稱
            if (name.Length > 6)
            {
                throw new ArgumentException("名稱請不要超過6個字。");
            }

            //順序
            sequence = sequence == "" ? "100" : sequence;
            if (!int.TryParse(sequence.Replace(",", ""), out sequenceInt))
            {
                throw new ArgumentException("請輸入一位或兩位正整數。");
            }
            else if (sequenceInt < 0 || sequenceInt > 99)
            {
                throw new ArgumentException("請輸入一位或兩位正整數。");
            }
        }

        /// <summary>
        /// 判定情況:id=999，新增項目；
        /// 情境1：Sequence大於目前項目總數 => 加在最後，Sequence=項目總數;
        /// 情境2：項目總數小於Sequence，代表要插在中間 => 將Sequence鎖定項目之後全數Sequence+1。
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="sequenceStr">順序str</param>
        public abstract void AddSave(string name, string sequenceStr);

        /// <summary>
        /// 判定情況:修改現值;
        /// 情境1：往前移 => endIndex 大於 startIndex => 修改位置之後所有項目位置-1
        /// 情境2：往後移 => endIndex 小於 startIndex => 修改位置之後所有項目位置+1
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="sequenceStr">順序str</param>
        /// <param name="cacheID">修改項目Id</param>
        public abstract void UpdateSave(string name, string sequenceStr, int cacheID);

        /// <summary>
        /// 修改判定規則，在起始到終點位置的項目都需要被修改。
        /// </summary>
        /// <param name="startIndex">修改的起始位置</param>
        /// <param name="endIndex">修改的終點位置</param>
        /// <param name="sequence">修改項目順序</param>
        /// <returns></returns>
        public virtual int UpdateSaveRule(int startIndex, int endIndex, int sequence)
        {
            if (
                (endIndex > startIndex && sequence <= endIndex && sequence > startIndex) ||
                (endIndex < startIndex && sequence >= endIndex && sequence < startIndex)
                )
            {
                sequence += endIndex > startIndex ? -1 : 1;
                return sequence;
            }
            return sequence;

        }

        /// <summary>
        /// 刪除項目。
        /// </summary>
        /// <param name="id">刪除Id</param>
        public abstract void Delete(int id);
    }
}
