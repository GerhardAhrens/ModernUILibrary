namespace ModernBaseLibrary.Text
{
    public class KeywordFSMBuilder : IFSMBuilder
    {
        public FSM Build()
        {
            var keywords = new List<string>()
            {
                "SELECT",
                "FROM",
                "WHERE",
                "AND",
                "OR",
                "ORDER BY",
                "GROUP BY"
            };

            var fsm = new FSM();

            var state0 = new FSMState() { Id = "0", IsStart = true, IsEnd = false }; fsm.States.Add(state0);
            foreach (var keyword in keywords.Select(x => x.ToLower()))
            {
                var currentState = state0;
                var l = keyword.Length; var index = 1;
                for (var i = 0; i < l; i++)
                {
                    var end = i == l-1;
                    var state = new FSMState() { Id = $"{keyword}{index}", IsStart = false, IsEnd = end };
                    fsm.States.Add(state);

                    var transition = new FSMTransition() { Start = currentState, End = state, Item = keyword[i] };
                    fsm.Transitions.Add(transition);

                    currentState = state; index++;
                }
            }

            return fsm;
        }
    }
}
