using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace lr1mod
{
    public partial class Result_form : Form
    {
        private List<double> randomNumbers = new List<double>();
        private List<double> newNumbers = new List<double>();
        private ulong currentState = 4728;

        private ulong number;
        private long k_A;
        private ulong k_M;
        private double first_num;
        private double second_num;
        double seed = 0.384;


        public Result_form(double first_number, double second_number, ulong num, long a, ulong m)
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

            number = num;
            k_A = a;
            k_M = m;
            first_num = first_number;
            second_num = second_number;

            randomNumbers.Clear();

        }


        void LfsrRandomGenerator(ulong num, ulong seed)
        {
            Lfsr64 lfsr = new Lfsr64();
            ulong maxUInt64 = ulong.MaxValue;

            for (ulong i = 0; i < num; i++)
            {
                lfsr.Clock(true);
                double randomDouble = (double)lfsr.Out / maxUInt64;
                randomNumbers.Add(randomDouble);
            }
        }


        //------------

        /*
        void GenerateRandomNumbers(ulong number, ulong minValue, ulong maxValue, List<BigInteger> randomNumbers)
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

        */

        //-------------

        double RegRandom(ref double seed)
        {
            seed = (k_A * seed) % k_M;
            return seed;
        }

        void GenerateRandomDoubles(ulong num, double seed, double first_num, double second_num)
        {
            randomNumbers.Clear();
            double currentSeed = seed;
            double range = second_num - first_num;

            for (ulong i = 0; i < num; i++)
            {
                currentSeed = RegRandom(ref currentSeed);
                double nextDouble = currentSeed / k_M; // В диапазоне от 0 до 1

                // Преобразовываем nextDouble в диапазоне от first_num до second_num
                double randomNumber = first_num + range * nextDouble;

                randomNumbers.Add(randomNumber);
            }
        }


        //----------

        private ulong LehmerRandom(ref ulong state)
        {
            state = ((ulong)k_A * state) % k_M;
            return state;
        }

        void GenerateLehmerD()
        {
            randomNumbers.Clear();
            for (ulong i = 0; i < number; i++)
            {
                ulong randomNumber = LehmerRandom(ref currentState);

                // Преобразование в дробное значение с 3 знаками после запятой
                double randomDouble = (double)randomNumber / (double)k_M;
                randomDouble = Math.Round(randomDouble, 3); // Округляем до 3 знаков после запятой

                // Ограничение значения в диапазоне [first_num, second_num]
                randomDouble = Math.Max(first_num, Math.Min(second_num, randomDouble));

                randomNumbers.Add(randomDouble);
            }
        }


        //-------------

        /*
        private void GenerateMidD(ulong number, BigInteger first_num, BigInteger second_num)
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
                BigInteger R2 = (BigInteger)R0 * R1;

                // Извлекаем середину R2
                long R2Middle = ExtractMiddle(R2);

                // Умножаем R2Middle на R1
                BigInteger randomNumber = (BigInteger)R2Middle * R1;

                randomNumber = randomNumber % 1844674407370955165 + 1;

                // Обновляем значения R0 и R1 для следующей итерации
                R0 = R2Middle;
                R1 = randomNumber;

                randomNumbers.Add(randomNumber);
            }
        }

        // Функция для извлечения середины числа
        private long ExtractMiddle(BigInteger number)
        {
            string binary = Convert.ToString((long)number, 2); // Преобразуем ulong в двоичную строку

            // Добавляем нули слева, чтобы получить 64 бита
            binary = binary.PadLeft(64, '0');

                string centerstr = binary.Substring(39, 51);  // 00.32..00 | 000000011..111000000 | 00.32.00
            centerstr.TrimStart('0'); //удалить незначащие нули в начале

            long result = Convert.ToInt64(centerstr, 2);

            return result;
        }

        */


        //--------------


        public void Transform1(List<double> randomNumbers, double a, double b)
        {
            newNumbers.Clear();
            double minValue = randomNumbers.Min();
            double maxValue = randomNumbers.Max();

            for (int i = 0; i < randomNumbers.Count; i++)
            {
                double normalizedValue = (randomNumbers[i] - minValue) / (maxValue - minValue);
                double transformedValue = a + normalizedValue * (b - a);
                newNumbers.Add(transformedValue);
            }
            MessageBox.Show("Трансформация завершена!");
        }

        public void Transform2(List<double> randomNumbers, double mean, double stdDev)
        {
            newNumbers.Clear();
            int batchSize = 6;
            int currentIndex = 0;

            while (currentIndex < randomNumbers.Count)
            {
                double sum = 0;

                // Суммируем batchSize (в данном случае 6) случайных чисел из randomNumbers
                for (int j = 0; j < batchSize && currentIndex < randomNumbers.Count; j++)
                {
                    sum += randomNumbers[currentIndex];
                    currentIndex++;
                }

                // Нормализуем сумму к стандартному нормальному распределению (мат. ожидание 0, стандартное отклонение 1)
                double normalizedValue = (sum - (batchSize / 2.0)) / Math.Sqrt(batchSize / 12.0);

                // Масштабируем нормализованное значение к заданным параметрам (mean и stdDev)
                double transformedValue = mean + stdDev * normalizedValue;

                newNumbers.Add(transformedValue);
            }

            MessageBox.Show("Трансформация завершена!");
        }

        public void Transform3(List<double> randomNumbers, double lambda)
        {
            newNumbers.Clear();
            foreach (var randomValue in randomNumbers)
            {
                if (randomValue > 0 && randomValue < 1)
                {
                    double exponentialValue = -Math.Log(1 - randomValue) / lambda;
                    newNumbers.Add(exponentialValue);
                }
            }
            MessageBox.Show("Трансформация завершена!");
        }

        public void Transform4(List<double> randomNumbers, double eta, double lambda)
        {
            newNumbers.Clear();
            Random random = new Random();

            foreach (var _ in randomNumbers)
            {
                double sum = 0;

                // Генерируем eta случайных чисел из экспоненциального распределения и суммируем их
                for (int i = 0; i < eta; i++)
                {
                    double randomExp = -Math.Log(random.NextDouble()) / lambda;
                    sum += randomExp;
                }

                // Получаем случайную величину X как сумму экспоненциально распределенных случайных величин
                newNumbers.Add(sum);
            }

            MessageBox.Show("Трансформация завершена!");
        }

        public List<double> Transform5(List<double> randomNumbers, double min, double max)
        {
            newNumbers.Clear();

            Random random = new Random();
            foreach (var randomValue in randomNumbers)
            {
                double R1 = randomValue;
                double R2 = random.NextDouble();

                double triangularValue = min + (max - min) * Math.Max(R1, R2);
                newNumbers.Add(triangularValue);
            }

            MessageBox.Show("Трансформация завершена!");
            return newNumbers;
        }

        public List<double> Transform6(List<double> randomNumbers, double a, double b)
        {
            newNumbers.Clear();
            Random random = new Random(); 

            // Проверяем, что у нас есть четное количество случайных чисел
            if (randomNumbers.Count % 2 != 0)
            {
                // Если нечетное, удаляем последний элемент
                randomNumbers.RemoveAt(randomNumbers.Count - 1);
            }

            // Имитация распределения Симпсона
            for (int i = 0; i < randomNumbers.Count; i += 2)
            {
                double y = randomNumbers[i] * (b - a) + a;
                double z = randomNumbers[i + 1] * (b - a) + a;
                double X = y + z;
                newNumbers.Add(X);
            }

            MessageBox.Show("Трансформация завершена!");
            return newNumbers;
        }



        private void button3_Click(object sender, EventArgs e)
        {
          
            button3.BackColor = Color.PowderBlue;
            button3.Enabled = true;

            randomNumbers.Clear();
            if (radioButton1.Checked == true)
            {
                GenerateLehmerD();    
            }
            else if(radioButton3.Checked == true)
            {
                GenerateRandomDoubles(number, seed, first_num, second_num);
            }

            MessageBox.Show("Генерация чисел завершена!");
         
            button2.BackColor = Color.PowderBlue;
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
            
            button3.BackColor = Color.PowderBlue;
                button3.Enabled = true;
            
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
         
            button3.BackColor = Color.PowderBlue;
            button3.Enabled = true;
        }


        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if(checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                button1.Enabled = true;
                button1.BackColor = Color.PowderBlue;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                button1.Enabled = true;
                button1.BackColor = Color.PowderBlue;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                button1.Enabled = true;
                button1.BackColor = Color.PowderBlue;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                button1.Enabled = true;
                button1.BackColor = Color.PowderBlue;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox1.Checked = false;
                checkBox6.Checked = false;
                button1.Enabled = true;
                button1.BackColor = Color.PowderBlue;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox1.Checked = false;
                button1.Enabled = true;
                button1.BackColor = Color.PowderBlue;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
              
                if (checkBox1.Checked == true)
                {
                    Transform1(randomNumbers, 0, 100);
                }
                else if (checkBox2.Checked == true)
                {
                    Transform2(randomNumbers, 50, 10);
                }
                else if (checkBox3.Checked == true)
                {
                    Transform3(randomNumbers, 1);
                }
                else if (checkBox4.Checked == true)
                {
                    Transform4(randomNumbers, 5, 1);
                }
                else if (checkBox5.Checked == true)
                {
                    Transform5(randomNumbers, 0, 1);
                }
                else if (checkBox6.Checked == true)
                {
                    Transform6(randomNumbers, 0, 1);
                }
            }
            else if (radioButton3.Checked == true)
            {
             
                if (checkBox1.Checked == true)
                {
                    Transform1(randomNumbers, 0, 100);
                }
                else if (checkBox2.Checked == true)
                {
                    Transform2(randomNumbers, 50, 10);
                }
                else if (checkBox3.Checked == true)
                {
                    Transform3(randomNumbers, 1);
                }
                else if (checkBox4.Checked == true)
                {
                    Transform4(randomNumbers, 5, 1);
                }
                else if (checkBox5.Checked == true)
                {
                    Transform5(randomNumbers, 0, 1);
                }
                else if (checkBox6.Checked == true)
                {
                    Transform6(randomNumbers, 0, 1);
                }
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            gist gistForm = new gist(newNumbers);
            gistForm.Show();
        }
    }

    public partial class Lfsr64
    {
        private ulong shiftRegister; // 64-битный регистр сдвига
        public ulong Out { get { return shiftRegister; } } // Выход регистра, 64 бита
        public byte OutLb { get { return (byte)(shiftRegister & 0xFF); } }
        public Lfsr64()
        {
            Reset();
        }
        public void Reset()
        {
            shiftRegister = 0x0000000000000001UL;
        }
        public void Clock(bool enable)
        {
            if (enable)
            {
                ulong nextBit =
                ((shiftRegister >> 63) & 0x1UL) ^
               ((shiftRegister >> 62) & 0x1UL) ^
               ((shiftRegister >> 61) & 0x1UL) ^
               ((shiftRegister >> 59) & 0x1UL) ^
               ((shiftRegister >> 57) & 0x1UL) ^
               ((shiftRegister >> 0) & 0x1UL);
                shiftRegister = (nextBit << 63) | (shiftRegister >> 1);
            }
        }


    }

}
