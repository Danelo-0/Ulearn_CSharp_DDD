using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public class Device
    {
        public int DeviceId;
        public string Name;


        public Device(int deviceId, string name)
        {
            this.DeviceId = deviceId;
            this.Name = name;

        }
    }

    public class Failure
    {
        public int DeviceId;
        public FailureType FailureTypeName;
        public DateTime TimeDevices;

        public Failure(int deviceId, FailureType failureTypeName, DateTime timeDevices)
        {
            this.DeviceId = deviceId;
            this.FailureTypeName = failureTypeName;
            this.TimeDevices = timeDevices;
        }
        public enum FailureType
        {
            Unexpected_shutdown,
            Short_non_responding,
            Hardware_failures,
            Connection_problems,
        }

        public static bool IsFailureSerious(FailureType failureType)
        {
            return failureType == FailureType.Unexpected_shutdown || failureType == FailureType.Hardware_failures;
        }

        public static bool Earlier(DateTime dateTime, DateTime timeDevices)
        {
            return dateTime > timeDevices;
        }
    }

    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            DateTime dateTime = new DateTime(year, month, day);

            List<DateTime> timesDevice = new List<DateTime>();
            foreach (var date in times)
            {
                timesDevice.Add(new DateTime((int)date[2], (int)date[1], (int)date[0]));
            }

            List<Failure> failureList = new List<Failure>();
            List<Device> deviceList = new List<Device>();
            for (int i = 0; i < deviceId.Length; i++)
            {
                var id = devices[i]["DeviceId"];
                var name = devices[i]["Name"];
                deviceList.Add(new Device((int)id, (string)name));
                failureList.Add(new Failure((int)id, (Failure.FailureType)failureTypes[i], timesDevice[i]));
            }

            return FindDevicesFailedBeforeDate(dateTime, deviceList, failureList);
        }

        public static List<string> FindDevicesFailedBeforeDate(
            DateTime dataTime,
            List<Device> deviceList,
            List<Failure> failureList)
        {
            var problematicDevices = new HashSet<int>();
            foreach (var failure in failureList)
            {
                if (Failure.IsFailureSerious(failure.FailureTypeName) && Failure.Earlier(dataTime, failure.TimeDevices))
                {
                    problematicDevices.Add(failure.DeviceId);
                }
            }

            var result = new List<string>();

            foreach (var device in deviceList)
            {
                if (problematicDevices.Contains(device.DeviceId))
                {
                    result.Add(device.Name);
                }
            }

            return result;
        }
    }
}