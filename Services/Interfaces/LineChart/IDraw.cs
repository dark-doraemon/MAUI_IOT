using LiveChartsCore.SkiaSharpView;
using MAUI_IOT.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Interfaces.LineChart
{
    public interface IDraw
    {
        void DrawChart(Packet packet, List<double> _accX, List<double> _accY, List<double> _accZ, List<double> _force, ObservableCollection<Data> Datas, DateTimeAxis _customAxis, object Sync);


    }
}
