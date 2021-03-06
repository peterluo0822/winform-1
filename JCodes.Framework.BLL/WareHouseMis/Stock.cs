using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using JCodes.Framework.Common;
using JCodes.Framework.Entity;
using JCodes.Framework.IDAL;
using JCodes.Framework.Common.Framework;

namespace JCodes.Framework.BLL
{
	public class Stock : BaseBLL<StockInfo>
    {
        public Stock() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
                        
        /// <summary>
        /// 获取当前库存报表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetCurrentStockReport(string condition)
        {
            IStock dal = baseDal as IStock;
            return dal.GetCurrentStockReport(condition);
        }

        /// <summary>
        /// 获取当期库存报表的记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int GetCurrentStockReportCount(string condition)
        {
            IStock dal = baseDal as IStock;
            return dal.GetCurrentStockReportCount(condition);
        }

        /// <summary>
        /// 检查该库房是否可以进行期初建账
        /// </summary>
        /// <param name="wareHouse"></param>
        /// <returns></returns>
        public bool CheckIsInitedWareHouse(string wareHouse, string itemNo)
        {
            bool result = false;
            string condition = string.Format("WareHouse='{0}' And ItemNo='{1}' ", wareHouse, itemNo);
            result = baseDal.IsExistRecord(condition);
            return result;
        }
                        
        /// <summary>
        /// 初始化库房信息
        /// </summary>
        /// <param name="detailInfo">备件详细信息</param>
        /// <param name="quantity">期初数量</param>
        /// <param name="wareHouse">库房名称</param>
        /// <returns></returns>
        public bool InitStockQuantity(ItemDetailInfo detailInfo, int quantity, string wareHouse)
        {
             IStock dal = baseDal as IStock;
             return dal.InitStockQuantity(detailInfo, quantity, wareHouse);
        }

        /// <summary>
        /// 增加库存
        /// </summary>
        /// <param name="ItemNo">备件编号</param>
        /// <param name="itemName">备件名称</param>
        /// <param name="quantity">库存属类</param>
        /// <returns></returns>
        public bool AddStockQuantiy(string ItemNo, string itemName, int quantity, string wareHouse)
        {
            IStock dal = baseDal as IStock;
            return dal.AddStockQuantiy(ItemNo, itemName, quantity, wareHouse);
        }

        /// <summary>
        /// 检查高库存预警状态
        /// </summary>
        /// <param name="wareHouse"></param>
        /// <returns></returns>
        public bool CheckStockHighWarning(string wareHouse)
        {
            bool result = false;
            string condition = string.Format("WareHouse='{0}' ", wareHouse);
            IStock dal = baseDal as IStock;
            List<StockInfo> stockList = dal.Find(condition);
            foreach (StockInfo info in stockList)
            {
                if (info.HighWarning > 0 && info.StockQuantity >= info.HighWarning)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 检查库存的低库存预警情况，有预警返回True，否则False
        /// </summary>
        /// <returns></returns>
        public bool CheckStockLowWarning(string wareHouse)
        {
            bool result = false;
            string condition = string.Format("WareHouse='{0}' ", wareHouse);
            IStock dal = baseDal as IStock;
            List<StockInfo> stockList = dal.Find(condition);
            foreach (StockInfo info in stockList)
            {
                if (info.LowWarning > 0 && info.StockQuantity <= info.LowWarning)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据备件编号获取库房信息
        /// </summary>
        /// <returns></returns>
        public StockInfo FindByItemNo(string itemNo, string wareHouse)
        {
            string condition = string.Format("ItemNo ='{0}' AND WareHouse='{1}'", itemNo, wareHouse);
            return baseDal.FindSingle(condition);
        }

        /// <summary>
        /// 根据备件编号获取库房信息
        /// </summary>
        /// <returns></returns>
        public StockInfo FindByItemNo(string itemNo)
        {
            string condition = string.Format("ItemNo ='{0}' ", itemNo);
            return baseDal.FindSingle(condition);
        }

        /// <summary>
        /// 根据备件编码查询库存数量
        /// </summary>
        /// <returns></returns>
        public int GetStockQuantity(string itemNo, string wareHouse)
        {
            int result = 0;
            StockInfo stockInfo = FindByItemNo(itemNo, wareHouse);
            if (stockInfo != null)
            {
                result = stockInfo.StockQuantity;
            }
            return result;
        }
                        
        /// <summary>
        /// 获取备件名称的库存数量列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetItemStockQuantityReport(string condition, string fieldName)
        {
            IStock dal = baseDal as IStock;
            return dal.GetItemStockQuantityReport(condition, fieldName);
        }
    }
}
