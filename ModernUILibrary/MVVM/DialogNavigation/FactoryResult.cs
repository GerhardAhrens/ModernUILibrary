namespace ModernUI.MVVM.Base
{
    using System;
    using System.Windows.Controls;

    public class FactoryResult : Tuple<UserControl, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryResult"/> class.
        /// </summary>
        public FactoryResult(UserControl workContent, string usedTime = "") : base(workContent, usedTime)
        {
            this.WorkContent = workContent;
            this.UsedTime = usedTime;
        }

        public UserControl WorkContent { get; private set; }

        public string UsedTime { get; set; }

        public string ButtonDescription { get; set; }
    }
}
