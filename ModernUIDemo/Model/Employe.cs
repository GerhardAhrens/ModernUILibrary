namespace ModernUIDemo.Model
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Nachname: {this.LastName}; Vorname: {this.FirstName}")]
    public class Employe
    {
        public Employe()
        {
        }

        public Employe(string lastName, string firstName, int salary, DateTime startDate, bool manager = false)
        {
            this.LastName = lastName;
            this.FirstName = firstName;
            this.Salary = salary;
            this.StartDate = startDate;
            this.Manager = manager;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Manager { get; set; }
        public int Salary { get; set; }
        public string Symbol { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName}, {this.LastName}";
        }
    }
}