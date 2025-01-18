using System;
using System.Collections.Generic;
using UnityEngine;

namespace STD
{
    public static class DIService
    {
        private static List<Type> devices = new List<Type>();
        static DIService()
        {
            devices.Add(typeof(TestDeviceMono));
            devices.Add(typeof(ButtonMono));
            devices.Add(typeof(MoveablePlatMono));
            devices.Add(typeof(LeverMono));
            devices.Add(typeof(SpikeMono));
            devices.Add(typeof(FansMono));
        }
        public static Type GoFind(string typename)
        {
            return devices.Find((val) => { return val.Name == typename; });
        }
    }
}
