using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2A
{
    public class ModelData : IDataErrorInfo
    {
		//Число узлов сетки
		int h;
		double p1;
		public double getP1
		{
			get
			{
				return p1;
			}
		}
		double p2;
		public delegate double F(double x, double p1, double p2);
		//массив значений в узлах сетки
		ArrayList value;

		public string Error => throw new NotImplementedException();

		public string this[string columnName] => throw new NotImplementedException();

		public ModelData(int h, double p1, double p2, F f)
		{
			this.h = h;
			this.p1 = p1;
			this.p2 = p2;
			value = new ArrayList();
		}
    }
}
