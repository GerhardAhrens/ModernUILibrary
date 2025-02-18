namespace ModernBaseLibrary.Text
{
    public class LiteralFSMBuilder : IFSMBuilder
    {
        public FSM Build()
        {
            var sigma = new List<char>();
            for (var c = 'A'; c <= 'Z'; c++)
            {
                sigma.Add(c);
            }

            for (var c = 'a'; c <= 'z'; c++)
            {
                sigma.Add(c);
            }

            sigma.Add('%');

            var fsm = new FSM();
            fsm.Sigma = sigma;

            var stateStart = new FSMState() { Id = "start", IsStart = true, IsEnd = false }; fsm.States.Add(stateStart);
            var stateInterm = new FSMState() { Id = "interm", IsStart = false, IsEnd = false }; fsm.States.Add(stateInterm);
            var stateEnd = new FSMState() { Id = "end", IsStart = false, IsEnd = true }; fsm.States.Add(stateEnd);

            foreach (var letter in sigma)
            {
                var transition = new FSMTransition() { Start = stateInterm, End = stateInterm, Item = letter };
                fsm.Transitions.Add(transition);
            }

            var transitionStartInterm = new FSMTransition() { Start = stateStart, End = stateInterm, Item = '\'' };
            fsm.Transitions.Add(transitionStartInterm);
            var transitionIntermEnd = new FSMTransition() { Start = stateInterm, End = stateEnd, Item = '\'' };
            fsm.Transitions.Add(transitionIntermEnd);

            return fsm;
        }
    }
}
