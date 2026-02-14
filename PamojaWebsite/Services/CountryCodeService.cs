using Microsoft.EntityFrameworkCore;
using PamojaWebsite.Data;
using PamojaWebsite.Data.Contexts;
using System;
using System.Diagnostics.Metrics;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace PamojaWebsite.Services
{
    public class CountryCodeService
    {
        private readonly ApplicationDbContext _context;

        public CountryCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ImportCountryCodesAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Country codes file not found.", filePath);

            var codes = await File.ReadAllLinesAsync(filePath);
            int i = 0;
            foreach (var code in codes.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                CountryCode Code = new CountryCode();
                if (!_context.CountryCode.Any(c => c.Code == code.Trim()))
                {
                    Code.Code = code.Trim();
                    Code.Country = "";
                    Code.ISOCode = "";
                    _context.CountryCode.Add(Code);
                    await _context.SaveChangesAsync();
                    await ImportCountriesWithCodesAsync("wwwroot/Data/CountriesISOCodes.txt",
                        Code, i);          
                }
                i++;
            }

            
        }
        public async Task ImportCountriesWithCodesAsync(string filePath, CountryCode Code, int index)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Country ISO codes file not found.", filePath);

            var codes = await File.ReadAllLinesAsync(filePath);
            var split = codes[index].Split("/");
            var ISOCode = split[0].Trim();
            if (!_context.CountryCode.Any(c => c.ISOCode == ISOCode))
            {
                
                Code.ISOCode = ISOCode;
                await ImportCountries("wwwroot/Data/Countries.txt", Code, index);
                    _context.Attach(Code!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            

        }
        public async Task ImportCountries(string filePath, CountryCode Code, int Index)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Countries file not found.", filePath);

            var codes = await File.ReadAllLinesAsync(filePath);

            var country = codes[Index];
            if (!_context.CountryCode.Any(c => c.Country == country && Code.Country == country))
            {
                Code.Country = country;
                _context.Attach(Code!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            

        }

    }
    
}
