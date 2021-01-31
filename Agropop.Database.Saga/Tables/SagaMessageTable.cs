using Amazon.DynamoDBv2.DataModel;
using System;

namespace Agropop.Database.Saga.Tables
{
    [DynamoDBTable("AgroPopSagaColletion")]
    public class SagaMessageTable
    {
        [DynamoDBHashKey] //Partition key
        public string Id
        {
            get; set;
        }
        
        // Properties included implicitly.
        public Object BodyMessage
        {
            get; set;
        }
    }
}
