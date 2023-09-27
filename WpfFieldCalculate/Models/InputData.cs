using OxyPlot;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Threading.Tasks;
using WpfFieldCalculate.Infrastructure;

namespace WpfFieldCalculate.Models
{
    /// <summary>
    /// Исходные данные.
    /// </summary>
    internal class InputData : ViewModel
    {
        public InputData()
        {
            Wires = new ObservableCollection<WireData>();
        }

        public ObservableCollection<WireData> Wires
        {
            get { return _wires; }
            set { Set(ref _wires, value); }
        }

        private ObservableCollection<WireData> _wires;


        public void UpdateCoordinate()
        {
            Parallel.ForEach(Wires, (w, e) =>
            {
                w.Circle.Points[0] = new DataPoint(w.X, w.Y);
                w.Mark.Points[0] = new DataPoint(w.X, w.Y);
                w.Vector.Points[0] = new DataPoint(w.X, w.Y);
                w.Arrow.EndPoint = new DataPoint(w.ToComplex.Real * 100000000, w.ToComplex.Imaginary * 100000000);
            });
        }

        public void UpdateStyle()
        {
            Parallel.ForEach(Wires, (w, e) =>
            {
                w.Circle.Color = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
                w.Circle.MarkerStroke = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
                w.Circle.TextColor = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
                w.Mark.MarkerType = w.I > 0 ? MarkerType.Cross : MarkerType.Circle;
                w.Mark.Color = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
                w.Mark.MarkerStroke = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
                w.Mark.TextColor = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
                w.Mark.MarkerFill = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
                w.Vector.Color = w.I == 0 ? OxyColors.LightGray : OxyColors.Black;
            });

            
        }
    }
}
