using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.CognitoSync;
using System.Net;
using Amazon.Runtime;

namespace AWS_SNSExamples.AWS_SNS
{
    public class SNSClientWrapper
    {
        public AmazonSimpleNotificationServiceClient snsClient;
        public SNSClientWrapper(AmazonSimpleNotificationServiceClient client)
        {
            this.snsClient = client;
        }

        //private CreatePlatformApplicationResult CreatePlatformApplication(string applicationName, Platform platform, string principal,String credential)
        //{
        //    CreatePlatformApplicationRequest platformApplicationRequest = new CreatePlatformApplicationRequest();
        //}
        public CreatePlatformEndpointResult CreatePlatformEndpoint(Platform platform,string platformToken, string applicationArn)
        {
            //CreatePlatformEndpointResult cper = new CreatePlatformEndpointResult();
            try
            {
                CreatePlatformEndpointRequest platformEndpointRequest = new CreatePlatformEndpointRequest();
                //platformEndpointRequest.CustomUserData = "";

                platformEndpointRequest.Token = platformToken;
                platformEndpointRequest.PlatformApplicationArn = applicationArn;
                return snsClient.CreatePlatformEndpoint(platformEndpointRequest);
            }
            catch(AmazonSimpleNotificationServiceException s) 
            {
                CreatePlatformEndpointResult cper = new CreatePlatformEndpointResult();
                cper.HttpStatusCode = HttpStatusCode.BadRequest;
                return cper;
            }
            //return cper;   
        }


        public PublishResult Publish(string endpointArn, Platform platform, Dictionary<string, Dictionary<string, MessageAttributeValue>> attributesMap,string messageSend)
        {
            PublishRequest publishRequest = new PublishRequest();
		Dictionary<string, MessageAttributeValue> notificationAttributes = getValidNotificationAttributes(attributesMap[platform]);
		if (notificationAttributes != null && notificationAttributes.Count()!=0) {
			
            publishRequest.MessageAttributes = notificationAttributes;
		}
		publishRequest.MessageStructure="json";
		// If the message attributes are not set in the requisite method,
		// notification is sent with default attributes
        //String message = getPlatformSampleMessage(platform,messageSend);
		Dictionary<string, string> messageMap = new Dictionary<string, string>();
		messageMap.Add(platform.Value.ToString(), messageSend);
		messageSend = SampleMessageGenerator.jsonify(messageMap);

		// For direct publish to mobile end points, topicArn is not relevant.
		publishRequest.TargetArn=endpointArn;

		publishRequest.Message=messageSend;
		return snsClient.Publish(publishRequest);
        }

        public AmazonWebServiceResponse Notification(Platform platform, string platformToken, Dictionary<string, Dictionary<string, MessageAttributeValue>> attrsMap, string mssg, string platformApplicationArn)
        {

		// The Platform Application Arn can be used to uniquely identify the
		// Platform Application.
		//String platformApplicationArn = "arn:aws:sns:us-east-1:776400555584:app/APNS_SANDBOX/uAround";

		// Create an Endpoint. This corresponds to an app on a device.
		CreatePlatformEndpointResult platformEndpointResult = CreatePlatformEndpoint(
				platform,
				platformToken, platformApplicationArn);
        //System.out.println(platformEndpointResult);
        if (platformEndpointResult.HttpStatusCode.Equals(HttpStatusCode.OK))
        {
            // Publish a push notification to an Endpoint.
            PublishResult publishResult = Publish(
                    platformEndpointResult.EndpointArn, platform, attrsMap, mssg);
            return publishResult;
        }
        return platformEndpointResult;
	}
        public Dictionary<string, MessageAttributeValue> getValidNotificationAttributes(Dictionary<string, MessageAttributeValue> notificationAttributes) 
        {
		    Dictionary<string, MessageAttributeValue> validAttributes = new Dictionary<string, MessageAttributeValue>();

		    if (notificationAttributes == null) return validAttributes;

                foreach(var item in notificationAttributes)
                {
                    if(!String.IsNullOrEmpty(item.Value.StringValue))
                    {
                        validAttributes.Add(item.Key,item.Value);
                    }
                }
		    return validAttributes;
	    }
        
        
        //public void demoNotification(Platform platform,string platformToken,
        //    Dictionary<string, Dictionary<string, MessageAttributeValue>> attrsMap) 
        //{
        //String platformApplicationArn = "arn:aws:sns:us-east-1:776400555584:app/APNS_SANDBOX/uAround";
        //// Create an Endpoint. This corresponds to an app on a device.
        //CreatePlatformEndpointResult platformEndpointResult = CreatePlatformEndpoint(
        //        platform,
        //        "CustomData - Useful to store endpoint specific data",
        //        platformToken, platformApplicationArn);
        //Console.WriteLine(platformEndpointResult);
        //   // System.out.println(platformEndpointResult);

        //// Publish a push notification to an Endpoint.
        //PublishResult publishResult = Publish(
        //        platformEndpointResult.EndpointArn, platform, attrsMap,getPlatformSampleMessage(platform,"Test"));
        //Console.WriteLine("Published! \n{MessageId="
        //        + publishResult.MessageId + "}");
        //// Delete the Platform Application since we will no longer be using it.
        ////deletePlatformApplication(platformApplicationArn);
	//}
        //public string getPlatformSampleMessage(Platform platform, string message)
        //{
        //    switch (platform)
        //    {
        //        case "APNS":
        //            return SampleMessageGenerator.getSampleAppleMessage(message);
        //        case "APNS_SANDBOX":
        //            return SampleMessageGenerator.getSampleAppleMessage(message);
        //        case "GCM":
        //            return SampleMessageGenerator.getSampleAndroidMessage(message);
        //        default:
        //            throw new PlatformNotSupportedException("Platform not supported : "
        //                    + platform.Value);
        //    }
        //}



    }
    
}
