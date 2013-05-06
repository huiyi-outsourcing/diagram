using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace diagram.Common
{
   public static class BasicInfo
    {
        private static string _CurrentSqlInfo;

        public static string CurrentSqlInfo
        {
            get { return BasicInfo._CurrentSqlInfo; }
            set { BasicInfo._CurrentSqlInfo = value; }
        }
        private static string _Wellname;

        public static string Wellname
        {
            get { return BasicInfo._Wellname; }
            set { BasicInfo._Wellname = value; }
        }

     
       public class SqlDataAccess
       {
           private string strConn;

           public SqlDataAccess(string connString)
           {
               strConn = connString;
           }

           #region 数据查询操作

           /// <summary>
           /// 执行一个sql语句，并将查询出的结果通过DataSet返回
           /// </summary>
           /// <param name="strSql">SQL语句字符串</param>
           /// <returns>查询出的结果通过DataSet返回</returns>
           public DataSet SelectDataSet(string strSql)
           {
               DataSet ds = new DataSet();

               SqlConnection myCn = new SqlConnection(strConn);
               try
               {
                   myCn.Open();
                   SqlDataAdapter adapter = new SqlDataAdapter(strSql, myCn);
                   adapter.Fill(ds);
                   adapter.Dispose();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCn.Close();
               }

               return ds;
           }

           /// <summary>
           /// 执行一个sql语句，并将查询出的结果通过DataTable返回
           /// </summary>
           /// <param name="strSql">SQL语句字符串</param>
           /// <returns>查询出的结果通过DataTable返回</returns>
           public DataTable SelectDataTable(string strSql)
           {
               DataSet ds = new DataSet();

               SqlConnection myCn = new SqlConnection(strConn);
               try
               {
                   myCn.Open();
                   SqlDataAdapter adapter = new SqlDataAdapter(strSql, myCn);
                   adapter.Fill(ds);
                   adapter.Dispose();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCn.Close();
               }

               if (ds.Tables.Count > 0)
               {
                   return ds.Tables[0];
               }
               else
               {
                   return null;
               }

           }

           /// <summary>
           /// 执行一个sql语句，并将查询出的结果通过DataSet返回
           ///	</summary>
           ///	<param name="strSql">SQL语句字符串</param>
           ///	<param name="strTableName">查询结果返回到DataSet时填充的表名</param>
           ///	<returns>查询出的结果通过DataSet返回</returns>
           public DataSet SelectDataSet(string strSql, string strTableName)
           {
               DataSet ds = new DataSet();

               SqlConnection myCn = new SqlConnection(strConn);
               try
               {
                   myCn.Open();
                   SqlDataAdapter adapter = new SqlDataAdapter(strSql, myCn);
                   adapter.Fill(ds, strTableName);
                   adapter.Dispose();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCn.Close();
               }

               return ds;
           }

           /// <summary>
           /// 执行一个带参数的sql语句或存储过程，并将查询出的结果通过DataSet返回
           /// </summary>
           /// <param name="dParams">带参数的SQL</param>
           ///	<returns>查询出的结果通过DataSet返回</returns>
           public DataSet SelectDataSet(SqlDataParamters dParams)
           {
               DataSet ds = new DataSet();

               SqlConnection myCn = new SqlConnection(strConn);
               SqlCommand myCmd = new SqlCommand();
               myCmd.Connection = myCn;
               try
               {
                   myCn.Open();
                   myCmd.CommandType = dParams.Commandtype;
                   myCmd.CommandText = dParams.CommandText;
                   myCmd.Parameters.Clear();

                   for (int i = 0; i < dParams.Count; i++)
                   {
                       myCmd.Parameters.Add(dParams[i]);
                   }

                   SqlDataAdapter da = new SqlDataAdapter(myCmd);
                   da.Fill(ds);
                   da.Dispose();
                   myCmd.Dispose();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCmd.Dispose();
                   myCn.Close();
               }

               return ds;
           }

           /// <summary>
           /// 执行一个带参数的sql语句或存储过程，并将查询出的结果通过DataTable返回
           /// </summary>
           /// <param name="dParams">带参数的SQL</param>
           ///	<returns>查询出的结果通过DataTable返回</returns>
           public DataTable SelectDataTable(SqlDataParamters dParams)
           {
               DataSet ds = new DataSet();

               SqlConnection myCn = new SqlConnection(strConn);
               SqlCommand myCmd = new SqlCommand();
               myCmd.Connection = myCn;
               try
               {
                   myCn.Open();
                   myCmd.CommandType = dParams.Commandtype;
                   myCmd.CommandText = dParams.CommandText;
                   myCmd.Parameters.Clear();

                   for (int i = 0; i < dParams.Count; i++)
                   {
                       myCmd.Parameters.Add(dParams[i]);
                   }

                   SqlDataAdapter da = new SqlDataAdapter(myCmd);
                   da.Fill(ds);
                   da.Dispose();
                   myCmd.Dispose();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCmd.Dispose();
                   myCn.Close();
               }

               if (ds.Tables.Count > 0)
               {
                   return ds.Tables[0];
               }
               else
               {
                   return null;
               }
           }

           /// <summary>
           /// 执行一个带参数的sql语句或存储过程，并将查询出的结果通过DataSet返回
           /// </summary>
           /// <param name="dParams">带参数的SQL</param>
           /// <param name="strTableName">查询结果返回到DataSet时填充的表名</param>
           /// <returns>查询出的结果通过DataSet返回</returns>
           public DataSet SelectDataSet(SqlDataParamters dParams, string strTableName)
           {
               DataSet ds = new DataSet();

               SqlConnection myCn = new SqlConnection(strConn);
               SqlCommand myCmd = new SqlCommand();
               myCmd.Connection = myCn;
               try
               {
                   myCn.Open();
                   myCmd.CommandType = dParams.Commandtype;
                   myCmd.CommandText = dParams.CommandText;
                   myCmd.Parameters.Clear();

                   for (int i = 0; i < dParams.Count; i++)
                   {
                       myCmd.Parameters.Add(dParams[i]);
                   }

                   SqlDataAdapter da = new SqlDataAdapter(myCmd);
                   da.Fill(ds, strTableName);
                   da.Dispose();
                   myCmd.Dispose();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCmd.Dispose();
                   myCn.Close();
               }

               return ds;
           }

           #endregion

           #region 执行SQL语句
           /// <summary>
           /// 执行一个sql语句，并返回受此SQL语句影响的行数
           ///	</summary>
           ///	<param name="strSql">SQL语句字符串</param>
           public int ExeSQL(string strSql)
           {
               int iRet;
               SqlConnection myCn = new SqlConnection(strConn);
               SqlCommand myCmd = new SqlCommand(strSql, myCn);
               try
               {
                   myCn.Open();
                   iRet = myCmd.ExecuteNonQuery();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   iRet = -1;
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCmd.Dispose();
                   myCn.Close();
               }

               return iRet;
           }

           /// <summary>
           /// 执行一个带参数的sql语句，并返回受此SQL语句影响的行数
           /// </summary>
           /// <param name="dParams">SQL参数</param>
           /// <returns>受此SQL语句影响的行数</returns>
           public int ExeSQL(SqlDataParamters dParams)
           {
               int iRet = 0;
               SqlConnection myCn = new SqlConnection(strConn);
               SqlCommand myCmd = new SqlCommand();
               myCmd.Connection = myCn;
               try
               {
                   myCn.Open();
                   myCmd.CommandType = dParams.Commandtype;
                   myCmd.CommandText = dParams.CommandText;
                   myCmd.Parameters.Clear();

                   for (int i = 0; i < dParams.Count; i++)
                   {
                       myCmd.Parameters.Add(dParams[i]);
                   }
                   iRet = myCmd.ExecuteNonQuery();
                   myCmd.Parameters.Clear();
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   iRet = -1;
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCmd.Dispose();
                   myCn.Close();
               }

               return iRet;
           }

           /// <summary>
           /// 以事务方式执行多个sql语句
           ///	</summary>
           ///	<param name="strSqls">SQL语句字符串数组</param>
           ///	<returns>True:事务成功，False：事务失败</returns>
           public bool ExeSQL(SqlDataParamters[] dParams)
           {
               bool bRet;

               SqlConnection myConn = new SqlConnection(strConn);
               myConn.Open();
               SqlTransaction trans = myConn.BeginTransaction();
               SqlCommand myCmd = new SqlCommand();
               myCmd.Transaction = trans;
               myCmd.Connection = myConn;

               try
               {
                   foreach (SqlDataParamters sqlParams in dParams)
                   {
                       myCmd.CommandType = sqlParams.Commandtype;
                       myCmd.CommandText = sqlParams.CommandText;
                       myCmd.Parameters.Clear();

                       for (int i = 0; i < sqlParams.Count; i++)
                       {
                           myCmd.Parameters.Add(sqlParams[i]);
                       }

                       myCmd.ExecuteNonQuery();
                   }

                   trans.Commit();
                   bRet = true;
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   trans.Rollback();
                   bRet = false;
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCmd.Dispose();
                   myConn.Close();
               }

               return bRet;
           }

           /// <summary>
           /// 以事务方式执行多个带参数的sql语句
           ///	</summary>
           ///	<param name="strSqls">SQL语句字符串数组</param>
           ///	<returns>True:事务成功，False：事务失败</returns>
           public bool ExeSQL(string[] strSqls)
           {
               bool bRet;

               SqlConnection myConn = new SqlConnection(strConn);
               myConn.Open();
               SqlTransaction trans = myConn.BeginTransaction();
               SqlCommand myCmd = new SqlCommand();
               myCmd.Transaction = trans;
               myCmd.Connection = myConn;

               try
               {
                   foreach (string sql in strSqls)
                   {
                       myCmd.CommandText = sql;
                       myCmd.ExecuteNonQuery();
                   }

                   trans.Commit();
                   bRet = true;
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   trans.Rollback();
                   bRet = false;
                   throw new Exception(e.Message);
               }
               finally
               {
                   myCmd.Dispose();
                   myConn.Close();
               }

               return bRet;
           }


           /// <summary>
           /// 执行一个sql语句，并返回SqlDataReader
           ///	</summary>
           ///	<param name="strSql">SQL语句字符串</param>
           public SqlDataReader ExeSQLReader(string strSql)
           {
               SqlConnection myConn = new SqlConnection(strConn);
               SqlCommand myCmd = new SqlCommand(strSql, myConn);

               try
               {
                   myConn.Open();
                   myCmd.CommandType = CommandType.Text;
                   myCmd.CommandText = strSql;
                   SqlDataReader myReader = myCmd.ExecuteReader();

                   return myReader;
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   myCmd.Dispose();
                   myConn.Close();
                   throw new Exception(e.Message);
               }
           }

           #endregion

           #region 其他常用的方法
           /// <summary>
           /// 返回记录中最大的编号(主要用于主细表操作，当增加主表后立即调用此方法返回主表ID供增加细表使用)
           ///	</summary>	
           ///	<param name="strFieldName">要查询的表名</param>
           ///	<param name="strTableName">要查询的字段名</param>
           public string GetMaxID(string strTableName, string strFieldName)
           {
               string strResult, strSql;
               DataSet ds = new DataSet();

               try
               {
                   strSql = "select top 1 " + strFieldName + " from " + strTableName + " order by " + strFieldName + " desc";
                   ds = SelectDataSet(strSql);
                   if (ds.Tables[0].Rows.Count == 0) //无数据
                   {
                       strResult = "";
                   }
                   else
                   {
                       strResult = ds.Tables[0].Rows[0][strFieldName].ToString();
                   }
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   ds.Dispose();
               }

               return strResult;
           }

           /// <summary>
           /// 返回符合条件的数据记录总条数
           ///	</summary>
           ///	<param name="strSql">查询条件SQL语句</param>
           public int GetRowCount(string strSql)
           {
               DataSet ds = new DataSet();
               try
               {
                   ds = SelectDataSet(strSql);
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   throw new Exception(e.Message);
               }
               finally
               {
                   ds.Dispose();
               }

               return ds.Tables[0].Rows.Count;
           }
       }
           #endregion

       #region SqlDataParamters类
       public class SqlDataParamters : System.Collections.CollectionBase
       {
           private string mCommandText;
           private CommandType mCommandType;
           private SqlParameter ParamterItem = null;

           public SqlDataParamters()
           {
               mCommandType = CommandType.Text;
               mCommandText = "";
           }

           public SqlDataParamters(string strCommandText)
           {
               mCommandType = CommandType.Text;
               mCommandText = strCommandText;
           }

           public SqlDataParamters(string strCommandText, CommandType cmdType)
           {
               mCommandType = cmdType;
               mCommandText = strCommandText;
           }

           public string CommandText
           {
               get
               {
                   return mCommandText;
               }
               set
               {
                   mCommandText = value;
               }
           }

           public CommandType Commandtype
           {
               get
               {
                   return mCommandType;
               }
               set
               {
                   mCommandType = value;
               }
           }

           public void Add(SqlParameter paramter)
           {
               List.Add(paramter);
           }

           public void Add(SqlParameter paramter, object paramterValue)
           {
               if (paramterValue == null || paramterValue.ToString() == "")
                   paramter.Value = DBNull.Value;
               else
                   paramter.Value = paramterValue;

               List.Add(paramter);
           }

           public void Add(string paramterName, object paramterValue)
           {
               SqlParameter paramter = null;

               if (paramterValue == null || paramterValue.ToString() == "")
                   paramter = new SqlParameter(paramterName, DBNull.Value);
               else
                   paramter = new SqlParameter(paramterName, paramterValue);

               List.Add(paramter);
           }

           public void Add(string paramterName, SqlDbType dbType, int size, object paramterValue)
           {
               SqlParameter paramter = null;

               paramter = new SqlParameter(paramterName, dbType, size);
               paramter.Value = paramterValue;

               List.Add(paramter);
           }


           public void Remove(int index)
           {
               if (index > -1 && index < Count)
                   List.RemoveAt(index);
           }

           public void Remove(string paramterName)
           {
               for (int i = 0; i < Count; i++)
               {
                   if (this[i].ParameterName == paramterName)
                   {
                       List.RemoveAt(i);
                       break;
                   }
               }
           }

           public SqlParameter this[int index]
           {
               get
               {
                   return (SqlParameter)List[index];
               }
               set
               {
                   List[index] = value;
               }
           }

           public SqlParameter this[string paramterName]
           {
               get
               {
                   if (ParamterItem == null || ParamterItem.ParameterName != paramterName)
                   {
                       ParamterItem = null;

                       for (int i = 0; i < Count; i++)
                       {
                           if (this[i].ParameterName == paramterName)
                           {
                               ParamterItem = (SqlParameter)List[i];
                               break;
                           }
                       }
                   }

                   return ParamterItem;
               }
               set
               {
                   for (int i = 0; i < Count; i++)
                   {
                       if (this[i].ParameterName == paramterName)
                       {
                           List[i] = value;
                           break;
                       }
                   }
               }
           }
       }
       #endregion

     

       
      
    }
   public class SqlDataAccess
   {
       private string strConn;

       public SqlDataAccess(string connString)
       {
           strConn = connString;
       }

       #region 数据查询操作

       /// <summary>
       /// 执行一个sql语句，并将查询出的结果通过DataSet返回
       /// </summary>
       /// <param name="strSql">SQL语句字符串</param>
       /// <returns>查询出的结果通过DataSet返回</returns>
       public DataSet SelectDataSet(string strSql)
       {
           DataSet ds = new DataSet();

           SqlConnection myCn = new SqlConnection(strConn);
           try
           {
               myCn.Open();
               SqlDataAdapter adapter = new SqlDataAdapter(strSql, myCn);
               adapter.Fill(ds);
               adapter.Dispose();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               myCn.Close();
           }

           return ds;
       }

       /// <summary>
       /// 执行一个sql语句，并将查询出的结果通过DataTable返回
       /// </summary>
       /// <param name="strSql">SQL语句字符串</param>
       /// <returns>查询出的结果通过DataTable返回</returns>
       public DataTable SelectDataTable(string strSql)
       {
           DataSet ds = new DataSet();

           SqlConnection myCn = new SqlConnection(strConn);
           try
           {
               myCn.Open();
               SqlDataAdapter adapter = new SqlDataAdapter(strSql, myCn);
               adapter.Fill(ds);
               adapter.Dispose();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               myCn.Close();
           }

           if (ds.Tables.Count > 0)
           {
               return ds.Tables[0];
           }
           else
           {
               return null;
           }

       }

       /// <summary>
       /// 执行一个sql语句，并将查询出的结果通过DataSet返回
       ///	</summary>
       ///	<param name="strSql">SQL语句字符串</param>
       ///	<param name="strTableName">查询结果返回到DataSet时填充的表名</param>
       ///	<returns>查询出的结果通过DataSet返回</returns>
       public DataSet SelectDataSet(string strSql, string strTableName)
       {
           DataSet ds = new DataSet();

           SqlConnection myCn = new SqlConnection(strConn);
           try
           {
               myCn.Open();
               SqlDataAdapter adapter = new SqlDataAdapter(strSql, myCn);
               adapter.Fill(ds, strTableName);
               adapter.Dispose();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               myCn.Close();
           }

           return ds;
       }

       /// <summary>
       /// 执行一个带参数的sql语句或存储过程，并将查询出的结果通过DataSet返回
       /// </summary>
       /// <param name="dParams">带参数的SQL</param>
       ///	<returns>查询出的结果通过DataSet返回</returns>
       public DataSet SelectDataSet(SqlDataParamters dParams)
       {
           DataSet ds = new DataSet();

           SqlConnection myCn = new SqlConnection(strConn);
           SqlCommand myCmd = new SqlCommand();
           myCmd.Connection = myCn;
           try
           {
               myCn.Open();
               myCmd.CommandType = dParams.Commandtype;
               myCmd.CommandText = dParams.CommandText;
               myCmd.Parameters.Clear();

               for (int i = 0; i < dParams.Count; i++)
               {
                   myCmd.Parameters.Add(dParams[i]);
               }

               SqlDataAdapter da = new SqlDataAdapter(myCmd);
               da.Fill(ds);
               da.Dispose();
               myCmd.Dispose();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               myCmd.Dispose();
               myCn.Close();
           }

           return ds;
       }

       /// <summary>
       /// 执行一个带参数的sql语句或存储过程，并将查询出的结果通过DataTable返回
       /// </summary>
       /// <param name="dParams">带参数的SQL</param>
       ///	<returns>查询出的结果通过DataTable返回</returns>
       public DataTable SelectDataTable(SqlDataParamters dParams)
       {
           DataSet ds = new DataSet();

           SqlConnection myCn = new SqlConnection(strConn);
           SqlCommand myCmd = new SqlCommand();
           myCmd.Connection = myCn;
           try
           {
               myCn.Open();
               myCmd.CommandType = dParams.Commandtype;
               myCmd.CommandText = dParams.CommandText;
               myCmd.Parameters.Clear();

               for (int i = 0; i < dParams.Count; i++)
               {
                   myCmd.Parameters.Add(dParams[i]);
               }

               SqlDataAdapter da = new SqlDataAdapter(myCmd);
               da.Fill(ds);
               da.Dispose();
               myCmd.Dispose();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               myCmd.Dispose();
               myCn.Close();
           }

           if (ds.Tables.Count > 0)
           {
               return ds.Tables[0];
           }
           else
           {
               return null;
           }
       }

       /// <summary>
       /// 执行一个带参数的sql语句或存储过程，并将查询出的结果通过DataSet返回
       /// </summary>
       /// <param name="dParams">带参数的SQL</param>
       /// <param name="strTableName">查询结果返回到DataSet时填充的表名</param>
       /// <returns>查询出的结果通过DataSet返回</returns>
       public DataSet SelectDataSet(SqlDataParamters dParams, string strTableName)
       {
           DataSet ds = new DataSet();

           SqlConnection myCn = new SqlConnection(strConn);
           SqlCommand myCmd = new SqlCommand();
           myCmd.Connection = myCn;
           try
           {
               myCn.Open();
               myCmd.CommandType = dParams.Commandtype;
               myCmd.CommandText = dParams.CommandText;
               myCmd.Parameters.Clear();

               for (int i = 0; i < dParams.Count; i++)
               {
                   myCmd.Parameters.Add(dParams[i]);
               }

               SqlDataAdapter da = new SqlDataAdapter(myCmd);
               da.Fill(ds, strTableName);
               da.Dispose();
               myCmd.Dispose();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               myCmd.Dispose();
               myCn.Close();
           }

           return ds;
       }

       #endregion

       #region 执行SQL语句
       /// <summary>
       /// 执行一个sql语句，并返回受此SQL语句影响的行数
       ///	</summary>
       ///	<param name="strSql">SQL语句字符串</param>
       public int ExeSQL(string strSql)
       {
           int iRet;
           SqlConnection myCn = new SqlConnection(strConn);
           SqlCommand myCmd = new SqlCommand(strSql, myCn);
           try
           {
               myCn.Open();
               iRet = myCmd.ExecuteNonQuery();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               iRet = -1;
               throw new Exception(e.Message);
           }
           finally
           {
               myCmd.Dispose();
               myCn.Close();
           }

           return iRet;
       }

       /// <summary>
       /// 执行一个带参数的sql语句，并返回受此SQL语句影响的行数
       /// </summary>
       /// <param name="dParams">SQL参数</param>
       /// <returns>受此SQL语句影响的行数</returns>
       public int ExeSQL(SqlDataParamters dParams)
       {
           int iRet = 0;
           SqlConnection myCn = new SqlConnection(strConn);
           SqlCommand myCmd = new SqlCommand();
           myCmd.Connection = myCn;
           try
           {
               myCn.Open();
               myCmd.CommandType = dParams.Commandtype;
               myCmd.CommandText = dParams.CommandText;
               myCmd.Parameters.Clear();

               for (int i = 0; i < dParams.Count; i++)
               {
                   myCmd.Parameters.Add(dParams[i]);
               }
               iRet = myCmd.ExecuteNonQuery();
               myCmd.Parameters.Clear();
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               iRet = -1;
               throw new Exception(e.Message);
           }
           finally
           {
               myCmd.Dispose();
               myCn.Close();
           }

           return iRet;
       }

       /// <summary>
       /// 以事务方式执行多个sql语句
       ///	</summary>
       ///	<param name="strSqls">SQL语句字符串数组</param>
       ///	<returns>True:事务成功，False：事务失败</returns>
       public bool ExeSQL(SqlDataParamters[] dParams)
       {
           bool bRet;

           SqlConnection myConn = new SqlConnection(strConn);
           myConn.Open();
           SqlTransaction trans = myConn.BeginTransaction();
           SqlCommand myCmd = new SqlCommand();
           myCmd.Transaction = trans;
           myCmd.Connection = myConn;

           try
           {
               foreach (SqlDataParamters sqlParams in dParams)
               {
                   myCmd.CommandType = sqlParams.Commandtype;
                   myCmd.CommandText = sqlParams.CommandText;
                   myCmd.Parameters.Clear();

                   for (int i = 0; i < sqlParams.Count; i++)
                   {
                       myCmd.Parameters.Add(sqlParams[i]);
                   }

                   myCmd.ExecuteNonQuery();
               }

               trans.Commit();
               bRet = true;
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               trans.Rollback();
               bRet = false;
               throw new Exception(e.Message);
           }
           finally
           {
               myCmd.Dispose();
               myConn.Close();
           }

           return bRet;
       }

       /// <summary>
       /// 以事务方式执行多个带参数的sql语句
       ///	</summary>
       ///	<param name="strSqls">SQL语句字符串数组</param>
       ///	<returns>True:事务成功，False：事务失败</returns>
       public bool ExeSQL(string[] strSqls)
       {
           bool bRet;

           SqlConnection myConn = new SqlConnection(strConn);
           myConn.Open();
           SqlTransaction trans = myConn.BeginTransaction();
           SqlCommand myCmd = new SqlCommand();
           myCmd.Transaction = trans;
           myCmd.Connection = myConn;

           try
           {
               foreach (string sql in strSqls)
               {
                   myCmd.CommandText = sql;
                   myCmd.ExecuteNonQuery();
               }

               trans.Commit();
               bRet = true;
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               trans.Rollback();
               bRet = false;
               throw new Exception(e.Message);
           }
           finally
           {
               myCmd.Dispose();
               myConn.Close();
           }

           return bRet;
       }


       /// <summary>
       /// 执行一个sql语句，并返回SqlDataReader
       ///	</summary>
       ///	<param name="strSql">SQL语句字符串</param>
       public SqlDataReader ExeSQLReader(string strSql)
       {
           SqlConnection myConn = new SqlConnection(strConn);
           SqlCommand myCmd = new SqlCommand(strSql, myConn);

           try
           {
               myConn.Open();
               myCmd.CommandType = CommandType.Text;
               myCmd.CommandText = strSql;
               SqlDataReader myReader = myCmd.ExecuteReader();

               return myReader;
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               myCmd.Dispose();
               myConn.Close();
               throw new Exception(e.Message);
           }
       }

       #endregion

       #region 其他常用的方法
       /// <summary>
       /// 返回记录中最大的编号(主要用于主细表操作，当增加主表后立即调用此方法返回主表ID供增加细表使用)
       ///	</summary>	
       ///	<param name="strFieldName">要查询的表名</param>
       ///	<param name="strTableName">要查询的字段名</param>
       public string GetMaxID(string strTableName, string strFieldName)
       {
           string strResult, strSql;
           DataSet ds = new DataSet();

           try
           {
               strSql = "select top 1 " + strFieldName + " from " + strTableName + " order by " + strFieldName + " desc";
               ds = SelectDataSet(strSql);
               if (ds.Tables[0].Rows.Count == 0) //无数据
               {
                   strResult = "";
               }
               else
               {
                   strResult = ds.Tables[0].Rows[0][strFieldName].ToString();
               }
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               ds.Dispose();
           }

           return strResult;
       }

       /// <summary>
       /// 返回符合条件的数据记录总条数
       ///	</summary>
       ///	<param name="strSql">查询条件SQL语句</param>
       public int GetRowCount(string strSql)
       {
           DataSet ds = new DataSet();
           try
           {
               ds = SelectDataSet(strSql);
           }
           catch (System.Data.SqlClient.SqlException e)
           {
               throw new Exception(e.Message);
           }
           finally
           {
               ds.Dispose();
           }

           return ds.Tables[0].Rows.Count;
       }
   }
       #endregion

   #region SqlDataParamters类
   public class SqlDataParamters : System.Collections.CollectionBase
   {
       private string mCommandText;
       private CommandType mCommandType;
       private SqlParameter ParamterItem = null;

       public SqlDataParamters()
       {
           mCommandType = CommandType.Text;
           mCommandText = "";
       }

       public SqlDataParamters(string strCommandText)
       {
           mCommandType = CommandType.Text;
           mCommandText = strCommandText;
       }

       public SqlDataParamters(string strCommandText, CommandType cmdType)
       {
           mCommandType = cmdType;
           mCommandText = strCommandText;
       }

       public string CommandText
       {
           get
           {
               return mCommandText;
           }
           set
           {
               mCommandText = value;
           }
       }

       public CommandType Commandtype
       {
           get
           {
               return mCommandType;
           }
           set
           {
               mCommandType = value;
           }
       }

       public void Add(SqlParameter paramter)
       {
           List.Add(paramter);
       }

       public void Add(SqlParameter paramter, object paramterValue)
       {
           if (paramterValue == null || paramterValue.ToString() == "")
               paramter.Value = DBNull.Value;
           else
               paramter.Value = paramterValue;

           List.Add(paramter);
       }

       public void Add(string paramterName, object paramterValue)
       {
           SqlParameter paramter = null;

           if (paramterValue == null || paramterValue.ToString() == "")
               paramter = new SqlParameter(paramterName, DBNull.Value);
           else
               paramter = new SqlParameter(paramterName, paramterValue);

           List.Add(paramter);
       }

       public void Add(string paramterName, SqlDbType dbType, int size, object paramterValue)
       {
           SqlParameter paramter = null;

           paramter = new SqlParameter(paramterName, dbType, size);
           paramter.Value = paramterValue;

           List.Add(paramter);
       }


       public void Remove(int index)
       {
           if (index > -1 && index < Count)
               List.RemoveAt(index);
       }

       public void Remove(string paramterName)
       {
           for (int i = 0; i < Count; i++)
           {
               if (this[i].ParameterName == paramterName)
               {
                   List.RemoveAt(i);
                   break;
               }
           }
       }

       public SqlParameter this[int index]
       {
           get
           {
               return (SqlParameter)List[index];
           }
           set
           {
               List[index] = value;
           }
       }

       public SqlParameter this[string paramterName]
       {
           get
           {
               if (ParamterItem == null || ParamterItem.ParameterName != paramterName)
               {
                   ParamterItem = null;

                   for (int i = 0; i < Count; i++)
                   {
                       if (this[i].ParameterName == paramterName)
                       {
                           ParamterItem = (SqlParameter)List[i];
                           break;
                       }
                   }
               }

               return ParamterItem;
           }
           set
           {
               for (int i = 0; i < Count; i++)
               {
                   if (this[i].ParameterName == paramterName)
                   {
                       List[i] = value;
                       break;
                   }
               }
           }
       }
   }
   #endregion
}
