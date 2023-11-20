using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBPediaSPARQLEndpointQuery
{
    public class PersonModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string AcademicDiscipline { get; set; }
        public string Occupation { get; set; }
        public string Awards { get; set; }

        public string Office { get; set; }
        public string KnownFor { get; set; }
        public string Img { get; set; }


        public PersonModel()
        {
            
        }
    }
}
