using System.Collections.Generic;

public class GutenRdfPath {
    private GutenRdfPath(string value) { Value = value; }
    public string Value { get; set; }
    public static GutenRdfPath EbookID { get { return new GutenRdfPath("//pgterms:ebook"); } }
    public static GutenRdfPath Title { get { return new GutenRdfPath("//dcterms:title"); } }
    public static GutenRdfPath Author { get { return new GutenRdfPath("//dcterms:creator/pgterms:agent/pgterms:name"); } }
    public static GutenRdfPath Language { get { return new GutenRdfPath("//dcterms:language"); } }
    public static GutenRdfPath Downloads { get { return new GutenRdfPath("//pgterms:downloads"); } }
    public static GutenRdfPath Subjects { get { return new GutenRdfPath("//dcterms:subject/rdf:Description/rdf:value"); } }
}

public class Ebook : Document 
{
    public string EbookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Language { get; set; }
    public string Downloads { get; set; }
    public List<string> Subjects { get; set; }
}