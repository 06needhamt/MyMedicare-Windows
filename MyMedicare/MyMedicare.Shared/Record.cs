using System;
using System.Collections.Generic;
using System.Text;

namespace MyMedicare
{
    public class Record
    {
        private User owner;
        private EnumRiskLevel riskLevel;
        private EnumTemperatureUnit temperatureUnit;
        private double temperature;
        private int bloodPressureLow;
        private int bloodPressureHigh;
        private int heartRate;
        private DateTime timeTaken;

        /// <summary>Initializes a new instance of the Record class.</summary>
        public Record(User owner, EnumRiskLevel riskLevel, EnumTemperatureUnit temperatureUnit, double temperature, int bloodPressureLow, int bloodPressureHigh, int heartRate, DateTime timeTaken)
        {
            this.owner = owner;
            this.riskLevel = riskLevel;
            this.temperatureUnit = temperatureUnit;
            this.temperature = temperature;
            this.bloodPressureLow = bloodPressureLow;
            this.bloodPressureHigh = bloodPressureHigh;
            this.heartRate = heartRate;
            this.timeTaken = timeTaken;
        }

        public User Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public EnumRiskLevel RiskLevel
        {
            get { return riskLevel; }
            set { riskLevel = value; }
        }

        public EnumTemperatureUnit TemperatureUnit
        {
            get { return temperatureUnit; }
            set { temperatureUnit = value; }
        }

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public int BloodPressureLow
        {
            get { return bloodPressureLow; }
            set { bloodPressureLow = value; }
        }

        public int BloodPressureHigh
        {
            get { return bloodPressureHigh; }
            set { bloodPressureHigh = value; }
        }

        public int HeartRate
        {
            get { return heartRate; }
            set { heartRate = value; }
        }

        public DateTime TimeTaken
        {
            get { return timeTaken; }
            set { timeTaken = value; }
        }
    }
}
