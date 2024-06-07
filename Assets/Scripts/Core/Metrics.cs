using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public static class UnitExtension
    {
        // Meters = Unity default unit
        
        public static Vector3 MetersToKilometers(this Vector3 v) => v / 1000.0f;
        public static Vector3 MetersToHectometers(this Vector3 v) => v / 100.0f;
        public static Vector3 MetersToDecameters(this Vector3 v) => v / 10.0f;
        public static Vector3 MetersToDecimeters(this Vector3 v) => v * 10.0f;
        public static Vector3 MetersToCentimeters(this Vector3 v) => v * 100.0f;
        public static Vector3 MetersToMillimeters(this Vector3 v) => v * 1000.0f;

        public static Vector3 KilometersToMeters(this Vector3 v) => v * 1000.0f;
        public static Vector3 HectometersToMeters(this Vector3 v) => v * 100.0f;
        public static Vector3 DecametersToMeters(this Vector3 v) => v * 10.0f;
        public static Vector3 DecimetersToMeters(this Vector3 v) => v / 10.0f;
        public static Vector3 CentimetersToMeters(this Vector3 v) => v / 100.0f;
        public static Vector3 MillimetersToMeters(this Vector3 v) => v / 1000.0f;
    }
}