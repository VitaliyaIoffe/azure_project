using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab_2A;
using System.Drawing;
using System.Windows.Forms.Design;
using System.IO;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms.DataVisualization.Charting;

namespace Wpf_Lab2A
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	
	public partial class MainWindow : Window
	{
		double Function(double x, double p1, double p2)
		{
			return x * p1 * p2;
		}
		Chart chart = new Chart();

		public static RoutedCommand AddElement = new RoutedCommand();
		public static RoutedCommand DrawSelectedCommand = new RoutedCommand();
		public static RoutedCommand DrawEqualsCommand = new RoutedCommand();
		public MainWindow()
		{
			DataContext = new ObservableModelData();
			InitializeComponent();
			winFormsHost.Child = chart;
		}
		private void ExecutedDrawElement(object sender, ExecutedRoutedEventArgs e)
		{
			chart.Series.Clear();
			chart.ChartAreas.Add(new ChartArea());
			chart.Series.Add("Series" + chart.Series.Count.ToString());
			chart.Series[chart.Series.Count - 1].Points.DataBindXY((ModelList.SelectedItem as ModelData).getX, (ModelList.SelectedItem as ModelData).getValue);
			chart.Series[chart.Series.Count - 1].ChartType = SeriesChartType.Spline;
			chart.Series[chart.Series.Count - 1].MarkerStyle = MarkerStyle.Square;
			chart.Series[chart.Series.Count - 1].MarkerSize = 5;
			chart.Series[chart.Series.Count - 1].Color = Color.Red;
			chart.Legends.Add("Legend" + chart.Legends.Count.ToString());
			chart.Series[chart.Series.Count - 1].LegendText = "P1 = " + (ModelList.SelectedItem as ModelData).getP1.ToString() + " P2 = " + (ModelList.SelectedItem as ModelData).getP2.ToString();
			for (int j = 0; j < chart.Series[chart.Series.Count - 1].Points.Count; j++)
				chart.Series[chart.Series.Count - 1].Points[j].Label =
				chart.Series[chart.Series.Count - 1].Points[j].YValues[0].ToString("F3");
		}

		private void CanExecuteAddElement(object sender, CanExecuteRoutedEventArgs e)
		{
			
			if (Validation.GetHasError(SetH))
			{
				e.CanExecute = false;
			}
			else
			{
				e.CanExecute = true;
			}
		}
		private void ExecutedEqualsElement(object sender, ExecutedRoutedEventArgs e)
		{
			chart.Series.Clear();
			foreach (var item in (DataContext as ObservableModelData).SubsetP1((ModelList.SelectedItem as ModelData)))
			{
				chart.ChartAreas.Add(new ChartArea());
				chart.Series.Add("Series" + chart.Series.Count.ToString());
				chart.Series[chart.Series.Count - 1].Points.DataBindXY(item.getX, item.getValue);
				chart.Series[chart.Series.Count - 1].ChartType = SeriesChartType.Spline;
				chart.Series[chart.Series.Count - 1].Color = Color.Red;
				chart.Legends.Add("Legend" + chart.Legends.Count.ToString());
				chart.Series[chart.Series.Count - 1].LegendText = " P2 = " + item.getP2.ToString();
				for (int j = 0; j < chart.Series[chart.Series.Count - 1].Points.Count; j++)
					chart.Series[chart.Series.Count - 1].Points[j].Label =
					chart.Series[chart.Series.Count - 1].Points[j].YValues[0].ToString("F3");
			}
		}

		private void CanExecuteEqualsElement(object sender, CanExecuteRoutedEventArgs e)
		{

			if (ModelList.SelectedItem != null)
			{
				e.CanExecute = true;
			}
			else
			{
				e.CanExecute = false;
			}
		}
		private void ExecutedAddElement(object sender, ExecutedRoutedEventArgs e)
		{
			String[] p = SetP.Text.Split(',');
			(DataContext as ObservableModelData).Add(new ModelData(Convert.ToInt32(SetH.Text), Convert.ToDouble(p[0]), Convert.ToDouble(p[1]), Function));
		}
	
		private void CanExecuteDrawElement(object sender, CanExecuteRoutedEventArgs e)
		{

			if (ModelList.SelectedItem != null)
			{
				e.CanExecute = true;
			}
			else
			{
				e.CanExecute = false;
			}
		}
		private void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				DataContext = new ObservableModelData();
				InitializeComponent();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
		}
		private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Stream fileStream = null;
			try
			{
				SaveFileDialog dialog = new SaveFileDialog();
				if (dialog.ShowDialog() == true)
				{
					fileStream = dialog.OpenFile();
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.Serialize(fileStream, DataContext as ObservableModelData);
					this.InitializeComponent();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
			finally
			{
				fileStream.Close();
			}
		}
		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Stream fileStream = null;
			try
			{
				OpenFileDialog dialog = new OpenFileDialog();
				if (dialog.ShowDialog() == true)
				{
					fileStream = dialog.OpenFile();
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					DataContext = binaryFormatter.Deserialize(fileStream) as ObservableModelData;
					this.InitializeComponent();
				}
				else
					throw new Exception("Error");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
			finally
			{
				fileStream.Close();
			}
		}
		private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				(DataContext as ObservableModelData).Remove((ModelData)ModelList.SelectedItem);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
		}

		private void CanDelete_Execute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (DataContext as ObservableModelData).Contains((ModelData)ModelList.SelectedItem);
		}
		private void CanSave_Execute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (DataContext as ObservableModelData).isChange;
		}

		private void SetP_MouseEnter(object sender, MouseEventArgs e)
		{
			SetP.Text = null;
		}
	}
}
