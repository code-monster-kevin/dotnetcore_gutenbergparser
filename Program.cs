using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Serilog;
using MongoDB.Bson;

namespace gutenberg_parser
{
    class Program
    {
        private static IEbookRepository ebookRepository;
        static void Main(string[] args)
        {
            string gbcachefolder = "D:\\gutenberg\\cache\\epub";
            const string connectionString = "mongodb://localhost:27017/ebookdb";

            var databaseContext = new DatabaseContext(connectionString);
            ebookRepository = new EbookRepository(databaseContext);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("gutenberg_parser.log")
                .CreateLogger();
            Log.Information("Parse started");

            processFolder(gbcachefolder);

            Log.Information("Parse ended");
        }

        private static void processFolder(string folderpath) {
            string[] files = Directory.GetFiles(folderpath, "*.rdf");
            foreach(string file in files) {
                parseEbook(file);
            }

            string [] subFolders = Directory.GetDirectories(folderpath);
            foreach(string folder in subFolders) {
                processFolder(folder);
            }
        }

        private static void parseEbook(string filepath) {
            Ebook ebook = readRdfFile(filepath);
            if (ebook != null) {
                ebookRepository.Add(ebook);
                // Console.WriteLine(JsonConvert.SerializeObject(ebook));
            }
            else {
                Log.Error("Ebook returned null: {0}", filepath);
            }
        }

        private static Ebook readRdfFile(string path) {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
                XmlNode rdfNode = doc.GetElementsByTagName("rdf:RDF")[0];
                manager.AddNamespace("rdf", rdfNode.Attributes["xmlns:rdf"].Value);
                manager.AddNamespace("rdfs", rdfNode.Attributes["xmlns:rdfs"].Value);
                manager.AddNamespace("dcterms", rdfNode.Attributes["xmlns:dcterms"].Value);
                manager.AddNamespace("pgterms", rdfNode.Attributes["xmlns:pgterms"].Value);

                XmlNode author = doc.SelectSingleNode(GutenRdfPath.Author.Value, manager);
                XmlNode title = doc.SelectSingleNode(GutenRdfPath.Title.Value, manager);
                XmlNode language = doc.SelectSingleNode(GutenRdfPath.Language.Value, manager);
                XmlNode ebookid = doc.SelectSingleNode(GutenRdfPath.EbookID.Value, manager);
                XmlNode downloads = doc.SelectSingleNode(GutenRdfPath.Downloads.Value, manager);              
                XmlNodeList subjects = doc.SelectNodes(GutenRdfPath.Subjects.Value, manager);

                List<string> subjectList = new List<string>();

                foreach(XmlNode subject in subjects) {
                    subjectList.Add(subject.InnerText);
                }

                Ebook ebook = new Ebook {
                    Id = ObjectId.GenerateNewId(),
                    Title = title?.InnerText,
                    Author = author?.InnerText,
                    Language = language?.InnerText,
                    EbookID = ebookid?.Attributes["rdf:about"].Value,
                    Downloads = downloads?.InnerText,
                    Subjects = subjectList
                };
                return ebook;
            }
            catch (Exception ex) {
                Log.Error("Parse file {0} error: {1}", path, ex.Message);
                return null;
            }
        }
    }
}
