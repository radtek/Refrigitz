﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace QuantumRefrigiz
{
    [Serializable]
    public class DrawKing
    {
        //A quantum move cannot be used to take a piece.
        public bool IsQuntumMove = false;
        //Pieces have rings around them, filled in with colour. These rings show the probability that the piece is in that square.
        public static bool KingGrayNotCheckedByQuantumMove = false;
        public static bool KingBrownNotCheckedByQuantumMove = false;
        public bool RingHalf = false;
        public int WinOcuuredatChiled = 0;
        private readonly object balanceLock = new object();
        private readonly object balanceLockS = new object();
        public static Image[] K = new Image[2]; 
        //Initiate Global Variables.
        List<int[]> ValuableSelfSupported = new List<int[]>();
      
        public bool MovementsAStarGreedyHuristicFoundT = false;
        public bool IgnoreSelfObjectsT = false;
        public bool UsePenaltyRegardMechnisamT = true;
        public bool BestMovmentsT = false;
        public bool PredictHuristicT = true;
        public bool OnlySelfT = false;
        public bool AStarGreedyHuristicT = false;

        public bool ArrangmentsChanged = false;
        public static double MaxHuristicxK = -20000000000000000;
        public float Row, Column;
        public Color color;
        public int[,] Table = null;
        public ThinkingQuantumChess[] KingThinkingQuantum = new ThinkingQuantumChess[AllDraw.KingMovments];
        public int Current = 0;
        public int Order;
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
        public void Dispose()
        {
            ValuableSelfSupported = null;
            K = null;
        }

        public double ReturnHuristic()
        {
            double a = 0;
            for (int ii = 0; ii < AllDraw.KingMovments; ii++)
                try
                {
                    a += KingThinkingQuantum[ii].ReturnHuristic(-1, -1, Order,false);
                }
                catch (Exception t)
                {
                    Log(t);
                }

            return a;
        }
        public bool MaxFound(ref bool MaxNotFound)
        {
            try
            {
                double a = ReturnHuristic();
                if (MaxHuristicxK < a)
                {
                    Object O2 = new Object();
                    lock (O2)
                    {
                        MaxNotFound = false;
                        if (ThinkingQuantumChess.MaxHuristicx < MaxHuristicxK)
                            ThinkingQuantumChess.MaxHuristicx = a;
                        MaxHuristicxK = a;
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
        //Constructor 1.
        /*public DrawKing(int CurrentAStarGredy, bool MovementsAStarGreedyHuristicTFou, bool IgnoreSelfObject, bool UsePenaltyRegardMechnisa, bool BestMovment, bool PredictHurist, bool OnlySel, bool AStarGreedyHuris, bool Arrangments)
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
        public DrawKing(int CurrentAStarGredy, bool MovementsAStarGreedyHuristicTFou, bool IgnoreSelfObject, bool UsePenaltyRegardMechnisa, bool BestMovment, bool PredictHurist, bool OnlySel, bool AStarGreedyHuris, bool Arrangments, float i, float j, Color a, int[,] Tab, int Ord, bool TB, int Cur//, ref AllDraw. THIS
            )
        {

            lock (balanceLock)
            {
                if (K[0] == null && K[1] == null)
                {
                    K[0] = Image.FromFile(AllDraw.ImagesSubRoot + "KG.png");
                    K[1] = Image.FromFile(AllDraw.ImagesSubRoot + "KB.png");
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
                //Iniatite Global Variables.
                Table = new int[8, 8];
                for (int ii = 0; ii < 8; ii++)
                    for (int jj = 0; jj < 8; jj++)
                        Table[ii, jj] = Tab[ii, jj];
                for (int ii = 0; ii < AllDraw.KingMovments; ii++)
                    KingThinkingQuantum[ii] = new ThinkingQuantumChess(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)i, (int)j, a, Tab, 8, Ord, TB, Cur, 2, 6);

                Row = i;
                Column = j;
                color = a;
                Order = Ord;
                Current = Cur;
            }
        }
        //Clone a Copy.
        public void Clone(ref DrawKing AA//, ref AllDraw. THIS
            )
        {
            int[,] Tab = new int[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Tab[i, j] = this.Table[i, j];
            //Initiate a Construction Object and Clone a Copy.
            AA = new DrawKing( CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, this.Row, this.Column, this.color, this.Table, this.Order, false, this.Current);
            AA.ArrangmentsChanged = ArrangmentsChanged;
            for (int i = 0; i < AllDraw.KingMovments; i++)
            {
                try
                {
                    AA.KingThinkingQuantum[i] = new ThinkingQuantumChess(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)this.Row, (int)this.Column);
                    this.KingThinkingQuantum[i].Clone(ref AA.KingThinkingQuantum[i]);
                }
                catch (Exception t)
                {
                    Log(t);
                    AA.KingThinkingQuantum[i] = null;
                }
            }
            AA.Table = new int[8, 8];
            for (int ii = 0; ii < 8; ii++)
                for (int jj = 0; jj < 8; jj++)
                    AA.Table[ii, jj] = Tab[ii, jj];
            AA.Row = Row;
            AA.Column = Column;
            AA.Order = Order;
            AA.Current = Current;
            AA.color = color;

        }
        //Draw an Instatnt King on the Table Method.
        public void DrawKingOnTable(ref Graphics g, int CellW, int CellH)
        {

            try
            {

                if (AllDraw.LastRow == Row && AllDraw.LastColumn == Column)
                    if (AllDraw.LastRow != AllDraw.NextRow || AllDraw.LastColumn == AllDraw.NextColumn)
                    {
                        AllDraw.LastRow = -1;
                        AllDraw.LastColumn = -1;
                        AllDraw.NextRow = -1;
                        AllDraw.NextColumn = -1;
                        IsQuntumMove = true;
                    }
                if (IsQuntumMove)
                    RingHalf = true;
                lock (balanceLockS)
                {
                    if (K[0] == null || K[1] == null)
                    {
                        K[0] = Image.FromFile(AllDraw.ImagesSubRoot + "KG.png");
                        K[1] = Image.FromFile(AllDraw.ImagesSubRoot + "KB.png");
                    }
                    if (((int)Row >= 0) && ((int)Row < 8) && ((int)Column >= 0) && ((int)Column < 8))
                    { //Gray Order.
                        if (Order == 1)
                        {
                            Object O1 = new Object();
                            lock (O1)
                            {    //Draw an Instant from File of Gray Soldeirs.
                                 //Draw an Instatnt Gray King Image On the Table.
                                g.DrawImage(K[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                if (RingHalf)
                                {
                                    g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 180);
                                    g.DrawImage(K[0], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                    if (Row != this.KingThinkingQuantum[0].Row || Column != this.KingThinkingQuantum[0].Column)
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((this.KingThinkingQuantum[0].Row * (float)CellW)), (int)(this.KingThinkingQuantum[0].Column * (float)CellH), CellW, CellH), -45, 180);
                                    else
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, 180);

                                    if (AllDraw.OrderPlate == 1)
                                        KingGrayNotCheckedByQuantumMove = true;
                                    else
                                        KingBrownNotCheckedByQuantumMove = true;
                                }                                
                            }

                        }
                        else
                        {
                            Object O1 = new Object();
                            lock (O1)
                            {    //Draw an Instant from File of Gray Soldeirs.
                                 //Draw an Instatnt Brown King Image On the Table.
                                g.DrawImage(K[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                if (RingHalf)
                                {
                                    g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 180);
                                    if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                        g.DrawImage(K[1], new Rectangle(AllDraw.NextRowQ * CellW, AllDraw.NextColumnQ * CellH, CellW, CellH));
                                    if (Row != this.KingThinkingQuantum[0].Row || Column != this.KingThinkingQuantum[0].Column)
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((this.KingThinkingQuantum[0].Row * (float)CellW)), (int)(this.KingThinkingQuantum[0].Column * (float)CellH), CellW, CellH), -45, 180);
                                    else
                                        if (AllDraw.NextRowQ != -1 && AllDraw.NextColumnQ != -1)
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.NextRowQ * CellW)), (int)(AllDraw.NextColumnQ * (float)CellH), CellW, CellH), -45, 180);

                                    if (AllDraw.OrderPlate == 1)
                                        KingGrayNotCheckedByQuantumMove = true;
                                    else
                                        KingBrownNotCheckedByQuantumMove = true;
                                }
                            }
                        }
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
//End of Documentation.