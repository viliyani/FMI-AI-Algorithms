using System.Collections.Generic;
using System.Linq;

namespace FirstSteps
{
    public class PriorityQueue
    {
        private List<State> states;
        public List<State> States { get { return states; } }

        public int Count
        {
            get
            {
                return states.Count;
            }
        }

        public PriorityQueue()
        {
            states = new List<State>();
        }

        public State Dequeue()
        {
            this.states = states.OrderBy(s => s.Fvalue).ToList();
            State bestState = states.First();
            states.RemoveAt(0);

            return bestState;
        }

        public void Enqueue(State state)
        {
            states.Add(state);
        }

    }
}
