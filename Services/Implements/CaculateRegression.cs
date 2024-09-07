using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Implements
{
    public class CaculateRegression
    {
        public static ObservableCollection<ObservablePoint> Regression(ObservableCollection<ObservablePoint> data)
        {
            int n = data.Count;

            var sumX = data.Sum(point => point.X);
            var sumY = data.Sum(point => point.Y);
            var sumXY = data.Sum(point => point.X * point.Y);
            var sumX2 = data.Sum(point => point.X * point.X);

            // Tính toán hệ số góc (slope) và đoạn đoàn (intercept)
            var slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            var intercept = (sumY - slope * sumX) / n;

            // Tạo danh sách điểm hồi quy
            var regressionPoints = new ObservableCollection<ObservablePoint>();

            // Sử dụng các giá trị x từ dữ liệu gốc để tính giá trị y của hồi quy
            foreach (var point in data)
            {
                var regY = slope * point.X + intercept;
                regressionPoints.Add(new ObservablePoint(point.X, regY));
            }

            return regressionPoints;
        }
    }
}
