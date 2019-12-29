﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;

namespace Refrigtz
{
    [Serializable]
    public class TakeRoot
    {
        String path3 = @"temp";
        String AllDrawReplacement = "";

        public static int AllDrawKind = 0;//0,1,2,3,4,5,6
        public static String AllDrawKindString = "";

        public RefrigtzDLL.AllDraw t = null;
        public QuantumRefrigiz.AllDraw tt = null;

        static void Log(Exception ex)
        {
            try
            {
                Object a = new Object();
                lock (a)
                {
                    string stackTrace = ex.ToString();
                    File.AppendAllText(FormRefrigtz.Root + "\\ErrorProgramRun.txt", stackTrace + ": On" + DateTime.Now.ToString()); // path of file where stack trace will be stored.
                }
            }
            catch (Exception t) { Log(t); }
        }
        void SetAllDrawKindString()
        {
            if (AllDrawKind == 4)
                AllDrawKindString = "AllDrawBT.asd";//Both True
            else
                if (AllDrawKind == 3)
                AllDrawKindString = "AllDrawFFST.asd";//First false second true
            else
                if (AllDrawKind == 2)
                AllDrawKindString = "AllDrawFTSF.asd";//First true second false
            else
                if (AllDrawKind == 1)
                AllDrawKindString = "AllDrawFFSF.asd";//Fist false second false


        }
        void SetAllDrawKind(bool UsePenaltyRegardMechnisam,bool AStarGreedyHuristic)
        {
            if (UsePenaltyRegardMechnisam && AStarGreedyHuristic)
                AllDrawKind = 4;
            else
          if ((!UsePenaltyRegardMechnisam) && AStarGreedyHuristic)
                AllDrawKind = 3;
            if (UsePenaltyRegardMechnisam && (!AStarGreedyHuristic))
                AllDrawKind = 2;
            if ((!UsePenaltyRegardMechnisam) && (!AStarGreedyHuristic))
                AllDrawKind = 1;
        }

        bool DrawManagement(bool FOUND,bool UsePenaltyRegardMechnisam, bool AStarGreedyHuristic)
        {
            SetAllDrawKind(UsePenaltyRegardMechnisam,AStarGreedyHuristic);

            //Set Configuration To True for some unknown reason!.
            //UpdateConfigurationTableVal = true;                             
            SetAllDrawKindString();

            bool Found = false;
            String P = Path.GetFullPath(path3);
            AllDrawReplacement = Path.Combine(P, AllDrawKindString);
            if (File.Exists(AllDrawKindString))
            {

                if (File.Exists(AllDrawReplacement))
                {
                    if (((new System.IO.FileInfo(AllDrawKindString).Length) < (new System.IO.FileInfo(AllDrawReplacement)).Length))
                    {
                        File.Delete(AllDrawKindString);
                        File.Copy(AllDrawReplacement, AllDrawKindString);
                        Found = true;
                    }
                    else if (((new System.IO.FileInfo(AllDrawKindString).Length) > (new System.IO.FileInfo(AllDrawReplacement)).Length))
                    {
                        if (File.Exists(AllDrawReplacement))
                            File.Delete(AllDrawReplacement);
                        File.Copy(AllDrawKindString, AllDrawReplacement);
                        Found = true;
                    }
                }
                else
                {
                    if (!Directory.Exists(Path.GetFullPath(path3)))
                        Directory.CreateDirectory(Path.GetFullPath(path3));
                    File.Copy(AllDrawKindString, AllDrawReplacement);
                    Found = true;

                }
                Found = true;
            }
            else if (File.Exists(AllDrawReplacement))
            {
                File.Copy(AllDrawReplacement, AllDrawKindString);
                Found = true;
            }

            return Found;
        }

        public bool Load(bool FOUND,bool Quantum,FormRefrigtz Curent, ref bool LoadTree, bool MovementsAStarGreedyHuristicFound, bool IInoreSelfObjects, bool UsePenaltyRegardMechnisam, bool BestMovments, bool PredictHuristic, bool OnlySelf, bool AStarGreedyHuristic, bool ArrangmentsChanged)
        {
            DrawManagement(FOUND, UsePenaltyRegardMechnisam,  AStarGreedyHuristic);

            bool DrawDrawen = false;
            //Load Middle Targets.
            try
            {
                if (File.Exists(FormRefrigtz.AllDrawKindString))
                {
                    if (FormRefrigtz.MovmentsNumber >= 0)
                    {
                        if (!Quantum)
                        {
                            GalleryStudio.RefregizMemmory tr = new GalleryStudio.RefregizMemmory(MovementsAStarGreedyHuristicFound, IInoreSelfObjects, UsePenaltyRegardMechnisam, BestMovments, PredictHuristic, OnlySelf, AStarGreedyHuristic, ArrangmentsChanged);
                            t =(RefrigtzDLL.AllDraw)tr.Load(Quantum, FormRefrigtz.OrderPlate);
                            if (t != null)
                            {
                                Curent.Draw = t;

                                LoadTree = true;
                                Curent.Draw = Curent.RootFound();

                                t = Curent.Draw;
                                //Curent.SetDrawFounding(ref FOUND, ref THIS, false);
                                DrawDrawen = true;

                                System.Windows.Forms.MessageBox.Show("Load Completed.");
                            }
                        }
                        else
                        {
                            GalleryStudio.RefregizMemmory tr = new GalleryStudio.RefregizMemmory(MovementsAStarGreedyHuristicFound, IInoreSelfObjects, UsePenaltyRegardMechnisam, BestMovments, PredictHuristic, OnlySelf, AStarGreedyHuristic, ArrangmentsChanged);
                           tt =(QuantumRefrigiz.AllDraw) tr.LoadQ(Quantum, FormRefrigtz.OrderPlate);
                            if (t != null)
                            {
                               
                                Curent.DrawQ = tt;

                                LoadTree = true;


                                Curent.DrawQ = Curent.RootFoundQ();

                                tt = Curent.DrawQ;

                                DrawDrawen = true;

                                System.Windows.Forms.MessageBox.Show("Load Completed.");
                            }
                        }
                    }
                    File.Delete(FormRefrigtz.AllDrawKindString);
                }
            }
            catch (Exception t) { Log(t); }
            return DrawDrawen;
        }
        public bool Save(bool FOUND,bool Quantum, FormRefrigtz Curent, ref bool LoadTree, bool MovementsAStarGreedyHuristicFound, bool IInoreSelfObjects, bool UsePenaltyRegardMechnisam, bool BestMovments, bool PredictHuristic, bool OnlySelf, bool AStarGreedyHuristic, bool ArrangmentsChanged)
        {
            /*if (!Quantum)
            {
                while (Curent.Draw.AStarGreedyString != null)
                    Curent.Draw = Curent.Draw.AStarGreedyString;
            }
            else
            {
                while (Curent.DrawQ.AStarGreedyString != null)
                    Curent.DrawQ = Curent.DrawQ.AStarGreedyString;
            }
           if (UsePenaltyRegardMechnisam && AStarGreedyHuristic)
                AllDrawKind = 4;
            else
                                                 if ((!UsePenaltyRegardMechnisam) && AStarGreedyHuristic)
                AllDrawKind = 3;
            if (UsePenaltyRegardMechnisam && (!AStarGreedyHuristic))
                AllDrawKind = 2;
            if ((!UsePenaltyRegardMechnisam) && (!AStarGreedyHuristic))
                AllDrawKind = 1;
            //Set Configuration To True for some unknown reason!.
            //UpdateConfigurationTableVal = true;                             
            SetAllDrawKindString();
            */
            try
            {
                if (!File.Exists(AllDrawKindString))
                {
                    GalleryStudio.RefregizMemmory rt = new GalleryStudio.RefregizMemmory(MovementsAStarGreedyHuristicFound, IInoreSelfObjects, UsePenaltyRegardMechnisam, BestMovments, PredictHuristic, OnlySelf, AStarGreedyHuristic, ArrangmentsChanged
                        );
                    if (!Quantum)
                    {
                        if (Curent.Draw != null)
                        {
                            Curent.Draw = Curent.RootFound();
                            rt.AllDrawCurrentAccess = Curent.Draw;
                            rt.RewriteAllDraw(FormRefrigtz.OrderPlate);
                            RefrigtzDLL.AllDraw.DrawTable = false;
                            Curent.SetBoxText("\r\nSaved Completed.");
                            Curent.RefreshBoxText();
                            //PictureBoxRefrigtz.SendToBack();
                            //PictureBoxTimerGray.SendToBack();
                            //PictureBoxTimerBrown.SendToBack();
                            //MessageBox.Show("Saved Completed.");
                        }
                    }
                    else {
                        if (Curent.DrawQ != null)
                        {
                            Curent.DrawQ = Curent.RootFoundQ();
                            rt.AllDrawCurrentAccessQ = Curent.DrawQ;
                            rt.RewriteAllDrawQ(FormRefrigtz.OrderPlate);
                            QuantumRefrigiz.AllDraw.DrawTable = false;
                            Curent.SetBoxText("\r\nSaved Completed.");
                            Curent.RefreshBoxText();
                            //PictureBoxRefrigtz.SendToBack();
                            //PictureBoxTimerGray.SendToBack();
                            //PictureBoxTimerBrown.SendToBack();
                            //MessageBox.Show("Saved Completed.");
                        }
                    }
                }
                else
                      if (File.Exists(AllDrawKindString))
                {
                    //DrawManagement(FOUND, UsePenaltyRegardMechnisam, AStarGreedyHuristic);

                    File.Delete(FormRefrigtz.AllDrawKindString);
                    GalleryStudio.RefregizMemmory rt = new GalleryStudio.RefregizMemmory(MovementsAStarGreedyHuristicFound, IInoreSelfObjects, UsePenaltyRegardMechnisam, BestMovments, PredictHuristic, OnlySelf, AStarGreedyHuristic, ArrangmentsChanged
                        );
                    //"Universal Root Founding";
                    if (Curent.Draw != null)
                    {
                        Curent.Draw = Curent.RootFound();
                        rt.AllDrawCurrentAccess = Curent.Draw;
                        rt.RewriteAllDraw(FormRefrigtz.OrderPlate);
                        RefrigtzDLL.AllDraw.DrawTable = false;
                        Curent.SetBoxText("\r\nSaved Completed.");
                        Curent.RefreshBoxText();
                        //PictureBoxRefrigtz.SendToBack();
                        //PictureBoxTimerGray.SendToBack();
                        //PictureBoxTimerBrown.SendToBack();
                        //MessageBox.Show("Saved Completed.");
                    }
                    //DrawManagement(FOUND, UsePenaltyRegardMechnisam, AStarGreedyHuristic);

                }
                return true;
            }
            catch (Exception t)
            {
                Log(t);
                return false;
            }
        }
    }
}

