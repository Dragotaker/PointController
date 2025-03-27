using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<PointData> Points { get; } = new ObservableCollection<PointData>();
    public ICommand AddPointCommand { get; }
    public ICommand DeletePointCommand { get; }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        // Инициализация команд
        AddPointCommand = new RelayCommand(AddPoint);
        DeletePointCommand = new RelayCommand(DeleteSelectedPoint);

        // Отключаем автоматическое добавление строк в DataGrid
        PointsGrid.CanUserAddRows = false;

        // Подписка на изменения коллекции
        Points.CollectionChanged += (s, e) => UpdateGraph();
    }

    private void AddPoint(object parameter)
    {
        // Добавляем новую точку с нулевыми координатами
        var newPoint = new PointData();
        newPoint.PropertyChanged += Point_PropertyChanged; // Подписываемся на изменения координат
        Points.Add(newPoint);

        // Фокусируемся на новой строке
        Dispatcher.BeginInvoke(new Action(() =>
        {
            PointsGrid.SelectedItem = newPoint;
            PointsGrid.ScrollIntoView(newPoint);
            PointsGrid.Focus();
        }));
    }

    private void Point_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Обновляем график при изменении координат точки
        if (e.PropertyName == nameof(PointData.X) || e.PropertyName == nameof(PointData.Y))
        {
            UpdateGraph();
        }
    }

    private void DeleteSelectedPoint(object parameter)
    {
        if (PointsGrid.SelectedItem is PointData selectedPoint)
        {
            selectedPoint.PropertyChanged -= Point_PropertyChanged; // Отписываемся от изменений
            Points.Remove(selectedPoint);
        }
        else if (Points.Count > 0)
        {
            var lastPoint = Points[^1];
            lastPoint.PropertyChanged -= Point_PropertyChanged;
            Points.RemoveAt(Points.Count - 1);
        }
    }

    private void UpdateGraph()
    {
        DrawingCanvas.Children.Clear();

        // Рисуем линии
        if (Points.Count > 1)
        {
            var polyline = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };

            foreach (var point in Points)
            {
                polyline.Points.Add(new System.Windows.Point(point.X, point.Y));
            }

            if (Points.Count > 2)
            {
                polyline.Points.Add(new System.Windows.Point(Points[0].X, Points[0].Y));
            }

            DrawingCanvas.Children.Add(polyline);
        }

        // Рисуем точки
        foreach (var point in Points)
        {
            var ellipse = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.Red,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 1
            };

            Canvas.SetLeft(ellipse, point.X - 4);
            Canvas.SetTop(ellipse, point.Y - 4);

            DrawingCanvas.Children.Add(ellipse);
        }
    }
}

public class PointData : INotifyPropertyChanged
{
    private double _x;
    private double _y;

    public double X
    {
        get => _x;
        set
        {
            _x = value;
            OnPropertyChanged(nameof(X));
        }
    }

    public double Y
    {
        get => _y;
        set
        {
            _y = value;
            OnPropertyChanged(nameof(Y));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

    public void Execute(object parameter) => _execute(parameter);

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
