using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMakerWPF.Classes
{
	internal class CityDictionary
	{
		public Dictionary<string, List<string>> Cities { get; private set; }

		public CityDictionary()
		{
			Cities = new Dictionary<string, List<string>>
			{
            //city names and post code dictionary is created acting as key and value respectively.
			//need to be updated if new addresses come in the future
				{"Aeroton",new List<string>{"2190"}},
				{"Bandar Puteri Puchong",new List <string> {"47100"}},
				{"Baoshan",new List <string>{"201907"}},
				{"Baoshan District",new List <string>{"201907"}},
				{"Bekasi",new List<string>{"17530"}},
				{"Brompton",new List<string>{"5007"}},
				{"Chaoyong District",new List<string>{"100026"}},
				{"City of Heshan",new List<string>{"529737"}},
				{"Dubai",new List <string>{""}},
				{"Ebisu Shibuya-ku",new List <string>{"150-0013"}},
				{"Hindmarsh",new List<string>{"5007"}},
				{"Jakarta Selatan",new List<string>{"12520"}},
				{"Jongno-gu",new List <string>{"3142"}},
				{"Karawatha",new List<string>{"4117" }},
				{"Kew",new List<string>{"3101"}},
				{"Kowloon Bay",new List <string>{""}},
				{"Malad West",new List <string>{"400 064"}},
				{"Mangere",new List <string>{"2022"}},
				{"Maquarie Park",new List<string>{"2113"}},
				{"Osborne Park",new List <string>{"6017"}},
				{"Pasig City",new List <string>{"1600" }},
				{"Puyallup",new List<string>{"98374"}},
				{"Regency Park",new List <string>{"5010"}},
				{"",new List <string>{"608526","401207"}},
				{"Taguig City",new List<string>{"1637"}},
				{"Thuan An Ditrict",new List<string>{"72000"}},
				{"Vroomshoop",new List<string>{"7680 AC"}}
			};
		}
	}
}
