using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using JCodes.Framework.Common;
using JCodes.Framework.Entity;
using JCodes.Framework.IDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using JCodes.Framework.Common.Framework.BaseDAL;
using JCodes.Framework.Common.Databases;

namespace JCodes.Framework.MySqlDAL
{
	/// <summary>
	/// DictType 的摘要说明。
	/// </summary>
    public class DictType : BaseDALMySql<DictTypeInfo>, IDictType
	{
		#region 对象实例及构造函数

		public static DictType Instance
		{
			get
			{
				return new DictType();
			}
		}
        public DictType()
            : base(MySqlPortal.gc._basicTablePre + "DictType", "ID")
		{
            sortField = "Seq";
            IsDescending = false;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override DictTypeInfo DataReaderToEntity(IDataReader dataReader)
        {
            DictTypeInfo dictTypeInfo = new DictTypeInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            dictTypeInfo.ID = reader.GetInt32("ID");
            dictTypeInfo.Name = reader.GetString("Name");
            dictTypeInfo.Remark = reader.GetString("Remark");
            dictTypeInfo.Seq = reader.GetString("Seq");
            dictTypeInfo.Editor = reader.GetString("Editor");
            dictTypeInfo.LastUpdated = reader.GetDateTime("LastUpdated");
            dictTypeInfo.PID = reader.GetInt32("PID");

            return dictTypeInfo;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(DictTypeInfo obj)
        {
            DictTypeInfo info = obj as DictTypeInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("Name", info.Name);
            hash.Add("Remark", info.Remark);
            hash.Add("Seq", info.Seq);
            hash.Add("Editor", info.Editor);
            hash.Add("LastUpdated", info.LastUpdated);
            hash.Add("PID", info.PID);

            return hash;
        }

        /// <summary>
        /// 获取所有字典类型的列表集合(Key为名称，Value为ID值）
        /// </summary>
        /// <param name="dictTypeId">字典类型ID</param>
        /// <returns></returns>
        public Dictionary<Int32, string> GetAllType(Int32 PID)
        {
            string sql = string.Format("select ID, Name from {0}DictType where PID ={3} order by {1} {2}",
                MySqlPortal.gc._basicTablePre, sortField, IsDescending ? "DESC" : "ASC", PID);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            Dictionary<Int32, string> list = new Dictionary<Int32, string>();
            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    string name = dr["Name"].ToString();
                    Int32 value = Convert.ToInt32(dr["ID"]);
                    if (!list.ContainsKey(value))
                    {
                        list.Add(value, name);
                    }
                }
            }
            return list;
        }


        public List<DictTypeNodeInfo> GetTree()
        {
            List<DictTypeNodeInfo> typeNodeList = new List<DictTypeNodeInfo>();
            string sql = string.Format("Select * From {0}DictType Order By PID, Seq ", MySqlPortal.gc._basicTablePre);
            Database db = CreateDatabase();
            DbCommand cmdWrapper = db.GetSqlStringCommand(sql);

            DataSet ds = db.ExecuteDataSet(cmdWrapper);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow[] dataRows = dt.Select(string.Format(" PID = {0} ", -1));
                for (int i = 0; i < dataRows.Length; i++)
                {
                    Int32 id = Convert.ToInt32(dataRows[i]["ID"]);
                    DictTypeNodeInfo DictTypeNodeInfo = GetNode(id, dt);
                    typeNodeList.Add(DictTypeNodeInfo);
                }
            }

            return typeNodeList;
        }

        private DictTypeNodeInfo GetNode(Int32 id, DataTable dt)
        {
            DictTypeInfo DictTypeInfo = this.FindByID(id);
            DictTypeNodeInfo DictTypeNodeInfo = new DictTypeNodeInfo(DictTypeInfo);

            DataRow[] dChildRows = dt.Select(string.Format(" PID={0} ", id));

            for (int i = 0; i < dChildRows.Length; i++)
            {
                Int32 childId = Convert.ToInt32(dChildRows[i]["ID"]);
                DictTypeNodeInfo childNodeInfo = GetNode(childId, dt);
                DictTypeNodeInfo.Children.Add(childNodeInfo);
            }
            return DictTypeNodeInfo;
        }
    }
}