using Amazon.CognitoSync;
using AWS_SNSExamples.Enums;

namespace AWS_SNSExamples.Models
{
    public class PushNotificationDto
    {
        public PushNotificationType NotificationType { get; set; }

        public string ToUserId { get; set; }
        public string ToDeviceId { get; set; }
        public string Text { get; set; }
    }
}