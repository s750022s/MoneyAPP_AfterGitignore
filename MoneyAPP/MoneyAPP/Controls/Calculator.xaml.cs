namespace MoneyAPP.Controls;

public partial class Calculator : ContentView
{
    List<string> formulas = new List<string>();
    string stagingData = "+";
    double total = 0;

    public event EventHandler<AlertRequestEventArgs> AlertRequest;

    public class OKButtonClickedEventArgs : EventArgs
    {
        public double Total { get; set; }
    }
    public delegate void OKButtonClickedEventHandler(object sender, OKButtonClickedEventArgs e);
    public event OKButtonClickedEventHandler OKButtonClicked;

    public Calculator()
	{
		InitializeComponent();
	}

    private void CalculatorClick(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string newValue = button.Text;

        switch (newValue)
        {
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
            case "C":
                Count_LB.Text = "";
                equal_LB.Text = "";
                formulas.Clear();
                stagingData = "+";
                total = 0;
                break;
            case "←":
                if (formulas.Count < 3)
                {
                    OnAlertRequested(new AlertRequestEventArgs("已無法刪除，請直接使用C清除功能", "", "OK"));
                    break;
                }
                if (stagingData == "")
                {
                    stagingData = formulas[formulas.Count() - 2] + formulas[formulas.Count() - 1];
                    formulas.RemoveAt(formulas.Count() - 2);
                    formulas.RemoveAt(formulas.Count() - 1);

                }
                stagingData = stagingData.Substring(0, stagingData.Length - 1);
                break;
            case "OK":
                if (total % 1 != 0)
                {
                    OnAlertRequested(new AlertRequestEventArgs("數值非整數，請手動輸入最後結果", "", "OK"));
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


        if (formulas.Count > 1)
        {
            total = 0;
            for (int i = 0; i < formulas.Count; i += 2)
            {
                double result = Calculate(total, formulas[i], Convert.ToDouble(formulas[i + 1]));
                total = result;
            }

        }

        string shows = "";
        for (int i = 1; i < formulas.Count; i++)
        {
            if (formulas[i].Length > 3)
            {
                shows += ThousandSeparator(formulas[i]);

            }
            else
            {
                shows += formulas[i];
            }

            Count_LB.Text = shows + stagingData;
            if (total > 999 || total < -999)
            {
                equal_LB.Text = "=" + ThousandSeparator(total.ToString());
            }
            else 
            {
                equal_LB.Text = "=" + total.ToString();
            }
            //equal_LB.Text = "=" + ThousandSeparator(total.ToString());

            if (Count_LB.Measure(Width, Height).Request.Width > Count_Border.WidthRequest)
            {
                OnAlertRequested(new AlertRequestEventArgs("長度已超過顯示範圍", "建議分多次計算", "OK"));
            }
        }
    }


    public class Operator
    {
        public Func<double, double, double> Operation { get; set; }
    }



    Dictionary<string, Operator> getOperatorDict = new Dictionary<string, Operator>()
        {
            {"+",new Operator { Operation = (a, b) => a + b }},
            {"-",new Operator { Operation = (a, b) => a - b }},
            {"x",new Operator { Operation = (a, b) => a * b }},
            {"÷",new Operator { Operation = (a, b) => a / b }}
        };

    private double Calculate(double operand1, string operatorSymbol, double operand2)
    {
        Operator newOperator = getOperatorDict[operatorSymbol];
        return newOperator.Operation(operand1, operand2);
    }

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

    public class AlertRequestEventArgs : EventArgs
    {
        public string Title { get; }
        public string Message { get; }
        public string Cancel { get; }

        public AlertRequestEventArgs(string title, string message, string cancel)
        {
            Title = title;
            Message = message;
            Cancel = cancel;
        }
    }

    protected virtual void OnAlertRequested(AlertRequestEventArgs e)
    {
        AlertRequest?.Invoke(this, e);
    }
}