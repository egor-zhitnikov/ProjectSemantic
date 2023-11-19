using VDS.RDF.Query;

namespace DBPediaSPARQLEndpointQuery
{
    public class Queries
    {
        public static List<PersonModel> GetAll() 
        {
            var result = new List<PersonModel>();
            SendQuery("SELECT DISTINCT ?name ?birthDate GROUP_CONCAT((?birthPlace); SEPARATOR=\"+\")  AS ?birthPlace ?abstract   GROUP_CONCAT((?subject); SEPARATOR=\"+\")  AS ?subject1  GROUP_CONCAT((?academicDisciplineName); SEPARATOR=\"+\")  AS ?academicDiscipline  GROUP_CONCAT((?occupationName); SEPARATOR=\"+\")  AS ?occupation  GROUP_CONCAT((?awardName ); SEPARATOR=\"+\") AS ?award   GROUP_CONCAT((?officeName); SEPARATOR=\"+\") AS ?office  GROUP_CONCAT((?knownForName); SEPARATOR=\"+\") AS ?knownFor\r\nWHERE\r\n{\r\n?n a dbo:Person.\r\n    ?n dbo:almaMater dbr:Taras_Shevchenko_National_University_of_Kyiv.\r\n    ?n rdfs:label ?name.\r\n\tFILTER langMatches( lang(?name), \"uk\" )\r\n    ?n dbo:abstract ?abstract.\r\n\tFILTER langMatches( lang(?abstract), \"uk\" )\r\n\t?n dbo:birthDate ?birthDate.\r\n\t?n dbo:birthPlace ?birthP.\r\n\t?birthP rdfs:label ?birthPlace.\r\n\tFILTER langMatches( lang(?birthPlace), \"uk\" ) \r\n\tOPTIONAL\r\n\t{\r\n\t?n dbp:subject ?subject.\r\n\t}.\r\n\tOPTIONAL\r\n\t{\r\n\t?n dbo:academicDiscipline ?academicDiscipline.\r\n\t?academicDiscipline rdfs:label ?academicDisciplineName.\r\n\tFILTER langMatches( lang(?academicDisciplineName), \"uk\" )\r\n\t}.\r\n\tOPTIONAL\r\n\t{\r\n\t?n dbp:occupation ?occupation.\r\n\t?occupation rdfs:label ?occupationName.\r\nFILTER langMatches( lang(?occupationName), \"uk\" )\r\n\t}.\r\n\t\r\n\r\n\t\r\nOPTIONAL{\r\n?n dbo:award ?award.\r\n?award rdfs:label ?awardName.\r\nFILTER langMatches( lang(?awardName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:office ?office.\r\n?office rdfs:label ?officeName.\r\nFILTER langMatches( lang(?officeName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:knownFor ?knownFor.\r\n?knownFor rdfs:label ?knownForName.\r\nFILTER langMatches( lang(?knownForName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:leader ?leader.\r\n?leader rdfs:label ?leaderName.\r\nFILTER langMatches( lang(?leaderName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:founder ?founder.\r\n?founder rdfs:label ?founderName.\r\nFILTER langMatches( lang(?founderName), \"uk\" )\r\n}.\r\n\r\n}");
            
            return result;
        }

        public static void GetByName(string Name) 
        {

        }

        public static SparqlResultSet SendQuery(string query) 
        {
            var result = new SparqlResultSet();
            try
            {
                SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"), "http://dbpedia.org");
                result =  endpoint.QueryWithResultSet(query);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException.Message.ToString());
            }
            return result;
        }
    }
}