using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hw05
{
    public class NBClassifier
    {
        // File Path for the Data Set
        private string dataFilePath;

        // All extracted records
        private List<Record> allRecords;

        // Records Sets
        private List<RecordsSet> allSets;

        public NBClassifier(string dataFilePath)
        {
            this.dataFilePath = dataFilePath;
            allRecords = new List<Record>();
            allSets = new List<RecordsSet>();
        }

        public void Run()
        {
            // 1. Extract Data From The File
            ReadDataFromFile();

            // 2. Create 10 sets with the records 
            CreateRecordsSets();

            // 3. Apply 10-Fold Cross-Validation
            Apply10FoldCrossValidation();
        }

        // Read and Extract the Records
        public void ReadDataFromFile()
        {
            // Read the lines from the file
            string[] lines = File.ReadAllLines(dataFilePath);

            // Create record objects
            foreach (var line in lines)
            {
                allRecords.Add(new Record(line.Split(',')));
            }
        }

        // Create 10 records sets
        public void CreateRecordsSets()
        {
            // Calculate needed numbers
            int totalItems = allRecords.Count;
            int numberOfSets = 10;
            int itemsPerStep = totalItems / numberOfSets;
            int parts = totalItems / itemsPerStep;

            // Add records for each set
            for (int i = 0; i < parts; i++)
            {
                int from = i * itemsPerStep;
                int to = i * itemsPerStep + itemsPerStep - 1;

                if (to > totalItems - 1)
                {
                    to = totalItems - 1;
                }

                // Create new set that will be added after that to the list which contains all sets
                RecordsSet newRecordSet = new RecordsSet();

                // Add records to the current set
                for (int j = from; j <= to; j++)
                {
                    newRecordSet.AddRecord(allRecords[j]);
                }

                // Add current set to the list with all sets
                allSets.Add(newRecordSet);
            }
        }

        // 10-Fold Cross-Validation
        public void Apply10FoldCrossValidation()
        {
            double allAccuracy = 0;

            for (int i = 0; i < 10; i++)
            {
                TestSetByIdx(i, ref allAccuracy);
            }

            double avgAccuracy = allAccuracy / 10;

            Console.WriteLine();
            Console.WriteLine($"==> Average Accuracy: {avgAccuracy:f2} %");
            Console.WriteLine();
            Console.WriteLine();
        }

        // Test 1 set but before that will train from other 9 sets
        public void TestSetByIdx(int testSetIdx, ref double allAccuracy)
        {
            // All probabilities
            List<Probability> probabilities = new List<Probability>();

            Type classType = typeof(Record);
            PropertyInfo[] properties = classType.GetProperties();

            // Calculate the probabilities from training data
            CalculateProbabilities(testSetIdx, probabilities, properties);

            // Test the test set 
            RecordsSet testSet = allSets[testSetIdx];

            int correctAnswers = 0; // total answers that are guessed by the AI
            int wrongAnswers = 0; // wrong answers which are not guessed

            // Loop all the records in the test set and try to guess them
            foreach (var testSetRecord in testSet.Records)
            {
                string correctClassName = testSetRecord.ClassName;

                double probabilityToBeDemocrat = CalculateProbabilityForTestSetRecord(testSetRecord, true, probabilities, properties);

                double probabilityToBeRepublican = CalculateProbabilityForTestSetRecord(testSetRecord, false, probabilities, properties);

                string guessedClassName = string.Empty;

                if (probabilityToBeDemocrat > probabilityToBeRepublican)
                {
                    guessedClassName = "democrat";
                }
                else
                {
                    guessedClassName = "republican";
                }

                if (correctClassName == guessedClassName)
                {
                    correctAnswers++;
                }
                else
                {
                    wrongAnswers++;
                }
            }

            double accuracy = (correctAnswers * 1.0) / (correctAnswers + wrongAnswers) * 1.0;
            accuracy *= 100;
            allAccuracy += accuracy;

            // Print result for this test set
            Console.WriteLine(new string('_', 20));
            Console.WriteLine($"Test Set: {testSetIdx + 1}");
            Console.WriteLine($"- Correct answers: {correctAnswers}");
            Console.WriteLine($"- Wrong answers: {wrongAnswers}");
            Console.WriteLine($"--> Accuracy: {accuracy:f2} %");
            Console.WriteLine();
            Console.WriteLine();
        }

        private double CalculateProbabilityForTestSetRecord(Record record, bool calcForDemocrat, List<Probability> probabilities, PropertyInfo[] properties)
        {
            string className = "democrat";

            if (calcForDemocrat == false)
            {
                className = "republican";
            }


            double resultProbability = 0;

            // Loop for each Property (Characteristic)
            foreach (var currentProp in properties)
            {
                if (currentProp.Name == "ClassName")
                {
                    continue;
                }

                string featureName = currentProp.Name;

                byte optionValue = (byte)record.GetType().GetProperty(featureName).GetValue(record);

                if (optionValue == 3)
                {
                    continue;
                }

                double currentProbability = probabilities
                                .Where(p => p.Name == featureName)
                                .Where(p => p.ClassName == className)
                                .Where(p => p.Option == optionValue)
                                .First()
                                .Percentage;

                resultProbability += Math.Log(currentProbability);

            }

            return resultProbability;
        }

        private void CalculateProbabilities(int testSetIdx, List<Probability> probabilities, PropertyInfo[] properties)
        {
            // Loop for each Property (Characteristic) and calculate probability
            foreach (var currentProp in properties)
            {
                if (currentProp.Name == "ClassName")
                {
                    continue;
                }

                // Helper Variables
                List<Record> trainRecords = GetTrainRecords(testSetIdx);
                string featureName = currentProp.Name;
                int totalDemocratYes = 0;
                int totalDemocratNo = 0;
                int totalDemocratBoth = 0;
                int totalRepublicanYes = 0;
                int totalRepublicanNo = 0;
                int totalRepublicanBoth = 0;

                // Probability -> Democrat and Yes
                totalDemocratYes = trainRecords
                                .Where(r => r.ClassName == "democrat")
                                .Where(
                                        r => (byte)r.GetType()
                                                    .GetProperty(featureName)
                                                    .GetValue(r) == 1
                                        )
                                .ToArray()
                                .Count();

                // Probability -> Democrat and No
                totalDemocratNo = trainRecords
                                .Where(r => r.ClassName == "democrat")
                                .Where(
                                        r => (byte)r.GetType()
                                                    .GetProperty(featureName)
                                                    .GetValue(r) == 2
                                        )
                                .ToArray()
                                .Count();

                // Probability -> Republican and Yes
                totalRepublicanYes = trainRecords
                                .Where(r => r.ClassName == "republican")
                                .Where(
                                        r => (byte)r.GetType()
                                                    .GetProperty(featureName)
                                                    .GetValue(r) == 1
                                        )
                                .ToArray()
                                .Count();

                // Probability -> Republican and No
                totalRepublicanNo = trainRecords
                                .Where(r => r.ClassName == "republican")
                                .Where(
                                        r => (byte)r.GetType()
                                                    .GetProperty(featureName)
                                                    .GetValue(r) == 2
                                        )
                                .ToArray()
                                .Count();

                totalDemocratBoth = totalDemocratYes + totalDemocratNo;
                totalRepublicanBoth = totalRepublicanYes + totalRepublicanNo;

                // Add probabilities for this feature to the list with all probabilities
                probabilities.Add(new Probability(
                        featureName,
                        1,
                        "democrat",
                        (totalDemocratYes * 1.0 / totalDemocratBoth * 1.0)
                    ));

                probabilities.Add(new Probability(
                        featureName,
                        2,
                        "democrat",
                        (totalDemocratNo * 1.0 / totalDemocratBoth * 1.0)
                    ));

                probabilities.Add(new Probability(
                        featureName,
                        1,
                        "republican",
                        (totalRepublicanYes * 1.0 / totalRepublicanBoth * 1.0)
                    ));

                probabilities.Add(new Probability(
                        featureName,
                        2,
                        "republican",
                        (totalRepublicanNo * 1.0 / totalRepublicanBoth * 1.0)
                    ));
            }
        }

        private List<Record> GetTrainRecords(int skipIdx)
        {
            List<Record> trainRecords = new List<Record>();

            for (int i = 0; i < allSets.Count; i++)
            {
                if (skipIdx == i)
                {
                    continue;
                }

                trainRecords.AddRange(allSets[i].Records);
            }

            return trainRecords;
        }
    }
}
