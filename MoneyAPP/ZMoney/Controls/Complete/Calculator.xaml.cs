namespace ZMoney.Controls;

/// <summary>
/// 計算機元件
/// </summary>
public partial class Calculator : ContentView
{
    /// <summary>
    /// 目前公式List
    /// </summary>
    List<string> formulas = new List<string>();

    /// <summary>
    /// 未進入公式List的暫存區
    /// </summary>
    string stagingData = "+";

    /// <summary>
    /// 總和
    /// </summary>
    double total = 0;

    /// <summary>
    /// OK按鈕事件
    /// </summary>
    public class OKButtonClickedEventArgs : EventArgs
    {
        public double Total { get; set; }
    }
    public delegate void OKButtonClickedEventHandler(object sender, OKButtonClickedEventArgs e);
    public event OKButtonClickedEventHandler OKButtonClicked;


    /// <summary>
    /// 建構計算機
    /// </summary>
    public Calculator()
	{
		InitializeComponent();
	}

    /// <summary>
    /// 計算機按鈕功能;
    /// 按下數字=> 存進stagingData;
    /// 按下計算符號 => 將stagingData整理存進公式List;
    /// 拆成兩個區域方便運算及退回;
    /// </summary>
    private void CalculatorClick(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string newValue = button.Text;

        switch (newValue)
        {
            //數字區
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

            //計算符號區
            case "+":
            case "-":
            case "x":
            case "÷":
            case "=":
                if (stagingData.Length > 1)
                {
                    formulas.Add(stagingData.Substring(0, 1));
                    formulas.Add(stagingData.Substring(1));
                }
                stagingData = newValue;
                break;

            //清空
            case "C":
                Count_LB.Text = "";
                equal_LB.Text = "";
                formulas.Clear();
                stagingData = "+";
                total = 0;
                break;

            //退回一格，先清除stagingData，再由後往前依序取出formulas，直到無法formulas.Count小於3
            case "←":
                if (formulas.Count < 3)
                {
                    throw new ArgumentException("已無法刪除，請直接使用C清除功能");
                    
                }
                if (stagingData == "")
                {
                    stagingData = formulas[formulas.Count() - 2] + formulas[formulas.Count() - 1];
                    formulas.RemoveAt(formulas.Count() - 2);
                    formulas.RemoveAt(formulas.Count() - 1);

                }
                stagingData = stagingData.Substring(0, stagingData.Length - 1);
                break;

            //檢查total，並通知調用頁
            case "OK":
                if (total % 1 != 0)
                {
                    throw new ArgumentException("數值非整數，請手動輸入最後結果");
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

        //每次變更都計算總和
        if (formulas.Count > 1)
        {
            total = 0;
            for (int i = 0; i < formulas.Count; i += 2)
            {
                double result = Calculate(total, formulas[i], Convert.ToDouble(formulas[i + 1]));
                total = result;
            }
        }

        //檢查formulas每個元素要不要加千位符
        string shows = "";
        for (int i = 1; i < formulas.Count; i++)
        {
            //公式區
            if (formulas[i].Length > 3)
            {
                shows += ThousandSeparator(formulas[i]);

            }
            else
            {
                shows += formulas[i];
            }

            Count_LB.Text = shows + stagingData;

            //總和區
            if (total > 999 || total < -999)
            {
                equal_LB.Text = "=" + ThousandSeparator(total.ToString());
            }
            else 
            {
                equal_LB.Text = "=" + total.ToString();
            }
            
            //長度超出畫面跳出提醒
            if (Count_LB.Measure(Width, Height).Request.Width > Count_Border.WidthRequest)
            {
                throw new ArgumentException("長度超過顯示範圍部分不顯示");
            }
        }
    }

    /// <summary>
    /// 計算符Model
    /// </summary>
    public class Operator
    {
        public Func<double, double, double> Operation { get; set; }
    }


    /// <summary>
    /// 計算符Dictionary
    /// </summary>
    Dictionary<string, Operator> getOperatorDict = new Dictionary<string, Operator>()
        {
            {"+",new Operator { Operation = (a, b) => a + b }},
            {"-",new Operator { Operation = (a, b) => a - b }},
            {"x",new Operator { Operation = (a, b) => a * b }},
            {"÷",new Operator { Operation = (a, b) => a / b }}
        };

    /// <summary>
    /// 計算
    /// </summary>
    /// <param name="operand1">被加(減乘除)數</param>
    /// <param name="operatorSymbol">計算符</param>
    /// <param name="operand2">加(減乘除)數</param>
    /// <returns>計算結果</returns>
    private double Calculate(double operand1, string operatorSymbol, double operand2)
    {
        Operator newOperator = getOperatorDict[operatorSymbol];
        return newOperator.Operation(operand1, operand2);
    }

    /// <summary>
    /// 加上千位符
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