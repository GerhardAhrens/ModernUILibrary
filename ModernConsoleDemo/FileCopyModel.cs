namespace ModernConsoleDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ModernConsole.CommandLine;

    [Help("== CommandLine Test Model ==")]
    public class FileCopyModel
    {
        [Flag("quelle", "q")]
        [Help("Quellverzeichnis")]
        public string source { get; set; }

        [Flag("ziel", "z")]
        [Help("Zielverzeichnis")]
        public string destination { get; set; }

        [Flag("username", "u")]
        [Help("Benutzername")]
        public string Username { get; set; }

        [Flag("delete", "d")]
        public bool IsDeleted
        {
            get
            {
                return this.Actions.Any(a => a == Action.Delete);
            }
            set
            {
                if (this.Actions.Any(a => a == Action.Delete) == false)
                {
                    this.Actions.Add(Action.Delete);
                }
            }
        }

        [Flag("action", "a")]
        public List<Action> Actions { get; set; } = Enum.GetValues(typeof(Action)).Cast<Action>().Select(v => v).ToList();

        public enum Action
        {
            Delete,
            Copy,
            Move
        }
    }
}