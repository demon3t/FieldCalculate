using WpfFieldCalculate.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace WpfFieldCalculate.Models
{
    /// <summary>
    /// Исходные данные.
    /// </summary>
    internal class InputData : ViewModel
    {
        public InputData()
        {
            Wires = new ObservableCollection<WireData>()
            {
                new WireData(200, new Point(-60, 100))
                {
                    Name = "Провод 1"
                },
                new WireData(-200, new Point(60, 100))
                {
                    Name = "Провод 2"
                },
                new WireData(0, new Point(0, 170))
                {
                    Name = "Провод 3",
                },
            };
        }

        public ObservableCollection<WireData> Wires
        {
            get { return _wires; }
            set { Set(ref _wires, value); }
        }

        private ObservableCollection<WireData> _wires;
    }
}
