using System.Collections;


namespace ZMoney.ViewModels
{
    public abstract class ListBaseClass
    {
        public abstract IEnumerable SetPageData();
        public abstract string GetContent(int id, out string sequence);
        protected void InputCheck(string name, string sequence, out int sequenceInt) 
        {
            if (name.Length > 6)
            {
                throw new ArgumentException("名稱請不要超過6個字。");
            }

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
        /// 情境2：項目總數小於Sequence，代表要插在中間 => 將Sequence鎖定項目之後全數Sequence+1
        /// </summary>
        public abstract void AddSave(string name, string sequenceStr);

        /// <summary>
        /// 判定情況:修改現值;
        /// 情境1：往前移 => endIndex 大於 startIndex => 修改位置之後所有項目位置-1
        /// 情境2：往後移 => endIndex 小於 startIndex => 修改位置之後所有項目位置+1
        /// </summary>
        public abstract void UpdateSave(string name, string sequenceStr, int cacheID);


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

        public abstract void Delete(int id);
    }
}
