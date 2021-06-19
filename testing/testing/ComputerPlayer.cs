﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    class ComputerPlayer
    {
        #region Data
        private int IMax; //The maximum value possible, Infinity
        private int IMin; //The minimum valie possible, -Infinity
        private double[,] ValuePlus; //2D array of the value each position on the board has for player 1
        private double[,] ValueMinus; //2D array of the value each position on the board has for player 2
        #endregion
        #region CTOR
        /// <summary>
        /// CTOR
        /// </summary>
        public ComputerPlayer()
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
                    ValueMinus[ValueMinus.GetLength(0) - i - 1, j] = ValuePlus[i, j];
                }
            }
        }
        #endregion
        #region Functions
        /// <summary>
        /// The main function, makes the best turn
        /// </summary>
        /// <param name="Map">The map of the game in the actual state of the game</param>
        /// <param name="Depth">How far down the game tree shall the function go</param>
        /// <returns></returns>
        public Move MakeTurn(Map Map, int Depth)
        {
            double Best = IMin;
            Move BestFound = null;
            //to prevent errors
            double Score;
            List<Move> Moves = Map.GetAllActions(); //A list of every possible action by every unit
                                                    //in the current state of the game
            foreach (Move Move in Moves)
            {
                if (!Move.Source.Player)
                {
                    Move.Execute();
                    Move.HealAttackExecute();
                    Score = EvaluateTurn(Map, Depth, IMin, IMax, true);
                    Move.Undo = true; //Since we are simulating every possible move, we need to undo the theoretical moves
                    Move.HealAttackExecute();
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
        /// <summary>
        /// Evaluate the value of a possible move
        /// </summary>
        /// <param name="Map">The map in the current theoretical state</param>
        /// <param name="Depth">How far down the game tree shall the function go</param>
        /// <param name="Alpha">The minimum value currently, don't check moves with lower value if maximizing</param>
        /// <param name="Beta">The maximum value currently, don't check moves with higher value if minimizing</param>
        /// <param name="Player">Who's "playing" this turn. True - max, false - min</param>
        /// <returns>The value of the turn</returns>
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
                    Move.Undo = true; //Since we are simulating every possible move, we need to undo the theoretical moves
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
                    Move.Undo = true; //Since we are simulating every possible move, we need to undo the theoretical moves
                    Move.Execute();
                    Move.Undo = false;
                   
                    Beta = Math.Min(Best, Beta);
                    if (Alpha >= Beta)
                        break;
                }
            }
            return Best;
        }
        /// <summary>
        /// Evaluate the current theoretical board
        /// </summary>
        /// <param name="Grid">The current theoretical board</param>
        /// <returns>The value of the board</returns>
        public double EvaluateBoard(Tile[,] Grid)
        {
            const int HpValue = 7;
            double Value = 0;
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j].Unit != null)
                    {
                        if (Grid[i, j].Unit.Player)
                        {
                            Value += -1*((Grid[i, j].Unit.Stats["HP"]*HpValue)+ValuePlus[j, i]);
                        }
                        else
                        {
                            Value += ((Grid[i, j].Unit.Stats["HP"]*HpValue)+ValueMinus[j, i]);
                        }
                    }
                }
            }
            return Value;
        }
        #endregion
    }
}