using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TriangleCalculator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static class banan
		{

			public static void Swap<T>(ref T lhs, ref T rhs)
			{
				T temp = lhs;
				lhs = rhs;
				rhs = temp;
			}
		}

		public struct FloatPoint {
			public float X;
			public float Y;
			public override string ToString()
			{
				return string.Format("Point<X = {0}, Y = {1}>", X.ToString(), Y.ToString());
			}
		}

		public struct Length
		{
			public double length;
			public Length(double length)
			{
				this.length = length;
			}
			
			/*public static implicit operator int(Length d)
			{
				return (int) d.length;
			}*/

			public static implicit operator double(Length d)
			{
				return d.length;
			}
		}

		public struct Angle
		{
			public double angle;
			public double angleR { get { return angle * (Math.PI / 180);  } }
			public Angle(double angle)
			{
				this.angle = angle;
			}

			/*public static implicit operator int(Angle d)
			{
				return (int) d.angle;
			}*/

			public static implicit operator double(Angle d)
			{
				return d.angle;
			}
		}

		public struct Triangle
		{
			public Length? a;
			public Length? b;
			public Length? c;
			public Angle? A;
			public Angle? B;
			public Angle? C;

			public double? sinusRelation
			{
				get
				{
					bool hasAllBInfo = B.HasValue && b.HasValue;
					bool hasAllAInfo = A.HasValue && a.HasValue;
					bool hasAllCInfo = C.HasValue && c.HasValue;
					return hasAllAInfo ? Math.Sin(A.Value.angleR) / a.Value : (hasAllBInfo ? Math.Sin(B.Value.angleR) / b.Value : (hasAllCInfo ? (double?)Math.Sin(C.Value.angleR) / c.Value : null));
				}
			}

			public double? cosRelation
			{
				get
				{
					bool hasAllABInfo = a.HasValue && b.HasValue;
					bool hasAllACInfo = a.HasValue && c.HasValue;
					bool hasAllBCInfo = b.HasValue && c.HasValue;
					return hasAllABInfo ? Math.Pow(a.Value, 2) * Math.Pow(b.Value, 2) - 2 * b.Value * a.Value :
						(hasAllACInfo ? Math.Pow(a.Value, 2) * Math.Pow(c.Value, 2) - 2 * c.Value * a.Value :
						(hasAllBCInfo ? (double?)Math.Pow(b.Value, 2) * Math.Pow(c.Value, 2) - 2 * c.Value * b.Value / c.Value : null));
				}
			}

			public double? sinusRelationAngle
			{
				get
				{
					bool hasAllBInfo = B.HasValue && b.HasValue;
					bool hasAllAInfo = A.HasValue && a.HasValue;
					bool hasAllCInfo = C.HasValue && c.HasValue;
					return hasAllAInfo ? a.Value / Math.Sin(A.Value.angleR) : (hasAllBInfo ? b.Value / Math.Sin(B.Value.angleR) : (hasAllCInfo ? c.Value / (double?)Math.Sin(C.Value.angleR) : null));
				}
			}

			public double? cosRelationAngle
			{
				get
				{
					bool hasAllABInfo = a.HasValue && b.HasValue;
					bool hasAllACInfo = a.HasValue && c.HasValue;
					bool hasAllBCInfo = b.HasValue && c.HasValue;
					return hasAllABInfo ? Math.Pow(a.Value, 2) * Math.Pow(b.Value, 2) - 2 * b.Value * a.Value :
						(hasAllACInfo ? Math.Pow(a.Value, 2) * Math.Pow(c.Value, 2) - 2 * c.Value * a.Value :
						(hasAllBCInfo ? (double?)Math.Pow(b.Value, 2) * Math.Pow(c.Value, 2) - 2 * c.Value * b.Value / c.Value : null));
				}
			}

			public Length? getSide(Angle angle)
			{
				if (sinusRelation.HasValue)
				{
					return new Length(sinusRelation.Value * Math.Sin(angle.angleR));
				}
				else if (cosRelation.HasValue)
				{
					return new Length(cosRelation.Value * Math.Cos(angle.angleR));
				}
				return null;
			}

			public Angle? getAngle(Length length)
			{
				if (sinusRelation.HasValue)
				{
					return new Angle(Math.Asin(sinusRelation.Value * length.length));
				}
				else if (cosRelation.HasValue)
				{
					return new Length(cosRelation.Value * Math.Cos(angle.angleR));
				}
				return null;
			}

			/*Knowing one angle and 2 lengths with 1 length with the same symbol as the angle*/
			public Triangle(TriangleInfo info)
			{
				a = info.a;
				b = info.b;
				c = info.c; 
				A = info.A;
				B = info.B;
				C = info.C;

				a = a.HasValue ? a : getSide(info.A.Value);
				b = b.HasValue ? b : getSide(info.B.Value);
				c = c.HasValue ? c : getSide(info.C.Value);
				A = A.Value;
				B = B.Value;
				C = C.Value;
			}
		}

		public struct TriangleInfo
		{
			public bool hasEnoughInfo
			{
				get
				{
					int infoAngle = (A == null) ? 0 : 1;
					infoAngle += (B == null) ? 0 : 1;
					infoAngle += (C == null) ? 0 : 1;
					int infoLength = (a == null) ? 0 : 1;
					infoLength += (b == null) ? 0 : 1;
					infoLength += (c == null) ? 0 : 1;
					if (infoLength == 3) return true;
					else if (infoLength == 0) return false;
					else if (infoLength < 3)
					{
						if (infoAngle + infoLength >= 3) return true;
					}
					return false;
				}
			}

			

			public Length? a;
			public Length? b;
			public Length? c;
			public Angle? A;
			public Angle? B;
			public Angle? C;
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		public void Window_Loaded(object sender, RoutedEventArgs e)
		{
			drawRelativeLine(new FloatPoint { X = -1f, Y = -1f }, new FloatPoint { X = 1, Y = 1 }, Brushes.White, 10);
			
		}

		public TriangleInfo getInfo()
		{
			TriangleInfo info = new TriangleInfo();
			info.a = (ALength.Text == "") ? (Length?) null : new Length(double.Parse(ALength.Text));
			info.b = (BLength.Text == "") ? (Length?) null : new Length(double.Parse(BLength.Text));
			info.c = (CLength.Text == "") ? (Length?) null : new Length(double.Parse(CLength.Text));
			info.A = (AAngle.Text == "") ? (Angle?) null : new Angle(double.Parse(AAngle.Text));
			info.B = (BAngle.Text == "") ? (Angle?) null : new Angle(double.Parse(BAngle.Text));
			info.C = (CAngle.Text == "") ? (Angle?) null : new Angle(double.Parse(CAngle.Text));
			return info;
		}

		public bool hasEnoughInfo()
		{
			int infoAngle = (AAngle.Text == "") ? 0 : 1;
			infoAngle += (BAngle.Text == "") ? 0 : 1;
			infoAngle += (CAngle.Text == "") ? 0 : 1;
			int infoLength = (ALength.Text == "") ? 0 : 1;
			infoLength += (BLength.Text == "") ? 0 : 1;
			infoLength += (CLength.Text == "") ? 0 : 1;
			if (infoLength == 3) return true;
			else if (infoLength == 0) return false;
			else if (infoLength <= 3)
			{
				if (infoAngle + infoLength >= 3) return true;
			}
			return false;
		}
		bool skipDataChange = false;
		public void dataOnChange(object sender, TextChangedEventArgs e)
		{
			if (skipDataChange) { skipDataChange = false; return; }
			
		}

		public void drawLine(Point pos1, Point pos2, Brush color, int width)
		{
			Line myLine = new Line();
			myLine.X1 = pos1.X;
			myLine.X2 = pos2.X;

			myLine.Y1 = pos1.Y;
			myLine.Y2 = pos2.Y;

			myLine.Stroke = color;
			myLine.StrokeThickness = width;

			ResultCanvas.Children.Add(myLine);
		}

		public void drawRelativeLine(FloatPoint pos1, FloatPoint pos2, Brush color, int width)
		{
			double widthHalf = CanvasGrid.ActualWidth / 2;
			double heightHalf = CanvasGrid.ActualHeight / 2;
			Debug.WriteLine("FloatPos1: {0}, FloatPos2: {1}", pos1, pos2);
			drawLine(new Point { X = pos1.X * widthHalf + widthHalf, Y = pos1.Y * heightHalf + heightHalf }, new Point { X = pos2.X * widthHalf + widthHalf, Y = pos2.Y * heightHalf + heightHalf }, Brushes.White, 10);
		}

		private void calculate_Click(object sender, RoutedEventArgs e)
		{
			TriangleInfo triangleInfo = getInfo();
			statusBarText.Text = triangleInfo.hasEnoughInfo ? "Has enough data" : "Has not enough data";
			if (!triangleInfo.hasEnoughInfo) return;
			skipDataChange = true;
			if (triangleInfo.a != null && triangleInfo.b != null && triangleInfo.C != null)
			{
				Triangle triangle = new Triangle(triangleInfo.a.Value, triangleInfo.b.Value, triangleInfo.C.Value		);
				AAngle.Text = triangle.AAngle.angle.ToString();
				BAngle.Text = triangle.BAngle.angle.ToString();
				CLength.Text = triangle.CLength.length.ToString();
			}
			else if (triangleInfo.a != null && triangleInfo.B != null & triangleInfo.c != null)
			{
				Triangle triangle = new Triangle(triangleInfo.a.Value, triangleInfo.B.Value, triangleInfo.c.Value);
				AAngle.Text = triangle.AAngle.angle.ToString();
				BLength.Text = triangle.BLength.length.ToString();
				CAngle.Text = triangle.CAngle.angle.ToString();
			}
			else if (triangleInfo.A != null && triangleInfo.b != null & triangleInfo.c != null)
			{
				Triangle triangle = new Triangle(triangleInfo.A.Value, triangleInfo.b.Value, triangleInfo.c.Value);
				ALength.Text = triangle.ALength.length.ToString();
				BAngle.Text = triangle.BAngle.angle.ToString();
				CAngle.Text = triangle.CAngle.angle.ToString();
			}
		}
	}
}
