using SQLite;

namespace MoneyAPP.Models
{
    /// <summary>
    /// ������Ƶ��c
    /// </summary>
    [Table("records")]
    public class RecordModel
    {
        /// <summary>
        /// �b��ID�APKey�A�|�۰ʱa�J�����ƪ���
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int RecordID { get; set; }

        /// <summary>
        /// ������J�����
        /// </summary>
	    public DateTime RecordDay { get; set; }

        /// <summary>
        /// ������J���ɶ�(HH:mm)
        /// </summary>
	    public TimeSpan RecordTime { get; set; }

        /// <summary>
        /// �O�_����X�A�p��ɻ�*-1
        /// </summary>
	    public bool IsExpenses { get; set; }

        /// <summary>
        /// �b��ID�A�ΥH�d��AccountModel
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// ���OID�A�ΥH�d��AccountModel
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [MaxLength(30)]
        public string? Item { get; set; }

        /// <summary>
        /// �������B
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// �����s�W�ɶ�
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// �O�_�w�R��
        /// </summary>
        public bool IsDelete { get; set; }
    }
}