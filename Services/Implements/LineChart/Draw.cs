using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using MAUI_IOT.Models.Data;
using MAUI_IOT.Services.Interfaces.LineChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MAUI_IOT.Services.Implements.LineChart
{
    public class Draw : IDraw
    {
        public void DrawChart(ObservableCollection<Data> data, ObservableCollection<ObservablePoint> _accX, ObservableCollection<ObservablePoint> _accY, ObservableCollection<ObservablePoint> _accZ, ObservableCollection<ObservablePoint> _force, object Sync)
        {
            if (data != null)
            {
                lock (Sync)
                {
                    _accX.Clear();
                    _accY.Clear();
                    _accZ.Clear();
                    _force.Clear();
                    foreach (var a in data)
                    {
                        //chart
                        double time = a.timestamp / 1000.0;
                        //chart
                        _accX.Add(new ObservablePoint(time, a.accX));
                        _accY.Add(new ObservablePoint(time, a.accY));
                        _accZ.Add(new ObservablePoint(time, a.accZ));
                        _force.Add(new ObservablePoint(a.accX, a.accY));
                        //table
                        // Datas.Add(a);


                    }
                }
            }
        }
        public void DrawChart(Packet packet, ObservableCollection<ObservablePoint> _accX, ObservableCollection<ObservablePoint> _accY, ObservableCollection<ObservablePoint> _accZ, ObservableCollection<ObservablePoint> _force, ObservableCollection<Data> Datas, object Sync)
        {
            if (packet != null)
            {
                lock (Sync)
                {
                    _accX.Clear();
                    _accY.Clear();
                    _accZ.Clear();
                    _force.Clear();
                    foreach (Data data in packet.data)
                    {
                        //chart
                        double time = data.timestamp / 1000.0;
                        //chart
                        _accX.Add(new ObservablePoint(time, data.accX));
                        _accY.Add(new ObservablePoint(time, data.accY));
                        _accZ.Add(new ObservablePoint(time, data.accZ));
                        _force.Add(new ObservablePoint(data.accX, data.accY));
                        //table
                        Datas.Add(data);

                    }
                }
            }
        }


    }
}
