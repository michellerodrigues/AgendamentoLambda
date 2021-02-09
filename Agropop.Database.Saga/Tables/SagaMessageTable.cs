using Amazon.DynamoDBv2.DataModel;
using System;

namespace Agropop.Database.Saga.Tables
{
    [DynamoDBTable("AgroPopSagaCollection")]
    public class SagaMessageTable
    {
        [DynamoDBHashKey("Id")]
        public string IdMsg
        {
            get; set;
        }

        public string TypeMessage
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

    }
}
