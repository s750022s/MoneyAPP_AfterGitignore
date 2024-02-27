namespace ZMoney.Controls;

/// <summary>
/// �p�������
/// </summary>
public partial class Calculator : ContentView
{
    /// <summary>
    /// �ثe����List
    /// </summary>
    List<string> formulas = new List<string>();

    /// <summary>
    /// ���i�J����List���Ȧs��
    /// </summary>
    string stagingData = "+";

    /// <summary>
    /// �`�M
    /// </summary>
    double total = 0;

    /// <summary>
    /// OK���s�ƥ�
    /// </summary>
    public class OKButtonClickedEventArgs : EventArgs
    {
        public double Total { get; set; }
    }
    public delegate void OKButtonClickedEventHandler(object sender, OKButtonClickedEventArgs e);
    public event OKButtonClickedEventHandler OKButtonClicked;


    /// <summary>
    /// �غc�p���
    /// </summary>
    public Calculator()
	{
		InitializeComponent();
	}

    /// <summary>
    /// �p������s�\��;
    /// ���U�Ʀr=> �s�istagingData;
    /// ���U�p��Ÿ� => �NstagingData��z�s�i����List;
    /// ���Ӱϰ��K�B��ΰh�^;
    /// </summary>
    private void CalculatorClick(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string newValue = button.Text;

        switch (newValue)
        {
            //�Ʀr��
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                if (formulas.Count() == 0)
                {
                    Count_LB.Text += newValue;
                }
                stagingData += newValue;
                break;

            //�p��Ÿ���
            case "+":
            case "-":
            case "x":
            case "��":
            case "=":
                if (stagingData.Length > 1)
                {
                    formulas.Add(stagingData.Substring(0, 1));
                    formulas.Add(stagingData.Substring(1));
                }
                stagingData = newValue;
                break;

            //�M��
            case "C":
                Count_LB.Text = "";
                equal_LB.Text = "";
                formulas.Clear();
                stagingData = "+";
                total = 0;
                break;

            //�h�^�@��A���M��stagingData�A�A�ѫ᩹�e�̧Ǩ��Xformulas�A����L�kformulas.Count�p��3
            case "��":
                if (formulas.Count < 3)
                {
                    throw new ArgumentException("�w�L�k�R���A�Ъ����ϥ�C�M���\��");
                    
                }
                if (stagingData == "")
                {
                    stagingData = formulas[formulas.Count() - 2] + formulas[formulas.Count() - 1];
                    formulas.RemoveAt(formulas.Count() - 2);
                    formulas.RemoveAt(formulas.Count() - 1);

                }
                stagingData = stagingData.Substring(0, stagingData.Length - 1);
                break;

            //�ˬdtotal�A�óq���եέ�
            case "OK":
                if (total % 1 != 0)
                {
                    throw new ArgumentException("�ƭȫD��ơA�Ф�ʿ�J�̫ᵲ�G");
                }
                else if (total < 0) 
                {
                    OKButtonClicked?.Invoke(this, new OKButtonClickedEventArgs { Total = total*-1 });
                }
                else
                {
                    OKButtonClicked?.Invoke(this, new OKButtonClickedEventArgs { Total = total });
                }
                break;
        }

        //�C���ܧ󳣭p���`�M
        if (formulas.Count > 1)
        {
            total = 0;
            for (int i = 0; i < formulas.Count; i += 2)
            {
                double result = Calculate(total, formulas[i], Convert.ToDouble(formulas[i + 1]));
                total = result;
            }
        }

        //�ˬdformulas�C�Ӥ����n���n�[�d���
        string shows = "";
        for (int i = 1; i < formulas.Count; i++)
        {
            //������
            if (formulas[i].Length > 3)
            {
                shows += ThousandSeparator(formulas[i]);

            }
            else
            {
                shows += formulas[i];
            }

            Count_LB.Text = shows + stagingData;

            //�`�M��
            if (total > 999 || total < -999)
            {
                equal_LB.Text = "=" + ThousandSeparator(total.ToString());
            }
            else 
            {
                equal_LB.Text = "=" + total.ToString();
            }
            
            //���׶W�X�e�����X����
            if (Count_LB.Measure(Width, Height).Request.Width > Count_Border.WidthRequest)
            {
                throw new ArgumentException("���׶W�L��ܽd�򳡤������");
            }
        }
    }

    /// <summary>
    /// �p���Model
    /// </summary>
    public class Operator
    {
        public Func<double, double, double> Operation { get; set; }
    }


    /// <summary>
    /// �p���Dictionary
    /// </summary>
    Dictionary<string, Operator> getOperatorDict = new Dictionary<string, Operator>()
        {
            {"+",new Operator { Operation = (a, b) => a + b }},
            {"-",new Operator { Operation = (a, b) => a - b }},
            {"x",new Operator { Operation = (a, b) => a * b }},
            {"��",new Operator { Operation = (a, b) => a / b }}
        };

    /// <summary>
    /// �p��
    /// </summary>
    /// <param name="operand1">�Q�[(���)��</param>
    /// <param name="operatorSymbol">�p���</param>
    /// <param name="operand2">�[(���)��</param>
    /// <returns>�p�⵲�G</returns>
    private double Calculate(double operand1, string operatorSymbol, double operand2)
    {
        Operator newOperator = getOperatorDict[operatorSymbol];
        return newOperator.Operation(operand1, operand2);
    }

    /// <summary>
    /// �[�W�d���
    /// </summary>
    private string ThousandSeparator(string number)
    {
        bool isDouble = false;
        string decimalPart = "";
        if (number.Contains(".") == true)
        {
            string[] parts = number.Split('.');
            number = parts[0];
            isDouble = true;
            decimalPart = parts[1];
        }
        int start = number.Length % 3;
        if (start == 0)
        {
            start = 3;
        }
        string show = number;
        for (int i = start; i < number.Length+1; i += 4)
        {
            string newShow = show.Insert(i, ",");
            show = newShow;
        }
        if (isDouble == true)
        {
            show += ".";
            show += decimalPart;
        }

        return show;
    }
}