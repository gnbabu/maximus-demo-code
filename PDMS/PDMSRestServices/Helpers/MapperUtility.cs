using PDMSRestServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MES.Cred.Jobs.Helpers
{
    public class MapperUtility
    {

        public static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToArray();

            // Create columns
            foreach (var prop in properties)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType)
                            ?? prop.PropertyType;

                dataTable.Columns.Add(prop.Name, type);
            }

            // Populate rows
            foreach (var item in items)
            {
                var values = new object[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration["RestApiSettings:mainDB"];
        }
        public static List<Provider_ID> MapProviderIds(
        List<Provider> providers)
        {
            var providerIds = new List<Provider_ID>();
            foreach(var provider in providers)
            {
                AddProviderId(
            providerIds,
            provider,
            "NPI",
            provider.NPI,
            provider.NPI_StartDate,
            provider.NPI_EndDate);

                AddProviderId(
                providerIds,
                provider,
                "SSN",
                provider.SSN);

                AddProviderId(
                providerIds,
                provider,
                "FEIN",
                provider.FEIN);

                AddProviderId(
                providerIds,
                provider,
                "CAQH",
                provider.CAQH);

                AddProviderId(
                providerIds,
                provider,
                "MCARE",
                provider.MCARE,
                provider.MCARE_StartDate,
                provider.MCARE_EndDate);

            }
            

            return providerIds;
        }

        private static void AddProviderId(
                        List<Provider_ID> providerIds,
                        Provider provider,
                        string providerIdType,
                        string providerIdValue,
                        string startDate = null,
                        string endDate = null)
        {
            if (string.IsNullOrWhiteSpace(providerIdValue))
                return;

            providerIds.Add(new Provider_ID
            {
                Segment_Id = provider.Segment_Id,
                Provider_Sequence_Number = provider.Provider_Sequence_Number,
                Enr_Prv_Id = provider.Enr_Prv_Id,
                Provider_Id_Type = providerIdType,
                Provider_Id_Value = providerIdValue,
                Pro_Start_Date = startDate ?? DateTime.Now.ToString("yyyyMMdd"),
                Pro_End_Date = endDate ?? "22991231",
                Attestation_Indicator = provider.Attestation_Indicator,
                Is_API_Request = 1
            });
        }

    }
}
