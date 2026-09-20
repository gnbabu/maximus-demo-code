
using Microsoft.Data.SqlClient;
using System;
using System.Data;
namespace MES.Cred.Jobs.Helpers
{
    /// <summary>
    /// Options to control SqlBulkCopy behavior.
    /// </summary>
    public sealed class BulkCopyOptions
    {
        // Good defaults for staging tables: fast and safe
        public SqlBulkCopyOptions CopyOptions { get; set; } =
            SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.KeepNulls;

        /// <summary>Rows per server roundtrip.</summary>
        public int BatchSize { get; set; } = 5000;

        /// <summary>Timeout per bulk copy in seconds.</summary>
        public int BulkCopyTimeoutSeconds { get; set; } =3600;

        /// <summary>Raise progress callback every N rows; 0 = disabled.</summary>
        public int NotifyAfter { get; set; } = 0;

        /// <summary>Optional progress callback.</summary>
        public Action<SqlRowsCopiedEventArgs>? OnRowsCopied { get; set; }
    }
}

