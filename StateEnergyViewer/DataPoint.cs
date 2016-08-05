using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEnergyViewer
{
    class DataPoint
    {
        public int Year;
        public int ConsumptionAmount;
        string state;

        public DataPoint(string year, string consumptionData, string state)
        {
            if (!int.TryParse(year,out Year))
            {
                throw new Exception("Year data could not be parsed for: " + year);
            }

            if(!int.TryParse(consumptionData, out ConsumptionAmount))
            {
                throw new Exception("Consumption data could not be parsed for: " + consumptionData);
            }

            this.state = state;
        }

        public string GetSummary()
        {
            return "In the year " + Year.ToString() + ", the state of " + state + " consumed "
                + ConsumptionAmount + " billion btu of energy.";
        }

        public System.Windows.Forms.DataVisualization.Charting.DataPoint getWindowsPoint()
        {
            return new System.Windows.Forms.DataVisualization.Charting.DataPoint(Year, ConsumptionAmount);
        }
    }
}
