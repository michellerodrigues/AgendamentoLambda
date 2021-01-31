using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.TestUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SagaApiLambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestSQSEventLambdaFunction()
        {
            var snsEvent = new SNSEvent
            {
                Records = new List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage()
                        {
                            Message = "foobar"
                        }
                    }
                }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };

            var function = new Function();
           // await function.FunctionHandler(snsEvent, context);

            Assert.Contains("Processed record foobar", logger.Buffer.ToString());
        }
    }
}
