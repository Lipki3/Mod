using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using System.Numerics;
using System.Collections;

namespace lr1mod
{
    public partial class tests : Form
    {

        private List<ulong> randomNumbers;
        public tests(List<ulong> randomNumbers)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(710, 310);

            this.randomNumbers = randomNumbers;
        }


        /*
         Суть данного теста заключается в определении соотношения между нулями и единицами 
        во всей двоичной последовательности. Цель — выяснить, действительно ли число нулей 
        и единиц в последовательности приблизительно одинаковы, как это можно было бы предположить
        в случае истинно случайной бинарной последовательности. Тест оценивает, насколько близка
        доля единиц к 0,5. Таким образом, число нулей и единиц должно быть примерно одинаковым.
        Если вычисленное в ходе теста значение вероятности p < 0,01, то данная двоичная
        последовательность не является истинно случайной. В противном случае последовательность
        носит случайный характер. Стоит отметить, что все последующие тесты проводятся при условии, 
        что пройден данный тест.
         */
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Суть данного теста заключается в определении соотношения между нулями и единицами во всей двоичной последовательности. Цель — выяснить, действительно ли число нулей и единиц в последовательности приблизительно одинаковы, как это можно было бы предположить в случае истинно случайной бинарной последовательности. Тест оценивает, насколько близка доля единиц к 0,5. Таким образом, число нулей и единиц должно быть примерно одинаковым. Если вычисленное в ходе теста значение вероятности p < 0,01, то данная двоичная последовательность не является истинно случайной. В противном случае последовательность носит случайный характер. Стоит отметить, что все последующие тесты проводятся при условии, что пройден данный тест.", "Частотный побитовый тест");

            // Порог для вероятности p
            double threshold = 0.01;

            // Преобразование чисел в биты
            List<int> bits = new List<int>();
            int zeroCount = 0;
            int oneCount = 0;

            foreach (ulong number in randomNumbers)
            {
                if (number >= 9223372036854775808)
                {
                    oneCount++;
                    /// seq.Add(1);
                }
                else
                {
                    zeroCount++;
                    //seq.Add(0);
                }
            }

            // Подсчет количества нулей и единиц

            label1.Text = "zeros: " + zeroCount;
            label2.Text = "ones: " + oneCount;

            // Вычисление вероятности p
            double p = (double)Math.Min(zeroCount, oneCount) / bits.Count;


            // Проверка на случайность
            bool isRandom = (Math.Abs(p - 0.5) <= threshold);

            if (isRandom)
            {
                MessageBox.Show("Тест успешно пройден!");
                checkBox1.Checked = true;
            }
            else
            {
                MessageBox.Show("Тест не пройден!");
                //   button1.BackColor = Color.LightPink;
            }
        }

        /*
         * Частотный блочный тест
        Суть теста — определение доли единиц внутри блока длиной m бит. Цель — 
        выяснить действительно ли частота повторения единиц в блоке длиной m бит
        приблизительно равна m/2, как можно было бы предположить в случае абсолютно 
        случайной последовательности. Вычисленное в ходе теста значение вероятности
        p должно быть не меньше 0,01. В противном случае (p < 0,01) двоичная
        последовательность не носит истинно случайный характер. Если принять
        m = 1, данный тест переходит в тест № 1 (частотный побитовый тест).
         */

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Суть теста — определение доли единиц внутри блока длиной m бит. Цель — выяснить, действительно ли частота повторения единиц в блоке длиной m бит приблизительно равна m/2, как это можно было бы предположить в случае абсолютно случайной последовательности. Вычисленное в ходе теста значение вероятности p должно быть не меньше 0,01. В противном случае (p < 0,01) двоичная последовательность не носит истинно случайный характер. Если принять m = 1, данный тест переходит в тест № 1 (частотный побитовый тест).", "Частотный блочный тест");

            int m = 10; // Длина блока
            double threshold = 0.01; // Пороговое значение вероятности p

            List<string> binaryStrings = randomNumbers.Select(number => Convert.ToString((long)number, 2)).ToList();

            List<int> binarySequence = new List<int>();
            foreach (string binaryString in binaryStrings)
            {
                foreach (char bit in binaryString)
                {
                    binarySequence.Add(bit - '0'); // Преобразование символа в int (0 или 1)
                }
            }

            int totalBlocks = binarySequence.Count / m;
            double p = CalculateFrequency(binarySequence, m, totalBlocks);

            bool isRandom = (Math.Abs(p - 0.5) <= threshold);

            if (isRandom)
            {
                MessageBox.Show("Тест успешно пройден!");
                checkBox2.Checked = true;
            }
            else
            {
                MessageBox.Show("Тест не пройден!");
                button2.BackColor = Color.LightPink;
            }
        }

        double CalculateFrequency(List<int> sequence, int m, int totalBlocks)
        {
            int oneCount = 0;

            for (int i = 0; i < totalBlocks; i++)
            {
                List<int> block = sequence.Skip(i * m).Take(m).ToList();
                int onesInBlock = block.Count(bit => bit == 1);
                oneCount += onesInBlock;
            }

            double p = (double)oneCount / (totalBlocks * m);
            return p;
        }


        /*
         * Тест на последовательность одинаковых битов
        Суть состоит в подсчёте полного числа рядов в исходной последовательности, где под 
        словом «ряд» подразумевается непрерывная подпоследовательность одинаковых битов. Ряд 
        длиной k бит состоит из k абсолютно идентичных битов, начинается и заканчивается с бита,
        содержащего противоположное значение. Цель данного теста — сделать вывод о том, 
        действительно ли количество рядов, состоящих из единиц и нулей с различными длинами, 
        соответствует их количеству в случайной последовательности. В частности, определяется
        быстро либо медленно чередуются единицы и нули в исходной последовательности. Если 
        вычисленное в ходе теста значение вероятности p < 0,01, то данная двоичная 
        последовательность не является истинно случайной. В противном случае можно считать 
        последовательность случайной.
         */
        private static List<int> ConvertToBinarySequence2(List<ulong> numbers)
        {
            List<int> binarySequence = new List<int>();

            foreach (ulong number in numbers)
            {
                string binaryString = Convert.ToString((long)number, 2);

                foreach (char bit in binaryString)
                {
                    int bitValue = bit - '0';
                    binarySequence.Add(bitValue);
                }
            }

            return binarySequence;
        }

        private bool Test3(List<ulong> randomNumbers)
        {
            List<int> binarySequence = ConvertToBinarySequence2(randomNumbers);

            int runLength = 0;
            int maxRunLength = 0;

            foreach (int bit in binarySequence)
            {
                if (bit == 0)
                {
                    if (runLength > maxRunLength)
                    {
                        maxRunLength = runLength;
                    }
                    runLength = 0;
                }
                else
                {
                    runLength++;
                }
            }

            label3.Text = "Sequence length: " + maxRunLength;
            double p = CalculateProbability3(maxRunLength, binarySequence.Count);

            return p >= 0.01;
        }

        private static double CalculateProbability3(int maxRunLength, int sequenceLength)
        {
            double mean = (sequenceLength + 1) / 3.0;
            double variance = (sequenceLength - 1) * (sequenceLength - 2) / 18.0;
            double z = (maxRunLength - mean) / Math.Sqrt(variance);

            return Math.Abs(1 - Erfc(Math.Abs(z) / Math.Sqrt(2)));
        }

        // Функция ошибок дополнительная (complementary error function)
        private static double Erfc(double x)
        {
            double t = 1.0 / (1.0 + 0.5 * Math.Abs(x));
            double tau = t * Math.Exp(-x * x - 1.26551223 +
                        t * (1.00002368 +
                        t * (0.37409196 +
                        t * (0.09678418 +
                        t * (-0.18628806 +
                        t * (0.27886807 +
                        t * (-1.13520398 +
                        t * (1.48851587 +
                        t * (-0.82215223 +
                        t * 0.17087277)))))))));
            return (x >= 0) ? tau : 2.0 - tau;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Суть состоит в подсчёте полного числа рядов в исходной последовательности, где под словом «ряд» подразумевается непрерывная подпоследовательность одинаковых битов. Ряд длиной k бит состоит из k абсолютно идентичных битов, начинается и заканчивается с бита, содержащего противоположное значение. Цель данного теста — сделать вывод о том, действительно ли количество рядов, состоящих из единиц и нулей с различными длинами, соответствует их количеству в случайной последовательности. В частности, определяется быстро либо медленно чередуются единицы и нули в исходной последовательности. Если вычисленное в ходе теста значение вероятности p < 0,01, то данная двоичная последовательность не является истинно случайной. В противном случае можно считать последовательность случайной.", "Тест на последовательность одинаковых битов");

            bool isRandom = Test3(randomNumbers);

            if (isRandom)
            {

                MessageBox.Show("Тест успешно пройден!");
                checkBox3.Checked = true;
            }
            else
            {
                MessageBox.Show("Тест не пройден!");
                button3.BackColor = Color.LightPink;
            }
        }

        /*
         Тест на последовательность единиц
          В данном тесте определяется самый длинный ряд единиц внутри блока длиной 
        m бит. Цель — выяснить действительно ли длина такого ряда соответствует ожиданиям
        длины самого протяжённого ряда единиц в случае абсолютно случайной последовательности.
        Если высчитанное в ходе теста значение вероятности p < 0,01 полагается, что
        исходная последовательность не является случайной. В противном случае делается 
        вывод о её случайности. Следует заметить, что из предположения о примерно одинаковой 
        частоте появления единиц и нулей (тест № 1) следует, что точно такие же результаты
        данного теста будут получены при рассмотрении самого длинного ряда нулей. Поэтому
        измерения можно проводить только с единицами.", "Тест на самую длинную
        последовательность единиц в блоке");
         */

        static int CountRuns(List<int> sequence)
        {
            int numberOfRuns = 0;
            int currentRun = 1;

            for (int i = 1; i < sequence.Count; i++)
            {
                if (sequence[i] != sequence[i - 1])
                {
                    numberOfRuns++;
                    currentRun = 1;
                }
                else
                {
                    currentRun++;
                }
            }

            numberOfRuns++; // Завершающий ряд
            return numberOfRuns;
        }

        static double CalculateProbability4(int numberOfRuns, int sequenceLength)
        {
            double mean = (2.0 * sequenceLength - 1.0) / 3.0;
            double variance = (16.0 * sequenceLength - 29.0) / 90.0;
            double z = (numberOfRuns - mean) / Math.Sqrt(variance);

            return Math.Abs(1 - (Erfc(Math.Abs(z) / Math.Sqrt(2))));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В данном тесте определяется самый длинный ряд единиц внутри блока длиной m бит. Цель — выяснить действительно ли длина такого ряда соответствует ожиданиям длины самого протяжённого ряда единиц в случае абсолютно случайной последовательности. Если высчитанное в ходе теста значение вероятности p < 0,01 полагается, что исходная последовательность не является случайной. В противном случае делается вывод о её случайности. Следует заметить, что из предположения о примерно одинаковой частоте появления единиц и нулей (тест № 1) следует, что точно такие же результаты данного теста будут получены при рассмотрении самого длинного ряда нулей. Поэтому измерения можно проводить только с единицами.", "Тест на самую длинную последовательность единиц в блоке");

            double threshold = 0.01; // Пороговое значение вероятности p

            List<int> binarySequence = ConvertToBinarySequence2(randomNumbers);
            int numberOfRuns = CountRuns(binarySequence);

            double p = CalculateProbability4(numberOfRuns, binarySequence.Count);

            bool isRandom;
            if (p >= threshold)
            {
                isRandom = true;
            }
            else
            {
                isRandom = false;
            }

            if (isRandom == true)
            {
                MessageBox.Show("Тест успешно пройден!");
                checkBox4.Checked = true;
            }
            else
            {
                MessageBox.Show("Тест не пройден!");
                button4.BackColor = Color.LightPink;
            }
        }


        public static bool Test5(List<ulong> randomNumbers, int substringLength)
        {
            List<int> binarySequence = ConvertToBinarySequence2(randomNumbers);
            int totalSubstrings = binarySequence.Count / substringLength;

            int sumOfRanks = 0;

            for (int i = 0; i < totalSubstrings; i++)
            {
                List<int> substring = binarySequence.Skip(i * substringLength).Take(substringLength).ToList();
                int rank = CalculateRank(substring);
                sumOfRanks += rank;
            }

            int averageRank = sumOfRanks / totalSubstrings;

            double p = CalculateProbability5(averageRank, substringLength);

            return p >= 0.01;
        }

        private static int CalculateRank(List<int> substring)
        {
            int m = substring.Count; // Длина подстроки
            int n = substring.Count; // Число подстрок

            int[][] matrix = new int[n][];

            for (int i = 0; i < n; i++)
            {
                matrix[i] = new int[m];
                int value = substring[i];
                for (int j = 0; j < m; j++)
                {
                    matrix[i][j] = value;
                }
            }

            int rank = 0;

            for (int col = 0; col < m; col++)
            {
                int pivotRow = -1;
                for (int row = 0; row < n; row++)
                {
                    if (matrix[row][col] == 1)
                    {
                        pivotRow = row;
                        break;
                    }
                }

                if (pivotRow != -1)
                {
                    rank++;

                    for (int row = 0; row < n; row++)
                    {
                        if (row != pivotRow && matrix[row][col] == 1)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                matrix[row][i] ^= matrix[pivotRow][i];
                            }
                        }
                    }
                }
            }

            return rank;
        }

        private static double CalculateProbability5(int rank, int substringLength)
        {
            int n = substringLength; // Длина подстроки
            int k = rank; // Ранг

            double p = 0;

            for (int i = k; i <= n; i++)
            {
                p += (double)(Choose(n, i) * Math.Pow(500, i) * Math.Pow(500, n - i));
            }

            return p;
        }

        // Метод для вычисления биномиальных коэффициентов C(n, k)
        private static long Choose(int n, int k)
        {
            if (k < 0 || k > n)
            {
                return 0;
            }

            if (k == 0 || k == n)
            {
                return 1;
            }

            long result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= (n - i + 1);
                result /= i;
            }

            return result;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Здесь производится расчёт рангов непересекающихся подматриц, построенных из исходной двоичной последовательности. Целью этого теста является проверка на линейную зависимость подстрок фиксированной длины, составляющих первоначальную последовательность. В случае если вычисленное в ходе теста значение вероятности p < 0,01, делается вывод о неслучайном характере входной последовательности бит. В противном случае считаем её абсолютно случайной. Данный тест так же присутствует в пакете DIEHARD.", "Тест рангов бинарных матриц");

            bool isRandom = Test5(randomNumbers, 10);   //15 - мимо

            if (isRandom == true)
            {
                MessageBox.Show("Тест успешно пройден!");
                checkBox5.Checked = true;
            }
            else
            {
                MessageBox.Show("Тест не пройден!");
                button5.BackColor = Color.LightPink;
            }
        }


        /*
         Спектральный тест
        Суть теста заключается в оценке высоты пиков дискретного преобразования Фурье исходной последовательности. 
        Цель — выявление периодических свойств входной последовательности, например, близко расположенных друг к 
        другу повторяющихся участков. Тем самым это явно демонстрирует отклонения от случайного характера исследуемой 
        последовательности. Идея состоит в том, чтобы число пиков, превышающих пороговое значение в 95 % по амплитуде,
        было значительно больше 5 %. Если вычисленное в ходе теста значение вероятности p < 0,01, то данная двоичная 
        последовательность не является абсолютно случайной. В противном случае она носит случайный характер.   ХХХ
         */


        static double[] ConvertToDoubleArray(ulong[] inputSequence)
        {
            double[] doubleArray = new double[inputSequence.Length];
            for (int i = 0; i < inputSequence.Length; i++)
            {
                doubleArray[i] = (double)inputSequence[i];
            }
            return doubleArray;
        }

        static double CalculateThreshold(double[] amplitudeSpectrum)
        {
            Array.Sort(amplitudeSpectrum);
            int thresholdIndex = (int)(0.95 * amplitudeSpectrum.Length);
            return amplitudeSpectrum[thresholdIndex];
        }

        // Ваша функция для подсчета пиков выше порогового значения
        static int CountPeaksAboveThreshold(double[] amplitudeSpectrum, double threshold)
        {
            int peakCount = amplitudeSpectrum.Count(x => x > threshold);
            return peakCount;
        }

        private void button6_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Суть теста заключается в оценке высоты пиков дискретного преобразования Фурье исходной последовательности. Цель — выявление периодических свойств входной последовательности, например, близко расположенных друг к другу повторяющихся участков. Тем самым это явно демонстрирует отклонения от случайного характера исследуемой последовательности. Идея состоит в том, чтобы число пиков, превышающих пороговое значение в 95 % по амплитуде, было значительно больше 5 %. Если вычисленное в ходе теста значение вероятности p < 0,01, то данная двоичная последовательность не является абсолютно случайной. В противном случае она носит случайный характер.", "Спектральный тест");

        ulong[] inputSequence = randomNumbers.ToArray(); // Преобразуем в массив

        // размер сегмента для обработки
        int segmentSize = 10000;

        int peakCount = 0;

        for (int i = 0; i < inputSequence.Length; i += segmentSize)
        {
            // Выделение сегмента
            double[] segment = new double[Math.Min(segmentSize, inputSequence.Length - i)];
            Array.Copy(ConvertToDoubleArray(inputSequence), i, segment, 0, segment.Length);

            // Преобразование сегмента Фурье
            Complex[] signal = segment.Select(x => new Complex(x, 0)).ToArray();
            Fourier.Forward(signal);
            double[] amplitudeSpectrum = signal.Select(x => x.Magnitude).ToArray();

            // Расчет порогового значения для сегмента
            double threshold = CalculateThreshold(amplitudeSpectrum);

            // Подсчет пиков для сегмента
            peakCount += CountPeaksAboveThreshold(amplitudeSpectrum, threshold);
        }

        // Оцените вероятность p на основе результатов анализа.
        double p = (double)peakCount / inputSequence.Length;

        if (p < 0.01)
        {
            MessageBox.Show("Тест успешно пройден!");
            checkBox6.Checked = true;
        }
        else
        {
            MessageBox.Show("Тест не пройден!");
            button6.BackColor = Color.LightPink;
        }
    }


    /*
     * Тест на совпадение неперекрывающихся шаблонов
    В данном тесте подсчитывается количество заранее определенных шаблонов, найденных в 
    исходной последовательности. Цель — выявить генераторы случайных или псевдослучайных чисел, 
    формирующие слишком часто заданные непериодические шаблоны. Как и в тесте № 8 на совпадение 
    перекрывающихся шаблонов для поиска конкретных шаблонов длиной m бит используется окно 
    также длиной m бит. Если шаблон не обнаружен, окно смещается на один бит. Если же шаблон
    найден, окно перемещается на бит, следующий за найденным шаблоном, и поиск продолжается 
    дальше. Вычисленное в ходе теста значение вероятности p должно быть не меньше 0,01. 
    В противном случае (p < 0,01), двоичная последовательность не является абсолютно случайной.
     */

    static double FindPatternProbability7(List<ulong> sequence, List<ulong> pattern)
        {
            int patternCount = 0;
            int sequenceLength = sequence.Count;
            int patternLength = pattern.Count;

            for (int i = 0; i <= sequenceLength - patternLength; i++)
            {
                bool match = true;
                for (int j = 0; j < patternLength; j++)
                {
                    if (sequence[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    patternCount++;
                    i += patternLength - 1; // Перескакиваем шаблон, чтобы избежать перекрытия
                }
            }

            double probability = (double)patternCount / (sequenceLength - patternLength + 1);
            return probability;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В данном тесте подсчитывается количество заранее определенных шаблонов, найденных в исходной последовательности. Цель — выявить генераторы случайных или псевдослучайных чисел, формирующие слишком часто заданные непериодические шаблоны. Как и в тесте № 8 на совпадение перекрывающихся шаблонов для поиска конкретных шаблонов длиной m бит используется окно также длиной m бит. Если шаблон не обнаружен, окно смещается на один бит. Если же шаблон найден, окно перемещается на бит, следующий за найденным шаблоном, и поиск продолжается дальше. Вычисленное в ходе теста значение вероятности p должно быть не меньше 0,01. В противном случае (p < 0,01), двоичная последовательность не является абсолютно случайной.", "Тест на совпадение неперекрывающихся шаблонов");

            List<ulong> pattern = new List<ulong> {1, 10, 21 };

            double p = FindPatternProbability7(randomNumbers, pattern);

            // Пороговое значение для вероятности
            double threshold = 0.01;

            if (p < threshold)
            {
                MessageBox.Show("Тест успешно пройден!");
                checkBox7.Checked = true;
            }
            else
            {
                MessageBox.Show("Тест не пройден!");
                button7.BackColor = Color.LightPink;
            }
        }

        /*
         * Тест на совпадение неперекрывающихся шаблонов
        В данном тесте подсчитывается количество заранее определенных шаблонов, найденных
        в исходной последовательности. Цель — выявить генераторы случайных или
        псевдослучайных чисел, формирующие слишком часто заданные непериодические 
        шаблоны. Как и в тесте № 8 на совпадение перекрывающихся шаблонов для поиска
        конкретных шаблонов длиной m бит используется окно также длиной m бит. Если
        шаблон не обнаружен, окно смещается на один бит. Если же шаблон найден, окно
        перемещается на бит, следующий за найденным шаблоном, и поиск продолжается
        дальше. Вычисленное в ходе теста значение вероятности p должно быть не меньше
        0,01. В противном случае (p < 0,01), двоичная последовательность не является
        абсолютно случайной.
         */

        static double FindPatternProbability8(List<ulong> sequence, List<ulong> pattern)
        {
            int patternCount = 0;
            int sequenceLength = sequence.Count;
            int patternLength = pattern.Count;

            for (int i = 0; i < sequenceLength; i++)
            {
                if (i + patternLength <= sequenceLength)
                {
                    bool match = true;
                    for (int j = 0; j < patternLength; j++)
                    {
                        if (sequence[i + j] != pattern[j])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        patternCount++;
                    }
                }
            }

            double probability = (double)patternCount / (sequenceLength - patternLength + 1);
            return probability;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Суть данного теста заключается в подсчете количества заранее определенных шаблонов, найденных в исходной последовательности. Как и в тесте № 7 на совпадение неперекрывающихся шаблонов для поиска конкретных шаблонов длиной m бит используется окно также длиной m бит. Сам поиск производится аналогичным образом. Если шаблон не обнаружен, окно смещается на один бит. Разница между этим тестом и тестом № 7 заключается лишь в том, что если шаблон найден, окно перемещается только на бит вперед, после чего поиск продолжается дальше. Вычисленное в ходе теста значение вероятности p должно быть не меньше 0,01. В противном случае (p < 0,01), двоичная последовательность не является абсолютно случайной.", "Тест на совпадение перекрывающихся шаблонов");

            List<ulong> pattern = new List<ulong> {1, 10, 21 };

            // Вызов функции для поиска шаблона и вычисления вероятности
            double p = FindPatternProbability8(randomNumbers, pattern);

            // Пороговое значение для вероятности
            double threshold = 0.01;

            if (p < threshold)
            {
                MessageBox.Show("Тест успешно пройден!");
                checkBox8.Checked = true;
            }
            else
            {
                MessageBox.Show("Тест не пройден!");
                button8.BackColor = Color.LightPink;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
