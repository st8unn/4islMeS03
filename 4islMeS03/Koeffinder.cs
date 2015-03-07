﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4islMeS03
{
    class Koeffinder
    {
        public int vIteration = 0;
        int vSystem=0;
        public double vDZel;
        public double vDSol;
        func equ1=new func();
        double[] massF;
        double[] massSolution;
        int vNumX;
        int vNumY;
        double vStepX;
        double vStepY;
        public Koeffinder() { }
        double[,] massF2d;
        double[,] massSolution2d;
        public double[,] massD2d;
        public void chang(bool t)
        {
            equ1.Changer(t);
        }
        public void massFfinder()
        {
            double minX=Math.Min(equ1.a,equ1.b);
            double maxX=Math.Max(equ1.a,equ1.b);
            double minY=Math.Min(equ1.c,equ1.d);
            double maxY=Math.Max(equ1.d,equ1.d);
            vStepX = (maxX - minX) / vNumX;
            vStepY = (maxY-minY)/vNumY;
            massF = new double[(vNumX - 1) * (vNumY - 1)];
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
            try
            {
                for (int i = 1; i < vNumX; i++)
                {
                    massF2d[i - 1, 0] -= equ1.bottomF(minX + i * vStepX) / (vStepY * vStepY);//X или Y?
                    massF[i - 1] -= equ1.bottomF(minX + i * vStepX) / (vStepY * vStepY);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            try
            {
                int coord = (vNumX - 1) * (vNumY - 2);
                for (int i = 1 ; i < vNumX; i++)
                {
                    massF2d[i - 1, vNumY - 2] -= equ1.topF(minX + i * vStepX) / (vStepY * vStepY);
                    massF[coord + i - 1] -= equ1.topF(minX + i * vStepX) / (vStepY * vStepY);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            //лево - шаг через vNumX
            try
            {
                int coord = 0;
                for (int i = 1; i < vNumY; i++)
                {
                    massF2d[0, i - 1] -= equ1.leftF(minY + i * vStepY) / (vStepX * vStepX);
                    coord = (i - 1) * (vNumX - 1);
                    massF[coord] -= equ1.leftF(minY + i * vStepY) / (vStepX * vStepX);
                }
            }
            catch (Exception)
            {

                throw;
            }
            //право
            try
            {
                int coord = 0;
                for (int i = 1; i <vNumY; i++)
                {
                    massF2d[vNumX - 2, i - 1] -= equ1.rigthF(minY + i * vStepY) / (vStepX * vStepX);
                    coord = (vNumY - 1) + (i - 1) * (vNumX - 1);
                    //massF[coord - 1] -= equ1.rigthF(minY + i * vStepY) / (vStepX * vStepX);
                }
            }
            catch (Exception)
            {

                throw;
            }
            //вроде все точки расставлены.
            //отдалка, удалить.
            double[,] massD2d = new double[vNumX - 1, vNumY - 1];
            N = 0;
            for (int j = 1; j < vNumY; j++)
            {
                for (int i = 1; i < vNumX; i++)
                {
                    massD2d[i - 1, j - 1] = massF2d[i - 1, j - 1] - massF[N];
                    //massF2d[i - 1, j - 1] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    //massF[N] = equ1.Heat(minX + i * vStepX, minY + j * vStepY);
                    N++;
                }
            }
            N = 0;
        }
        double fGiveP(int i,int j)
        {
            if ((i == 0) || (i == vNumX) || (j == 0) || (j == vNumY))
                return 0;
            int coord = i + (vNumX - 1) * (j - 1) - 1;//почти уверен, что это так
            double k = massSolution[coord];
            return k;
        }
        double fGiveP2d(int i, int j)
        {
            if ((i == 0) || (i == vNumX) || (j == 0) || (j == vNumY))
                return 0;
            double k = massSolution2d[i-1, j-1];
            return k;
        }
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
                    massSolution[N] = (massF[N] - fGiveP(i + 1, j) * koefL - fGiveP(i, j + 1) * koefD - fGiveP(i - 1, j) * koefL - fGiveP(i, j - 1) * koefD) / koefA;
                    
                    //massSolution[N] = (massF[N] - fGiveP(i + 1, j) * koefL - fGiveP(i, j + 1) * koefD) / (koefA * massSolution[N] + fGiveP(i - 1, j) * koefL + fGiveP(i, j - 1) * koefD);
                    {
                        temp1 = massSolution2d[i - 1, j - 1];
                        double mf = massF2d[i - 1, j - 1];
                        double mf2 = massF[N];
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
        public double Calc(int i,int j)
        {
            double minX = Math.Min(equ1.a, equ1.b);
            double maxX = Math.Max(equ1.a, equ1.b);
            double minY = Math.Min(equ1.c, equ1.d);
            double maxY = Math.Max(equ1.d, equ1.d);
            double x = minX + vStepX * i;
            double y = minY + vStepY * j;
            //return 1 - x * x - y * y;
            return equ1.Pres(x, y);
        }
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
            massSolution = new double[(vNumX - 1) * (vNumY - 1)];
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
            vDZel = result;
            return result;
        }
    }
}
