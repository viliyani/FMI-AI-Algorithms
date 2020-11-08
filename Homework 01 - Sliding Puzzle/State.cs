using System;
using System.Collections.Generic;

namespace FirstSteps
{
    public class State
    {
        public int Depth { get; set; }
        public int ManhattanDistance { get; set; }
        public int Fvalue { get; set; }
        public int ZeroRow { get; set; }
        public int ZeroCol { get; set; }
        public string Direction { get; set; }
        public State Parent { get; set; }
        public List<State> Neighbours { get; set; }
        public List<int> Data { get; set; }

        public State() { }

        public override string ToString()
        {
            return $"[state info] Fvalue: {Fvalue} Depth: {Depth}, Distance: {ManhattanDistance}, Data: {String.Join(" ", Data)}";
        }
    }
}
