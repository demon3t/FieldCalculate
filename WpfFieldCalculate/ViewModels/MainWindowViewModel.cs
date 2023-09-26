using HandyControl.Themes;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfFieldCalculate.Infrastructure;
using WpfFieldCalculate.Models;

namespace WpfFieldCalculate.ViewModels
{
    /// <summary>
    /// ViewModel MainWindow.
    /// </summary>
    internal class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            AddWireCommand = new RelayCommand(OnAddWireCommandExecuted, CanAddWireCommandExecute);
            DeleteWireCommand = new RelayCommand(OnDeleteWireCommandExecuted, CanDeleteWireCommandExecute);

            Grapf = new PlotModel()
            {
                TextColor = OxyColor.FromArgb(255, 50, 109, 242),
                PlotAreaBorderColor = OxyColor.FromArgb(255, 50, 109, 242),
                Axes =
                {
                    new LinearAxis
                    {
                        Title = "X",
                        TitleColor = OxyColors.Transparent,
                        MinorTicklineColor = OxyColor.FromArgb(255, 50,109, 242),
                        TicklineColor = OxyColor.FromArgb(255, 50,109, 242),
                        Position = AxisPosition.Bottom,
                        IsZoomEnabled = false,
                        MajorGridlineThickness = 1,
                        MajorGridlineStyle = LineStyle.Solid,
                        MajorGridlineColor = OxyColors.LightBlue,
                    },
                    new LinearAxis
                    {
                        Title = "Y ",
                        TitleColor = OxyColors.Transparent,
                        MinorTicklineColor =OxyColor.FromArgb(255, 50, 109, 242),
                        TicklineColor = OxyColor.FromArgb(255, 50,109, 242),
                        Position = AxisPosition.Left,
                        IsZoomEnabled = false,
                        MajorGridlineThickness = 1,
                        MajorGridlineStyle = LineStyle.Solid,
                        MajorGridlineColor = OxyColors.LightBlue,
                    },
                },
                
            };

            InputData = new InputData();
            OutputData = new OutputData(InputData);

            foreach (var w in InputData.Wires)
            {
                w.PropertyChanged += (sender, e) =>
                {
                    UpdateGrapf();
                };
            }

            InputData.Wires.CollectionChanged += (sender, e) =>
            {
                UpdateGrapf();
            };

            UpdateGrapf();
        }

        private void UpdateGrapf()
        {
            Grapf.Series.Clear();
            Grapf.Annotations.Clear();

            foreach (var wire in InputData.Wires)
            {
                DrowVector(wire);
                DrowWire(wire);
                DrowInduction(wire);
            }

            DrowSummaryInduction();

            SetMargin();
            Grapf.InvalidatePlot(true);
        }

        private void DrowSummaryInduction()
        {
            var arrow = new ArrowAnnotation
            {
                StartPoint = new DataPoint(0, 0),
                EndPoint = new DataPoint(OutputData.ToComplex.Real * 100000000, OutputData.ToComplex.Imaginary * 100000000),
                Color = OxyColors.Blue,
                HeadLength = 7,
                HeadWidth = 2
            };

            Grapf.Annotations.Add(arrow);
        }

        private void SetMargin()
        {
            var allPoints = Grapf.Series
                .OfType<LineSeries>()
                .SelectMany(series => series.Points)
                .Concat(Grapf.Annotations
                    .OfType<ArrowAnnotation>()
                    .SelectMany(annotation => new[] { annotation.StartPoint, annotation.EndPoint }));

            var minX = allPoints.Min(point => point.X);
            var minY = allPoints.Min(point => point.Y);
            var maxX = allPoints.Max(point => point.X);
            var maxY = allPoints.Max(point => point.Y);

            // Вычисляем размеры текущей области графика
            var currentXRange = maxX - minX;
            var currentYRange = maxY - minY;

            // Устанавливаем соотношение сторон (5:3)
            var aspectRatio = 5.0 / 3.0;

            // Вычисляем размеры новой области графика
            var targetXRange = Math.Max(currentXRange, currentYRange * aspectRatio);
            var targetYRange = targetXRange / aspectRatio;

            // Рассчитываем новые значения для минимумов и максимумов обоих осей
            var centerX = (minX + maxX) / 2.0;
            var centerY = (minY + maxY) / 2.0;
            var newXMin = centerX - targetXRange / 2.0 - 20; // 20 - отступ
            var newXMax = centerX + targetXRange / 2.0 + 20;
            var newYMin = centerY - targetYRange / 2.0 - 20; // 20 - отступ
            var newYMax = centerY + targetYRange / 2.0 + 20;

            // Устанавливаем новые значения для осей X и Y
            Grapf.Axes[0].Maximum = newXMax;
            Grapf.Axes[0].Minimum = newXMin;

            Grapf.Axes[1].Maximum = newYMax;
            Grapf.Axes[1].Minimum = newYMin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wire"></param>
        private void DrowInduction(WireData wire)
        {
            if (wire.InductionAbs == 0)
            {
                return;
            }

            var arrow = new ArrowAnnotation
            {
                StartPoint = new DataPoint(0, 0),
                EndPoint = new DataPoint(wire.ToCompex.Real * 100000000, wire.ToCompex.Imaginary * 100000000),
                Color = OxyColors.Blue,
                HeadLength = 7,
                HeadWidth = 2
            };

            Grapf.Annotations.Add(arrow);
        }

        /// <summary>
        /// Отобразить линии до точки расчёта.
        /// </summary>
        /// <param name="wire">Данные провода</param>
        private void DrowVector(WireData wire)
        {
            var vector = new LineSeries()
            {
                MarkerType = MarkerType.None,
                LineStyle = LineStyle.LongDash,
                StrokeThickness = 1.5,
                Color = OxyColors.Blue,
                RenderInLegend = false,
            };

            vector.Points.Add(new DataPoint(wire.Coordinate.X, wire.Coordinate.Y));
            vector.Points.Add(new DataPoint(0, 0));

            Grapf.Series.Add(vector);
        }

        /// <summary>
        /// Отобразить провода.
        /// </summary>
        /// <param name="wire">Данные провода.</param>
        private void DrowWire(WireData wire)
        {
            var circle = new LineSeries()
            {
                MarkerType = MarkerType.Circle,
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Solid,
                MarkerSize = 10,
                MarkerFill = OxyColors.White,
                MarkerStrokeThickness = 2,
                MarkerStroke = OxyColors.Blue, 
                RenderInLegend = false,
            };

            circle.Points.Add(new DataPoint(wire.Coordinate.X, wire.Coordinate.Y));

            Grapf.Series.Add(circle);

            if (wire.I != 0)
            {
                var mark = new LineSeries()
                {
                    MarkerType = wire.I > 0 ? MarkerType.Cross : MarkerType.Circle,
                    Color = OxyColors.Blue,
                    LineStyle = LineStyle.Solid,
                    MarkerSize = 3,
                    MarkerFill = OxyColors.Blue,
                    MarkerStrokeThickness = 2,
                    MarkerStroke = OxyColors.Blue,
                    RenderInLegend = false,
                };

                mark.Points.Add(new DataPoint(wire.Coordinate.X, wire.Coordinate.Y));

                Grapf.Series.Add(mark);
            }
        }

        /// <summary>
        /// График.
        /// </summary>
        public PlotModel Grapf { get; }

        /// <summary>
        /// Выбранная запись.
        /// </summary>
        public WireData SelectedWire { get; set; }

        /// <summary>
        /// Входные данные.
        /// </summary>
        public InputData InputData
        {
            get { return _inputData; }
            set { Set(ref _inputData, value); }
        }

        private InputData _inputData;

        /// <summary>
        /// Расчётные данные.
        /// </summary>
        public OutputData OutputData
        {
            get { return _outputData; }
            set { Set(ref _outputData, value); }
        }

        private OutputData _outputData;


        #region Добавить провод

        public ICommand AddWireCommand { get; }

        private bool CanAddWireCommandExecute(object p)
        {
            return true;
        }

        private void OnAddWireCommandExecuted(object p)
        {
            var w = new WireData(0, new Point(0, 0))
            {
                Name = $"Провод {InputData.Wires.Count}"
            };

            w.PropertyChanged += (sender, e) =>
            {
                UpdateGrapf();
            };

            InputData.Wires.Add(w);

            UpdateGrapf();
        }

        #endregion

        #region Удалить провод

        public ICommand DeleteWireCommand { get; }

        private bool CanDeleteWireCommandExecute(object p)
        {
            return SelectedWire is not null && InputData.Wires.Count > 1;
        }

        private void OnDeleteWireCommandExecuted(object p)
        {
            var index = InputData.Wires.IndexOf(SelectedWire) == 0 ? 0 : InputData.Wires.IndexOf(SelectedWire) - 1;
            InputData.Wires.Remove(SelectedWire);

            SelectedWire = InputData.Wires[index];
        }

        #endregion
    }
}
