using Amazon.DynamoDBv2.DataModel;
using System;

namespace Agropop.Database.Saga.Tables
{
    [DynamoDBTable("AgroPopSagaCollection")]
    public class SagaMessageTable
    {
        [DynamoDBHashKey] //Partition key
        public string IdMsr
        {
            get; set;
        }      
        
    }
}
