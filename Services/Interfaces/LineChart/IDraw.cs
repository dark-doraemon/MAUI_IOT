using LiveChartsCore.Defaults;
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
        public void DrawChart(ObservableCollection<Data> data, ObservableCollection<ObservablePoint> _accX, ObservableCollection<ObservablePoint> _accY, ObservableCollection<ObservablePoint> _accZ, ObservableCollection<ObservablePoint> _force, object Sync);
        public void DrawChart(Packet packet, ObservableCollection<ObservablePoint> _accX, ObservableCollection<ObservablePoint> _accY, ObservableCollection<ObservablePoint> _accZ, ObservableCollection<ObservablePoint> _force, ObservableCollection<Data> Datas, object Sync);

    }
}
