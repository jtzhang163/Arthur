using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GMCC.Sorter.Dispatcher.Controls.Machine
{
    public class GridHelper
    {
        public static readonly DependencyProperty ShowBorderProperty =
            DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(GridHelper),
        new PropertyMetadata(OnShowBorderChanged));

        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.RegisterAttached("GridLineThickness", typeof(double), typeof(GridHelper),
            new PropertyMetadata(OnGridLineThicknessChanged));

        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.RegisterAttached("GridLineBrush", typeof(Brush), typeof(GridHelper),
            new PropertyMetadata(OnGridLineBrushChanged));

        #region ShowBorder
        public static bool GetShowBorder(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowBorderProperty);
        }
        public static void SetShowBorder(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowBorderProperty, value);
        }
        private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if ((bool)e.OldValue)
            {
                grid.Loaded -= (s, arg) => { };
            }
            if ((bool)e.NewValue)
            {
                grid.Loaded += new RoutedEventHandler(GridLoaded);
            }
        }
        #endregion

        #region GridLineThickness
        public static double GetGridLineThickness(DependencyObject obj)
        {
            return (double)obj.GetValue(GridLineThicknessProperty);
        }
        public static void SetGridLineThickness(DependencyObject obj, double value)
        {
            obj.SetValue(GridLineThicknessProperty, value);
        }
        private static void OnGridLineThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region GridLineBrush
        public static Brush GetGridLineBrush(DependencyObject obj)
        {
            Brush brush = (Brush)obj.GetValue(GridLineBrushProperty);
            return brush == null ? Brushes.LightGray : brush;
        }
        public static void SetGridLineBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(GridLineBrushProperty, value);
        }
        private static void OnGridLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion

        private static void GridLoaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            var row_count = grid.RowDefinitions.Count;
            var column_count = grid.ColumnDefinitions.Count;

            #region 第一版：单元格合并后，无法正确显示GridLine
            var rows = grid.RowDefinitions.Count;
            var columns = grid.ColumnDefinitions.Count;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var border = new Border()
                    {
                        BorderBrush = GetGridLineBrush(grid),
                        BorderThickness = new Thickness(GetGridLineThickness(grid))
                    };
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    grid.Children.Add(border);
                }
            }
            #endregion

            #region 第二版：支持单元格合并，但是无法设置grid cell内元素与边框的距离
            //var controls = grid.Children;
            //var count = controls.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    var item = controls[i] as FrameworkElement;
            //    var border = new Border()
            //    {
            //        BorderBrush = new SolidColorBrush(Colors.YellowGreen),
            //        BorderThickness = new Thickness(1)
            //    };

            //    var row = Grid.GetRow(item);
            //    var column = Grid.GetColumn(item);
            //    var rowspan = Grid.GetRowSpan(item);
            //    var columnspan = Grid.GetColumnSpan(item);

            //    Grid.SetRow(border, row);
            //    Grid.SetColumn(border, column);
            //    Grid.SetRowSpan(border, rowspan);
            //    Grid.SetColumnSpan(border, columnspan);

            //    grid.Children.Add(border);

            //}
            #endregion

            #region 第三版：支持grid cell元素与边框距离设置，但是方法是将cell内元素放到border中，先删除，再添加！
            //var controls = grid.Children;
            //var count = controls.Count;

            //for (int i = 0; i < count; i++)
            //{
            //    var item = controls[i] as FrameworkElement;
            //    var row = Grid.GetRow(item);
            //    var column = Grid.GetColumn(item);
            //    var rowspan = Grid.GetRowSpan(item);
            //    var columnspan = Grid.GetColumnSpan(item);

            //    var settingThickness = GetGridLineThickness(grid);
            //    Thickness thickness = new Thickness(settingThickness / 2);
            //    if (row == 0)
            //        thickness.Top = settingThickness;
            //    if (row + rowspan == row_count)
            //        thickness.Bottom = settingThickness;
            //    if (column == 0)
            //        thickness.Left = settingThickness;
            //    if (column + columnspan == column_count)
            //        thickness.Right = settingThickness;

            //    var border = new Border()
            //    {
            //        BorderBrush = GetGridLineBrush(grid),
            //        BorderThickness = thickness,
            //        Padding = new Thickness(20)
            //    };
            //    Grid.SetRow(border, row);
            //    Grid.SetColumn(border, column);
            //    Grid.SetRowSpan(border, rowspan);
            //    Grid.SetColumnSpan(border, columnspan);


            //    grid.Children.RemoveAt(i);
            //    border.Child = item;
            //    grid.Children.Insert(i, border);
            //}
            #endregion
        }
    }
}
