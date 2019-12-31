using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace RefrigtzDLL
{
    [Serializable]
    public class DrawSoldier : ThingsConverter
    {
        
        StringBuilder Space = new StringBuilder("&nbsp;");
#pragma warning disable CS0414 // The field 'DrawSoldier.Spaces' is assigned but its value is never used
        int Spaces = 0;
#pragma warning restore CS0414 // The field 'DrawSoldier.Spaces' is assigned but its value is never used


        public int WinOcuuredatChiled = 0; public int LoseOcuuredatChiled = 0;
        //Iniatate Global Variables.
        //private readonly object balancelock = new object();
        //private readonly object balancelockS = new object();
        List<int[]> ValuableSelfSupported = new List<int[]>();

        [filed:NonSerialized]  public Image[] S = new Image[2];
        [filed:NonSerialized]  public Image[] E = new Image[2];
        [filed:NonSerialized]  public Image[] H = new Image[2];
        [filed:NonSerialized]  public Image[] C = new Image[2];
        [filed:NonSerialized]  public Image[] M = new Image[2];
        [filed:NonSerialized]  public Image[] K = new Image[2];
        public bool MovementsAStarGreedyHuristicFoundT = false;
        public bool IgnoreSelfObjectsT = false;
        public bool UsePenaltyRegardMechnisamT = true;
        public bool BestMovmentsT = false;
        public bool PredictHuristicT = true;
        public bool OnlySelfT = false;
        public bool AStarGreedyHuristicT = false;
        public bool ArrangmentsChanged = false;
        public static int MaxHuristicxS = int.MinValue;
        public float Row, Column;
        public Color color;
        public ThinkingChess[] SoldierThinking = new ThinkingChess[AllDraw.SodierMovments];
        public int[,] Table = null;
        public int Order = 0;
        public int Current = 0;
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
            //long Time = TimeElapced.TimeNow();Spaces++;
            ValuableSelfSupported = null;
            S = null;
            ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("Dispose:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
        }
        public bool MaxFound(ref bool MaxNotFound)
        {
            //long Time = TimeElapced.TimeNow();Spaces++;
            int a = ReturnHuristic();
            if (MaxHuristicxS < a)
            {
                Object O2 = new Object();
                lock (O2)
                {
                    MaxNotFound = false;
                    if (ThinkingChess.MaxHuristicx < MaxHuristicxS)
                        ThinkingChess.MaxHuristicx = a;
                    MaxHuristicxS = a;
                }
                ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("MaxFound:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
                return true;
            }

            MaxNotFound = true;
            ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("MaxFound:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
            return false;
        }
        public int ReturnHuristic()
        {
            int HaveKilled = 0;
            //long Time = TimeElapced.TimeNow();Spaces++;
            int a = 0;
            for (var ii = 0; ii < AllDraw.SodierMovments; ii++)

                a += SoldierThinking[ii].ReturnHuristic(-1, -1, Order, false,ref HaveKilled);

            ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("ReturnHuristic:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
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
            //long Time = TimeElapced.TimeNow();Spaces++;
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
                //Initiate Global Variables.  
                Table = new int[8, 8];
                for (var ii = 0; ii < 8; ii++)
                    for (var jj = 0; jj < 8; jj++)
                        Table[ii, jj] = Tab[ii, jj];
                for (var ii = 0; ii < AllDraw.SodierMovments; ii++)

                    SoldierThinking[ii] = new ThinkingChess(ii,1,CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)i, (int)j, a, Tab, 4, Ord, TB, Cur, 16, 1);
                Row = i;
                Column = j;
                color = a;
                Order = Ord;
                Current = Cur;
            }
            ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("DrawSoldier:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
        }
        //Clone a Copy Method.
        public void Clone(ref DrawSoldier AA//, ref AllDraw. THIS
            )
        {
            //long Time = TimeElapced.TimeNow();Spaces++;
            int[,] Tab = new int[8, 8];
            for (var i = 0; i < 8; i++)
                for (var j = 0; j < 8; j++)
                    Tab[i, j] = this.Table[i, j];
            //Initiate a Object and Assignemt of a Clone to Construction of a Copy.

            AA = new DrawSoldier(CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, this.Row, this.Column, this.color, Tab, this.Order, false, this.Current
                );
            AA.ArrangmentsChanged = ArrangmentsChanged;
            for (var i = 0; i < AllDraw.SodierMovments; i++)
            {

                AA.SoldierThinking[i] = new ThinkingChess(i,1,CurrentAStarGredyMax, MovementsAStarGreedyHuristicFoundT, IgnoreSelfObjectsT, UsePenaltyRegardMechnisamT, BestMovmentsT, PredictHuristicT, OnlySelfT, AStarGreedyHuristicT, ArrangmentsChanged, (int)this.Row, (int)this.Column);
                this.SoldierThinking[i].Clone(ref AA.SoldierThinking[i]);

            }
            AA.Table = new int[8, 8];
            for (var ii = 0; ii < 8; ii++)
                for (var jj = 0; jj < 8; jj++)
                    AA.Table[ii, jj] = Tab[ii, jj];
            AA.Row = Row;
            AA.Column = Column;
            AA.Order = Order;
            AA.Current = Current;
            AA.color = color;
            ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("Clone:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
        }
        int[,] CloneATable(int[,] Tab)
        {
            //long Time = TimeElapced.TimeNow();Spaces++;
            Object O = new Object();
            lock (O)
            {
                //Create and new an Object.
                int[,] Table = new int[8, 8];
                //Assigne Parameter To New Objects.
                for (var i = 0; i < 8; i++)
                    for (var j = 0; j < 8; j++)
                        Table[i, j] = Tab[i, j];
                //Return New Object.
                ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("CloneATable:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
                return Table;
            }

        }
        bool[,] CloneATable(bool[,] Tab)
        {
            //long Time = TimeElapced.TimeNow();Spaces++;
            Object O = new Object();
            lock (O)
            {
                //Create and new an Object.
                bool[,] Table = new bool[8, 8];
                //Assigne Parameter To New Objects.
                for (var i = 0; i < 8; i++)
                    for (var j = 0; j < 8; j++)
                        Table[i, j] = Tab[i, j];
                //Return New Object.
                ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("CloneATable:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
                return Table;
            }

        }
        //Drawing Soldiers On the Table Method..
        public void DrawSoldierOnTable(ref Graphics g, int CellW, int CellH)
        {
            object balancelockS = new object();

            lock (balancelockS)
            {
                if (g == null)
                    return;
                //long Time = TimeElapced.TimeNow();Spaces++;
                try
                {

                    if (S[0] == null || S[1] == null)
                    {
                        S[0] = Image.FromFile(AllDraw.ImagesSubRoot + "SG.png");
                        S[1] = Image.FromFile(AllDraw.ImagesSubRoot + "SB.png");
                    }
                    if (C[0] == null || C[1] == null)
                    {
                        C[0] = Image.FromFile(AllDraw.ImagesSubRoot + "BrG.png");
                        C[1] = Image.FromFile(AllDraw.ImagesSubRoot + "BrB.png");
                    }
                    if (E[0] == null || E[1] == null)
                    {
                        E[0] = Image.FromFile(AllDraw.ImagesSubRoot + "EG.png");
                        E[1] = Image.FromFile(AllDraw.ImagesSubRoot + "EB.png");
                    }
                    if (H[0] == null || H[1] == null)
                    {
                        H[0] = Image.FromFile(AllDraw.ImagesSubRoot + "HG.png");
                        H[1] = Image.FromFile(AllDraw.ImagesSubRoot + "HB.png");
                    }
                    if (M[0] == null || M[1] == null)
                    {
                        M[0] = Image.FromFile(AllDraw.ImagesSubRoot + "MG.png");
                        M[1] = Image.FromFile(AllDraw.ImagesSubRoot + "MB.png");
                    }
                    if (K[0] == null || K[1] == null)
                    {
                        K[0] = Image.FromFile(AllDraw.ImagesSubRoot + "KG.png");
                        K[1] = Image.FromFile(AllDraw.ImagesSubRoot + "KB.png");
                    }
                    //When Conversion Solders Not Occured.
                    if (!ConvertOperation((int)Row, (int)Column, color, CloneATable(Table), Order, false, Current))
                    {

                        //Gray Color.
                        if (((int)Row >= 0) && ((int)Row < 8) && ((int)Column >= 0) && ((int)Column < 8))
                        {


                            //If Order is Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                    g.DrawImage(S[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));

                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Draw an Instatnt of Brown Soldier File On the Table.
                                    g.DrawImage(S[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }


                        }
                        else//If Minsister Conversion Occured.
                            if (ConvertedToMinister)
                        {

                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Draw of Gray Minsister Image File By an Instant.
                                    g.DrawImage(M[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Draw a Image File on the Table Form n Instatnt One.
                                    g.DrawImage(M[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }

                        }
                        else if (ConvertedToCastle)//When Castled Converted.
                        {

                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Create on the Inststant of Gray Castles Images.
                                    g.DrawImage(C[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {    //Draw an Instant from File of Gray Soldeirs.
                                     //Creat of an Instant of Brown Image Castles.
                                    g.DrawImage(C[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }

                        }
                        else if (ConvertedToHourse)//When Hourse Conversion Occured.
                        {


                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Draw an Instatnt of Gray Hourse Image File.
                                    g.DrawImage(H[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (int)CellH), CellW, CellH));
                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Creat of an Instatnt Hourse Image File.
                                    g.DrawImage(H[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }


                        }
                        else if (ConvertedToElefant)//When Elephant Conversion.
                        {

                            //Color of Gray.
                            if (Order == 1)
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Draw an Instatnt Image of Gray Elephant.
                                    g.DrawImage(E[0], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }
                            else
                            {
                                Object O1 = new Object();
                                lock (O1)
                                {//Draw of Instant Image of Brown Elephant.
                                    g.DrawImage(E[1], new Rectangle((int)(Row * (float)CellW), (int)(Column * (float)CellH), CellW, CellH));
                                }
                            }


                        }
                    }

                }
                catch (Exception t)
                {
                    Log(t);
                }
                ////{ AllDraw.OutPut.Append("\r\n");for (int l = 0; l < Spaces; l++) AllDraw.OutPut.Append(Space);  AllDraw.OutPut.Append("DrawSoldierOnTable:" + (TimeElapced.TimeNow() - Time).ToString());}Spaces--;
            }
        }
    }
}
//End of Documentation.