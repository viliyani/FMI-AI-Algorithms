using System;

namespace Hw05
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Settings
            string dataFilePath = "../../votingrecords.data";

            // Naive Bayes Classifier
            var classifier = new NBClassifier(dataFilePath);

            classifier.Run();
        }
    }
}
