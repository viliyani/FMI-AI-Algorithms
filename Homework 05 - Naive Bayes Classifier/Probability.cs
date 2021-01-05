namespace Hw05
{
    public class Probability
    {
        public Probability(string name, byte option, string className, double percentage)
        {
            Name = name;
            Option = option;
            ClassName = className;
            Percentage = percentage;
        }

        public string Name { get; set; }

        public byte Option { get; set; }

        public string ClassName { get; set; }

        public double Percentage { get; set; }

    }
}
