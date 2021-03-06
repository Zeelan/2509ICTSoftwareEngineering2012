﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows;

namespace COES.Models
{
    /// <summary>
    /// Manages the database connection and queries.
    /// </summary>
    static class DatabaseManager
    {
        static SQLiteConnection con;
        static String dbFile = "database.s3db";
        static String dbConnectionString ;


        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseManager"/> class.
        /// </summary>
        static DatabaseManager()
        {
            dbConnectionString = String.Format("Data Source={0};Version=3", dbFile);

            //check if file exits.... else create and populate
            if (File.Exists(dbFile))
            {
                con = new SQLiteConnection(dbConnectionString);
                con.Open();
            }
            else
            {
                // automatically creates a blank database file using the same name
                con = new SQLiteConnection(dbConnectionString);
                con.Open();

                //now populate....

                String sql = @"

            DROP TABLE IF EXISTS customer;
CREATE TABLE customer (	customer_id			INTEGER 	PRIMARY KEY ASC,
						first_name 			VARCHAR 	NOT NULL,
						last_name			VARCHAR 	NOT NULL,
						phone_number		VARCHAR     NOT NULL UNIQUE,
						credit_card_name	VARCHAR		NULL,
						credit_card_number	INTEGER		NULL,
						credit_card_expiry	VARCHAR		NULL,
						street_no			VARCHAR		NULL,
						street				VARCHAR		NULL,
						suburb_post_code	VARCHAR		NULL,
						status				VARCHAR		NULL,
						status_date			DATE		NULL,
						created_date		DATE		NULL);

DROP INDEX IF EXISTS customer_n1;					
CREATE INDEX customer_n1 ON customer (phone_number);
						
DROP TRIGGER IF EXISTS customer_ai;
CREATE TRIGGER customer_ai AFTER INSERT ON customer
	BEGIN
		UPDATE customer 
		SET created_date = DATETIME('NOW'), 
		    status_date = DATETIME('NOW'),
			status = 'A'
		WHERE rowid = new.rowid;
	END;
DROP TRIGGER IF EXISTS customer_au;
CREATE TRIGGER customer_au AFTER UPDATE ON customer
	BEGIN
		UPDATE customer 
		SET status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;


DROP TABLE IF EXISTS customer_order;
CREATE TABLE customer_order ( 	customer_order_id	INTEGER 	PRIMARY KEY ASC,
								customer_id			INTEGER  	NULL,
								order_date			DATE		NULL,
								created_date		DATE		NULL,
								delivery_flag		VARCHAR		NULL,
								paid_status			VARCHAR		NULL,
								status				VARCHAR		NULL,
								total_cost			DECIMAL(4,2)NULL);
DROP TRIGGER IF EXISTS cusotmer_order_ai;
CREATE TRIGGER customer_order_ai AFTER INSERT ON customer_order
	BEGIN
		UPDATE customer_order 
		SET created_date = DATETIME('NOW'), 
            status = 'N',
			paid_status = 'N',
			total_cost = 0
		WHERE rowid = new.rowid;
	END;
								
DROP TABLE IF EXISTS order_item;
CREATE TABLE order_item ( 	order_item_id	INTEGER 	PRIMARY KEY ASC,
							order_id		INTEGER  	NOT NULL,
							menu_item_id	INTEGER  	NOT NULL,
							item_cost		DECIMAL(4,2)NULL);
							


DROP TABLE IF EXISTS menu_item;
CREATE TABLE menu_item (	menu_item_id	INTEGER 	PRIMARY KEY ASC,
							menu_item_name	INTEGER  	NOT NULL,
							description     VARCHAR		NOT NULL,
							status_date		DATE		NULL,
							created_date	DATE		NULL,
							item_cost		DECIMAL(4,2)	NULL);
							
DROP TRIGGER IF EXISTS menu_item_ai;
CREATE TRIGGER menu_item_ai AFTER INSERT ON menu_item
	BEGIN
		UPDATE menu_item 
		SET created_date = DATETIME('NOW'), 
		    status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;
DROP TRIGGER IF EXISTS menu_item_au;
CREATE TRIGGER menu_item_au AFTER UPDATE ON menu_item
	BEGIN
		UPDATE menu_item 
		SET status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;
	
DROP TABLE IF EXISTS menu;
CREATE TABLE menu (	menu_id			INTEGER 	PRIMARY KEY ASC,
					menu_item_id	INTEGER  	NOT NULL,
					created_date	DATE		NOT NULL);
							
DROP TRIGGER IF EXISTS menu_ai;
CREATE TRIGGER menu_ai AFTER INSERT ON menu
	BEGIN
		UPDATE menu 
		SET created_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;

DROP TABLE IF EXISTS admin_staff;
CREATE TABLE admin_staff (	admin_staff_id		INTEGER 	PRIMARY KEY ASC,
                            admin_staff_name	VARCHAR		NOT NULL,
							p_word				INTEGER  	NOT NULL,
							status_date     	DATE		NULL,
							created_date		DATE		NULL);
							
DROP TRIGGER IF EXISTS admin_staff_ai;
CREATE TRIGGER admin_staff_ai AFTER INSERT ON admin_staff
	BEGIN
		UPDATE admin_staff 
		SET created_date = DATETIME('NOW'), 
		    status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;
							
                        
	";		    // end of literal string for sql create of database.
		
                 //run as quickquery
                QuickQuery(sql);


            }



        }

        /// <summary>
        /// Performs a given query on a database and returns a <see cref="DataTable"/> containing the result.
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataTable Query(string query)
        {
            DataTable resultsTable = new DataTable();
            try
            {
                // Open a connection to the database file.
                
                // Setup command to handle sql.
                SQLiteCommand command = new SQLiteCommand(query,con);

                // Read the results then transfer using the reader into a generic DataTable.
                SQLiteDataReader reader = command.ExecuteReader();
                resultsTable.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return resultsTable;
        }

        /// <summary> 
        /// used to insert a dictionery of values into the specified table
        /// </summary>
        /// <param name="table">Name of the table to insert data into</param>
        /// <param name="tableData">A dictionery of type String,String  key,value, to insert</param>
        /// <returns>true if successful false if not</returns>
        public static bool Insert(String table, Dictionary<String, String> tableData)
        {
            // build the string to insert into the database

            String keys = "";
            String values = "";

            // grab a list of keys
            List<String> list = new List<String>(tableData.Keys);

            // check we have list values
            if(list.Count()>0)
            {
                //build strings for sql
                for (int i = 0; i < (list.Count()-1); i++)
                {
                    keys += String.Format(" {0}, ", list[i].ToString());
                    values += String.Format(" '{0}' , ", tableData[list[i].ToString()].ToString());
                }
                // don't add the last comma
                keys += String.Format(" {0} ", list[list.Count()-1].ToString());
                values += String.Format(" '{0}'  ", tableData[list[(list.Count()-1)]].ToString());
            }

            // construct the complete sql statement
            String sql = String.Format("INSERT INTO {0} ({1}) VALUES ({2}); ",table,keys,values);

            try
            {
                QuickQuery(sql);
            }
            catch (Exception)
            {
                
                return false;
            }
            return true;
        }


        ///
        /// <summary> 
        /// used to run queries that only require a return of true of false.
        /// </summary>
        /// <param name="table">Complete SQL statment to run</param>
        /// <returns>true if successful false if not</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static int QuickQuery(String sql)
        {         
            //Setup the sql command
            SQLiteCommand mycom = new SQLiteCommand(sql,con);

            //execute the command
            int result = mycom.ExecuteNonQuery();

            return result;
        }

        /// <summary>
        /// count the number of results
        /// </summary>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <returns>Count of rows matching query</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static int CountQuery(String table, String where = " 1=1; ")
        {
            String sql = String.Format("SELECT count(*) as counter FROM {0} WHERE {1} ;", table, where);
            SQLiteCommand mycom = new SQLiteCommand(sql, con);
            DataTable ct = new DataTable();

            try{
            
            SQLiteDataReader reader = mycom.ExecuteReader();
                ct.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            DataRow cr = ct.Rows[0];
            return int.Parse(cr["counter"].ToString());
        }

        /// <summary> 
        /// used to update a dictionery of values into the specified table
        /// </summary>
        /// <param name="table">Name of the table to update data in</param>
        /// <param name="tableData">A dictionery of type String,String  key,value, to update</param>
        /// <returns>true if successful false if not</returns>
        public static bool Update(String table, Dictionary<String, String> tableData, String where = " 1=1 " )
        {
            // build the string to insert into the database

            String keyvalues = "";
           
            // grab a list of keys
            List<String> list = new List<String>(tableData.Keys);

            // check we have list values
            if (list.Count() > 0)
            {
                //build strings for sql
                for (int i = 0; i < (list.Count() - 1); i++)
                {
                    keyvalues += String.Format(" {0} = '{1}' , ", list[i].ToString(), tableData[list[i].ToString()].ToString());
                }
                // don't add the last comma
                keyvalues += String.Format(" {0} = '{1}' ", list[(list.Count() - 1)].ToString(), tableData[list[(list.Count() - 1)].ToString()].ToString());
            }

            // construct the complete sql statement
            String sql = String.Format("UPDATE {0} SET {1} WHERE {2} ; ", table, keyvalues, where);

            try
            {
                QuickQuery(sql);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }



        /// <summary> 
        /// used to update a dictionery of values into the specified table,, handles escape chars
        /// </summary>
        /// <param name="table">Name of the table to update data in</param>
        /// <param name="tableData">A dictionery of type String,String  key,value, to update</param>
        /// <returns>true if successful false if not</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2241:Provide correct arguments to formatting methods")]
        public static bool Update2(String table, Dictionary<String, String> tableData, String where = " 1=1 ")
        {
            
            //build SQL
            String kv = "";

            // grab a list of keys
            List<String> list = new List<String>(tableData.Keys);
           // check we have list values
            if (list.Count() > 0)
            {
                //build strings for sql
                for (int i = 0; i < (list.Count() - 1); i++)
                {
                    kv += String.Format(" {0} = @{0} , ", list[i].ToString() );
                }
                // don't add the last comma
                kv += String.Format(" {0} = @{0} ", list[(list.Count() - 1)].ToString(), list[(list.Count() - 1)].ToString() );
            }

            // construct the complete sql statement
            String sql = String.Format("UPDATE {0} SET {1} WHERE {2} ; ", table, kv, where);
            
            //start the commandbuilder
            SQLiteCommand mycom = new SQLiteCommand(sql, con);

            if (list.Count() > 0)
            {
                for (int i = 0; i < (list.Count() - 1); i++)
                {
                    mycom.Parameters.AddWithValue(String.Format("@{0}", list[i].ToString()), tableData[list[i].ToString()].ToString() );
                }
                // don't add the last comma
                 mycom.Parameters.AddWithValue(String.Format("@{0}", list[(list.Count() - 1)].ToString()), tableData[list[(list.Count() - 1)].ToString()].ToString());
            }
            try
            {
                mycom.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }





        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static bool Insert2(String table, Dictionary<String, String> tableData)
        {
            // build the string to insert into the database

            String keys = "";
            String values = "";

            // grab a list of keys
            List<String> list = new List<String>(tableData.Keys);

            // check we have list values
            if (list.Count() > 0)
            {
                //build strings for sql
                for (int i = 0; i < (list.Count() - 1); i++)
                {
                    keys += String.Format(" {0}, ", list[i].ToString() );
                    values += String.Format(" @{0} , ", list[i].ToString() );
                }
                // don't add the last comma
                keys += String.Format(" {0} ", list[list.Count() - 1].ToString() );
                values += String.Format(" @{0}  ", list[list.Count() - 1].ToString() );
            }

            // construct the complete sql statement
            String sql = String.Format("INSERT INTO {0} ({1}) VALUES ({2}); ", table, keys, values);
            
            //start the commandbuilder
            SQLiteCommand mycom = new SQLiteCommand(sql, con);

            if (list.Count() > 0)
            {
                for (int i = 0; i < (list.Count() - 1); i++)
                {
                    mycom.Parameters.AddWithValue(String.Format("@{0}", list[i].ToString()), tableData[list[i].ToString()].ToString());
                }
                // don't add the last comma
                mycom.Parameters.AddWithValue(String.Format("@{0}", list[(list.Count() - 1)].ToString()), tableData[list[(list.Count() - 1)].ToString()].ToString());
            }

            try
            {
                mycom.ExecuteNonQuery();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        /// <summary>
        /// get the last autoincrement value.
        /// </summary>
        /// <returns>the last auto increment value</returns>
        public static long GetLastAutoID()
        {
            return con.LastInsertRowId;
        }

    }
}


