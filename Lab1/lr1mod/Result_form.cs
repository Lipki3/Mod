using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace lr1mod
{
    public partial class Result_form : Form
    {
        private List<ulong> randomNumbers = new List<ulong>();
        private ulong currentState;

        private ulong number;
        private long k_A;
        private ulong k_M;
        private ulong first_num;
        private ulong second_num;
        ulong seed;
        private long R0 = 357; private ulong R1 = 127449;


        public Result_form(ulong first_number, ulong second_number, ulong num, long a, ulong m)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(310, 150);
            double mat_ex = (first_number + second_number) / 2;
            label2.Text = mat_ex.ToString("0.###"); 
            label7.Text = num.ToString();
            label9.Text = second_number.ToString("0.###"); 
            label8.Text = first_number.ToString("0.###"); 
            label10.Text = a.ToString();
            label11.Text = m.ToString();
            label13.Text = "Подождите, пока программа завершит работу...";
            label13.Visible = false;

            number = num;
            k_A = a;
            k_M = m;
            first_num = first_number;
            second_num = second_number;

            randomNumbers.Clear();

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                // Создаем буфер для хранения случайных байтов
                byte[] buffer = new byte[8]; // 8 байтов для генерации ulong
                                             // Заполняем буфер случайными данными
                rng.GetBytes(buffer);
                // Преобразуем байты в ulong
                seed = BitConverter.ToUInt64(buffer, 0);
            }

            currentState = seed; // Используем seed, объявленную в конструкторе
        }

        //------------
        void GenerateRandomNumbers(ulong number, ulong minValue, ulong maxValue, List<ulong> randomNumbers)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("Минимальное значение должно быть меньше или равно максимальному значению.");
            }

            byte[] buffer = new byte[8]; // 8 байт для ulong
            Random random = new Random();

            for (ulong i = 0; i < number; i++)
            {
                random.NextBytes(buffer); // Генерируем случайные байты

                // Преобразуем байты в ulong
                ulong randomNumber = BitConverter.ToUInt64(buffer, 0);

                // Масштабируем число до нужного диапазона
                double scalingFactor = (double)(maxValue - minValue) / ulong.MaxValue;
                ulong scaledNumber = (ulong)(randomNumber * scalingFactor + minValue);

                randomNumbers.Add(scaledNumber);
            }
        }

        //-------------

        ulong RegRandom(ref ulong seed)
        {
            seed = ((ulong)k_A * seed) % k_M;
            return seed;
        }

        void GenerateRegD(ulong num, ulong a, ulong m, ulong first, ulong second)
        {
            randomNumbers.Clear();
            ulong currentSeed = seed; // текущее начальное значение

            for (ulong i = 0; i < num; i++)
            {
                // новое начальное значение для следующей итерации с помощью метода регистров обратной связи
                currentSeed = RegRandom(ref currentSeed);

                ulong next = currentSeed;

                randomNumbers.Add(next);
            }
        }

        //----------

        private ulong LehmerRandom(ref ulong state)
        {
            state = ((ulong)k_A * state) % k_M;
            return state;
        }

        void GenerateLehmerD(ulong number)
        {
            randomNumbers.Clear();
            for (ulong i = 0; i < number; i++)
            {
                ulong randomNumber = LehmerRandom(ref currentState);

                randomNumbers.Add(randomNumber);
            }
        }


        //-------------


        private void GenerateMidD(ulong number, long first_num, ulong second_num)
        {
            randomNumbers.Clear();

            R0 = first_num;
            R1 = second_num;
            // Проверьте, что начальные значения R0 и R1 не равны нулю
            if (R0 == 0 || R1 == 0)
            {
                throw new ArgumentException("Начальные значения R0 и R1 должны быть ненулевыми.");
            }

            for (ulong i = 0; i < number; i++)
            {
                // Умножаем R0 на R1
                ulong R2 = (ulong)R0 * R1;

                // Извлекаем середину R2
                long R2Middle = ExtractMiddle(R2);

                // Умножаем R2Middle на R1
                ulong randomNumber = (ulong)R2Middle * R1;

                randomNumber = randomNumber % 1844674407370955165 + 1;

                // Обновляем значения R0 и R1 для следующей итерации
                R0 = R2Middle;
                R1 = randomNumber;

                randomNumbers.Add(randomNumber);
            }
        }

        // Функция для извлечения середины числа
        private long ExtractMiddle(ulong number)
        {
            string binary = Convert.ToString((long)number, 2); // Преобразуем ulong в двоичную строку

            // Добавляем нули слева, чтобы получить 64 бита
            binary = binary.PadLeft(64, '0');

            string centerstr = binary.Substring(4, 56);  // 56 бит середина
            centerstr.TrimStart('0'); //удалить незначащие нули в начале

            long result = Convert.ToInt64(centerstr, 2);

            return result;
        }




        //--------------


        private void button3_Click(object sender, EventArgs e)
        {
            label13.Visible = true;
            button3.BackColor = Color.NavajoWhite;
            button3.Enabled = true;

            randomNumbers.Clear();
            if (radioButton1.Checked == true)
            {
                GenerateLehmerD(number);
            }
            else if(radioButton2.Checked == true)
            {
                 GenerateMidD(number, k_A, k_M);
            }
            else if(radioButton3.Checked == true)
            {
                GenerateRegD(number, (ulong)k_A, k_M, first_num, second_num);
            }
            else
            {
                GenerateRandomNumbers(number, first_num, second_num, randomNumbers);
            }

            MessageBox.Show("Генерация чисел завершена!");
            label13.Text = "Успешно! Теперь вы можете увидеть результат.";
            button2.BackColor = Color.NavajoWhite;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gist gistForm = new gist(randomNumbers);
            gistForm.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
            button3.BackColor = Color.NavajoWhite;
                button3.Enabled = true;
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            button3.BackColor = Color.NavajoWhite;
                button3.Enabled = true;
            
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
         
            button3.BackColor = Color.NavajoWhite;
            button3.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            button3.BackColor = Color.NavajoWhite;
            button3.Enabled = true;
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

       
    }
}
