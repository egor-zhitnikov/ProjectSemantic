
using OpenLinkedDataLibrary.DBPedia;
using OpenLinkedDataLibrary.Wikidata;

namespace DBPediaSPARQLEndpointQueryTests
{
    public class Tests
    {

        [Test]
        public void DBpedia_GetAll_PEOPLE_COUNT_EQUAL()
        {
            var actual = DBPediaPersonQueries.GetAll();
            Assert.AreEqual(91, actual.Count);
        }

        [Test]
        public void Wikidata_GetAll_PEOPLE_COUNT_EQUAL()
        {
            var actual = WikidataPersonQueries.GetAll();
            Assert.AreEqual(2090, actual.Count);
        }


        [Test]
        public void SendQuery_EMPTY_QUERY()
        {
            Assert.That("Query string can't be empty.", Is.EqualTo(Assert.Throws<ArgumentException>(() => DBPediaPersonQueries.SendQuery("")).Message.ToString()));
        }

        [Test]
        public void SendQuery_GOOD_REQUEST()
        {
            string query = "SELECT DISTINCT ?name ?birthDate GROUP_CONCAT((?birthPlace); SEPARATOR=\"+\")  AS ?birthPlace   GROUP_CONCAT((?occupationName); SEPARATOR=\"+\")  AS ?occupation ?abstract GROUP_CONCAT((?awardName ); SEPARATOR=\"+\") AS ?award   GROUP_CONCAT((?officeName); SEPARATOR=\"+\") AS ?office  GROUP_CONCAT((?knownForName); SEPARATOR=\"+\") AS ?knownFor\r\nWHERE\r\n{\r\n?n a dbo:Person.\r\n    ?n dbo:almaMater dbr:Taras_Shevchenko_National_University_of_Kyiv.\r\n    ?n rdfs:label  ?name.\r\n  FILTER langMatches( lang(?name), \"uk\" )\r\n    ?n dbo:abstract ?abstract.\r\n\tFILTER langMatches( lang(?abstract), \"uk\" )\r\n\t?n dbo:birthDate ?birthDate.\r\n\t?n dbo:birthPlace ?birthP.\r\n\t?birthP rdfs:label ?birthPlace.\r\n\tFILTER langMatches( lang(?birthPlace), \"uk\" ) \r\nOPTIONAL\r\n{\r\n?n dbo:award ?award.\r\n?award rdfs:label ?awardName.\r\nFILTER langMatches( lang(?awardName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:office ?office.\r\n?office rdfs:label ?officeName.\r\nFILTER langMatches( lang(?officeName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:knownFor ?knownFor.\r\n?knownFor rdfs:label ?knownForName.\r\nFILTER langMatches( lang(?knownForName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:occupation ?occupation.\r\n?occupation rdfs:label ?occupationName.\r\nFILTER langMatches( lang(?occupationName), \"uk\" )\r\n}.\r\n}";
            Assert.DoesNotThrow(() => DBPediaPersonQueries.SendQuery(query));
        }

        [Test]
        public void GetUniqueStrings_EMPTY_STRING()
        {
            Assert.That(emptyList, Is.EqualTo(DBPediaPersonQueries.GetUniqueStrings("")));
        }

        [Test]
        [TestCase("\"Гребінки+Україна+Українська Радянська Соціалістична Республіка+Україна (Лодзинське воєводство)+Україна+Українська Радянська Соціалістична Республіка+Україна (Лодзинське воєводство)+Союз Радянських Соціалістичних Республік\"")]
        public void GetUniqueStrings_NORMAL_STRING(string info)
        {
            var actual = DBPediaPersonQueries.GetUniqueStrings(info);
            Assert.AreEqual(5, actual.Count);
        }

        [Test]
        public void GetUniqueStrings_WITH_ONE_ELEMENT()
        {
            var actual = DBPediaPersonQueries.GetUniqueStrings("Київ");
            Assert.AreEqual(1, actual.Count); 
        }

        static List<string> emptyList = new List<string>();
        static List<string> listWithOneElement = new List<string>() { "Київ" };
        static List<string> fullList = new List<string>() { "Гребінки", "Україна", "Українська Радянська Соціалістична Республіка", "Україна (Лодзинське воєводство)", "Україна", "Українська Радянська Соціалістична Республіка", "Україна (Лодзинське воєводство)", "Союз Радянських Соціалістичних Республік" };


        [Test]
        public void ConcatStrings_EMPTY_LIST()
        {
            Assert.AreEqual("",DBPediaPersonQueries.ConcatStrings(emptyList));
        }
        [Test]
        public void ConcatStrings_FULL_LIST()
        {
            Assert.That(
                 "Гребінки, Україна, Українська Радянська Соціалістична Республіка, Україна (Лодзинське воєводство), Україна, Українська Радянська Соціалістична Республіка, Україна (Лодзинське воєводство), Союз Радянських Соціалістичних Республік"
                , Is.EqualTo(DBPediaPersonQueries.ConcatStrings(fullList)));
        }
        
        [Test]
        public void ConcatStrings_LIST_WITH_ONE_ELEMENT()
        {
            Assert.That("Київ", Is.EqualTo(DBPediaPersonQueries.ConcatStrings(listWithOneElement)));
        }

    }
}