/**
 * 
 * @author : Kai Tam    
 *  
*/

namespace Company.Service.Models
{
    public class CompanyRecord
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string Website { get; set; }
    }
}
