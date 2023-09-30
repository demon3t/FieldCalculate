using HandyControl.Themes;
using HandyControl.Tools.Converter;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
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
            InitializeCommand();
            InitializeGraph();
            InitializeData();

            UpdateCommand?.Execute(null);
        }

        /// <summary>
        /// Инициализация графика/
        /// </summary>
        private void InitializeGraph()
        {
            Graph = new PlotModel()
            {
                TextColor = OxyColors.Black,
                PlotAreaBorderColor = OxyColors.Black,
                Axes =
                {
                    new LinearAxis
                    {
                        Title = "x",
                        Position = AxisPosition.Bottom,
                        IsZoomEnabled = false,
                        TitleColor = OxyColors.Transparent,
                        MinorTicklineColor = OxyColors.Transparent,
                        TicklineColor = OxyColors.Black,
                        MajorGridlineThickness = 1,
                        MajorGridlineStyle = LineStyle.Solid,
                        MajorGridlineColor = OxyColors.LightGray,
                    },
                    new LinearAxis
                    {
                        Title = "y",
                        Position = AxisPosition.Left,
                        IsZoomEnabled = false,
                        TitleColor = OxyColors.Transparent,
                        MinorTicklineColor = OxyColors.Transparent,
                        TicklineColor = OxyColors.Black,
                        MajorGridlineThickness = 1,
                        MajorGridlineStyle = LineStyle.Solid,
                        MajorGridlineColor = OxyColors.LightGray,
                    },
                },
            };
        }



        /// <summary>
        /// Инициализация команд.
        /// </summary>
        private void InitializeCommand()
        {
            AddWireCommand = new RelayCommand(OnAddWireCommandExecuted, CanAddWireCommandExecute);
            DeleteWireCommand = new RelayCommand(OnDeleteWireCommandExecuted, CanDeleteWireCommandExecute);
            UpdateCommand = new RelayCommand(OnUpdateCommandExecuted, CanUpdateCommandExecute);
            EscapeCommand = new RelayCommand(OnEscapeCommandExecuted, CanEscapeCommandExecute);
        }

        /// <summary>
        /// Инициализация данных.
        /// </summary>
        private void InitializeData()
        {
            InputData = new InputData();
            OutputData = new OutputData(InputData);
            Graph.Annotations.Add(OutputData.Arrow);

            InputData.Wires.CollectionChanged += (sender, e) =>
            {
                UpdateCommand?.Execute(null);
            };
        }

        #region Свойства

        /// <summary>
        /// График.
        /// </summary>
        public PlotModel Graph { get; private set; }

        /// <summary>
        /// Выбранная запись.
        /// </summary>
        public WireData SelectedWire
        { 
            get{ return _selectedWire; }
            set
            {
                if (_selectedWire != null)
                {
                    _selectedWire.OnUnselecteted();
                }

                if (value != null)
                {
                    value.OnSelecteted();
                }

                Set(ref _selectedWire, value);
            }
        }

        private WireData _selectedWire;

        /// <summary>
        /// Входные данные.
        /// </summary>
        public InputData InputData
        {
            get { return _inputData; }
            private set { Set(ref _inputData, value); }
        }

        private InputData _inputData;

        /// <summary>
        /// Расчётные данные.
        /// </summary>
        public OutputData OutputData
        {
            get { return _outputData; }
            private set { Set(ref _outputData, value); }
        }

        private OutputData _outputData;

        #endregion

        private void SetMargin()
        {
            var allPoints = Graph.Series
                .OfType<LineSeries>()
                .SelectMany(series => series.Points)
                .Concat(Graph.Annotations
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
            var newXMin = centerX - targetXRange / 2.0 - 50; // 20 - отступ
            var newXMax = centerX + targetXRange / 2.0 + 50;
            var newYMin = centerY - targetYRange / 2.0 - 50; // 20 - отступ
            var newYMax = centerY + targetYRange / 2.0 + 50;

            // Устанавливаем новые значения для осей X и Y
            Graph.Axes[0].Maximum = newXMax;
            Graph.Axes[0].Minimum = newXMin;

            Graph.Axes[1].Maximum = newYMax;
            Graph.Axes[1].Minimum = newYMin;
        }

        #region EscapeCommand

        public ICommand EscapeCommand { get; private set; }

        private bool CanEscapeCommandExecute(object p)
        {
            return true;
        }

        private void OnEscapeCommandExecuted(object p)
        {
            SelectedWire = null;
        }

        #endregion

        #region Обновить данные

        public ICommand UpdateCommand { get; private set; }

        private bool CanUpdateCommandExecute(object p)
        {
            return InputData.Wires.Count != 0;
        }

        private void OnUpdateCommandExecuted(object p)
        {
            OutputData.Caculate();
            OutputData.UpdateStyle();
            OutputData.UpdateCoordinate();

            InputData.UpdateStyle();
            InputData.UpdateCoordinate();

            SetMargin();
            Graph.InvalidatePlot(true);
        }

        #endregion

        #region Добавить провод

        public ICommand AddWireCommand { get; private set; }

        private bool CanAddWireCommandExecute(object p)
        {
            return true;
        }

        private void OnAddWireCommandExecuted(object p)
        {
            var r = new Random();

            var wire = new WireData(Graph, InputData)
            {
                I = r.Next(-200, 200),
                X = r.Next(-200, 200),
                Y = r.Next(-200, 200),
                Name = $"Провод {InputData.Wires.Count + 1}"
            };

            InputData.Wires.Add(wire);

            wire.PropertyChanged += (sender, e) =>
            {
                UpdateCommand?.Execute(null);
            };

            UpdateCommand?.Execute(null);
        }

        #endregion

        #region Удалить провод

        public ICommand DeleteWireCommand { get; private set; }

        private bool CanDeleteWireCommandExecute(object p)
        {
            return SelectedWire is not null;
        }

        private void OnDeleteWireCommandExecuted(object p)
        {
            var index = InputData.Wires.IndexOf(SelectedWire) == 0 ?
                0 : InputData.Wires.IndexOf(SelectedWire) - 1;

            Graph.Series.Remove(SelectedWire.Circle);
            Graph.Series.Remove(SelectedWire.Mark);
            Graph.Series.Remove(SelectedWire.Vector);
            Graph.Annotations.Remove(SelectedWire.Arrow);

            InputData.Wires.Remove(SelectedWire);

            if (InputData.Wires.Count != 0)
            {
                SelectedWire = InputData.Wires[index];
            }
        }

        #endregion
    }
}
