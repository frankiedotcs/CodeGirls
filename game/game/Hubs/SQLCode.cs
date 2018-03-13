using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3
{
    public class SQLCode
    {
        private string sql;
        /// <summary>
        /// SQL that returns the value of the entire data string in the DB
        /// </summary>
        /// <returns>the sql select statement</returns>
        public string viewGrid()
        {
            try
            {
                sql = "SELECT * FROM dbo.lifeGame";
                return sql;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Query that updates the specified value in the data column to 1,alive
        /// </summary>
        /// <param name="row">row of the value to be updated</param>
        /// <param name="column">column value to be updated</param>
        /// <returns>the sql update statement</returns>
        public string UpdateGridAlive(int row, int column)
        {
            int sub = 0;
            try
            {
                if(row == 1){
                    sub = 13 + column;
                }
			    else if (row == 2)
                {
                    sub = 25 + column;
                }
                else if (row == 3)
                {
                    sub = 37 + column;
                }
                else if (row == 4)
                {
                    sub = 49 + column;
                }
                else if (row == 5)
                {
                    sub = 61 + column;
                }
                else if (row == 6)
                {
                    sub = 73 + column;
                }
                else if (row == 7)
                {
                    sub = 85 + column;
                }
                else if (row == 8)
                {
                    sub = 97 + column;
                }
                else if (row == 9)
                {
                    sub = 109 + column;
                }
                else if (row == 10)
                {
                    sub = 121 + column;
                }
                else
                {
                    sub = 1; //default sub value?
                }
                if (sub == 14)
                {
                    sql = "UPDATE dbo.lifeGame SET data = SUBSTRING(data, 1,13) + REPLACE(SUBSTRING(data,14,1),'0','1') + SUBSTRING(data,15,144)";
                    return sql;
                }
                else if (sub == 130)
                {
                    sql = "UPDATE dbo.lifeGame SET data = SUBSTRING(data,1,129) + REPLACE(SUBSTRING(data,130,1),'0','1') + SUBSTRING(data,131,144)";
                    return sql;
                }
                else
                {
                    sql = "UPDATE dbo.Table SET data = SUBSTRING(data,1," + (sub - 1) + ") + REPLACE(SUBSTRING(data, " + sub + ", 1),'0','1') + SUBSTRING(data," + (sub + 1) + ", 144) ";
                    return sql;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Query that updates the specified value in the data column to 0,dead
        /// </summary>
        /// <param name="row">row of the value to be updated</param>
        /// <param name="column">column of the value to be updated</param>
        /// <returns>the sql update statement</returns>
        public string UpdateGridDead(int row, int column)
        {
            int sub = 0;
            try
            {
                if (row == 1)
                {
                    sub = 13 + column;
                }
                else if (row == 2)
                {
                    sub = 25 + column;
                }
                else if (row == 3)
                {
                    sub = 37 + column;
                }
                else if (row == 4)
                {
                    sub = 49 + column;
                }
                else if (row == 5)
                {
                    sub = 61 + column;
                }
                else if (row == 6)
                {
                    sub = 73 + column;
                }
                else if (row == 7)
                {
                    sub = 85 + column;
                }
                else if (row == 8)
                {
                    sub = 97 + column;
                }
                else if (row == 9)
                {
                    sub = 109 + column;
                }
                else if (row == 10)
                {
                    sub = 121 + column;
                }
                else
                {
                    sub = 1; //default sub value?
                }

                if (sub == 14)
                {
                    sql = "UPDATE dbo.lifeGame SET data = SUBSTRING(data,1,13) + REPLACE(SUBSTRING(data,14,1),'1','0') + SUBSTRING(data,15,144)";
                    return sql;
                }
                else if (sub == 130)
                {
                    sql = "UPDATE dbo.lifeGame SET data = SUBSTRING(data,1,129) + REPLACE(SUBSTRING(data,130,1),'1','0') + SUBSTRING(data,131,144)";
                    return sql;
                }
                else
                {
                    sql = "UPDATE dbo.Table SET data = SUBSTRING(data,1," + (sub - 1) +") + REPLACE(SUBSTRING(data, " + sub + ", 1),'1','0') + SUBSTRING(data,"+ (sub + 1) + ", 144) " +
                    "WHERE SUBSTRING(data, " + sub + ", 1) == '1'";
                    return sql;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Resets the values in the DB to all 0's
        /// </summary>
        /// <returns>the sql update statement</returns>
        public string ResetGrid()
        {
            try
            {
                sql = "UPDATE dbo.lifeGame SET data = '000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000'";
                return sql;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }            
        }
    }
}