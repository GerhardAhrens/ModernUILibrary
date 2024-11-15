
namespace ModernUIDemo.Model
{
    using System.Collections.ObjectModel;

    public class Dept
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _ID;

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private ObservableCollection<Dept> _Children;

        public ObservableCollection<Dept> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        public override string ToString()
        {
            return $"{ID}.{Name}"; 
        }
    }
}
