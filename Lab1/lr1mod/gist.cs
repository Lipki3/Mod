using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using FastReport.DataVisualization.Charting;

namespace lr1mod
{
    public partial class gist : Form
    {
        private List<ulong> randomNumbers;

        public gist(List<ulong> randomNumbers)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(310, 510);

            this.randomNumbers = randomNumbers;


            Series histogramSeries = chart1.Series["Series1"];

            //  минимальное и максимальное значение из randomNumbers
            ulong minValue = randomNumbers.Min();
            ulong maxValue = randomNumbers.Max();

            //  количество интервалов для гистограммы
            int numBins = 256; 

            //  ширина каждого интервала 
            double binWidth = (double)(maxValue - minValue) / numBins;

            // массив для подсчета количества значений в каждом интервале
            int[] binCounts = new int[numBins];

            foreach (var value in randomNumbers)
            {
                int binIndex = (int)((value - minValue) / binWidth);
                if (binIndex >= 0 && binIndex < numBins)
                {
                    binCounts[binIndex]++;
                }
            }

            for (int i = 0; i < numBins; i++)
            {
                double binCenter = minValue + (i + 0.5) * binWidth;
                histogramSeries.Points.AddXY(binCenter, binCounts[i]);
            }

            chart1.Invalidate();




            double variance = CalculateVariance(randomNumbers);


        }

        double CalculateVariance(List<ulong> randomNumbers)
        {
            if (randomNumbers.Count < 2)
            {
                throw new ArgumentException("Для расчета дисперсии требуется как минимум два значения.");
            }

            double avg;
            double sum = 0;
            foreach (var value in randomNumbers)
            {
                sum += (double)value;
            }

            avg = sum / randomNumbers.Count;
            label3.Text = avg.ToString();

            double variance = randomNumbers.Select(value => Math.Pow(value - avg, 2)).Sum() / (randomNumbers.Count - 1);

            label4.Text = variance.ToString();

            return variance;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tests frm = new tests(randomNumbers);
            frm.Show();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        
        }
    }
}


