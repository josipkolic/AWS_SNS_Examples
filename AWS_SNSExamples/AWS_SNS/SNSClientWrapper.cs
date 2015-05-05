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
using AWS_SNSExamples.Enums;

namespace AWS_SNSExamples.AWS_SNS
{
    public class SNSClientWrapper
    {
        public AmazonSimpleNotificationServiceClient snsClient;
        public SNSClientWrapper(AmazonSimpleNotificationServiceClient client)
        {
            this.snsClient = client;
        }

        // <summary>
        // 
        // </summary>
        // <param name="applicationName"></param>
        // <param name="platform"></param>
        // <param name="principal"></param>
        // <param name="credential"></param>
        // <returns></returns>
        //private CreatePlatformApplicationResult CreatePlatformApplication(string applicationName, Platform platform, string principal, String credential)
        //{
        //    CreatePlatformApplicationRequest platformApplicationRequest = new CreatePlatformApplicationRequest();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="platformToken"></param>
        /// <param name="applicationArn"></param>
        /// <returns></returns>
        public CreatePlatformEndpointResult CreatePlatformEndpoint(string platformToken, string applicationArn)
        {
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
        }

        // <summary>
        // 
        // </summary>
        // <param name="endpointArn"></param>
        // <param name="platform"></param>
        // <param name="attributesMap"></param>
        // <param name="messageSend"></param>
        // <returns></returns>
        public PublishResult Publish(string endpointArn, string platform,string messageSend)
        {
            PublishRequest publishRequest = new PublishRequest();

		publishRequest.MessageStructure="json";
		// If the message attributes are not set in the requisite method,
		// notification is sent with default attributes

		Dictionary<string, string> messageMap = new Dictionary<string, string>();
		messageMap.Add(platform, messageSend);
		messageSend = SampleMessageGenerator.jsonify(messageMap);

		// For direct publish to mobile end points, topicArn is not relevant.
		publishRequest.TargetArn=endpointArn;

		publishRequest.Message=messageSend;
		return snsClient.Publish(publishRequest);
        }

        public AmazonWebServiceResponse Notification(string platform, string platformToken, string mssg, string platformApplicationArn)
        {

		// Create an Endpoint. This corresponds to an app on a device.
		CreatePlatformEndpointResult platformEndpointResult = CreatePlatformEndpoint(
				platformToken, platformApplicationArn);
        //System.out.println(platformEndpointResult);
        if (platformEndpointResult.HttpStatusCode.Equals(HttpStatusCode.OK))
        {
            // Publish a push notification to an Endpoint.
            PublishResult publishResult = Publish(
                    platformEndpointResult.EndpointArn,platform, mssg);
            return publishResult;
        }
        return platformEndpointResult;
	}
        
    }
    
}
