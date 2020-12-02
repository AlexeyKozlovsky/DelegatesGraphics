using DelegatesGraphics.Infrastructure.Commands;
using DelegatesGraphics.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DelegatesGraphics.ViewModels {
    class MainWindowVM : INotifyPropertyChanged {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

        #region Fields
        private MathFunc function;
        private PlotModel plotModel;

        private string startString = "-10";
        private string stopString = "10";
        private string stepString = "1";
        #endregion

        #region Properties
        public MathFunc Function {
            get => function; set {
                function = value;
                OnPropertyChanged("Function");
            }
        }

        public PlotModel PlotModel {
            get => plotModel; set {
                plotModel = value;
                OnPropertyChanged("PlotModel");
            }
        }

        public string StartString {
            get => startString; set {
                startString = value;
                OnPropertyChanged("StartString");
            }
        }

        public string StopString {
            get => stopString; set {
                stopString = value;
                OnPropertyChanged("StopString");
            }
        }

        public string StepString {
            get => stepString; set {
                stepString = value;
                OnPropertyChanged("StepString");
            }
        }
        #endregion

        #region Commands
        private bool Check() {
            try {
                double.Parse(startString);
                double.Parse(stopString);
                double.Parse(stepString);
            } catch {
                return false;
            }

            return true;
        }
        #region DrawLineCommand
        public ICommand DrawLineCommand { get; }
        private void OnDrawLineCommandExecute(object p) => CreateLine();
        private bool OnDrawLineCommandExecuted(object p) => Check();
        #endregion

        #region DrawParabolaCommand
        public ICommand DrawParabolaCommand { get; }
        private void OnDrawParabolaCommandExecute(object p) => CreateParabola();
        private bool OnDrawParabolaCommandExecuted(object p) => Check();
        #endregion

        #region DrawCubeParabola
        public ICommand DrawCubeParabolaCommand { get; }
        private void OnDrawCubeParabolaCommandExecute(object p) => CreateCubeParabola();
        private bool OnDrawCubeParabolaCommandExecuted(object p) => Check();
        #endregion

        #endregion

        #region Methods
        private void UpdatePlotModel(double start, double stop, double step) {
            plotModel = new PlotModel();

            LinearAxis axeX = new LinearAxis();
            axeX.Minimum = start;
            axeX.Maximum = stop;
            axeX.Position = AxisPosition.Bottom;

            LinearAxis axeY = new LinearAxis();
            axeY.Minimum = function.Min.Y;
            axeY.Maximum = function.Max.Y;
            axeY.Position = AxisPosition.Left;

            plotModel.Axes.Add(axeX);
            plotModel.Axes.Add(axeY);

            LineSeries lineSeries = new LineSeries();
            foreach (Point point in function.Points) 
                lineSeries.Points.Add(new DataPoint(point.X, point.Y));

            plotModel.Series.Add(lineSeries);
            OnPropertyChanged("PlotModel");
        }
        private void CreateGraph() {
            double start, stop, step;
            try {
                start = double.Parse(startString);
                stop = double.Parse(stopString);
                step = double.Parse(StepString);
            } catch {
                return;
            }

            function?.Tabulate(start, stop, step);
            UpdatePlotModel(start, stop, step);
        }
        public void CreateLine() {
            function = new MathFunc((double x) => x);
            CreateGraph();
        }

        public void CreateParabola() {
            function = new MathFunc((double x) => Math.Pow(x, 2));
            CreateGraph();
        }

        public void CreateCubeParabola() {
            function = new MathFunc((double x) => Math.Pow(x, 3));
            CreateGraph();
        }
        #endregion

        #region Constructors
        public MainWindowVM() {
            DrawLineCommand = new LambdaCommand(OnDrawLineCommandExecute, OnDrawLineCommandExecuted);
            DrawParabolaCommand = new LambdaCommand(OnDrawParabolaCommandExecute, OnDrawParabolaCommandExecuted);
            DrawCubeParabolaCommand = new LambdaCommand(OnDrawCubeParabolaCommandExecute, OnDrawCubeParabolaCommandExecuted);
        }
        #endregion
    }
}
