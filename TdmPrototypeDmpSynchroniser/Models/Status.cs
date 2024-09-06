using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TdmPrototypeDmpSynchroniser.Models;

public class Status
{
    public string Description { get; set; } = default!;

    public bool Success { get; set; }
}