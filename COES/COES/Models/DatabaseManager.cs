using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace COES.Models
{
    /// <summary>
    /// Manages the database connection and queries.
    /// </summary>
    class DatabaseManager
    {
        string dbFile = "Data Source=test3.s3db;Version=3";

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseManager"/> class.
        /// </summary>
        public DatabaseManager()
        {

        }

        /// <summary>
        /// Performs a given query on a database and returns a <see cref="DataTable"/> containing the result.
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataTable Query(string query)
        {
            DataTable resultsTable = new DataTable();
            try
            {
                // Open a connection to the database file.
                SQLiteConnection con = new SQLiteConnection(dbFile);
                con.Open();

                // Setup command to handle sql.
                SQLiteCommand command = new SQLiteCommand(query, con);

                // Read the results then transfer using the reader into a generic DataTable.
                SQLiteDataReader reader = command.ExecuteReader();
                resultsTable.Load(reader);
                reader.Close();
                con.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return resultsTable;
        }

    }
}
