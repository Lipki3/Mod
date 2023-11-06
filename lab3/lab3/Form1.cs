using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double ro = 0;
            if (textBox1.Text != "-")
            {
                ro = Convert.ToDouble(textBox1.Text); // Параметр геометрического распределения для входного потока
            }
            double pi1 = 0;
            if (textBox2.Text != "-")
            {
                pi1 = Convert.ToDouble(textBox2.Text);  // Параметр геометрического распределения для времени обслуживания в π1
            }
            double pi2 = 0;
            if (textBox3.Text != "-")
            {
                pi2 = Convert.ToDouble(textBox3.Text);  // Параметр геометрического распределения для времени обслуживания в π2
            }
            int N = 0;
            if (textBox4.Text != "-")
            {
                N = Convert.ToInt32(textBox4.Text); // Число заявок в накопителе
            }
            int totalSimulations = 0;
            if (textBox5.Text != "-")
            {
                totalSimulations = Convert.ToInt32(textBox5.Text); // Общее количество моделирований для сбора статистики
            }

          //  MessageBox.Show($"ro: {ro}, pi1: {pi1}, pi2: {pi2}, N: {N}, totalSimulations: {totalSimulations}");


            int totalRequests = 0; // Общее число обработанных заявок
            int queueLengthSum = 0; // Сумма длин очередей

            Random random = new Random();

            for (int i = 0; i < totalSimulations; i++)
            {
                int queueLength = 0;

                // Моделирование прихода заявок и обслуживание
                while (queueLength <= N)
                {
                    // Моделирование прихода заявки по геометрическому распределению
                    if (random.NextDouble() > ro)
                    {
                        // Заявка пришла
                        queueLength++;

                        // Проверка, превышает ли очередь N, и отбрасывание заявок при необходимости
                        if (queueLength > N)
                        {
                            queueLength--;
                            break;
                        }

                        // Моделирование времени обслуживания в π1
                        if (random.NextDouble() <= pi1)
                        {
                            queueLength--;

                            // Проверка на отрицательное значение queueLength после обслуживания
                            if (queueLength < 0)
                            {
                                queueLength = 0;
                            }

                            // Моделирование времени обслуживания в π2
                            if (random.NextDouble() <= pi2)
                            {
                                queueLength--;

                                // Проверка на отрицательное значение queueLength после обслуживания
                                if (queueLength < 0)
                                {
                                    queueLength = 0;
                                }

                                // Обработанная заявка
                                totalRequests++;
                            }
                        }
                    }
                    else
                    {
                        // Заявка не пришла
                        break;
                    }
                }

                queueLengthSum += queueLength;
            }

            double absoluteThroughput = (double)totalRequests / totalSimulations;
            double averageQueueLength = (double)queueLengthSum / totalSimulations;

            label8.Text = absoluteThroughput.ToString();
            label9.Text = averageQueueLength.ToString();
        }
    }
}