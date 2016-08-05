using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEnergyViewer
{
    class DataManipulator
    {
        List<DataPoint> data;
        double averageX;
        double averageY;
        double varianceX;
        double varianceY;
        double covarianceXY;
        double standardDeviationX;
        double standardDeviationY;

        public double RCoefficient { get; set; }
        public double Slope { get; set; }
        public double Intercept { get; set; }

        public DataManipulator (List<DataPoint> points)
        {
            data = points;
            calculateEquation();
        }

        public void CullData(int maxEntries)
        {
            List<DataPoint> culledData = new List<DataPoint>();

            //Get the x most recent data points; data must be sorted first
            for (int i = 0; i <= maxEntries && data.Count >= i; i++)
            {
                culledData.Add(data[i]);
            }
            data = culledData;
            calculateEquation();
        }

        void calculateEquation()
        {
            int sumX = 0;
            int sumY = 0;
            foreach (DataPoint point in data)
            {
                sumX += point.Year;
                sumY += point.ConsumptionAmount;
            }
            averageX = sumX / data.Count;
            averageY = sumY / data.Count;

            varianceX = 0;
            varianceY = 0;
            covarianceXY = 0;
            foreach (DataPoint point in data)
            {
                varianceX += Math.Pow(point.Year - averageX, 2);
                varianceY += Math.Pow(point.ConsumptionAmount - averageY, 2);
                covarianceXY += (point.Year - averageX) * (point.ConsumptionAmount - averageY);
            }
            standardDeviationX = Math.Sqrt(varianceX);
            standardDeviationY = Math.Sqrt(varianceY);
            RCoefficient = covarianceXY / Math.Sqrt(varianceX * varianceY);

            Slope = RCoefficient * standardDeviationY / standardDeviationX;
            Intercept = averageY - Slope * averageX;
        }

        public int PredictConsumption(int year)
        {
            return (int)(Intercept + year * Slope);
        }
    }
}
