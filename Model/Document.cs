using MongoDB.Bson;

public interface IDocument
{
    ObjectId Id { get; set; }
}

public abstract class Document : IDocument
{
   public ObjectId Id { get; set; }
}