using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.CognitoSync;
using AWS_SNSExamples.Models;
using System.Configuration;

namespace AWS_SNSExamples.AWS_SNS
{
    public class SNSMobilePush
    {
        private SNSClientWrapper snsClientWrapper;

        public SNSMobilePush(AmazonSimpleNotificationServiceClient snsClient)
        {
            this.snsClientWrapper = new SNSClientWrapper(snsClient);
        }
        public Dictionary<string, Dictionary<string, MessageAttributeValue>> attributesMap()
        {
            Dictionary<string, Dictionary<string, MessageAttributeValue>> attributesDictionary= new Dictionary<string, Dictionary<string, MessageAttributeValue>>();
            attributesDictionary.Add("GCM", null);
		    attributesDictionary.Add("APNS", null);
            //attributesDictionary.Add("APNS_SANDBOX", null);
            return attributesDictionary;
        }
		
        
	 public void SendPushNotification(PushNotificationDto notification)
        {
            Guid APID;
            bool isApid = Guid.TryParse(notification.ToDeviceId, out APID);
            if (isApid)
            {
                //is Android
               string text = SampleMessageGenerator.getSampleAndroidMessage(notification);
                Dictionary<string, Dictionary<string, MessageAttributeValue>> test = attributesMap();
                snsClientWrapper.Notification(Platform.GCM, notification.ToDeviceId, test, text, ConfigurationManager.AppSettings["AndroidPlatformApplicationArn"]);
            }
            else
            {
                //is Ios
                string text = SampleMessageGenerator.getSampleAppleMessage(notification);

                Dictionary<string, Dictionary<string, MessageAttributeValue>> test = attributesMap();
                snsClientWrapper.Notification(Platform.APNS, notification.ToDeviceId, test, text, ConfigurationManager.AppSettings["ApplePlatformApplicationArn"]);
            }
        }

     //public void demoAppleAppNotification(string deviceToken)
     //{
     //    // TODO: Please fill in following values for your application. You can
     //    // also change the notification payload as per your preferences using
     //    // the method
     //    // com.amazonaws.sns.samples.tools.SampleMessageGenerator.getSampleAppleMessage()
     //     // This is 64 hex characters.
     //    Dictionary<string, Dictionary<string, MessageAttributeValue>> test = attributesMap();
     //    snsClientWrapper.demoNotification(Platform.APNS_SANDBOX, deviceToken, test);
     //}
    }

    
}
