using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lr1mod
{
    public partial class Main_form : Form
    {
        public Main_form()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox3.BackColor = Color.White;
            textBox4.BackColor = Color.White;
            textBox5.BackColor = Color.White;

            ulong first; ulong second;
            ulong m, num; long a;
           
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Генератор");
                if (textBox1.Text == "") textBox1.BackColor = Color.Khaki;
                if (textBox2.Text == "") textBox2.BackColor = Color.Khaki;
                if (textBox3.Text == "") textBox3.BackColor = Color.Khaki;
                if (textBox4.Text == "") textBox4.BackColor = Color.Khaki;
                if (textBox5.Text == "") textBox5.BackColor = Color.Khaki;
            }
            else
            {
                num = ulong.Parse(textBox3.Text);
                a = long.Parse(textBox4.Text);
                m = ulong.Parse(textBox5.Text);
                first = ulong.Parse(textBox1.Text);
                second = ulong.Parse(textBox2.Text);
                if (second <= first)
                {
                    MessageBox.Show("Первое число не должно быть меньшим или равным второму", "Генератор");
                }
                else
                {
                    Result_form frm = new Result_form(first, second, num, a, m);
                    frm.Show();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Отменяем ввод, если символ не является цифрой или Backspace
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Отменяем ввод, если символ не является цифрой или Backspace
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Отменяем ввод, если символ не является цифрой или Backspace
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Отменяем ввод, если символ не является цифрой или Backspace
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Отменяем ввод, если символ не является цифрой или Backspace
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
