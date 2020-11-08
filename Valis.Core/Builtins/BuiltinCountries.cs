using System.Collections.ObjectModel;
using System.Globalization;

namespace Valis.Core
{
    public static class BuiltinCountries
    {
        public static readonly VLCountry Bulgaria = new VLCountry { CountryId = 1, Name = "Bulgaria" };
        public static readonly VLCountry Cyprus = new VLCountry { CountryId = 2, Name = "Cyprus" };
        public static readonly VLCountry France = new VLCountry { CountryId = 3, Name = "France" };
        public static readonly VLCountry Germany = new VLCountry { CountryId = 4, Name = "Germany" };
        public static readonly VLCountry Greece = new VLCountry { CountryId = 5, Name = "Greece" };

        internal static Collection<VLCountry> s_countries;
        public static Collection<VLCountry> Countries { get { return s_countries; } }

        static BuiltinCountries()
        {
            s_countries = new Collection<VLCountry>();
            s_countries.Add(Bulgaria);
            s_countries.Add(Cyprus);
            s_countries.Add(France);
            s_countries.Add(Germany);
            s_countries.Add(Greece);
        }

        public static VLCountry GetCountryById(int countryId, bool throwExceptionIfNotExists = true)
        {
            foreach(var item in Countries)
            {
                if (item.CountryId == countryId)
                    return item;
            }
            if (throwExceptionIfNotExists)
            {
                throw new VLException(string.Format(CultureInfo.InvariantCulture, "BuiltinCountries.GetCountryById() do not recognise countryId {0}!", countryId));
            }

            return null;
        }
    }
}
