using SQLite;

namespace ZMoney.Models
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
        public int Id { get; set; }

        /// <summary>
        /// ������J������ήɶ�
        /// </summary>
	    public DateTime RecordDateTime { get; set; }

        /// <summary>
        /// �O�_����X�A�p��ɻ�*-1
        /// </summary>
	    public bool IsExpenses { get; set; }

        /// <summary>
        /// �b��ID�A�ΥH�d��AccountModel
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// ���OID�A�ΥH�d��AccountModel
        /// </summary>
        public int CategoryId { get; set; }

#nullable enable
        /// <summary>
        /// ��������
        /// </summary>
        [MaxLength(30)]
        public string? Description { get; set; }
#nullable disable

        /// <summary>
        /// �������B
        /// </summary>
        public int AmountOfMoney { get; set; }

        /// <summary>
        /// �O�_�w�R��
        /// </summary>
        public bool IsDelete { get; set; }
    }
}