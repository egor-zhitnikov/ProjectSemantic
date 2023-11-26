using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLinkedDataLibrary.DBPedia
{
    public class PersonModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string AcademicDegree { get; set; }
        public string Occupation { get; set; }
        public string Awards { get; set; }
        public string Office { get; set; }
        public string KnownFor { get; set; }
        public string Img { get; set; }
        public string NominatedFor { get; set; }
        public string NotableWork { get; set; }
        public string MemberOf { get; set; }
        public string PositionHeld { get; set; }
        public string ParticipantIn { get; set; }
        public PersonModel()
        {
           
        }
    }
}
