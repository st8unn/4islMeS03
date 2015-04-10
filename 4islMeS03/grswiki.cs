using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4islMeS03
{
    class grswiki
    {
        public int vIteration = 0;
        public int vSystem = 0;
        public double vDMeth;
        public double vDSol;
        func equ1 = new func();
        bool triggerB = false;
        bool triggerA = false;
        public double[] massSolution;
        public double[] massSolutionPrev;
        //число отрезков
        public int vNumX;
        //число отрезков
        public int vNumY;
        public double vStepX;
        public double vStepY;
        public double[] massF;
        public double[] massH;
        public double[,] massH2d;
        public double[] massR;
        public double[,] massR2d;
        public grswiki() { }
        public double[,] massF2d;//воздействие
        public double[,] massSolution2d;//координаты/тепло
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

                }
            }
            //затем все точки граничные вписать
            //сначала вписываем нижнюю - для всех от 1 до vNumX-1; правда здесь вектор смещён на 1 вверх
            for (int i = 1; i < vNumX; i++)
            {
                massF2d[i - 1, 0] -= equ1.bottomF(minX + i * vStepX) / (vStepY * vStepY);//X или Y?

            }
            int coord = (vNumX - 1) * (vNumY - 2);
            for (int i = 1; i < vNumX; i++)
            {
                massF2d[i - 1, vNumY - 2] -= equ1.topF(minX + i * vStepX) / (vStepY * vStepY);
            }
            //лево - шаг через vNumX
             for (int i = 1; i < vNumY; i++)
            {
                massF2d[0, i - 1] -= equ1.leftF(minY + i * vStepY) / (vStepX * vStepX);
             }
            //право
            for (int i = 1; i < vNumY; i++)
            {
                massF2d[vNumX - 2, i - 1] -= equ1.rigthF(minY + i * vStepY) / (vStepX * vStepX);
             }
            //вроде все точки расставлены.
            //отладка, удалить.
            N = 0;
        }
        //даже не уверен, что это. Якобы значение тепла в точке
        double fGiveP2d(int i, int j)
        {
            if ((i == 0) || (i == vNumX) || (j == 0) || (j == vNumY))
                return 0;
            double k = massSolution2d[i - 1, j - 1];
            return k;
        }
  
        //вычисляет воздействие в точке
        public double Calc(int i, int j)
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
        
        #region wiki
        public double GradSolverWiki(int X, int Y, int N, double eps)
        {
            triggerA = triggerB = false;
            double result = 0;
            vNumX = X;
            vNumY = Y;
            int NMax = N;
            vIteration = 0;
            massFfinder();
            //massSolution = new double[(vNumX - 1) * (vNumY - 1)];
            massR2d = new double[vNumX - 1,vNumY - 1];
            /*for (int i = 0; i < (vNumX - 1) * (vNumY - 1); i++)
            {
                massSolution[i] = 0;
            }*/
            massSolution2d = new double[vNumX - 1, vNumY - 1];
            double temp;
            //считая, что massSolution2d=0
            //massR=
            //massR2d=VectorSum(massF2d,VectorMulti(massSolution2d),1,-1);
            massR2d = massF2d;
            massH2d = massR2d;
            for (int i = 0; i < NMax; i++)
            {
                //double temp1 = prStep();
                vIteration = i + 1;
                //result = GradStep();
                temp = GradStepWiki();
                if (temp > 0)
                    result = temp;
                if (result < eps)
                    break;
                if (triggerA || triggerB)
                    break;
                /*if (vIteration == 38)
                {
                    vIteration += 0;
                }*/
                /*if (prevres > result)
                {
                    prevres = result;
                    massSolutionPrev = massSolution;
                }
                else
                {
                    massSolution = massSolutionPrev;
                    massH = new double[(vNumX - 1) * (vNumY - 1)];
                    prevres = 300;
                }*/
            }
            double Dif = 0;
            double temp1;
            massD2d = new double[vNumX - 1, vNumY - 1];
            for (int j = 0; j < vNumY - 1; j++)
            {
                for (int i = 0; i < vNumX - 1; i++)
                {
                    //temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i] - Calc(i + 1, j + 1));
                    temp1 = Math.Abs(massSolution2d[i,j] - Calc(i + 1, j + 1));
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
        public double GradStepWiki()
        {
            double[,] temp;
            temp=massSolution2d;
            double result=0;
            double[,] massR2dPr= new double[vNumX - 1,vNumY - 1];
            massR2dPr= massR2d;            
            double alpha = vAlphaWiki(massR2d,massH2d);
            massSolution2d = VectorSum(massSolution2d, massH2d, 1, alpha);//2
            massR2d = VectorSum(massR2d, VectorMulti(massH2d), 1, -alpha);
            double betha = vBethaWiki(massR2d,massR2dPr);
            massH2d = VectorSum(massR2d, massH2d, 1, betha);

            for (int j = 0; j < vNumY - 1; j++)
            {
                for (int i = 0; i < vNumX - 1; i++)
                {
                    result = Math.Max(result, Math.Abs(temp[i, j] - massSolution2d[i, j]));
                }
            }

            return result;
        }
        public double vAlphaWiki(double[,] mRin, double[,] mHin)
        {
            double result = 0;
            result = SkalMult(mRin, mRin);
            double temp = SkalMult(VectorMulti(mHin), mHin);
            /*double[] temp = VectorMulti(massSolution);
            double temp2 = 0;
            for (int i = 0; i < massR.Length; i++)
            {
                temp2 += temp[i] * massSolution[i];
            }*/
            if (temp == 0)
                return 0;
            return result / temp;
        }
        public double vBethaWiki(double[,] mRinN,double[,] mRinP)
        {
            double result = 0;
            result = SkalMult(mRinN,mRinN);
            double temp = 0;
            temp = SkalMult(mRinP, mRinP);
            if (temp == 0)
                return 0;
            return result / temp;
        }
        #endregion                
        #region vector2
        public double[,] VectorMulti(double[,] varibl)
        {
            double koefL = 1 / (vStepX * vStepX);
            double koefD = 1 / (vStepY * vStepY);
            double koefA = -2 * (koefL + koefD);
            double[] result = new double[(vNumX - 1) * (vNumY - 1)];
            double[,] result2 = new double[vNumX - 1, vNumY - 1];
            double i1, i2, i3, i4, i5;
            //теперь переделываем это под необходимый формат
            for (int i = 0; i < vNumX - 1; i++)
            {
                for (int j = 0; j < vNumY - 1; j++)
                {
                    i1 = i2 = i3 = i4 = i5 = 0;
                    i1 = varibl[i , j] * koefA;
                    //побокам
                    if (i - 1 > -1)
                        i2 = varibl[i - 1 , j] * koefL;
                    if (i + 1 < vNumX - 1)
                        //if (i + 1 < vNumX )
                        i3 = varibl[i + 1 ,j] * koefL;
                    //сверху-снизу
                    if (j - 1 > -1)
                        i4 = varibl[i , j - 1] * koefD;
                    //if (j + 1 < vNumY - 1)
                    if (j + 1 < vNumY - 1)
                        i5 = varibl[i , j + 1] * koefD;
                    result2[i, j] = i1 + i2 + i3 + i4 + i5;
                }
            }
            return result2;
        }
        public double[,] VectorSum(double[,] a, double[,] b, double ak, double bk)
        {
            double[,] result = new double[(vNumX - 1),(vNumY - 1)];
            for (int i = 0; i < vNumX - 1; i++)
            {
                for (int j = 0; j < vNumY - 1; j++)
                {
                    result[i, j] = ak * a[i,j] + bk * b[i,j];
                }
            }
            return result;
        }
        public double SkalMult(double[,] a, double[,] b)
        {
            double result = 0;
            double[] temp = new double[vNumX-1];
            for (int i = 0; i < vNumX - 1; i++)
            {
                
                for (int j = 0; j < vNumY - 1; j++)
                {
                    temp[i] += a[i, j] * b[i, j];
                }
            }
            result=temp.Sum();
            return result;
        }
        #endregion
    }
}
