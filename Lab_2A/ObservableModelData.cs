using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2A
{
	public class ObservableModelData : ObservableCollection<ModelData>
	{
		bool change;
		public bool isChange
		{
			get
			{
				return change;
			}
			set
			{
				change = value;
			}
		}
		IEnumerable<ModelData> SubsetP1(ModelData md)
		{
			return from item in base.Items where item.getP1 == md.getP1 select item;
		}
	}
}
