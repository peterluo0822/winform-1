﻿using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace JCodes.Framework.Entity
{
    public class DataTypeInfo : IDXDataErrorInfo
    {
        public DataTypeInfo()
        { }

        private string guid;

        [DisplayName("GUID")]
        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        /// <summary>
        /// 类型名
        /// </summary>
        private string name;

        [DisplayName("类型名")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string chineseName;

        [DisplayName("类型名称")]
        public string ChineseName
        {
            get { return chineseName; }
            set { chineseName = value; }
        }

        private string stdType;

        [DisplayName("标准类型")]
        public string StdType
        {
            get { return stdType; }
            set { stdType = value; }
        }

        private string length;

        [DisplayName("长度")]
        public string Length
        {
            get { return length; }
            set { length = value; }
        }

        private string precision;

        [DisplayName("精度")]
        public string Precision
        {
            get { return precision; }
            set { precision = value; }
        }

        private string remark;

        [DisplayName("备注")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 用来保存行数据中字段名，错误信息
        /// </summary>
        public Dictionary<string, ErrorInfo> lstInfo
        {
            get;
            set;
        }

        #region IDXDataErrorInfo Members
        //<gridControl1>
        void IDXDataErrorInfo.GetPropertyError(string propertyName, ErrorInfo info)
        {
            // 添加自定义错误
            if (lstInfo != null && lstInfo.Count > 0 && lstInfo.ContainsKey(propertyName) && !string.IsNullOrEmpty(lstInfo[propertyName].ErrorText))
            {
                info.ErrorText = lstInfo[propertyName].ErrorText;
                info.ErrorType = lstInfo[propertyName].ErrorType;
            }
        }
        void IDXDataErrorInfo.GetError(ErrorInfo info) { }
        //</gridControl1>

        #endregion
    }
}
