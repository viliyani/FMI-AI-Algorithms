using System;

namespace Hw05
{
    public class Record
    {
        public Record(params string[] args)
        {
            ClassName = args[0];
            HandicappedInfants = ProccessOption(args[1]);
            WaterProjectCostSharing = ProccessOption(args[2]);
            AdobtionOfTheBudgetResolution = ProccessOption(args[3]);
            PhysicianFeeFreeze = ProccessOption(args[4]);
            ElSalvadorAid = ProccessOption(args[5]);
            ReligiousGroupsInSchool = ProccessOption(args[6]);
            AntiSatelliteTestBan = ProccessOption(args[7]);
            AidToNicaraguanContras = ProccessOption(args[8]);
            MxMissile = ProccessOption(args[9]);
            Immigration = ProccessOption(args[10]);
            SynfuelsCorporationCutback = ProccessOption(args[11]);
            EducationSpending = ProccessOption(args[12]);
            SuperfundRightToSue = ProccessOption(args[13]);
            Crime = ProccessOption(args[14]);
            DutyFreeExports = ProccessOption(args[15]);
            ExportAdministrationActSouthAfrica = ProccessOption(args[16]);
        }

        public string ClassName { get; set; }

        public byte HandicappedInfants { get; set; }

        public byte WaterProjectCostSharing { get; set; }

        public byte AdobtionOfTheBudgetResolution { get; set; }

        public byte PhysicianFeeFreeze { get; set; }

        public byte ElSalvadorAid { get; set; }

        public byte ReligiousGroupsInSchool { get; set; }

        public byte AntiSatelliteTestBan { get; set; }

        public byte AidToNicaraguanContras { get; set; }

        public byte MxMissile { get; set; }

        public byte Immigration { get; set; }

        public byte SynfuelsCorporationCutback { get; set; }

        public byte EducationSpending { get; set; }

        public byte SuperfundRightToSue { get; set; }

        public byte Crime { get; set; }

        public byte DutyFreeExports { get; set; }

        public byte ExportAdministrationActSouthAfrica { get; set; }

        private byte ProccessOption(string input)
        {
            if (input == "y")
            {
                return 1;
            }
            else if (input == "n")
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }
}
