﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace QuantumRefrigiz
{
    [Serializable]
    public class DrawSoldier : ThingsConverter
    {
        //Pawns cannot make quantum moves.
        //A quantum move cannot be used to take a piece.
        bool IsQuntumMove = false;
        //Pieces have rings around them, filled in with colour. These rings show the probability that the piece is in that square.
        bool RingHalf = false;
        public int WinOcuuredatChiled = 0;
        //Iniatate Global Variables.
        private readonly object balanceLock = new object();
        private readonly object balanceLockS = new object();
        List<int[]> ValuableSelfSupported = new List<int[]>();

        public static Image[] S = new Image[2];
        public bool MovementsAStarGreedyHuristicFoundT = false;
        public bool IgnoreSelfObjectsT = false;
        public bool UsePenaltyRegardMechnisamT = true;
        public bool BestMovmentsT = false;
        public bool PredictHuristicT = true;
        public bool OnlySelfT = false;
        public bool AStarGreedyHuristicT = false;
        public bool ArrangmentsChanged = false;
        public static double MaxHuristicxS = Double.MinValue;
        public float RowS, ColumnS;
        public Color color;
        public ThinkingQuantumChess[] SoldierThinkingQuantum = new ThinkingQuantumChess[AllDraw.SodierMovments];
        public int[,] Table = null;
        public int Order = 0;
        public int Current = 0;
        int CurrentAStarGredyMax = -1;
        static void Log(Exception ex)
        {
            try
            {
                Object a = new Object();
                lock (a)
                {
                    string stackTrace = ex.ToString();
                    File.AppendAllText(AllDraw.Root + "\\ErrorProgramRun.txt", stackTrace + ": On" + DateTime.Now.ToString()); // path of file where stack trace will be stored.
                }
            }
            catch (Exception t) { Log(t); }
        }
        public bool AccessIsQuntumMove
        {
            get { return IsQuntumMove; }
        }
        public void Dispose()
        {
            ValuableSelfSupported = null;
            S = null;
        }
        public bool MaxFound(ref bool MaxNotFound)
        {
            try
            {
                double a = ReturnHuristic();
                if (MaxHuristicxS < a)
                {
                    Object O2 = new Object();
                    lock (O2)
                    {
                        MaxNotFound = false;
                        if (ThinkingQuantumChess.MaxHuristicx < MaxHuristicxS)
                            ThinkingQuantumChess.MaxHuristicx = a;
                        MaxHuristicxS = a;
                    }
                    return true;
                }
            }
            catch (Exception t)
            {
                Log(t);

            }
            MaxNotFound = true;
            return false;
        }
        public double ReturnHuristic()
        {
            double a = 0;
            for (int ii = 0; ii < AllDraw.SodierMovments; ii++)
                try
                {
                    a += SoldierThinkingQuantum[ii].ReturnHuristic(-1, -1, Order,false);
                }
                catch (Exception t)
                {
                    Log(t);
                }
            return a;
        }
        //Constructor 1.
        /* public DrawSoldier(int CurrentAStarGredy, bool MovementsAStarGreedyHuristicTFou, bool IgnoreSelfObject, bool UsePenaltyRegardMechnisa, bool BestMovment, bool PredictHurist, bool OnlySel, bool AStarGreedyHuris, bool Arrangments)
         {
             CurrentAStarGredyMax = CurrentAStarGredy;
             MovementsAStarGreedyHuristicFoundT = MovementsAStarGreedyHuristicTFou;
             IgnoreSelfObjectsT = IgnoreSelfObject;
             UsePenaltyRegardMechnisamT = UsePenaltyRegardMechnisa;
             BestMovmentsT = BestMovment;
             PredictHuristicT = PredictHurist;
             OnlySelfT = OnlySel;
             AStarGreedyHuristicT = AStarGreedyHuris;
             ArrangmentsChanged = Arrangments;
         }
         */
        //Constructor 2.
        public DrawSoldier(int CurrentAStarGredy, bool MovementsAStarGreedyHuristicTFou, bool IgnoreSelfObject, bool UsePenaltyRegardMechnisa, bool BestMovment, bool PredictHurist, bool OnlySel, bool AStarGreedyHuris, bool Arrangments, float i, float j, Color a, int[,] Tab, int Ord, bool TB, int Cur//, ref AllDraw. THIS
            ) :
            base(Arrangments, (int)i, (int)j, a, Tab, Ord, TB, Cur)
        {

            lock (balanceLock)
            {
                if (S[0] == null && S[1] == null)
                {
                    S[0] = Image.FromFile(AllDraw.ImagesSubRoot + "SG.png");
                    S[1] = Image.FromFile(AllDraw.ImagesSubRoot + "SB.png");
                }


                CurrentAStarGredyMax = CurrentAStarGredy;
                MovementsAStarGreedyHuristicFoundT = MovementsAStarGreedyHuristicTFou;
                IgnoreSelfObjectsT = IgnoreSelfObject;
                UsePenaltyRegardMechnisamT = UsePenaltyRegardMechnisa;
                BestMovmentsT = BestMovment;
                PredictHuristicT = PredictHurist;
                OnlySelfT = OnlySel;
                AStarGreedyHuristicT = AStarGreedyHuris;
                ArrangmentsChanged = Arrangments;
                //Initiate Global Variables.  
                Table = new int[8, 8];
                for (int ii = 0; ii < 8; ii++)
                    for (int jj = 0; jj < 8; jj++)
                        Table[ii, jj] = Tab[ii, jj];
                if (i >= 8 || j >= 8)
                    i = 7;
                for (int ii = 0; ii < AllDraw.SodierMovments; ii++)

                    SoldierThinkingQuantum[ii] = new ThinkingQuantumChess(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)i, (int)j, a, Tab, 4, Ord, TB, Cur, 16, 1);
                RowS = i;
                ColumnS = j;
                color = a;
                Order = Ord;
                Current = Cur;
            }
        }
        //Clone a Copy Method.
        public void Clone(ref DrawSoldier AA//, ref AllDraw. THIS
            )
        {
            int[,] Tab = new int[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Tab[i, j] = this.Table[i, j];
            //Initiate a Object and Assignemt of a Clone to Construction of a Copy.

            AA = new DrawSoldier(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, this.Row, this.Column, this.color, Tab, this.Order, false, this.Current
                );
            AA.ArrangmentsChanged = ArrangmentsChanged;
            for (int i = 0; i < AllDraw.SodierMovments; i++)
            {
                try
                {
                    AA.SoldierThinkingQuantum[i] = new ThinkingQuantumChess(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)this.Row, (int)this.Column);
                    this.SoldierThinkingQuantum[i].Clone(ref AA.SoldierThinkingQuantum[i]);
                }
                catch (Exception t)
                {
                    Log(t);
                    AA.SoldierThinkingQuantum[i] = null;
                }
            }
            AA.Table = new int[8, 8];
            for (int ii = 0; ii < 8; ii++)
                for (int jj = 0; jj < 8; jj++)
                    AA.Table[ii, jj] = Tab[ii, jj];
            AA.RowS = RowS;
            AA.ColumnS = ColumnS;
            AA.Order = Order;
            AA.Current = Current;
            AA.color = color;

        }
        //Drawing Soldiers On the Table Method..
        public void DrawSoldierOnTable(ref Graphics g, int CellW, int CellH)
        {

            lock (balanceLockS)
            {
                if (S[0] == null || S[1] == null)
                {
                    S[0] = Image.FromFile(AllDraw.ImagesSubRoot + "SG.png");
                    S[1] = Image.FromFile(AllDraw.ImagesSubRoot + "SB.png");
                }
                //When Conversion Solders Not Occured.
                if (!ConvertOperation((int)Row, (int)Column, color, Table, Order, false, Current))
                {

                    //Gray Color.
                    if (((int)Row >= 0) && ((int)Row < 8) && ((int)Column >= 0) && ((int)Column < 8))
                    {
                        try
                        {

                            //If Order is Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                        //Draw an Instant from File of Gray Soldeirs.
                                         //Draw a Gray Castles Instatnt Image On hte Tabe.
                                        g.DrawImage(S[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));

                                    if (ArrangmentsChanged)
                                    {
                                        if (Table[Row, Column - 1] < 0)
                                        {
                                            RingHalf = false;
                                        }
                                    }
                                    else
                                    {
                                        if (Table[Row, Column + 1] > 0)
                                        {
                                            RingHalf = false;
                                        }
                                    }

                                    if (RingHalf)
                                    {

                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);                                        
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(S[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(S[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);
                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Draw an Instatnt of Brown Soldier File On the Table.
                                    g.DrawImage(S[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (ArrangmentsChanged)
                                    {
                                        if (Table[Row, Column - 1] > 0)
                                        {
                                            RingHalf = false;
                                        }
                                    }
                                    else
                                    {
                                        if (Table[Row, Column + 1] < 0)
                                        {
                                            RingHalf = false;
                                        }
                                    }

                                    if (RingHalf)
                                    {

                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);                                        
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(S[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(S[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);
                                }
                            }
                        }
                        catch (Exception t)
                        {
                            Log(t);
                        }

                    }
                    else//If Minsister Conversion Occured.
                        if (ConvertedToMinister)
                    {
                        try
                        {
                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Draw of Gray Minsister Image File By an Instant.
                                    g.DrawImage(DrawMinister.M[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawMinister.M[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawMinister.M[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;

                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);

                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Draw a Image File on the Table Form n Instatnt One.
                                    g.DrawImage(DrawMinister.M[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawMinister.M[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawMinister.M[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;

                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);
                                    
                                }
                            }
                        }
                        catch (Exception t)
                        {
                            Log(t);
                        }
                    }
                    else if (ConvertedToCastle)//When Castled Converted.
                    {
                        try
                        {
                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Create on the Inststant of Gray Castles Images.
                                    g.DrawImage(DrawCastle.C[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawCastle.C[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawCastle.C[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                        }
                                    }

                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);
                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Creat of an Instant of Brown Image Castles.
                                    g.DrawImage(DrawCastle.C[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawCastle.C[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawCastle.C[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);

                                }
                            }
                        }
                        catch (Exception t)
                        {
                            Log(t);
                        }
                    }
                    else if (ConvertedToHourse)//When Hourse Conversion Occured.
                    {

                        try
                        {
                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Draw an Instatnt of Gray Hourse Image File.
                                    g.DrawImage(DrawHourse.H[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawHourse.H[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawHourse.H[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;

                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);

                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Creat of an Instatnt Hourse Image File.
                                    g.DrawImage(DrawHourse.H[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawHourse.H[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawHourse.H[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);

                                }
                            }
                        }
                        catch (Exception t)
                        {
                            Log(t);
                        }

                    }
                    else if (ConvertedToElefant)//When Elephant Conversion.
                    {
                        try
                        {
                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Draw an Instatnt Image of Gray Elephant.
                                    g.DrawImage(DrawElefant.E[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawElefant.E[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawElefant.E[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;

                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);                                    
                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Draw of Instant Image of Brown Elephant.
                                    g.DrawImage(DrawElefant.E[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                    if (RingHalf)
                                    {
                                        double Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        //if (Prob > 0)
                                        {
                                            if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                            {
                                                g.DrawImage(DrawElefant.E[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);
                                                if (AllDraw.QuntumTable[0, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1 && AllDraw.QuntumTable[1, AllDraw.NextRowQ, AllDraw.NextColumnQ] == -1)
                                                {
                                                    AllDraw.QuntumTable[0, (int)Row, (int)Column] = AllDraw.NextRowQ;
                                                    AllDraw.QuntumTable[1, (int)Row, (int)Column] = AllDraw.NextColumnQ;
                                                }
                                            }
                                            else
                                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                                            {
                                                AllDraw.NextRowQ = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                                                AllDraw.NextColumnQ = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                                                g.DrawImage(DrawElefant.E[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                                g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, (int)Prob);

                                            }
                                            AllDraw.QuntumTable[0, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;
                                            AllDraw.QuntumTable[1, AllDraw.QuntumTable[0, (int)Row, (int)Column], AllDraw.QuntumTable[1, (int)Row, (int)Column]] = -1;

                                        }
                                    }
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);

                                }
                            }

                        }
                        catch (Exception t)
                        {
                            Log(t);
                        }
                    }
                }
            }
        }
    }
}
//End of Documentation.