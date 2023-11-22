using AngleSharp.Common;
using DBPediaSPARQLEndpointQuery;
using System.Text.RegularExpressions;
using VDS.RDF.Query;

namespace OpenLinkedDataLibrary.DBPedia
{
    public class DBPediaPersonQueries
    {
        public static List<PersonModel> GetAll()
        {
            var result = new List<PersonModel>();
            var dataset = SendQuery(
                "SELECT DISTINCT ?name ?img ?birthDate GROUP_CONCAT((?birthPlace); SEPARATOR=\"+\")  AS ?birthPlace     GROUP_CONCAT((?occupationName); SEPARATOR=\"+\")  AS ?occupation ?abstract GROUP_CONCAT((?awardName ); SEPARATOR=\"+\") AS ?award   GROUP_CONCAT((?officeName); SEPARATOR=\"+\") AS ?office  GROUP_CONCAT((?knownForName); SEPARATOR=\"+\") AS ?knownFor\r\nWHERE\r\n{\r\n?n a dbo:Person.\r\n?n dbo:thumbnail ?img.\r\n    ?n dbo:almaMater dbr:Taras_Shevchenko_National_University_of_Kyiv.\r\n    ?n rdfs:label ?name.\r\n\tFILTER langMatches( lang(?name), \"uk\" )\r\n    ?n dbo:abstract ?abstract.\r\n\tFILTER langMatches( lang(?abstract), \"uk\" )\r\n\t?n dbo:birthDate ?birthDate.\r\n\t?n dbo:birthPlace ?birthP.\r\n\t?birthP rdfs:label ?birthPlace.\r\n\tFILTER langMatches( lang(?birthPlace), \"uk\" ) \r\n\r\n OPTIONAL\r\n  {\r\n  ?n dbo:academicDiscipline ?academicDiscipline.\r\n  ?academicDiscipline rdfs:label ?academicDisciplineName.\r\n  FILTER langMatches( lang(?academicDisciplineName), \"uk\" )\r\n  }.\r\n   \r\nOPTIONAL{\r\n?n dbo:award ?award.\r\n?award rdfs:label ?awardName.\r\nFILTER langMatches( lang(?awardName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:office ?office.\r\n?office rdfs:label ?officeName.\r\nFILTER langMatches( lang(?officeName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:knownFor ?knownFor.\r\n?knownFor rdfs:label ?knownForName.\r\nFILTER langMatches( lang(?knownForName), \"uk\" )\r\n}.\r\n}"
                );
            foreach (var item in dataset)
            {
                result.Add(FromRDFToPersonModel(item));
            }
            return result;
        }

        public static SearchStatusEnum GetPersonByName(string name, out List<PersonModel> model)
        {
            model = new List<PersonModel>();
            var rset = GetAll();

            model = rset.Where(X => Regex.IsMatch(X.Name, name)).ToList();
            if (model.Count != 0)
                return SearchStatusEnum.Success;
            return SearchStatusEnum.No_matches;
        }

        public static SparqlResultSet SendQuery(string query)
        {
            if (query.Length == 0)
                throw new ArgumentException("Query string can't be empty.");
            else if (query == null)
                return null;
            var result = new SparqlResultSet();
            try
            {
                SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"), "http://dbpedia.org");
                result = endpoint.QueryWithResultSet(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message.ToString());
            }
            return result;
        }

        public static List<string> GetUniqueStrings(string info)
        {
            var input = info.Split('+').ToList();
            List<string> uniqueList = input.Distinct().ToList();
            return uniqueList;
        }

        public static string ConcatStrings(List<string> list)
        {
            string info = string.Empty;
            if (list.Count == 1)
                return info += list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                info += list[i] + ", ";
            }
            return info += list[list.Count - 1];
        }

        public static PersonModel FromRDFToPersonModel(ISparqlResult item)
        {
            var person = new List<string>();
            for (int i = 0; i < item.Count; i++)
            {
                var a = item[i].ToDictionary();
                string f = string.Empty;
                a.TryGetValue("Value", out f);
                if (f == null) a.TryGetValue("Uri", out f);
                if (string.IsNullOrEmpty(f)) f = "N/A";
                person.Add(f);
            }
            var model = new PersonModel();
            model.Name = person[0];
            model.Img = person[1];
            model.BirthDate = person[2];
            model.BirthPlace = ConcatStrings(GetUniqueStrings(person[3]));
            model.Occupation = ConcatStrings(GetUniqueStrings(person[4]));
            model.Description = person[5];
            model.Awards = ConcatStrings(GetUniqueStrings(person[6]));
            model.Office = ConcatStrings(GetUniqueStrings(person[7]));
            model.KnownFor = ConcatStrings(GetUniqueStrings(person[8]));
            return model;
        }
    }
}