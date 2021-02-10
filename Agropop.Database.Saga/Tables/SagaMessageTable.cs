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

        [DynamoDBProperty(AttributeName = "Msg")]
        public string Msg
        {
            get; set;
        }

        [DynamoDBProperty(AttributeName = "TypeMsg")]
        public string TypeMsg
        {
            get; set;
        }

    }
}
