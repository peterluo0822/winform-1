using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using JCodes.Framework.Common;
using JCodes.Framework.Entity;

namespace JCodes.Framework.BLL
{
	public class ReportMonthlyHeader : BaseBLL<ReportMonthlyHeaderInfo>
    {
        public ReportMonthlyHeader() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// ��ȡ�±���
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetMonthlyReport(string condition)
        {
            string sql = string.Format(@"Select * from WM_ReportMonthlyHeader {0} order by ReportYear, ReportMonth", condition);
            return SqlTable(sql);
        }

        /// <summary>
        /// �����½ᱨ�����Ը��ǣ����������£��������
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertOrUpdate(ReportMonthlyHeaderInfo info)
        {
            int headerID = 0;
            string condition = string.Format(" ReportYear={0} AND ReportMonth={1} AND ReportType={2} ", 
                info.ReportYear, info.ReportMonth, info.ReportType);
            ReportMonthlyHeaderInfo existInfo = base.FindSingle(condition);
            if (existInfo == null)
            {
                headerID = baseDal.Insert2(info);
            }
            else
            {
                existInfo.Note = info.Note;
                existInfo.CreateDate = DateTime.Now;
                existInfo.Creator = info.Creator;
                existInfo.ReportTitle = info.ReportTitle;
                baseDal.Update(existInfo, existInfo.ID.ToString());
                headerID = existInfo.ID;
            }
            return headerID;
        }
    }
}