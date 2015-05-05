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
using AWS_SNSExamples.Enums;

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

            if (notification.ToDeviceId.Length==64)
            {
                //is Ios
                string iOSMessage = SampleMessageGenerator.getSampleAppleMessage(notification);
                Dictionary<string, Dictionary<string, MessageAttributeValue>> test = attributesMap();
                snsClientWrapper.Notification(PlatformType.iOSPlatform, notification.ToDeviceId, iOSMessage, ConfigurationManager.AppSettings["ApplePlatformApplicationArn"]);
            }
            else
            {
                //is Android
                string androidMessage = SampleMessageGenerator.getSampleAndroidMessage(notification);
                Dictionary<string, Dictionary<string, MessageAttributeValue>> test = attributesMap();
                snsClientWrapper.Notification(PlatformType.AndroidPlatform, notification.ToDeviceId, androidMessage, ConfigurationManager.AppSettings["AndroidPlatformApplicationArn"]);
               
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
