using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab_2A;
using System.IO;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;

namespace Wpf_Lab2A
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			DataContext = new ObservableModelData();
			InitializeComponent();
			LoadLineChartData();
		}
		private void LoadLineChartData()
		{
			((LineSeries)mcChart.Series[0]).ItemsSource =
				new KeyValuePair<DateTime, int>[]{
			new KeyValuePair<DateTime, int>(DateTime.Now, 100),
			new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(1), 130),
			new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(2), 150),
			new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(3), 125),
			new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(4), 155) };
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
		
	}
}
