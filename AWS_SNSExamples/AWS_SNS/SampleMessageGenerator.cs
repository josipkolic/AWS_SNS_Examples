using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AWS_SNSExamples.Enums;
using AWS_SNSExamples.Models;

namespace AWS_SNSExamples.AWS_SNS
{
    public class SampleMessageGenerator
    {
        public static String defaultMessage = "This is the default message";
        private static JsonConverter objectMapper;
        public static string jsonify(Object message) {
		try {
			return JsonConvert.SerializeObject(message);
		} 
        catch (Exception e) {
			throw e;
		}
	}

        public static string getSampleAppleMessage(PushNotificationDto notificationObj)
        {
            Dictionary<string, Object> appleMessageMap = new Dictionary<String, Object>();
            Dictionary<string, Object> appMessageMap = new Dictionary<String, Object>();
            appMessageMap.Add("alert", notificationObj.Text);
            appMessageMap.Add("badge", 1);
            appMessageMap.Add("sound", "beep");
            appleMessageMap.Add("aps", appMessageMap);
           // appleMessageMap.Add("actionId", notificationObj.ActionId);
            appleMessageMap.Add("tag", Enum.GetName(typeof(PushNotificationType), notificationObj.NotificationType).ToString());
            appleMessageMap.Add("device_token",notificationObj.ToDeviceId);
            return jsonify(appleMessageMap);
        }
        public static string getSampleAndroidMessage(PushNotificationDto notificationObj)
        {
            Dictionary<string, Object> extraParameters = new Dictionary<string, Object>();
            Dictionary<string, Object> androidMessageMap = new Dictionary<string, Object>();
            androidMessageMap.Add("collapse_key", "Welcome");
            extraParameters.Add("message", notificationObj.Text);
            extraParameters.Add("apid", notificationObj.ToDeviceId);
           // extraParameters.Add("actionId", notificationObj.ActionId);
            extraParameters.Add("tag", Enum.GetName(typeof(PushNotificationType), notificationObj.NotificationType).ToString());
            androidMessageMap.Add("data", extraParameters);
            androidMessageMap.Add("delay_while_idle", true);
            androidMessageMap.Add("time_to_live", 125);
            androidMessageMap.Add("dry_run", false);
            return jsonify(androidMessageMap);
        }
        private static Dictionary<string, string> getData(string message)
        {
            Dictionary<string, string> payload = new Dictionary<string, string>();
            payload.Add("message", message);
            return payload;
        }

    }
}
