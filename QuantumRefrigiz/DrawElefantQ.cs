using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace QuantumRefrigiz
{
    [Serializable]
    public class DrawElefantQ

    {
        //A quantum move cannot be used to take a piece.
        public bool IsQuntumMove = false;
        //Pieces have rings around them, filled in with colour. These rings show the probability that the piece is in that square.
        public bool RingHalf = false;
        public int WinOcuuredatChiled = 0;public int LoseOcuuredatChiled = 0;
        //private readonly object balancelock = new object();
        //private readonly object balancelockS = new object();
        public static Image[] E = new Image[2];
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
        public static double MaxHuristicxE = -20000000000000000;
        public float Row, Column;
        public ThinkingQuantumChess[] ElefantThinkingQuantum = new ThinkingQuantumChess[AllDraw.ElefantMovments];
        public int[,] Table = null;
        public Color color;
        public int Current = 0;
        public int Order;
        int CurrentAStarGredyMax = -1;

        static void Log(Exception ex)
        {
            
                Object a = new Object();
                lock (a)
                {
                    string stackTrace = ex.ToString();
                    File.AppendAllText(AllDraw.Root + "\\ErrorProgramRun.txt", stackTrace + ": On" + DateTime.Now.ToString()); // path of file where stack trace will be stored.
                }
           
        }
        public void Dispose()
        {
            ValuableSelfSupported = null;
            E = null;
        }
        public bool MaxFound(ref bool MaxNotFound)
        {
            
                double a = ReturnHuristic();
                if (MaxHuristicxE < a)
                {
                    Object O2 = new Object();
                    lock (O2)
                    {
                        MaxNotFound = false;
                        if (ThinkingQuantumChess.MaxHuristicx < MaxHuristicxE)
                            ThinkingQuantumChess.MaxHuristicx = a;
                        MaxHuristicxE = a;
                    }
                    return true;
                }
           
            MaxNotFound = true;
            return false;
        }
        public double ReturnHuristic()
        {
            double a = 0;
            for (var ii = 0; ii < AllDraw.ElefantMovments; ii++)
                
                    a += ElefantThinkingQuantum[ii].ReturnHuristic(-1, -1, Order, false);
               

            return a;
        }

        //Constructor 1.
        /*public DrawElefantQ(int CurrentAStarGredy, bool MovementsAStarGreedyHuristicTFou, bool IgnoreSelfObject, bool UsePenaltyRegardMechnisa, bool BestMovment, bool PredictHurist, bool OnlySel, bool AStarGreedyHuris, bool Arrangments)
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
        }*/
        //Constructor 2.
        public DrawElefantQ(int CurrentAStarGredy, bool MovementsAStarGreedyHuristicTFou, bool IgnoreSelfObject, bool UsePenaltyRegardMechnisa, bool BestMovment, bool PredictHurist, bool OnlySel, bool AStarGreedyHuris, bool Arrangments, float i, float j, Color a, int[,] Tab, int Ord, bool TB, int Cur//,ref AllDraw. THIS
            )
        {
            object balancelock = new object();

            lock (balancelock)
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
                //Initiate Global Variables By Local Parameters.
                Table = new int[8, 8];
                for (var ii = 0; ii < 8; ii++)
                    for (int jj = 0; jj < 8; jj++)
                        Table[ii, jj] = Tab[ii, jj];
                for (var ii = 0; ii < AllDraw.ElefantMovments; ii++)
                    ElefantThinkingQuantum[ii] = new ThinkingQuantumChess(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)i, (int)j, a, Tab, 16, Ord, TB, Cur, 4, 2);

                Row = i;
                Column = j;
                color = a;
                Order = Ord;
                Current = Cur;
            }

        }
        //Clone a Copy.
        public void Clone(ref DrawElefantQ AA//, ref AllDraw. THIS
            )
        {
            int[,] Tab = new int[8, 8];
            for (var i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Tab[i, j] = this.Table[i, j];
            //Initiate a Constructed Object an Clone a Copy.
            AA = new DrawElefantQ( CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, this.Row, this.Column, this.color, this.Table, this.Order, false, this.Current);
            AA.ArrangmentsChanged = ArrangmentsChanged;
            for (var i = 0; i < AllDraw.ElefantMovments; i++)
            {
                
                    AA.ElefantThinkingQuantum[i] = new ThinkingQuantumChess(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)this.Row, (int)this.Column);
                    this.ElefantThinkingQuantum[i].Clone(ref AA.ElefantThinkingQuantum[i]);
               
            }
            AA.Table = new int[8, 8];
            for (var ii = 0; ii < 8; ii++)
                for (int jj = 0; jj < 8; jj++)
                    AA.Table[ii, jj] = Tab[ii, jj];
            AA.Row = Row;
            AA.Column = Column;
            AA.Order = Order;
            AA.Current = Current;
            AA.color = color;

        }
        //Draw an Instatnt Elephant On the Table.
        public void DrawElefantOnTable(ref Graphics g, int CellW, int CellH)
        {
            object balancelockS = new object();

            
                int LastRow = -1, LastColumn = -1;
                if (AllDraw.LastRowQ != Row && AllDraw.LastColumnQ != Column&&AllDraw.LastRowQ!=-1&&AllDraw.LastColumnQ!=-1)
                    
                    {
                        if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1)
                        {
                            LastRow = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                        LastColumn = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                        IsQuntumMove = true;
                    }
                        else
                        if (AllDraw.LastRowQ != -1 && AllDraw.LastColumnQ != -1)
                        {
                            LastRow = AllDraw.LastRowQ;
                            LastColumn = AllDraw.LastColumnQ;
                            AllDraw.LastRowQ = -1;
                        AllDraw.LastColumnQ = -1;
                        IsQuntumMove = true;
                    }
                        AllDraw.LastRowQ = -1;
                        AllDraw.LastColumnQ = -1;
                        AllDraw.NextRowQ = -1;
                        AllDraw.NextColumnQ = -1;
                     
                    }

                if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[1, (int)Row, (int)Column] != -1)
                {
                    LastRow = AllDraw.QuntumTable[0, (int)Row, (int)Column];
                    LastColumn = AllDraw.QuntumTable[1, (int)Row, (int)Column];
                    RingHalf = true;
                }
                else
                    if (IsQuntumMove)
                {
                    RingHalf = true;
                    if (AllDraw.LastRowQ != -1 && AllDraw.LastColumnQ != -1)
                    {
                        LastRow = AllDraw.LastRowQ;
                        LastColumn = AllDraw.LastColumnQ;
                        AllDraw.LastRowQ = -1;
                        AllDraw.LastColumnQ = -1;
                    }
                }

                lock (balancelockS)
                {
                    if (E[0] == null || E[1] == null)
                    {
                        E[0] = Image.FromFile(AllDraw.ImagesSubRoot + "EG.png");
                        E[1] = Image.FromFile(AllDraw.ImagesSubRoot + "EB.png");
                    }

                    //Gray Color.
                    if (((int)Row >= 0) && ((int)Row < 8) && ((int)Column >= 0) && ((int)Column < 8))
                    {
                        if (Order == 1)
                        {
                            Object O1 = new Object();
                            lock (O1)
                            {    //Draw an Instant from File of Gray Soldeirs.
                                 //Draw an Instatnt Gray Elephant On the Table.
                                g.DrawImage(E[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                if (RingHalf)
                                {
                                    double Prob = 180;
                                    if (AllDraw.Less != 0)
                                        Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                    g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                    if (LastRow != -1 && LastColumn != -1)
                                    {
                                        g.DrawImage(E[0], new Rectangle(LastRow * CellW, LastColumn * CellH, CellW, CellH));
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((LastRow * CellW)), (int)(LastColumn * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        AllDraw.QuntumTable[0, (int)Row, (int)Column] = LastRow;
                                        AllDraw.QuntumTable[1, (int)Row, (int)Column] = LastColumn;
                                          AllDraw.QuntumTable[0, LastRow, LastColumn] = -1;
                                         AllDraw.QuntumTable[1, LastRow, LastColumn] = -1;
                                    }
                                    else
                                    if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[1, (int)Row, (int)Column] != -1)
                                    {
                                        g.DrawImage(E[0], new Rectangle(AllDraw.QuntumTable[0, (int)Row, (int)Column] * CellW, AllDraw.QuntumTable[1, (int)Row, (int)Column] * CellH, CellW, CellH));
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.QuntumTable[0, (int)Row, (int)Column] * CellW)), (int)(AllDraw.QuntumTable[1, (int)Row, (int)Column] * (float)CellH), CellW, CellH), -45, (int)Prob);

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
                                 //Draw an Instatnt Brown Elepehnt On the Table.
                                g.DrawImage(E[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                if (RingHalf)
                                {
                                    double Prob = 180;
                                    if (AllDraw.Less != 0)
                                        Prob = 180 * (AllDraw.Less / Double.MaxValue);
                                    g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, (int)Prob);
                                    if (LastRow != -1 && LastColumn != -1)
                                    {
                                        g.DrawImage(E[1], new Rectangle(LastRow * CellW, LastColumn * CellH, CellW, CellH));
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((LastRow * CellW)), (int)(LastColumn * (float)CellH), CellW, CellH), -45, (int)Prob);
                                        AllDraw.QuntumTable[0, (int)Row, (int)Column] = LastRow;
                                        AllDraw.QuntumTable[1, (int)Row, (int)Column] = LastColumn;
                                          AllDraw.QuntumTable[0, LastRow, LastColumn] = -1;
                                         AllDraw.QuntumTable[1, LastRow, LastColumn] = -1;
                                    }
                                    else
                                    if (AllDraw.QuntumTable[0, (int)Row, (int)Column] != -1 && AllDraw.QuntumTable[1, (int)Row, (int)Column] != -1)
                                    {
                                        g.DrawImage(E[1], new Rectangle(AllDraw.QuntumTable[0, (int)Row, (int)Column] * CellW, AllDraw.QuntumTable[1, (int)Row, (int)Column] * CellH, CellW, CellH));
                                        g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((AllDraw.QuntumTable[0, (int)Row, (int)Column] * CellW)), (int)(AllDraw.QuntumTable[1, (int)Row, (int)Column] * (float)CellH), CellW, CellH), -45, (int)Prob);

                                    }
                                }
                                else
                                    g.DrawArc(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)((Row * (float)CellW)), (int)(Column * (float)CellH), CellW, CellH), -45, 360);
                            }
                        }
                    }
                }
           
        }
    }
}
//End of Documentation.