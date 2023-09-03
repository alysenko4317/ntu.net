namespace CalcApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button0_Click(object sender, EventArgs e)
        {
            textBox1.Text += '0';
        }

        private void button_digit_click(object sender, EventArgs e)
        {
            if (sender is Button)
                textBox1.Text += (sender as Button).Text;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}