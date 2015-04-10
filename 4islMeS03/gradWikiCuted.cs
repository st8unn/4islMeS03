using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4islMeS03
{
    class gradWikiCuted : grswiki
    {
        double[,] LowMatrix;
        private void massF2dCutter()
        {
            int vNumXinit = vNumX / 2 - 1;
            int vNumYinit = vNumY / 2 - 1;
            for (int j = vNumYinit; j < vNumY - 1; j++)
            {
                for (int i = vNumXinit; i < vNumX - 1; i++)
                {
                    massF2d[i, j] = 0;
                }
            }
        }
        new public double[,] VectorMulti(double[,] varibl)
        {
            double koefL = 1 / (vStepX * vStepX);
            double koefD = 1 / (vStepY * vStepY);
            double koefA = -2 * (koefL + koefD);
            double[] result = new double[(vNumX - 1) * (vNumY - 1)];
            double[,] result2 = new double[vNumX - 1, vNumY - 1];
            double i1, i2, i3, i4, i5;
            int vNumXCuted = vNumX / 2 - 1;
            int vNumYCuted = vNumY / 2 - 1;
            //левая почти половина
            for (int i = 0; i < vNumXCuted; i++)
            {
                for (int j = 0; j < vNumY - 1; j++)
                {
                    i1 = i2 = i3 = i4 = i5 = 0;
                    i1 = varibl[i, j] * koefA;
                    //побокам
                    if (i - 1 > -1)
                        i2 = varibl[i - 1, j] * koefL;
                    if (i + 1 < vNumX - 1)
                        //if (i + 1 < vNumX )
                        i3 = varibl[i + 1, j] * koefL;
                    //сверху-снизу
                    if (j - 1 > -1)
                        i4 = varibl[i, j - 1] * koefD;
                    //if (j + 1 < vNumY - 1)
                    if (j + 1 < vNumY - 1)
                        i5 = varibl[i, j + 1] * koefD;
                    result2[i, j] = i1 + i2 + i3 + i4 + i5;
                }
            }
            for (int i = vNumXCuted; i < vNumX-1; i++)
            {
                for (int j = 0; j < vNumYCuted; j++)
                {
                    i1 = i2 = i3 = i4 = i5 = 0;
                    i1 = varibl[i, j] * koefA;
                    //побокам
                    if (i - 1 > -1)
                        i2 = varibl[i - 1, j] * koefL;
                    if (i + 1 < vNumX - 1)
                        //if (i + 1 < vNumX )
                        i3 = varibl[i + 1, j] * koefL;
                    //сверху-снизу
                    if (j - 1 > -1)
                        i4 = varibl[i, j - 1] * koefD;
                    //if (j + 1 < vNumY - 1)
                    if (j + 1 < vNumY - 1)
                        i5 = varibl[i, j + 1] * koefD;
                    result2[i, j] = i1 + i2 + i3 + i4 + i5;
                }
            }
            //остальные по умолчанию инициализируются нулями
            return result2;
        }
        public double GradSolverWikiCuted(int X, int Y, int N, double eps)
        {
            //проверка, что x нечётный
            X = X / 2 + 1;
            //проверка, что y нечётный
            Y = Y / 2 + 1;
            return GradSolverWiki(X, Y, N, eps);
        }
        
        //делаем ограничение на >0 по каждой координате
        //то есть мы должны изменить F получаемый, убрав нужные строки
        //или не должны
        public double GradSolverWiki2(int X, int Y, int N, double eps)
        {
            //проверка, что x чётный, чтобы число узлов было нечётным.
            X = (X / 2) * 2;
            //проверка, что y чётный
            Y = (Y / 2) * 2;
            double result = 0;
            int vNumXinit, vNumYinit;
            //вычисления для одинарного шага.
            {
                vNumX = X;
                vNumY = Y;
                vNumXinit = vNumX;
                vNumYinit = vNumY;
                int NMax = N;
                vIteration = 0;
                massFfinder();
                massF2dCutter();
                massR2d = new double[vNumX - 1, vNumY - 1];
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
                }
            }
            int vIterationOld = vIteration;
            LowMatrix = massSolution2d;
            //число точек внутренних*3 и плюс два, как отрезков
            X = (X - 2) * 3 + 2;
            Y = (Y - 2) * 3 + 2;
            vIteration = 0;
            //теперь вычисляем для двойного (на самом деле тройного) шага
            {
                vNumX = X;
                vNumY = Y;
                int NMax = N;
                vIteration = 0;
                massFfinder();
                massF2dCutter();
                massR2d = new double[vNumX - 1, vNumY - 1];
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
                }
            }
            double temp1;
            vNumX = vNumXinit;
            vNumY = vNumYinit;
            vNumXinit = vNumX / 2 - 1;
            vNumYinit = vNumY / 2 - 1;
            vIteration = vIterationOld;
            //из-за подхода нужно изменить координаты.
            //или считать в 4 подхода, что экономичнее, т.к. нет ифов
            massD2d = new double[vNumX - 1, vNumY - 1];
            
            result = D2Filler1();
            vDMeth = result;
            return result;
        }
        //просто заполняю само решение
        public double D2Filler1()
        {
            double result = 0;
            int vNumYinit = vNumY / 2 - 1;
            int vNumXinit = vNumX / 2 - 1 ;
            double temp1;
            double Dif = 0;
            int x, y;
            //1;
            for (int j = 0; j < vNumYinit; j++)
            {
                for (int i = 0; i < vNumXinit; i++)
                {
                    //temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i] - Calc(i + 1, j + 1));
                    x = 1 + 3 * i;
                    y = 1 + 3 * j;
                    temp1 = Math.Abs(LowMatrix[i, j] - massSolution2d[x, y]);
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                }
            }
            //2;
            for (int j = vNumYinit + 1; j < vNumY - 1; j++)
            {
                for (int i = 0; i < vNumXinit; i++)
                {
                    //temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i] - Calc(i + 1, j + 1));
                    x = 1 + 3 * i;
                    y = (3 * vNumYinit + 1) + 1 + 3 * (j - vNumYinit - 1);
                    temp1 = Math.Abs(LowMatrix[i, j] - massSolution2d[x, y]);
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                }
            }
            //3;
            for (int j = 0; j < vNumYinit; j++)
            {
                for (int i = vNumYinit + 1; i < vNumX - 1; i++)
                {
                    //temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i] - Calc(i + 1, j + 1));
                    x = (3 * vNumXinit + 1) + 1 + 3 * (i - vNumXinit - 1);
                    y = 1 + 3 * j;
                    temp1 = Math.Abs(LowMatrix[i, j] - massSolution2d[x, y]);
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                }
            }
            //4;
            for (int j = vNumYinit + 1; j < vNumY - 1; j++)
            {
                for (int i = vNumXinit + 1; i < vNumX - 1; i++)
                {
                    //temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i] - Calc(i + 1, j + 1));
                    x = (3 * vNumXinit + 1) + 1 + 3 * (i - vNumXinit - 1);
                    y = (3 * vNumYinit + 1) + 1 + 3 * (j - vNumYinit - 1);
                    temp1 = Math.Abs(LowMatrix[i, j] - massSolution2d[x, y]);
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                }
            }
            //между
            
            {
                int j = vNumYinit;
                for (int i = 0; i < vNumXinit; i++)
                {
                    x = 1 + 3 * i;
                    y = 1 + 3 * j;
                    temp1 = Math.Abs(LowMatrix[i, j] - massSolution2d[x, y]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                }
            }
            {
                int i = vNumXinit;
                for (int j = 0; j < vNumYinit; j++)
                {
                    x = 1 + 3 * i;
                    y = 1 + 3 * j;
                    temp1 = Math.Abs(LowMatrix[i, j] - massSolution2d[x, y]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                }
            }
            //здесь он должен выдавать 0
            
            {
                int j = vNumYinit;
                for (int i = vNumYinit + 1; i < vNumX - 1; i++)
                {
                    //temp1 = Math.Abs(massSolution2d[i - 1, j - 1] - Calc(i, j));
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i] - Calc(i + 1, j + 1));
                    x=(3 * vNumXinit + 1) + 1 + 3 * (i - vNumXinit - 1);
                    y = 1 + 3 * j;
                    temp1 = Math.Abs(LowMatrix[i, j] - massSolution2d[x, y]);
                    //temp1 = Math.Abs(massSolution[j * (vNumX - 1) + i]);
                    Dif = Math.Max(Dif, temp1);
                    massD2d[i, j] = temp1;
                }
            }
            vDSol = Dif;
            return result;

        }
        new public double GradStepWiki()
        {
            double[,] temp;
            temp = massSolution2d;
            double result = 0;
            double[,] massR2dPr = new double[vNumX - 1, vNumY - 1];
            massR2dPr = massR2d;
            double alpha = vAlphaWiki(massR2d, massH2d);
            massSolution2d = VectorSum(massSolution2d, massH2d, 1, alpha);//2
            massR2d = VectorSum(massR2d, VectorMulti(massH2d), 1, -alpha);
            double betha = vBethaWiki(massR2d, massR2dPr);
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
        
        new public double vAlphaWiki(double[,] mRin, double[,] mHin)
        {
            double result = 0;
            result = SkalMult(mRin, mRin);
            double temp = SkalMult(VectorMulti(mHin), mHin);
            if (temp == 0)
                return 0;
            return result / temp;
        }
        new public double vBethaWiki(double[,] mRinN, double[,] mRinP)
        {
            double result = 0;
            result = SkalMult(mRinN, mRinN);
            double temp = 0;
            temp = SkalMult(mRinP, mRinP);
            if (temp == 0)
                return 0;
            return result / temp;
        }
        
        public double SkalMult(double[,] a, double[,] b)
        {
            double result = 0;
            double[] temp = new double[vNumX - 1];
            for (int i = 0; i < vNumX - 1; i++)
            {

                for (int j = 0; j < vNumY - 1; j++)
                {
                    temp[i] += a[i, j] * b[i, j];
                }
            }
            result = temp.Sum();
            return result;
        }
    }
}
