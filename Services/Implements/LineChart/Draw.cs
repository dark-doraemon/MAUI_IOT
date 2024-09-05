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
        public void DrawChart(ObservableCollection<Data> data, List<double> _accX, List<double> _accY, List<double> _accZ, List<double> _force, ObservableCollection<Data> Datas, DateTimeAxis _customAxis, object Sync)
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
                        _accX.Add(a.accX);
                        _accY.Add(a.accY);
                        _accZ.Add(a.accZ);
                        _force.Add(a.force);
                        //table
                        //  Datas.Add(a);

                        if (_accX.Count > 250) _accX.RemoveAt(0);
                        if (_accY.Count > 250) _accY.RemoveAt(0);
                        if (_accZ.Count > 250) _accZ.RemoveAt(0);
                        if (_force.Count > 250) _force.RemoveAt(0);


                        _customAxis.CustomSeparators = GetSeparators();
                    }
                }
            }
        }

        public void DrawChart(Packet packet, List<double> _accX, List<double> _accY, List<double> _accZ, List<double> _force, ObservableCollection<Data> Datas, DateTimeAxis _customAxis, object Sync)
        {
            if (packet != null)
            {
                lock (Sync)
                {
                    foreach (Data data in packet.data)
                    {
                        //chart
                        _accX.Add(data.accX);
                        _accY.Add(data.accY);
                        _accZ.Add(data.accZ);
                        _force.Add(data.force);
                        //table
                        Datas.Add(data);

                        if (_accX.Count > 250) _accX.RemoveAt(0);
                        if (_accY.Count > 250) _accY.RemoveAt(0);
                        if (_accZ.Count > 250) _accZ.RemoveAt(0);
                        if (_force.Count > 250) _force.RemoveAt(0);


                        _customAxis.CustomSeparators = GetSeparators();
                    }
                }
            }
        }
        private double[] GetSeparators()
        {
            var now = DateTime.Now;

            return new double[]
            {
            now.AddSeconds(-25).Ticks,
            now.AddSeconds(-20).Ticks,
            now.AddSeconds(-15).Ticks,
            now.AddSeconds(-10).Ticks,
            now.AddSeconds(-5).Ticks,
            now.Ticks
            };
        }

    }
}
