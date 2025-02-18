namespace ModernBaseLibrary.Text
{
    public class OperatorFSMBuilder : IFSMBuilder
    {
        public FSM Build()
        {
            var operators = new List<string>()
            {
                "=",
                "<",
                ">",
                "like"
            };

            var fsm = new FSM();

            var state0 = new FSMState() { Id = "0", IsStart = true, IsEnd = false }; fsm.States.Add(state0);
            var stateEnd = new FSMState() { Id = "end", IsStart = false, IsEnd = true }; fsm.States.Add(stateEnd);
            foreach (var op in operators)
            {
                var currentState = state0;
                foreach (var c in op.Substring(0, op.Length-1))
                {
                    var state = new FSMState() { Id = $"{op}{c}", IsStart = false, IsEnd = false }; fsm.States.Add(state);
                    var transition = new FSMTransition() { Start = currentState, End = state, Item = c };
                    fsm.Transitions.Add(transition);

                    currentState = state;
                }

                var transitionEnd = new FSMTransition() { Start = currentState, End = stateEnd, Item = op.Last() };
                fsm.Transitions.Add(transitionEnd);
            }

            return fsm;
        }
    }
}
