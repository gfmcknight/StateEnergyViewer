using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace StateEnergyViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            richTextBox1.Clear();

            string state;
            try
            {
                state = listBox1.SelectedItem.ToString();
            }
            catch
            {
                state = "CA";
            }

            WebClient client = new WebClient();
            Stream data = client.OpenRead("http://api.eia.gov/series/?api_key=YOUR_KEY_HERE&series_id=SEDS.TETCB." + state + ".A");
            StreamReader reader = new StreamReader(data);

            SimpleTextParser parser = new SimpleTextParser(reader.ReadLine());
            this.Text = "Showing Energy Consumption for" + parser.Tokens[10];

            List<DataPoint> points = new List<DataPoint>();
            for (int i = parser.Tokens.IndexOf("data") + 1; i < parser.Tokens.Count - 1; i += 2)
            {
                points.Add(new DataPoint(parser.Tokens[i], parser.Tokens[i + 1], state));
            }

            points.OrderBy(z => z.Year);
            richTextBox1.AppendText(points[0].GetSummary() + "\n");

            DataManipulator trendFinder = new DataManipulator(points);
            trendFinder.CullData(10);
            if (trendFinder.Slope < 0)
            {
                richTextBox1.AppendText(state + " has been decreasing its energy consumption in the last 10 years.\n");
            }
            else if (trendFinder.Slope > 0)
            {
                richTextBox1.AppendText(state + " has been increasing its energy consumption in the last 10 years.\n");
            }
            richTextBox1.AppendText("In the year 2020, this state will likely consume " +
                trendFinder.PredictConsumption(2020).ToString() + " billion btu of energy.\n");

            richTextBox1.AppendText("Data from the U.S. Energy Information Administration.");

            chart1.Series.Last().Points.Clear();
            foreach (DataPoint point in points)
            {
                chart1.Series.Last().Points.Add(point.getWindowsPoint());
            }
            
        }
    }
}
