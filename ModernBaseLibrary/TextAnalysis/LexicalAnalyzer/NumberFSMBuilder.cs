namespace ModernBaseLibrary.Text
{
    public class NumberFSMBuilder : IFSMBuilder
    {
        public FSM Build()
        {
            var sigma = new List<char>();
            for (var c = '0'; c <= '9'; c++)
            {
                sigma.Add(c);
            }

            var fsm = new FSM();
            fsm.Sigma = sigma;

            var state0 = new FSMState() { Id = "0", IsStart = true, IsEnd = false }; fsm.States.Add(state0);
            var stateEnd = new FSMState() { Id = "1", IsStart = false, IsEnd = true }; fsm.States.Add(stateEnd);
            foreach (var letter in sigma)
            {
                var transition0 = new FSMTransition() { Start = state0, End = stateEnd, Item = letter };
                fsm.Transitions.Add(transition0);
            }            

            return fsm;
        }
    }
}
