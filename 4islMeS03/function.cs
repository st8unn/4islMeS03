using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4islMeS03
{
    class func
    {
        //тестовые функции варианта 2 и 4.
        public double a=-1;
        public double b=1;
        public double c=-1;
        public double d=1;
        bool styl;
        public func()
        {
            styl = true;
        }

        public double leftF(double y)
        {
            double result = 0;
            //result = -y * y + 1;
            if(styl)
                result = -y * y;
            else
                //result = 1;
                result = Math.Sin(Math.PI * a * y);
            return result;
        }
        public double rigthF(double y)
        {
            double result = 0;
            //result = -y * y + 1;
            if(styl)
                result = -y * y;
            else
                //result = Math.Exp(Math.Pow(Math.Sin(Math.PI * y), 2));
                result = Math.Sin(Math.PI * b * y);
            return result;
        }
        public double bottomF(double x)
        {
            double result = 0;
            //result = Math.Abs(Math.Sin(Math.PI * x));
            if (styl)
                result = -x * x;
            else
                //result = 1;
                result = Math.Sin(Math.PI * c * x);
            return result;
        }
        public double topF(double x)
        {
            double result = 0;
            if (styl)
            //result = Math.Abs(Math.Sin(Math.PI * x));
                result = -x * x;
            else
            {
                //result = Math.Exp(Math.Pow(Math.Sin(Math.PI * x), 2));
                result = Math.Sin(Math.PI * d * x);
            }
            return result;
        }
        public double Heat(double x,double y)
        {
            double result = 0;
            //result = Math.Abs(Math.Pow((Math.Sin(Math.PI * x * y)), 3));
            if (styl)
                result = -4;
            else
            {
                /*double temp1 = Math.Exp(Math.Pow(Math.Sin(Math.PI * x), 2));
                result += 2 * temp1 * Math.Pow(Math.PI * x * Math.Cos(Math.PI * x * y), 2);
                result += 2 * temp1 * Math.Pow(Math.PI * y * Math.Cos(Math.PI * x * y), 2);
                result -= 2 * temp1 * Math.Pow(Math.PI * x * Math.Sin(Math.PI * x * y), 2);
                result -= 2 * temp1 * Math.Pow(Math.PI * y * Math.Sin(Math.PI * x * y), 2);
                result += 4 * temp1 * Math.Pow(Math.PI * x * Math.Sin(Math.PI * x * y) * Math.Cos(Math.PI * x * y), 2);
                result += 4 * temp1 * Math.Pow(Math.PI * y * Math.Sin(Math.PI * x * y) * Math.Cos(Math.PI * x * y), 2);*/
                result = -Math.Pow(Math.PI * x, 2) * Math.Sin(Math.PI * x * y) - Math.Pow(Math.PI * y, 2) * Math.Sin(Math.PI * x * y);
            }
            return result;
        }
        public double Pres(double x, double y)
        {
            double result = 0;
            if (styl)
                result = 1 - x * x - y * y;
            else
                //result = Math.Exp(Math.Pow(Math.Sin(Math.PI * x * y), 2));
                result = Math.Sin(Math.PI * x * y);
            return result;
        }
        public void Changer(bool t)
        {
            styl = t;
            if (t)
            {
                a=-1;
                b=1;
                c=-1;
                d=1;
            }
            else
            {
                /*a = 0;
                b = 1;
                c = 0;
                d = 1;*/
                a = 1;
                b = 2;
                c = 2;
                d = 3;
            }
        }
    }
    class func1
    {
        public double a = 0;
        public double b = 1;
        public double c = 0;
        public double d = 1;
        public func1()
        {

        }
        public double leftF(double y)
        {
            double result = 0;
            //result = -y * y + 1;
            result = 1;
            return result;
        }
        public double rigthF(double y)
        {
            double result = 0;
            //result = -y * y + 1;
            result = Math.Exp(Math.Pow(Math.Sin(Math.PI * y),2));
            return result;
        }
        public double bottomF(double x)
        {
            double result = 0;
            //result = Math.Abs(Math.Sin(Math.PI * x));
            result = 1;
            return result;
        }
        public double topF(double x)
        {
            double result = 0;
            //result = Math.Abs(Math.Sin(Math.PI * x));
            result = Math.Exp(Math.Pow(Math.Sin(Math.PI * x), 2));
            return result;
        }
        public double Heat(double x, double y)
        {
            double result = 0;
            //result = Math.Abs(Math.Pow((Math.Sin(Math.PI * x * y)), 3));
            //result = -4;
            double temp1 = Math.Exp(Math.Pow(Math.Sin(Math.PI * x), 2));
            result += 2 * temp1 * Math.Pow(Math.PI * x * Math.Cos(Math.PI * x * y), 2);
            result += 2 * temp1 * Math.Pow(Math.PI * y * Math.Cos(Math.PI * x * y), 2);
            result -= 2 * temp1 * Math.Pow(Math.PI * x * Math.Sin(Math.PI * x * y), 2);
            result -= 2 * temp1 * Math.Pow(Math.PI * y * Math.Sin(Math.PI * x * y), 2);
            result += 4 * temp1 * Math.Pow(Math.PI * x * Math.Sin(Math.PI * x * y) * Math.Cos(Math.PI * x * y), 2);
            result += 4 * temp1 * Math.Pow(Math.PI * y * Math.Sin(Math.PI * x * y) * Math.Cos(Math.PI * x * y), 2);
            return result;
        }
        public double Pres(double x, double y)
        {
            double result = 0;
            result = Math.Exp(Math.Pow(Math.Sin(Math.PI*x*y),2));
            return result;
        }
    }
}
