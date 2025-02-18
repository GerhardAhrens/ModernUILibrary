namespace ModernBaseLibrary.Text
{
    public class FSM
    {
        public List<char> Sigma { get; set; }

        public List<FSMState> States { get; set; }

        public List<FSMTransition> Transitions { get; set; }

        public FSM()
        {
            States = new List<FSMState>();
            Transitions = new List<FSMTransition>();
        }

        public bool Simulate(string pattern)
        {
            var currentState = States.FirstOrDefault(x => x.IsStart);
            foreach (var c in pattern.ToLower())
            {
                var transitions = Transitions.Where(t => t.Start.Id == currentState.Id).ToList();
                foreach (var transition in transitions)
                {
                    currentState = transition.Transition(c);
                    if (currentState != null) break;
                }

                if (currentState == null) return false;
            }

            return currentState.IsEnd;
        }
    }

    public class FSMState
    {
        public string Id { get; set; }

        public bool IsStart { get; set; }

        public bool IsEnd { get; set; }
    }

    public class FSMTransition
    {
        public FSMState Start { get; set; }

        public FSMState End { get; set; }

        public char Item { get; set; }

        public FSMState Transition(char c)
        {
            if (Item == c) return End;
            return null;
        }
    }
}
