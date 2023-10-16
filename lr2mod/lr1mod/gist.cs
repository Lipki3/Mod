using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using System.Drawing;
using FastReport.DataVisualization.Charting;

namespace lr1mod
{
    public partial class gist : Form
    {
        private List<double> randomNumbers;

        public gist(List<double> randomNumbers)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(690, 310);

            this.randomNumbers = randomNumbers;

            Series histogramSeries = chart1.Series["Series1"];

            // Минимальное и максимальное значение из randomNumbers
            double minValue = randomNumbers.Min();
            double maxValue = randomNumbers.Max();

            // Количество интервалов для гистограммы
            int numBins = 256;

            // Ширина каждого интервала
            double binWidth = (maxValue - minValue) / numBins;

            // Массив для подсчета количества значений в каждом интервале
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
        }
        

        private void button1_Click_1(object sender, EventArgs e)
        {
           // tests frm = new tests(randomNumbers);
            //frm.Show();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        
        }
    }
}


