﻿using AngleSharp.Common;
using System;
using VDS.RDF.Query;

namespace DBPediaSPARQLEndpointQuery
{
    public class Queries
    {
        public static List<PersonModel> GetAll() 
        {
            var result = new List<PersonModel>();
            var dataset=SendQuery(
                "SELECT DISTINCT ?name ?img ?birthDate GROUP_CONCAT((?birthPlace); SEPARATOR=\"+\")  AS ?birthPlace     GROUP_CONCAT((?occupationName); SEPARATOR=\"+\")  AS ?occupation ?abstract GROUP_CONCAT((?awardName ); SEPARATOR=\"+\") AS ?award   GROUP_CONCAT((?officeName); SEPARATOR=\"+\") AS ?office  GROUP_CONCAT((?knownForName); SEPARATOR=\"+\") AS ?knownFor\r\nWHERE\r\n{\r\n?n a dbo:Person.\r\n?n dbo:thumbnail ?img.\r\n    ?n dbo:almaMater dbr:Taras_Shevchenko_National_University_of_Kyiv.\r\n    ?n rdfs:label ?name.\r\n\tFILTER langMatches( lang(?name), \"uk\" )\r\n    ?n dbo:abstract ?abstract.\r\n\tFILTER langMatches( lang(?abstract), \"uk\" )\r\n\t?n dbo:birthDate ?birthDate.\r\n\t?n dbo:birthPlace ?birthP.\r\n\t?birthP rdfs:label ?birthPlace.\r\n\tFILTER langMatches( lang(?birthPlace), \"uk\" ) \r\n\r\n OPTIONAL\r\n  {\r\n  ?n dbo:academicDiscipline ?academicDiscipline.\r\n  ?academicDiscipline rdfs:label ?academicDisciplineName.\r\n  FILTER langMatches( lang(?academicDisciplineName), \"uk\" )\r\n  }.\r\n   \r\nOPTIONAL{\r\n?n dbo:award ?award.\r\n?award rdfs:label ?awardName.\r\nFILTER langMatches( lang(?awardName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:office ?office.\r\n?office rdfs:label ?officeName.\r\nFILTER langMatches( lang(?officeName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:knownFor ?knownFor.\r\n?knownFor rdfs:label ?knownForName.\r\nFILTER langMatches( lang(?knownForName), \"uk\" )\r\n}.\r\n}"
                );
            foreach (var item in dataset)
            {
                result.Add(FromRDFToPersonModel(item));
            }
            return result;
        }

        public static SearchStatusEnum SearchByName(string name,out List<PersonModel> model) 
        {
            model = new List<PersonModel>();
            var rset = SendQuery(
                "SELECT DISTINCT ?name ?birthDate GROUP_CONCAT((?birthPlace); SEPARATOR=\"+\")  AS ?birthPlace   GROUP_CONCAT((?occupationName); SEPARATOR=\"+\")  AS ?occupation ?abstract GROUP_CONCAT((?awardName ); SEPARATOR=\"+\") AS ?award   GROUP_CONCAT((?officeName); SEPARATOR=\"+\") AS ?office  GROUP_CONCAT((?knownForName); SEPARATOR=\"+\") AS ?knownFor\r\nWHERE\r\n{\r\n?n a dbo:Person.\r\n    ?n dbo:almaMater dbr:Taras_Shevchenko_National_University_of_Kyiv.\r\n    ?n rdfs:label  ?name.\r\n  FILTER(REGEX(?name, \"" + name + "\",\"i\"))\r\nFILTER langMatches( lang(?name), \"uk\" )\r\n    ?n dbo:abstract ?abstract.\r\n\tFILTER langMatches( lang(?abstract), \"uk\" )\r\n\t?n dbo:birthDate ?birthDate.\r\n\t?n dbo:birthPlace ?birthP.\r\n\t?birthP rdfs:label ?birthPlace.\r\n\tFILTER langMatches( lang(?birthPlace), \"uk\" ) \r\nOPTIONAL\r\n{\r\n?n dbo:award ?award.\r\n?award rdfs:label ?awardName.\r\nFILTER langMatches( lang(?awardName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:office ?office.\r\n?office rdfs:label ?officeName.\r\nFILTER langMatches( lang(?officeName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:knownFor ?knownFor.\r\n?knownFor rdfs:label ?knownForName.\r\nFILTER langMatches( lang(?knownForName), \"uk\" )\r\n}.\r\nOPTIONAL\r\n{\r\n?n dbp:occupation ?occupation.\r\n?occupation rdfs:label ?occupationName.\r\nFILTER langMatches( lang(?occupationName), \"uk\" )\r\n}.\r\n}"
                );
            if (rset.Count > 0)
            {
                foreach (var item in rset) 
                {
                    model.Add(FromRDFToPersonModel(item));
                }               
                return SearchStatusEnum.Success;
            }
            return SearchStatusEnum.No_matches;
            
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

        static List<string> GetUniqueStrings(string info)
        {
            var input = info.Split('+').ToList();
            List<string> uniqueList = input.Distinct().ToList();
            return uniqueList;
        }

        static string ConcatStrings(List<string> list)
        {
            string info = string.Empty;
            for (int i = 0; i < list.Count - 1; i++)
            {
                info += list[i] + ", ";
            }
            return info += list[list.Count - 1];
        }

        static PersonModel FromRDFToPersonModel(ISparqlResult item) 
        {
            var person = new List<string>();
            for (int i = 0; i < item.Count; i++)
            {
                var a = item[i].ToDictionary();
                string f = string.Empty;
                a.TryGetValue("Value", out f);
                if (f == null) a.TryGetValue("Uri", out f);
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