using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchBar.RequestRed
{
    public class RednotificationInfo
    {

        private RednotificationHead _rednotificationHead;
        private List<RednotificationDetail> _rednotificationDetails;

        public RednotificationInfo()
        {
            _rednotificationHead = new RednotificationHead();
            _rednotificationDetails = new List<RednotificationDetail>();

        }


        public RednotificationHead notificationHead
        {
            get { return _rednotificationHead; }
            set { _rednotificationHead = value; }
        }

        public List<RednotificationDetail> notificationDetails
        {
            get { return _rednotificationDetails; }
            set { _rednotificationDetails = value; }
        }
    }

    public class RednotificationHead
    {

        private string _pId;
        private string _processFlag;
        private string _processRemark;
        private string _requestBillNo;
        private string _rednotificationNo;
        private string _statusCode;
        private string _statusMsg;
        private string _invoiceType;
        private string _companyTaxNo;
        private string _redNotificationType;
        private string _orignInvoiceCode;
        private string _orignInvoiceNo;
        private string _isMutiRate;
        private string _date;
        private string _purchaserName;
        private string _purchaseTaxNo;
        private string _sellerName;
        private string _sellerTaxNo;
        private string _amountWithoutTax;
        private string _amountWithTax;
        private string _taxRate;
        private string _taxAmount;
        private string _requestMemo;
        private string _goodsNoVer;
        private string _taxKind;

        /// <summary>
        /// 销货清单唯一ID
        /// </summary>
        public string PId
        {
            get { return _pId; }
            set { _pId = value; }
        }

        /// <summary>
        /// 处理标识
        /// </summary>
        public string ProcessFlag
        {
            get { return _processFlag; }
            set { _processFlag = value; }
        }

        /// <summary>
        /// 处理备注
        /// </summary>
        public string ProcessRemark
        {
            get { return _processRemark; }
            set { _processRemark = value; }
        }

        /// <summary>
        /// 申请单编号
        /// </summary>
        public string RequestBillNo
        {
            get { return _requestBillNo; }
            set { _requestBillNo = value; }
        }

        /// <summary>
        /// 红字信息表编号
        /// </summary>
        public string RednotificationNo
        {
            get { return _rednotificationNo; }
            set { _rednotificationNo = value; }
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyTaxNo
        {
            get { return _companyTaxNo; }
            set { _companyTaxNo = value; }
        }

        /// <summary>
        /// 信息表类型
        /// </summary>
        public string RedNotificationType
        {
            get { return _redNotificationType; }
            set { _redNotificationType = value; }
        }

        /// <summary>
        /// 原发票代码
        /// </summary>
        public string OrignInvoiceCode
        {
            get { return _orignInvoiceCode; }
            set { _orignInvoiceCode = value; }
        }

        /// <summary>
        /// 原发票号码
        /// </summary>
        public string OrignInvoiceNo
        {
            get { return _orignInvoiceNo; }
            set { _orignInvoiceNo = value; }
        }

        /// <summary>
        /// 是否多税率
        /// </summary>
        public string IsMutiRate
        {
            get { return _isMutiRate; }
            set { _isMutiRate = value; }
        }

        /// <summary>
        /// 填开日期
        /// </summary>
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// 购方公司名称
        /// </summary>
        public string PurchaserName
        {
            get { return _purchaserName; }
            set { _purchaserName = value; }
        }

        /// <summary>
        /// 购方税号
        /// </summary>
        public string PurchaseTaxNo
        {
            get { return _purchaseTaxNo; }
            set { _purchaseTaxNo = value; }
        }

        /// <summary>
        /// 销方公司名称
        /// </summary>
        public string SellerName
        {
            get { return _sellerName; }
            set { _sellerName = value; }
        }

        /// <summary>
        /// 销方公司税号
        /// </summary>
        public string SellerTaxNo
        {
            get { return _sellerTaxNo; }
            set { _sellerTaxNo = value; }
        }

        /// <summary>
        /// 不含税金额
        /// </summary>
        public string AmountWithoutTax
        {
            get { return _amountWithoutTax; }
            set { _amountWithoutTax = value; }
        }

        /// <summary>
        /// 含税金额
        /// </summary>
        public string AmountWithTax
        {
            get { return _amountWithTax; }
            set { _amountWithTax = value; }
        }

        /// <summary>
        /// 税率
        /// </summary>
        public string TaxRate
        {
            get { return _taxRate; }
            set { _taxRate = value; }
        }

        /// <summary>
        /// 税额
        /// </summary>
        public string TaxAmount
        {
            get { return _taxAmount; }
            set { _taxAmount = value; }
        }

        /// <summary>
        /// 申请说明
        /// </summary>
        public string RequestMemo
        {
            get { return _requestMemo; }
            set { _requestMemo = value; }
        }

        /// <summary>
        /// 编码版本号
        /// </summary>
        public string GoodsNoVer
        {
            get { return _goodsNoVer; }
            set { _goodsNoVer = value; }
        }

        /// <summary>
        /// 发票类型代码
        /// </summary>
        public string InvoiceType
        {
            get { return _invoiceType; }
            set { _invoiceType = value; }
        }

        /// <summary>
        /// 税率标识
        /// </summary>
        public string TaxKind
        {
            get { return _taxKind; }
            set { _taxKind = value; }
        }

        /// <summary>
        /// 信息表状态代码
        /// </summary>
        public string StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        /// <summary>
        /// 信息表状态说明
        /// </summary>
        public string StatusMsg
        {
            get { return _statusMsg; }
            set { _statusMsg = value; }
        }
    }

    public class RednotificationDetail
    {

        private string _itemName;
        private string _unit;
        private string _unitPrice;
        private string _taxRate;
        private string _itemSpec;
        private string _quantity;
        private string _amountWithoutTax;
        private string _amountWithTax;
        private string _taxAmount;
        private string _priceMethod;
        private string _goodsTaxNo;
        private string _corpGoodsNo;
        private string _taxPer;
        private string _taxPerCon;
        private string _zeroTax;

        /// <summary>
        /// 货物名称
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public string UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        /// <summary>
        /// 税率
        /// </summary>
        public string TaxRate
        {
            get { return _taxRate; }
            set { _taxRate = value; }
        }

        /// <summary>
        /// 规格
        /// </summary>
        public string ItemSpec
        {
            get { return _itemSpec; }
            set { _itemSpec = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>
        /// 不含税金额
        /// </summary>
        public string AmountWithoutTax
        {
            get { return _amountWithoutTax; }
            set { _amountWithoutTax = value; }
        }

        /// <summary>
        /// 含税金额
        /// </summary>
        public string AmountWithTax
        {
            get { return _amountWithTax; }
            set { _amountWithTax = value; }
        }

        /// <summary>
        /// 税额
        /// </summary>
        public string TaxAmount
        {
            get { return _taxAmount; }
            set { _taxAmount = value; }
        }

        /// <summary>
        /// 价格方式
        /// </summary>
        public string PriceMethod
        {
            get { return _priceMethod; }
            set { _priceMethod = value; }
        }

        /// <summary>
        /// 税收分类编码
        /// </summary>
        public string GoodsTaxNo
        {
            get { return _goodsTaxNo; }
            set { _goodsTaxNo = value; }
        }

        /// <summary>
        /// 企业自编码
        /// </summary>
        public string CorpGoodsNo
        {
            get { return _corpGoodsNo; }
            set { _corpGoodsNo = value; }
        }

        /// <summary>
        /// 优惠政策标识
        /// </summary>
        public string TaxPer
        {
            get { return _taxPer; }
            set { _taxPer = value; }
        }

        /// <summary>
        /// 优惠政策内容
        /// </summary>
        public string TaxPerCon
        {
            get { return _taxPerCon; }
            set { _taxPerCon = value; }
        }

        /// <summary>
        /// 零税率标识
        /// </summary>
        public string ZeroTax
        {
            get { return _zeroTax; }
            set { _zeroTax = value; }
        }
    }

}
