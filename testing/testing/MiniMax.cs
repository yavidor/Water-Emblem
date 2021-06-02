using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    class MiniMax
    {
        private int IMax;
        private int IMin;
        private double[,] ValuePlus;
        private double[,] ValueMinus;
        public MiniMax()
        {
            this.IMax = int.MaxValue;
            this.IMin = int.MinValue;
            this.ValuePlus = new double[24, 16] {
             {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
             {  0,  0,  0,  0,  0,  0, -5, -5, -5, -5,  0,  0,  0,  0,  0,  0},
             {  0,  0,  0,  0, -3, -3, -3, -3, -3, -3, -3, -3,  0,  0,  0,  0},
             {  0,  0,  0, -5, -2,  5,5.5,  3,  3,5.5,  5, -2, -5,  0,  0,  0},
             {  0, -5, -5,  0,  4,  5,5.5,  3,  3,5.5,  5,  4,  0, -5, -5,  0},
             {  0, -3, -2,  0,  6,  5,5.5,  3,  3,5.5,  5,  6,  0, -5, -5,  0},
             {  0, -3, -2,  5,  5,  4,  0,  5,  5,  0,  4,  4,  5, -2, -3,  0},
             {  0, -3, -3,  0,  1,  2,  0,4.5,4.5,  0,  1,  2,  0, -3, -3,  0},
             {  0, -3, -2,  1,  1,  1,  0,4.5,4.5,  0,  1,  1,  1, -2, -3,  0},
             {  0, -5, -3,  2,  2,  2,  0,4.5,4.5,  0,  2,  2,  2, -3, -5,  0},
             {  0,  0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  0,  0,  0,  0,  0},
             {  0, -5, -5, -5, -5, -5,3.5,  3,  3,3.5, -5, -5, -5, -5, -5,  0},
             {  0, -5, -5, -5, -5, -5,3.5,  2,  2,3.5, -5, -5, -5, -5, -5,  0},
             {  0,  0,  0,  0,  0,  0,  0,  1,  1,  0,  0,  0,  0,  0,  0,  0},
             {  0, -5, -5, -5, -5, -5,  0,0.5,0.5,  0, -5, -5, -5, -5, -5,  0},
             {  0, -5, -5, -5, -5, -5,  0,0.3,0.3,  0, -5, -5, -5, -5, -5,  0},
             {  0, -5, -5,  0, -5, -5,  0, -1, -1,  0, -5, -5,  0, -5, -5,  0},
             {  0, -5, -5, -5, -5, -5,  0, -2, -2,  0, -5, -5, -5, -5, -5,  0},
             {  0, -5, -5,  0, -5, -5, -5, -5, -5, -5, -5, -5,  0, -5, -5,  0},
             {  0, -5, -5,  0, -5, -5, -5, -5, -5, -5, -5, -5,  0, -5, -5,  0},
             {  0,  0,  0, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5,  0,  0,  0},
             {  0,  0,  0,  0, -5, -5, -5, -5, -5, -5, -5, -5,  0,  0,  0,  0},
             {  0,  0,  0,  0,  0,  0, -5, -5, -5, -5,  0,  0,  0,  0,  0,  0},
             {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}
};
            this.ValueMinus = new double[24, 16];
            for (int i = 0; i < ValueMinus.GetLength(0); i++)
            {
                for (int j = 0; j < ValueMinus.GetLength(1); j++)
                {
                    ValueMinus[ValueMinus.GetLength(0) - i - 1, j] =ValuePlus[i, j];
                }
            }
        }
        public Move MakeTurn(Map Map, int Depth)
        {
            double Best = IMin;
            Move BestFound = null;
            //to prevent errors
            double Score;
            List<Move> Moves = Map.GetAllActions();
            foreach (Move Move in Moves)
            {
                if (!Move.Source.Player)
                {
                    Move.Execute();
                    Score = EvaluateTurn(Map, Depth, IMin, IMax, true);
                    Move.Undo = true;
                    Move.Execute();
                    Move.Undo = false;
                    if (Score > Best)
                    {
                        Best = Score;
                        BestFound = Move;
                    }

                }
            }
            BestFound.HealAttackExecute();
            BestFound.Execute();
            return BestFound;
        }
        public double EvaluateTurn(Map Map, int Depth, double Alpha, double Beta, bool Player)
        {
            double Best;
            Tile[,] Grid = Map.Grid;
            if (Depth == 0)
            {
                return EvaluateBoard(Grid);
            }
            List<Move> Moves = Map.GetAllActions();//A list of every possible action by every unit
                                                   //in the current state of the game
            if (Player)
            {
                Best = IMin;

                foreach (Move Move in Moves)
                {

                    Move.Execute();
                    Move.HealAttackExecute();
                    Best = Math.Max(EvaluateTurn(Map, Depth - 1, Alpha, Beta, false), Best);
                    Move.Undo = true;
                    Move.Execute();
                    Move.HealAttackExecute();
                    Move.Undo = false;

                    Alpha = Math.Max(Best, Alpha);
                    if (Alpha >= Beta)
                        break;
                }
            }
            else
            {
                Best = IMax;
                foreach (Move Move in Moves)
                { 
                    Move.Execute();
                    Best = Math.Min(EvaluateTurn(Map, Depth - 1, Alpha, Beta, true), Best);
                    Move.Undo = true;
                    Move.Execute();
                    Move.Undo = false;
                   
                    Beta = Math.Min(Best, Beta);
                    if (Alpha >= Beta)
                        break;
                }
            }
            return Best;
        }
        public double EvaluateBoard(Tile[,] Grid)
        {
            double Value = 0;
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j].Unit != null)
                    {
                        if (Grid[i, j].Unit.Player)
                        {
                            Value += -1*(Grid[i, j].Unit.Stats["HP"]+ValuePlus[j, i]);
                        }
                        else
                        {
                            Value += (Grid[i, j].Unit.Stats["HP"]+ValueMinus[j, i]);
                        }
                    }
                }
            }
            return Value;
        }
    }
}