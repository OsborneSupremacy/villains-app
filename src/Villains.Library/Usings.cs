global using Amazon.DynamoDBv2;
global using Amazon.Lambda.APIGatewayEvents;
global using Amazon.Lambda.Core;
global using Amazon.S3;
global using FluentResults;
global using FluentValidation;
global using System.Net;
global using Villains.Library.Extensions;
global using Villains.Library.Messaging;
global using Villains.Library.Models;
global using Villains.Library.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]