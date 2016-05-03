using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MyMedicare
{
    [DataContract()]
    public class Record
    {
        [DataMember()]
        private User owner;
        [DataMember()]
        private EnumRiskLevel riskLevel;
        [DataMember()]
        private EnumTemperatureUnit temperatureUnit;
        [DataMember()]
        private double temperature;
        [DataMember()]
        private double bloodPressureLow;
        [DataMember()]
        private double bloodPressureHigh;
        [DataMember()]
        private double heartRate;
        [DataMember()]
        private DateTime timeTaken;

        /// <summary>Initializes a new instance of the Record class.</summary>
        public Record(User owner, EnumRiskLevel riskLevel, EnumTemperatureUnit temperatureUnit, double temperature, double bloodPressureLow, double bloodPressureHigh, double heartRate, DateTime timeTaken)
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

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string unit = "";
            if (temperatureUnit == EnumTemperatureUnit.CELCIUS)
                unit = "\u00B0C";
            else if (temperatureUnit == EnumTemperatureUnit.FAHRENHEIT)
                unit = "\u2109F";
            return owner.Username + "\n"
                   + "Temperature " + temperature + " " + unit + "\n"
                   + "Blood Pressure Low " + bloodPressureLow + "\n"
                   + "Blood Pressure High " + bloodPressureHigh + "\n"
                   + "Heart Rate " + heartRate + "\n"
                   + "Risk Level " + riskLevel.ToString() + "\n"
                   + "Time Taken " + timeTaken.ToString() + "\n";
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

        public double BloodPressureLow
        {
            get { return bloodPressureLow; }
            set { bloodPressureLow = value; }
        }

        public double BloodPressureHigh
        {
            get { return bloodPressureHigh; }
            set { bloodPressureHigh = value; }
        }

        public double HeartRate
        {
            get { return heartRate; }
            set { heartRate = value; }
        }

        public DateTime TimeTaken
        {
            get { return timeTaken; }
            set { timeTaken = value; }
        }

        public string UnitString
        {
            get {
                if (temperatureUnit == EnumTemperatureUnit.CELCIUS)
                    return "\u00B0C";
                else if (temperatureUnit == EnumTemperatureUnit.FAHRENHEIT)
                    return "\u2109F";
                return "";
            }
        }
        public string TemperatureString
        {
            get
            {
                return "Temperature " + temperature + " " + UnitString;
            }
        }
        public string BloodPressureLowString
        {
            get
            {
                return "Blood Pressure Low " + bloodPressureLow;
            }
        }
        public string BloodPressureHighString
        {
            get
            {
                return "Blood Pressure High " + bloodPressureHigh;
            }
        }

        public string HeartRateString
        {
            get
            {
                return "Heart Rate " + heartRate;
            }
        }

        public string RiskLevelString
        {
            get
            {
                string str = riskLevel.ToString();
                return "Risk Level " + str.First().ToString().ToUpper() + str.Substring(1).ToLowerInvariant();
            }
        }
    }
}
