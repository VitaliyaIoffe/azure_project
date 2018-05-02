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
		F f;
		public double getP2
		{
			get
			{
				return p2;
			}
		}
		public delegate double F(double x, double p1, double p2);
		//массив значений в узлах сетки
		public ArrayList getValue
		{
			get
			{
				return value;
			}
		}
		ArrayList value;
		public ArrayList getX
		{
			get
			{
				return x;
			}
		}
		ArrayList x;
		public void getValues()
		{
			double temp = 1.0 / h;
			for(double i = 0; i < 1; i += temp)
			{
				x.Add(i);
				value.Add(f(i, p1, p2));
			}
		}
		public string Error => throw new NotImplementedException();
		 
		public string this[string columnName]
		{
			get
			{
				string error = String.Empty;
				switch (columnName)
				{
					case "h":
						if (h <= 2)
						{
							error = "Число узлов сетки <=2";
						}
						break;
				}
				return error;
			}
		}

		public ModelData(int h, double p1, double p2, F f)
		{
			this.h = h;
			this.p1 = p1;
			this.p2 = p2;
			x = new ArrayList();
			value = new ArrayList();
			this.f = new F(f);
			getValues();
		}
		public override String ToString()
		{
			return "h = " + h.ToString() + " p1 = " + p1.ToString() + " p2 = " + p2.ToString();
		}
    }
}
