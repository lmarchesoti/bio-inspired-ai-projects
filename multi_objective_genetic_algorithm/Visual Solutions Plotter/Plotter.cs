using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visual_Solutions_Plotter {
    public class Plotter {
        public static void DisplayMapping(int minX, int maxX, int minY, int maxY, IEnumerable<Tuple<double, double>> dots) {
            MainForm form = new MainForm();
            form.Map.XRangeStart = minX;
            form.Map.XRangeEnd = maxX;
            form.Map.YRangeStart = minY;
            form.Map.YRangeEnd = maxY;
            List<Double> xdata = new List<Double>();
            List<Double> ydata = new List<Double>();
            foreach (Tuple<double, double> dot in dots) {
                xdata.Add(dot.Item1);
                ydata.Add(dot.Item2);
            }
            form.Map.XData = xdata.ToArray();
            form.Map.YData = ydata.ToArray();
            form.Map.Refresh();
            form.Refresh();
            form.Focus();
            form.ShowDialog();
        }
    }
}
