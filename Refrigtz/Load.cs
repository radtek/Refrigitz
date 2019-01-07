﻿/***********************************************************************************************************************************
 * Ramin Edjlal*********************************************************************************************************************
 * Orderic Linear Model*************************************************************************************************************
 * (+) :Problem Solved.*************************************************************************************************************
 * (-) :No Need to Solved.**********************************************************************************************************
 * (*) :Problem Exist.**************************************************************************************************************
 * (_) :Undetermined.***************************************************************************************************************
 * (QC-OK) :No Risk Control Need.***************************************************************************************************
 * *********************************************************************************************************************************
 *{RefrigtzDLL.RefrigtzDLL.RefrigtzDLL.AllDraw:[ (+:Sum(63))************************************************************************************************************
 *(*:Sum(4))************************************************************************************************************************
 * (-:sum(2)) (_:Sum(0))]}**********************************************************************************************************
 * 2: (+:Sum(3)) (-:Sum(1)) (*:Sum(2))**********************************************************************************************
 * 3: (+:Sum(4)) (*:Sum(1)) 4:(+:Sum(6)) 5:(+:Sum(2)) (-:Sum(1)) 5.(+:Sum(6)) (*:Sum(2)) *******************************************
 * 7.(+:Sum(2)) (*:Sum(1))**********************************************************************************************************
 * 9.(+:Sum(4)) (*:Sum(1)) (-:Sum(1)) 10.(+:Sum(4)) (*:Sum(2)) 11.(+:Sum(4)) 14.(+:Sum(2)) (*:Sum(1))*******************************
 * 12.(+:Sum(2)) (*:Sum(2)) 13.(+:Sum(4)) 15.(+:Sum(6)) 16.(+:Sum(2))***************************************************************
 * *********************************************************************************************************************************
 * {ChessGenetic:[(+:Sum(22))]}*****************************************************************************************************
 * *********************************************************************************************************************************
 * {ChessPredict:[(+:Sum(10)) (_:Sum(1))]}******************************************************************************************
 * *********************************************************************************************************************************
 * {RefrigtzDLL.ChessRules:[+:Sum(48)) (_ :Sum(1)) *********************************************************************************************
 * (-:Sum(5)) (*:Sum(2))]}**********************************************************************************************************
 * 2:(+:Sum(1)) 4:(+:Sum(5)*********************************************************************************************************
 * 3.(+:Sum(2)) 4.(*:Sum(3))********************************************************************************************************
 * *********************************************************************************************************************************
 * {RefrigtzDLL.ThinkingChess:[(+:Sum(26)) (*:Sum(1))]} 5:(+:Sum(3)) 6.(+:Sum(+))***************************************************************
 * *********************************************************************************************************************************
 * FormeRefregitz:[+:Sum(32)] [*:Sum(10)] [-:Sum(1)] [_:Sum(3)]* [*:Sum(2)]*********************************************************
 * *********************************************************************************************************************************
 * {Timer:[(+:Sum(12))]}************************************************************************************************************
 * 2.(+:Sum(1))*********************************************************************************************************************
 * *********************************************************************************************************************************
 * {All:[(+:Sum(155)) (_ :Sum(2)) **************************************************************************************************
 * (-:Sum(7)) (*:Sum(7))]} (E:Sum(163))*********************************************************************************************
 * Finished :1395/1/16**************************************************************************************************************
 * {All:(+:Sum(4)) (_:Sum(0)) (-:Sum(1)) (*:Sum(2)) (E:Sum(6))**********************************************************************
 * Finished: 1395/1/20**************************************************************************************************************
 * {All: (+:Sum(11))}***************************************************************************************************************
 * Finished :1395/1/22**************************************************************************************************************
 * {All: (+:Sum(5)) (-:Sum(1))******************************************************************************************************
 * {All: (+:Sum(6)) (*:Sum(2))}*****************************************************************************************************
 * {All:(+:Sum(2)) (*:Sum(1))}******************************************************************************************************
 * {All:(+:Sum(1)) (*:Sum(1))}******************************************************************************************************
 * {All:(+:Sum(2)) (*:Sum(2))}******************************************************************************************************
 * {All:(+:Sum(7)) (*:Sum(1)) (-:Sum(1))}*******************************************************************************************
 * {All:(+:Sum(36)) (*:Sum(15))) )(-:Sum(1)) (_:Sum(3)))]}**************************************************************************
 * {All:(+:Sum(2)) (*:Sum(2))}******************************************************************************************************
 * {All:(+:Sum(10))*****************************************************************************************************************
 * {All::Sum(2)) (*:Sum(1))*********************************************************************************************************
 * {All: (+:Sum(6))*****************************************************************************************************************
 * {All:(+:Sum(2))******************************************************************************************************************
 * {All:(*:Sum(2))******************************************************************************************************************
 * Metrics**************************************************************************************************************************
 * Coust Total:(1000000T/30)*26=866666T*********************************************************************************************
 3. Count Per 1000 Line (LOC)=866666/7.398=117[117278.68]T**************************************************************************
 * Total Days=26********************************************************************************************************************
 * Total Faults=155*****************************************************************************************************************
 * Total Lines=7398*****************************************************************************************************************
 5. Faults Per Day-Person=155[163]/26=5.9(F/Da-P)[6.26]*****************************************************************************
 6. Lines Per Day-Person=7398/26=286.5(L/Da-P)**************************************************************************************
 4. Documentation Per 1000 Lines:7*(7398/1000)=51.8(D/TL)***************************************************************************
 * Documentation Count Per Pages=1000/7=142(T/P)************************************************************************************
 7. Documentatio Per (KOLC) Lines  =(7/7398)/1000=9.4(D/L)**************************************************************************
 1.* Faults per (10000) lines=155[163]/1000=0.155[0.0155](F/L)[0.163]***************************************************************
 2.* Errors Per (1000) Lines =7/1000=0.007(E/L)*************************************************************************************
 * Count Total=(1)+(2)+(3)+(4)+(5)+(6)=470.762[471.13[117632.13]]*******************************************************************
 * FP=Count Total +[0.65+0.01*sig(fi)]**********************************************************************************************
 * Fp=470.762[471.13[470.9905[117632.13]]]+4.8412[5.3[5.35[1176.9713]]]=475.6032=[476.49[476.34[118809.1013]]]**********************
 * Risk Analysis********************************************************************************************************************
 * P=155/7396=0.21******************************************************************************************************************
 * C=475.6[476.34[118809.1013]]*****************************************************************************************************
 * RE=P*C=100[100.03[24949.911273]]((Fault per Lines)at Count)**********************************************************************
 * L=P*E^(1/3)*(t^(4/3))=1200[2000]*2(40-20-40)*(2^(4/3))=4880[8000]****************************************************************
 * (Electric Power)P=E*T=120mA*220*26*24=(16473)*10^-3=16*100T=1600T****************************************************************
 1. * 1600*30D=48000T***************************************************************************************************************
 2. * (Emunity Force)=1000000T-1P***************************************************************************************************
 3. *  (Faulting)=0.2*(48000+1000000)=209600****************************************************************************************
 43 * (Cut And Paste) =-0.09*((1)+(2)+(3))=-0.09*1257600=-113184T*******************************************************************
 *  BAC=(1)+(2)+(3)=1257600T-(4)=1144416T*******************************************************************************************
 * Count(RS)=162********************************************************************************************************************
 * Count(PB)=22*********************************************************************************************************************
 * P(RS)=0.88**1**Risk Control***********************************************************************************************************************
 * P(PB)=0.12**4**Managements and Cuation Programing***********************************************************************************************************************
 * *********************************************************************************************************************************/
/*1.1Result of Analysis:The AStarGreedyGreedy thinking with illustrative tree determination 
 1.2The needs of Simplyfing Code means performance increamentals.
 1.3The Ilegal non performable for leak resources
 1.4.Very simplifing usable informative structure
 2.1.suggestions:Simplyfing the code without leak of resourceses
 2.2increamental error correction performance
 2.3.increamental sync discovery.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Data.OleDb;
using System.Media;
using System.IO;
namespace Refrigtz
{
    [Serializable]
    public partial class Load : Form
    {
        public FormRefrigtz ttt = null;
        bool exit = false;
        //Intiate  Global Variables.
        Thread tt = null;
        Thread t = null;
        public static Load THIS = null;
        static void Log(Exception ex)
        {
            try
            {
                Object a = new Object();
                //lock (a)
                {
                    string stackTrace = ex.ToString();
                    File.AppendAllText(FormRefrigtz.Root + "\\ErrorProgramRun.txt", stackTrace + ": On" + DateTime.Now.ToString()); // path of file where stack trace will be stored.
                }
            }
            catch (Exception t) { Log(t); }
        }
        //Constructor
        public Load()
        {
            //Initaie Global Variables.
            InitializeComponent();
            tt = new Thread(new ThreadStart(BaseThread));
            tt.Start();
            t = new Thread(new ThreadStart(LoadThread));
            t.Start();

        }

        //Form Load Method Event Handling.
        private void Load_Load(object sender, EventArgs e)
        {
            if (!exit)
            {
                exit = true;
                THIS = this;
                this.Location = this.PointToScreen(this.Location);
            }
            else
            {
                (new TakeRoot()).Save(ttt.Quantum, ttt, ref ttt.LoadTree, ttt.MovementsAStarGreedyHuristicFound, ttt.IInoreSelfObjects, ttt.UsePenaltyRegardMechnisam, ttt.BestMovments, ttt.PredictHuristic, ttt.OnlySelf, ttt.AStarGreedyHuristic, ttt.ArrangmentsChanged);
                Application.Exit();
            }
        }
        //Delegate Of Form Close Visibility.
        delegate void SetLoadVisibleCallback();

        public void SetLoadVisible()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it continue;s true.
            if (this.InvokeRequired)
            {
                try
                {

                    SetLoadVisibleCallback d = new SetLoadVisibleCallback(SetLoadVisible);
                    this.Invoke(new Action(() => this.Hide()));
                }
                catch (Exception t) { Log(t); }
            }
            else
            {
                try
                {
                    this.Hide();
                }
                catch (Exception t) { Log(t); }
            }

        }
        //Delegate Of Form Close Visibility.
        delegate void SetCloseVisibleCallback();

        public void SetCloseVisible()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it continue;s true.
            if (this.InvokeRequired)
            {
                try
                {

                    SetCloseVisibleCallback d = new SetCloseVisibleCallback(SetCloseVisible);
                    this.Invoke(new Action(() => this.Close()));
                }
                catch (Exception t) { Log(t); }
            }
            else
            {
                try
                {
                    this.Close();
                }
                catch (Exception t) { Log(t); }
            }

        }
        //Task Load Loop On Waiting Load.
        void LoadThread()
        {
            Object O = new Object();
            //lock (O)
            {
                do
                {
                    System.Threading.Thread.Sleep(1);
                } while (!FormRefrigtz.LoadedTable);

                SetLoadVisible();
            }
        }
        //Base Task Load.
        void BaseThread()
        {
            //Initiate Task.
            ttt = new FormRefrigtz(false);
            ttt.Sec.ShowDialog();
            ttt.ShowDialog();
            Process.GetCurrentProcess().Kill();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

    }
}
//End Of Documents.