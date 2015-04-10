using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4islMeS03
{
    class grs04
    {
        public int vIteration = 0;
        int vSystem=0;
        public double vDMeth;
        public double vDSol;
        func equ1=new func();
        bool triggerB=false;
        bool triggerA=false;
        double[] massSolution;
        double[] massSolutionPrev;
        int vNumX;
        int vNumY;
        double vStepX;
        double vStepY;
        double[] massF;
        double[] massH;
        double[] massR;
        public grs04() { }
        double[,] massF2d;//воздействие
        double[,] massSolution2d;//координаты/тепло
        public double[,] massD2d;
        public void chang(bool t)
        {
            equ1.Changer(t);
        }
        //расчёт воздействия
        public void massFfinder()
        {
            double minX = Math.Min(equ1.a, equ1.b);
            double maxX = Math.Max(equ1.a, equ1.b);
            double minY = Math.Min(equ1.c, equ1.d);
            double maxY = Math.Max(equ1.d, equ1.d);
            vStepX = (maxX - minX) / vNumX;
            vStepY = (maxY - minY) / vNumY;
            massF = new double[(vNumX - 1) * (vNumY - 1)];
            massH = new double[(vNumX - 1) * (vNumY - 1)];
            massF2d = new double[vNumX - 1, vNumY - 1];
            /*сначала заполнять по f*/
            int N = 0;
            for (int j = 1; j < vNumY; j++)
            {
                for (int i = 1; i < vNumX; i++)
                {
                    massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    N++;
                }
            }
            //затем все точки граничные вписать
            //сначала вписываем нижнюю - для всех от 1 до vNumX-1; правда здесь вектор смещён на 1 вверх
            for (int i = 1; i < vNumX; i++)
            {
                massF2d[i - 1, 0] -= equ1.bottomF(minX + i * vStepX) / (vStepY * vStepY);//X или Y?
                massF[i - 1] -= equ1.bottomF(minX + i * vStepX) / (vStepY * vStepY);
            }
            int coord = (vNumX - 1) * (vNumY - 2);
            for (int i = 1; i < vNumX; i++)
            {
                massF2d[i - 1, vNumY - 2] -= equ1.topF(minX + i * vStepX) / (vStepY * vStepY);
                massF[coord + i - 1] -= equ1.topF(minX + i * vStepX) / (vStepY * vStepY);
            }
            //лево - шаг через vNumX
            coord = 0;
            for (int i = 1; i < vNumY; i++)
            {
                massF2d[0, i - 1] -= equ1.leftF(minY + i * vStepY) / (vStepX * vStepX);
                coord = (i - 1) * (vNumX - 1);
                massF[coord] -= equ1.leftF(minY + i * vStepY) / (vStepX * vStepX);
            }
            //право
            coord = 0;
            for (int i = 1; i < vNumY; i++)
            {
                    massF2d[vNumX - 2, i - 1] -= equ1.rigthF(minY + i * vStepY) / (vStepX * vStepX);
                    coord = (vNumX - 1) + (i - 1) * (vNumX - 1);
                    massF[coord - 1] -= equ1.rigthF(minY + i * vStepY) / (vStepX * vStepX);
            }
            //вроде все точки расставлены.
            //отладка, удалить.
            double[,] massD2d2 = new double[vNumX - 1, vNumY - 1];
            N = 0;
            /*
            for (int j = 1; j < vNumY; j++)
            {
                for (int i = 1; i < vNumX; i++)
                {
                    massD2d2[i - 1, j - 1] = massF2d[i - 1, j - 1] - massF[N];
                    massF[N] = massF2d[i - 1, j - 1];
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    N++;
                }
            }*/

            for (int i = 1; i < vNumX; i++)
            {
                for (int j = 1; j < vNumY; j++)
                {
                    massD2d2[i - 1, j - 1] = massF2d[i - 1, j - 1] - massF[N];
                    massF[N] = massF2d[i - 1, j - 1];
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    N++;
                }
            }
            N = 0;
        }
        //даже не уверен, что это. Якобы значение тепла в точке
        double fGiveP2d(int i, int j)
        {
            if ((i == 0) || (i == vNumX) || (j == 0) || (j == vNumY))
                return 0;
            double k = massSolution2d[i-1, j-1];
            return k;
        }
        //каждый шаг для Зейделя
        public double prStep()
        {//фор легче для понимания сделать двойным
            double result=0;
            double temp1,temp2;
            int N=0;
            double koefL=1/(vStepX*vStepX);
            double koefD=1/(vStepY*vStepY);
            double koefA=-2*(koefL+koefD);
            for (int j = 1; j < vNumY; j++)
            {
                for (int i = 1; i < vNumX; i++)
                {
                    //он умеет делить на ноль, увы.
                    /*if ((koefA * massSolution[N] + fGiveP(i - 1, j) * koefL + fGiveP(i, j - 1) * koefD) == 0)
                    {
                        massSolution[N] = 1;
                    }
                    else*/
                    //всё, что справа, то есть правее и выше? На свои коэффициенты - всё, что слева и снизу. А в общем пофиг.

                    
                    //massSolution[N] = (massF[N] - fGiveP(i + 1, j) * koefL - fGiveP(i, j + 1) * koefD) / (koefA * massSolution[N] + fGiveP(i - 1, j) * koefL + fGiveP(i, j - 1) * koefD);
                    {
                        temp1 = massSolution2d[i - 1, j - 1];
                        double mf = massF2d[i - 1, j - 1];

                        double mr = -fGiveP2d(i + 1, j);
                        double mu = -fGiveP2d(i, j + 1);
                        double ml = -fGiveP2d(i - 1, j);
                        double md = -fGiveP2d(i, j - 1);
                        massSolution2d[i - 1, j - 1] = (mf + mr * koefL + mu * koefD + ml * koefL + md * koefD) / koefA;
                        temp2 = (mf + mr * koefL + mu * koefD + ml * koefL + md * koefD) / koefA;
                        result = Math.Max(result, Math.Abs(temp2 - temp1));
                        //massSolution2d[i - 1, j - 1] = (massF[N] - fGiveP2d(i + 1, j) * koefL - fGiveP2d(i, j + 1) * koefD - fGiveP2d(i - 1, j) * koefL - fGiveP2d(i, j - 1) * koefD) / koefA;
                    }
                    N++;
                }
             
            }
            for (int i = 1; i <= (vStepX-1)*(vStepY-1); i++)
            {
               
            }
            return result;
        }
        //вычисляет воздействие в точке
        public double Calc(int i,int j)
        {
            //слишком много перестраховываюсь
            /*double minX = Math.Min(equ1.a, equ1.b);
            double maxX = Math.Max(equ1.a, equ1.b);
            double minY = Math.Min(equ1.c, equ1.d);
            double maxY = Math.Max(equ1.d, equ1.d);*/
            double minX = equ1.a;
            double minY = equ1.c;
            double x = minX + vStepX * i;
            double y = minY + vStepY * j;
            //return 1 - x * x - y * y;
            return equ1.Pres(x, y);
        }
        //тоже для Зейделя
        public double Calculation(int X, int Y,int N,double eps)
        {
/*            if (vSystem == 0)
            {
                equ1 = new func();
            }*/
            double result = 0;
            vNumX = X;
            vNumY = Y;
            int NMax = N;
            vIteration = 0;
            massFfinder();

            massSolution2d = new double[vNumX - 1, vNumY - 1];
            for (int i = 0; i < NMax; i++)
            {
                //double temp1 = prStep();
                vIteration = i+1;
                result = prStep();
                if (result < eps)
                    break;
            }
            double Dif=0;
            double temp1;
            massD2d = new double[vNumX - 1, vNumY - 1];
            for (int j = 1; j < vNumY; j++)
            {
                for (int i = 1; i < vNumX; i++)
                {
                    temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    Dif=Math.Max(Dif,temp1);
                    massD2d[i - 1, j - 1] = temp1;
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    N++;
                }
            }
            vDSol = Dif;
            vDMeth = result;
            return result;
        }
        public double GradStep()
        {
            double[] temp = new double[(vNumX - 1) * (vNumY - 1)];
            //есть шанс, что при равенстве он просто присваивает ссылку
            temp = massSolution;
            //разница между шагами
            double result = 0;
            //невязка
            massR = VectorSum(VectorMulti(massSolution), massF, 1, -1);
            //вычисляем бета по невязке и предыдущему значению
            double betha = vBetha(massH,massR);
            
            //massH = VectorSum(VectorMulti(massSolution), massF, -1, 1);
            //double[] massHPrev = massH;//нужен ли? далее можно обойтись без него
            massH = VectorSum(massR, massH, 1, betha);
            //используется уже новое H
            double alpha = vAlpha(massH);
            if ((Math.Abs(betha) < 1e-13)&&(betha>0))
            {
                triggerB = true;
                return -1;
            }
            Console.WriteLine(betha);
            //теперь новое
            massSolution = VectorSum(massSolution, massH, 1, alpha);
            //здесь обязан быть блок разницы
            if (Math.Abs(alpha) < 1e-13)
            {
                triggerA = true;
                return -1;
            }
            for (int i = 0; i < (vNumX-1)*(vNumY-1); i++)
            {
                result = Math.Max(result, Math.Abs(temp[i] - massSolution[i]));
            }
            return result;
        }
        #region wiki
        public double GradStepWiki()
        {
            double[] temp = new double[(vNumX - 1) * (vNumY - 1)];
            temp = massSolution;
            double result = 0;
            double alpha = vAlphaWiki();//1
            massSolution = VectorSum(massSolution, massH, 1, alpha);//2
            double[] massRprev = massR;
            massR = VectorSum(massR, VectorMulti(massSolution), 1, -alpha);
            double betha = vBethaWiki(massRprev);
            massH = VectorSum(massR, massSolution, 1, betha);
            
            for (int i = 0; i < (vNumX - 1) * (vNumY - 1); i++)
            {
                result = Math.Max(result, Math.Abs(temp[i] - massSolution[i]));
            }
            return result+10;
        }
        public double vAlphaWiki()
        {
            double result=0;
            foreach (double i in massR)
            {
                result += i * i;
            }
            double[] temp = VectorMulti(massSolution);
            double temp2 = 0;
            for (int i = 0; i < massR.Length; i++)
            {
                temp2 += temp[i] * massSolution[i];
            }
            if (temp2 == 0)
                return 0;
            return result / temp2;
        }
        public double vBethaWiki(double[] massRPrev)
        {
            double result = 0;
            foreach (double i in massR)
            {
                result += i * i;//а тут ещё и предыдущий должен быть, блин
            }
            double temp2 = 0;
            foreach (double i in massRPrev)
            {
                temp2 += i * i;
            }
            if (temp2 == 0)
                return 0;
            return result / temp2;
        }
        #endregion
        public double GradSolver(int X, int Y, int N, double eps)
        {
            triggerA = triggerB = false;
            double result = 0;
            vNumX = X;
            vNumY = Y;
            int NMax = N;
            vIteration = 0;
            massFfinder();
            massSolution = new double[(vNumX - 1) * (vNumY - 1)];
            massR = new double[(vNumX - 1) * (vNumY - 1)];
            for (int i = 0; i < (vNumX-1)*(vNumY-1); i++)
            {
                massSolution[i] = 0;
            }
            massSolution2d = new double[vNumX - 1, vNumY - 1];
            double temp;
            double prevres=300;
            for (int i = 0; i < NMax; i++)
            {
                //double temp1 = prStep();
                vIteration = i + 1;
                //result = GradStep();
                temp = GradStep();
                if (temp > 0)
                    result = temp;
                if (result < eps)
                    break;
                if ( triggerA || triggerB)
                    break;
                if(vIteration==38)
                {
                    vIteration += 0;
                }
                if(prevres>result)
                {
                    prevres=result;
                    massSolutionPrev = massSolution;
                }
                else
                {
                    massSolution = massSolutionPrev;
                    massH=new double[(vNumX-1)*(vNumY-1)];
                    prevres = 300;
                }
            }
            double Dif = 0;
            double temp1;
            massD2d = new double[vNumX - 1, vNumY - 1];
            for (int j = 0; j < vNumY-1; j++)
            {
                for (int i = 0; i < vNumX-1; i++)
                {
                    //temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i] - Calc(i + 1, j + 1));
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    N++;
                }
            }
            vDSol = Dif;
            vDMeth = result;
            return result;
        }
        //подаётся Hs
        private double vAlpha(double[] massHin)
        {
            double result,temp2;
            result = SkalMult(VectorSum(VectorMulti(massSolution), massF, 1, -1),massHin);
            temp2 = SkalMult(VectorMulti(massHin), massHin);
            if (temp2 == 0)
                return 0;
            if (Math.Abs(result / temp2) > 10)
            {
                double k;
            }
            return -result/temp2;
        }
        private double vBetha(double[] massHin, double[] massRin)
        {
            double result,temp2;
            result = SkalMult(VectorMulti(massHin),massRin);
            temp2 = SkalMult(VectorMulti(massHin), massHin);
            if (temp2 == 0)
                return 0;
            /*if(Math.Abs(result/temp2)<1e-1)
            {
                double k;
            }*/
            return result/temp2;
        }
        #region vector
        private double[] VectorMultiOld(double[] varibl)
        {
            double koefL = 1 / (vStepX * vStepX);
            double koefD = 1 / (vStepY * vStepY);
            double koefA = -2 * (koefL + koefD);
            double[] result = new double[(vNumX - 1) * (vNumY - 1)];
            for (int i = 0; i < (vNumX - 1) * (vNumY - 1); i++)
            {
                {//переписать нафиг. Смотреть, как на 2d получается
                    result[i] += varibl[i] * koefA;
                    if(i-1>-1)
                        result[i] += koefL * varibl[i - 1];
                    if(i+1<varibl.Length)
                        result[i] += koefL * varibl[i + 1];
                    if(i + vNumX - 1<varibl.Length)
                        result[i] += koefD * varibl[i + vNumX - 1];
                    if(i-vNumX -1>-1)
                        result[i] += koefD * varibl[i - vNumX - 1];
                }
            }
            return result;
        }
        //скорректированный
        private double[] VectorMultinottooold(double[] varibl)
        {
            double koefL = 1 / (vStepX * vStepX);
            double koefD = 1 / (vStepY * vStepY);
            double koefA = -2 * (koefL + koefD);
            double[] result = new double[(vNumX - 1) * (vNumY - 1)];
            double[,] result2 = new double[vNumX - 1, vNumY - 1];           
            for (int i = 0; i < vNumX-1; i++)
            {
                for (int j = 0; j < vNumY-1; j++)
                {
                    result2[i, j] += varibl[i + (vNumX - 1) * j] * koefA;
                    //побокам
                    if (i - 1 > -1)
                        result2[i, j] += varibl[i - 1 + (vNumX - 1) * j] * koefL;
                    if (i + 1 < vNumX - 1)
                    //if (i + 1 < vNumX )
                        result2[i, j] += varibl[i + 1 + (vNumX - 1) * j] * koefL;
                    //сверху-снизу
                    if (j - 1 > -1)
                        result2[i, j] += varibl[i + (vNumX - 1) * (j - 1)] * koefD;
                    //if (j + 1 < vNumY - 1)
                    if (j + 1 < vNumY-1)
                        result2[i, j] += varibl[i + (vNumX - 1) * (j + 1)] * koefD;
                }
            }
            /*
            for (int i = 0; i < (vNumX - 1) * (vNumY - 1); i++)
            {
                {//переписать нафиг. Смотреть, как на 2d получается
                    result[i] += varibl[i] * koefA;
                    if (i - 1 > -1)
                        result[i] += koefL * varibl[i - 1];
                    if (i + 1 < varibl.Length)
                        result[i] += koefL * varibl[i + 1];
                    if (i + vNumX - 1 < varibl.Length)
                        result[i] += koefD * varibl[i + vNumX - 1];
                    if (i - vNumX - 1 > -1)
                        result[i] += koefD * varibl[i - vNumX - 1];
                }
            }
             */
            int k = 0;
            for (int i = 0; i < vNumX-1; i++)
            {
                for (int j = 0; j < vNumY-1; j++)
                {
                    result[k] = result2[i, j];
                    k++;
                }
            }
            return result;
        }
        private double[] VectorMulti(double[] varibl)
        {
            double koefL = 1 / (vStepX * vStepX);
            double koefD = 1 / (vStepY * vStepY);
            double koefA = -2 * (koefL + koefD);
            double[] result = new double[(vNumX - 1) * (vNumY - 1)];
            double[,] result2 = new double[vNumX - 1, vNumY - 1];
            double i1, i2, i3, i4, i5;            
            for (int i = 0; i < vNumX - 1; i++)
            {
                for (int j = 0; j < vNumY - 1; j++)
                {
                    i1 = i2 = i3 = i4 = i5 = 0;
                    i1 = varibl[i + (vNumX - 1) * j] * koefA;
                    //побокам
                    if (i - 1 > -1)
                        i2 = varibl[i - 1 + (vNumX - 1) * j] * koefL;
                    if (i + 1 < vNumX - 1)
                        //if (i + 1 < vNumX )
                        i3 = varibl[i + 1 + (vNumX - 1) * j] * koefL;
                    //сверху-снизу
                    if (j - 1 > -1)
                        i4 = varibl[i + (vNumX - 1) * (j - 1)] * koefD;
                    //if (j + 1 < vNumY - 1)
                    if (j + 1 < vNumY - 1)
                        i5 = varibl[i + (vNumX - 1) * (j + 1)] * koefD;
                    result2[i, j] = i1 + i2 + i3 + i4 + i5;
                }
            }
            /*
            for (int i = 0; i < (vNumX - 1) * (vNumY - 1); i++)
            {
                {//переписать нафиг. Смотреть, как на 2d получается
                    result[i] += varibl[i] * koefA;
                    if (i - 1 > -1)
                        result[i] += koefL * varibl[i - 1];
                    if (i + 1 < varibl.Length)
                        result[i] += koefL * varibl[i + 1];
                    if (i + vNumX - 1 < varibl.Length)
                        result[i] += koefD * varibl[i + vNumX - 1];
                    if (i - vNumX - 1 > -1)
                        result[i] += koefD * varibl[i - vNumX - 1];
                }
            }
             */
            int k = 0;
            for (int i = 0; i < vNumX - 1; i++)
            {
                for (int j = 0; j < vNumY - 1; j++)
                {
                    result[k] = result2[i, j];
                    k++;
                }
            }
            return result;
        }
        private double[] VectorSum(double[] a, double[] b, double ak, double bk)
        {
            double[] result = new double[(vNumX-1) * (vNumY-1)];
            for (int i = 0; i < (vNumX - 1) * (vNumY - 1); i++)
            {
                result[i] = ak * a[i] + bk * b[i];
            }
            return result;
        }
        private double SkalMult(double[] a, double[] b)
        {
            double result = 0;
            for (int i = 0; i < (vNumX - 1) * (vNumY - 1); i++)
            {
                result += a[i] * b[i];
            }
            return result;
        }
        #endregion
    }
}
