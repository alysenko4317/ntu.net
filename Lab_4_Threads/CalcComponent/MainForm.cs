using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcComponent
{
    public partial class MainForm : Form
    {
        Calculator _calculator;

        public MainForm()
        {
            InitializeComponent();

            _calculator = new Calculator();
            _calculator.FactorialComplete +=
                new Calculator.FactorialCompleteHandler(this.FactorialHandler);
            _calculator.FactorialMinusOneComplete +=
               new Calculator.FactorialCompleteHandler(this.FactorialMinusHandler);
            _calculator.AddTwoComplete +=
               new Calculator.AddTwoCompleteHandler(this.AddTwoHandler);
            _calculator.LoopComplete +=
               new Calculator.LoopCompleteHandler(this.LoopDoneHandler);
        }

        private void buttonFactorial1_Click(object sender, System.EventArgs e)
        {
            // passes the value typed in the txtValue to _calculator._varFact1
            _calculator._varFact1 = int.Parse(_textValue.Text);
            // disables the button until this calculation is complete.
            _buttonFactorial1.Enabled = false;
            _calculator.factorialAsync();
        }

        private void buttonFactorial2_Click(object sender, System.EventArgs e)
        {
            _calculator._varFact2 = int.Parse(_textValue.Text);
            _buttonFactorial2.Enabled = false;
            _calculator.factorialMinusOneAsync();
        }

        private void buttonAddTwo_Click(object sender, System.EventArgs e)
        {
            _calculator._varAddTwo = int.Parse(_textValue.Text);
            _buttonAddTwo.Enabled = false;
            _calculator.addTwoAsync();
        }

        private void buttonRunLoops_Click(object sender, System.EventArgs e)
        {
            _calculator._varLoopValue = int.Parse(_textValue.Text);
            _buttonRunLoops.Enabled = false;
            // lets the user know that a loop is running
            _labelRunLoops.Text = "Looping";  
            _calculator.runLoopAsync();
        }

        private void FactorialHandler(double Value, double Calculations)
        {
            // displays the returned value in the appropriate label
            _labelFactorial1.Text = Value.ToString();
            // re-enables the button so it can be used again
            _buttonFactorial1.Enabled = true;
            // updates the label that displays the total calculations performed
            _labelTotalCalculations.Text = "Всього виконано обчислень: " + Calculations.ToString();
        }

        private void FactorialMinusHandler(double Value, double Calculations)
        {
            _labelFactorial2.Text = Value.ToString();
            _buttonFactorial2.Enabled = true;
            _labelTotalCalculations.Text = "Всього виконано обчислень: " + Calculations.ToString();
        }

        private void AddTwoHandler(int Value, double Calculations)
        {
            _labelAddTwo.Text = Value.ToString();
            _buttonAddTwo.Enabled = true;
            _labelTotalCalculations.Text = "Всього виконано обчислень: " + Calculations.ToString();
        }

        private void LoopDoneHandler(double Calculations, int Count)
        {
            _buttonRunLoops.Enabled = true;
            _labelRunLoops.Text = Count.ToString();
            _labelTotalCalculations.Text = "Всього виконано обчислень:  " + Calculations.ToString();
        }
    }
}
