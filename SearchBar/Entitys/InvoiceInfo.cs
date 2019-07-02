using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchBar.Entitys
{
    public class InvoiceInfo
    {
        private InvoiceHead _invoiceHead;
        private List<InvoiceDetail> _invoiceDetails;

        public InvoiceInfo()
        {
            this._invoiceHead = new InvoiceHead();
            this._invoiceDetails = new List<InvoiceDetail>();
        }

        public InvoiceHead Head
        {
            get
            {
                return this._invoiceHead;
            }
            set
            {
                this._invoiceHead = value;
            }
        }

        public List<InvoiceDetail> InvoiceDetails
        {
            get
            {
                return this._invoiceDetails;
            }
            set
            {
                this._invoiceDetails = value;
            }
        }
    }

    public class InvoiceHead
    {
        private string _pId;
        private string _processFlag;
        private string _processRemark;
        private string _invoiceType;
        private string _invoiceCode;
        private string _invoiceNo;
        private string _invoiceDate;
        private string _invoiceKind;
        private string _invoiceStyle;
        private string _invoiceStatus;
        private string _checkCode;
        private string _cipherText;
        private string _checkerName;
        private string _machineCode;
        private string _cashierName;
        private string _invoicerName;
        private string _sellerName;
        private string _sellerTaxNo;
        private string _sellerAddress;
        private string _sellerTel;
        private string _sellerBankName;
        private string _sellerBankAccount;
        private string _purchaserName;
        private string _purchaserTaxNo;
        private string _purchaserId;
        private string _purchaserAddress;
        private string _purchaserTel;
        private string _purchaserBankName;
        private string _purchaserBankAccount;
        private string _manufactureName;
        private string _vehicleType;
        private string _vehicleBrand;
        private string _productionArea;
        private string _certificationNo;
        private string _importCertificateNo;
        private string _commodityInspectionNo;
        private string _engineNo;
        private string _vehicleNo;
        private string _taxPaidProof;
        private string _tonnage;
        private string _maxCapacity;
        private string _amountWithTax;
        private string _amountWithoutTax;
        private string _taxRate;
        private string _taxAmount;
        private string _goodsTaxNo;
        private string _goodsNoVer;
        private int _taxPer;
        private string _taxPerCon;
        private string _zeroTax;
        private string _chargeTaxAuthorityNa;
        private string _chargeTaxAuthorityCo;
        private string _originalInvoiceNo;
        private string _originalInvoiceCode;
        private string _redNotificationNo;
        private string _remark;
        private string _listGoodsName;
        private string _taxKind;
        private string _ext1;
        private string _ext2;
        private string _ext3;
        private string _ext4;
        private string _ext5;

        public string InvoiceType
        {
            get
            {
                return this._invoiceType;
            }
            set
            {
                this._invoiceType = value;
            }
        }

        public string CashierName
        {
            get
            {
                return this._cashierName;
            }
            set
            {
                this._cashierName = value;
            }
        }

        public string CheckerName
        {
            get
            {
                return this._checkerName;
            }
            set
            {
                this._checkerName = value;
            }
        }

        public string InvoicerName
        {
            get
            {
                return this._invoicerName;
            }
            set
            {
                this._invoicerName = value;
            }
        }

        public string SellerName
        {
            get
            {
                return this._sellerName;
            }
            set
            {
                this._sellerName = value;
            }
        }

        public string SellerTaxNo
        {
            get
            {
                return this._sellerTaxNo;
            }
            set
            {
                this._sellerTaxNo = value;
            }
        }

        public string SellerAddress
        {
            get
            {
                return this._sellerAddress;
            }
            set
            {
                this._sellerAddress = value;
            }
        }

        public string SellerTel
        {
            get
            {
                return this._sellerTel;
            }
            set
            {
                this._sellerTel = value;
            }
        }

        public string SellerBankName
        {
            get
            {
                return this._sellerBankName;
            }
            set
            {
                this._sellerBankName = value;
            }
        }

        public string SellerBankAccount
        {
            get
            {
                return this._sellerBankAccount;
            }
            set
            {
                this._sellerBankAccount = value;
            }
        }

        public string PurchaserName
        {
            get
            {
                return this._purchaserName;
            }
            set
            {
                this._purchaserName = value;
            }
        }

        public string PurchaserTaxNo
        {
            get
            {
                return this._purchaserTaxNo;
            }
            set
            {
                this._purchaserTaxNo = value;
            }
        }

        public string PurchaserId
        {
            get
            {
                return this._purchaserId;
            }
            set
            {
                this._purchaserId = value;
            }
        }

        public string PurchaserAddress
        {
            get
            {
                return this._purchaserAddress;
            }
            set
            {
                this._purchaserAddress = value;
            }
        }

        public string PurchaserTel
        {
            get
            {
                return this._purchaserTel;
            }
            set
            {
                this._purchaserTel = value;
            }
        }

        public string PurchaserBankName
        {
            get
            {
                return this._purchaserBankName;
            }
            set
            {
                this._purchaserBankName = value;
            }
        }

        public string PurchaserBankAccount
        {
            get
            {
                return this._purchaserBankAccount;
            }
            set
            {
                this._purchaserBankAccount = value;
            }
        }

        public string ManufactureName
        {
            get
            {
                return this._manufactureName;
            }
            set
            {
                this._manufactureName = value;
            }
        }

        public string VehicleType
        {
            get
            {
                return this._vehicleType;
            }
            set
            {
                this._vehicleType = value;
            }
        }

        public string VehicleBrand
        {
            get
            {
                return this._vehicleBrand;
            }
            set
            {
                this._vehicleBrand = value;
            }
        }

        public string ProductionArea
        {
            get
            {
                return this._productionArea;
            }
            set
            {
                this._productionArea = value;
            }
        }

        public string CertificationNo
        {
            get
            {
                return this._certificationNo;
            }
            set
            {
                this._certificationNo = value;
            }
        }

        public string ImportCertificateNo
        {
            get
            {
                return this._importCertificateNo;
            }
            set
            {
                this._importCertificateNo = value;
            }
        }

        public string CommodityInspectionNo
        {
            get
            {
                return this._commodityInspectionNo;
            }
            set
            {
                this._commodityInspectionNo = value;
            }
        }

        public string EngineNo
        {
            get
            {
                return this._engineNo;
            }
            set
            {
                this._engineNo = value;
            }
        }

        public string VehicleNo
        {
            get
            {
                return this._vehicleNo;
            }
            set
            {
                this._vehicleNo = value;
            }
        }

        public string TaxPaidProof
        {
            get
            {
                return this._taxPaidProof;
            }
            set
            {
                this._taxPaidProof = value;
            }
        }

        public string Tonnage
        {
            get
            {
                return this._tonnage;
            }
            set
            {
                this._tonnage = value;
            }
        }

        public string MaxCapacity
        {
            get
            {
                return this._maxCapacity;
            }
            set
            {
                this._maxCapacity = value;
            }
        }

        public string AmountWithTax
        {
            get
            {
                return this._amountWithTax;
            }
            set
            {
                this._amountWithTax = value;
            }
        }

        public string TaxRate
        {
            get
            {
                return this._taxRate;
            }
            set
            {
                this._taxRate = value;
            }
        }

        public string TaxAmount
        {
            get
            {
                return this._taxAmount;
            }
            set
            {
                this._taxAmount = value;
            }
        }

        public string GoodsTaxNo
        {
            get
            {
                return this._goodsTaxNo;
            }
            set
            {
                this._goodsTaxNo = value;
            }
        }

        public string GoodsNoVer
        {
            get
            {
                return this._goodsNoVer;
            }
            set
            {
                this._goodsNoVer = value;
            }
        }

        public int TaxPer
        {
            get
            {
                return this._taxPer;
            }
            set
            {
                this._taxPer = value;
            }
        }

        public string TaxPerCon
        {
            get
            {
                return this._taxPerCon;
            }
            set
            {
                this._taxPerCon = value;
            }
        }

        public string ZeroTax
        {
            get
            {
                return this._zeroTax;
            }
            set
            {
                this._zeroTax = value;
            }
        }

        public string OriginalInvoiceNo
        {
            get
            {
                return this._originalInvoiceNo;
            }
            set
            {
                this._originalInvoiceNo = value;
            }
        }

        public string OriginalInvoiceCode
        {
            get
            {
                return this._originalInvoiceCode;
            }
            set
            {
                this._originalInvoiceCode = value;
            }
        }

        public string RedNotificationNo
        {
            get
            {
                return this._redNotificationNo;
            }
            set
            {
                this._redNotificationNo = value;
            }
        }

        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
            }
        }

        public string ListGoodsName
        {
            get
            {
                return this._listGoodsName;
            }
            set
            {
                this._listGoodsName = value;
            }
        }

        public string PId
        {
            get
            {
                return this._pId;
            }
            set
            {
                this._pId = value;
            }
        }

        public string InvoiceCode
        {
            get
            {
                return this._invoiceCode;
            }
            set
            {
                this._invoiceCode = value;
            }
        }

        public string InvoiceNo
        {
            get
            {
                return this._invoiceNo;
            }
            set
            {
                this._invoiceNo = value;
            }
        }

        public string CheckCode
        {
            get
            {
                return this._checkCode;
            }
            set
            {
                this._checkCode = value;
            }
        }

        public string MachineCode
        {
            get
            {
                return this._machineCode;
            }
            set
            {
                this._machineCode = value;
            }
        }

        public string CipherText
        {
            get
            {
                return this._cipherText;
            }
            set
            {
                this._cipherText = value;
            }
        }

        public string InvoiceDate
        {
            get
            {
                return this._invoiceDate;
            }
            set
            {
                this._invoiceDate = value;
            }
        }

        public string ProcessFlag
        {
            get
            {
                return this._processFlag;
            }
            set
            {
                this._processFlag = value;
            }
        }

        public string ProcessRemark
        {
            get
            {
                return this._processRemark;
            }
            set
            {
                this._processRemark = value;
            }
        }

        public string InvoiceKind
        {
            get
            {
                return this._invoiceKind;
            }
            set
            {
                this._invoiceKind = value;
            }
        }

        public string InvoiceStyle
        {
            get
            {
                return this._invoiceStyle;
            }
            set
            {
                this._invoiceStyle = value;
            }
        }

        public string AmountWithoutTax
        {
            get
            {
                return this._amountWithoutTax;
            }
            set
            {
                this._amountWithoutTax = value;
            }
        }

        public string TaxKind
        {
            get
            {
                return this._taxKind;
            }
            set
            {
                this._taxKind = value;
            }
        }

        public string InvoiceStatus
        {
            get
            {
                return this._invoiceStatus;
            }
            set
            {
                this._invoiceStatus = value;
            }
        }

        public string ChargeTaxAuthorityNa
        {
            get
            {
                return this._chargeTaxAuthorityNa;
            }
            set
            {
                this._chargeTaxAuthorityNa = value;
            }
        }

        public string ChargeTaxAuthorityCo
        {
            get
            {
                return this._chargeTaxAuthorityCo;
            }
            set
            {
                this._chargeTaxAuthorityCo = value;
            }
        }

        public string Ext1
        {
            get
            {
                return this._ext1;
            }
            set
            {
                this._ext1 = value;
            }
        }

        public string Ext2
        {
            get
            {
                return this._ext2;
            }
            set
            {
                this._ext2 = value;
            }
        }

        public string Ext3
        {
            get
            {
                return this._ext3;
            }
            set
            {
                this._ext3 = value;
            }
        }

        public string Ext4
        {
            get
            {
                return this._ext4;
            }
            set
            {
                this._ext4 = value;
            }
        }

        public string Ext5
        {
            get
            {
                return this._ext5;
            }
            set
            {
                this._ext5 = value;
            }
        }

        public InvoiceHead()
        {
            this._pId = "";
            this._processFlag = "0";
            this._processRemark = "未处理";
            this._invoiceType = "";
            this._invoiceCode = "";
            this._invoiceNo = "";
            this._invoiceDate = "";
            this._invoiceKind = "";
            this._invoiceStyle = "";
            this._invoiceStatus = "";
            this._checkCode = "";
            this._cipherText = "";
            this._checkerName = "";
            this._machineCode = "";
            this._cashierName = "";
            this._invoicerName = "";
            this._sellerName = "";
            this._sellerTaxNo = "";
            this._sellerAddress = "";
            this._sellerTel = "";
            this._sellerBankName = "";
            this._sellerBankAccount = "";
            this._purchaserName = "";
            this._purchaserTaxNo = "";
            this._purchaserId = "";
            this._purchaserAddress = "";
            this._purchaserTel = "";
            this._purchaserBankName = "";
            this._purchaserBankAccount = "";
            this._manufactureName = "";
            this._vehicleType = "";
            this._vehicleBrand = "";
            this._productionArea = "";
            this._certificationNo = "";
            this._importCertificateNo = "";
            this._commodityInspectionNo = "";
            this._engineNo = "";
            this._vehicleNo = "";
            this._taxPaidProof = "";
            this._tonnage = "";
            this._maxCapacity = "";
            this._amountWithTax = "0.00";
            this._amountWithoutTax = "0.00";
            this._taxRate = "";
            this._taxAmount = "0.00";
            this._goodsTaxNo = "";
            this._goodsNoVer = "";
            this._taxPer = 0;
            this._taxPerCon = "";
            this._zeroTax = "";
            this._chargeTaxAuthorityNa = "";
            this._chargeTaxAuthorityCo = "";
            this._originalInvoiceNo = "";
            this._originalInvoiceCode = "";
            this._redNotificationNo = "";
            this._remark = "";
            this._listGoodsName = "";
            this._taxKind = "";
            this._ext1 = "";
            this._ext2 = "";
            this._ext3 = "";
            this._ext4 = "";
            this._ext5 = "";
        }
    }

    public class InvoiceDetail
    {
        private string _detailKind;
        private string _itemName;
        private string _itemSpec;
        private string _unit;
        private string _unitPrice;
        private string _quantity;
        private string _taxRate;
        private string _taxItem;
        private string _amountWithTax;
        private string _amountWithoutTax;
        private string _taxAmount;
        private string _discountWithTax;
        private string _discountWithoutTax;
        private string _discountTaxAmount;
        private string _priceMethod;
        private string _goodsTaxNo;
        private string _goodsNoVer;
        private string _taxPer;
        private string _taxperCon;
        private string _zeroTax;
        private string _corpGoodsNo;
        private string _taxDedunction;

        public string ItemName
        {
            get
            {
                return this._itemName;
            }
            set
            {
                this._itemName = value;
            }
        }

        public string ItemSpec
        {
            get
            {
                return this._itemSpec;
            }
            set
            {
                this._itemSpec = value;
            }
        }

        public string Unit
        {
            get
            {
                return this._unit;
            }
            set
            {
                this._unit = value;
            }
        }

        public string Quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                this._quantity = value;
            }
        }

        public string TaxRate
        {
            get
            {
                return this._taxRate;
            }
            set
            {
                this._taxRate = value;
            }
        }

        public string TaxItem
        {
            get
            {
                return this._taxItem;
            }
            set
            {
                this._taxItem = value;
            }
        }

        public string TaxAmount
        {
            get
            {
                return this._taxAmount;
            }
            set
            {
                this._taxAmount = value;
            }
        }

        public string DiscountTaxAmount
        {
            get
            {
                return this._discountTaxAmount;
            }
            set
            {
                this._discountTaxAmount = value;
            }
        }

        public string PriceMethod
        {
            get
            {
                return this._priceMethod;
            }
            set
            {
                this._priceMethod = value;
            }
        }

        public string GoodsTaxNo
        {
            get
            {
                return this._goodsTaxNo;
            }
            set
            {
                this._goodsTaxNo = value;
            }
        }

        public string GoodsNoVer
        {
            get
            {
                return this._goodsNoVer;
            }
            set
            {
                this._goodsNoVer = value;
            }
        }

        public string TaxPer
        {
            get
            {
                return this._taxPer;
            }
            set
            {
                this._taxPer = value;
            }
        }

        public string TaxperCon
        {
            get
            {
                return this._taxperCon;
            }
            set
            {
                this._taxperCon = value;
            }
        }

        public string ZeroTax
        {
            get
            {
                return this._zeroTax;
            }
            set
            {
                this._zeroTax = value;
            }
        }

        public string TaxDedunction
        {
            get
            {
                return this._taxDedunction;
            }
            set
            {
                this._taxDedunction = value;
            }
        }

        public string UnitPrice
        {
            get
            {
                return this._unitPrice;
            }
            set
            {
                this._unitPrice = value;
            }
        }

        public string CorpGoodsNo
        {
            get
            {
                return this._corpGoodsNo;
            }
            set
            {
                this._corpGoodsNo = value;
            }
        }

        public string AmountWithTax
        {
            get
            {
                return this._amountWithTax;
            }
            set
            {
                this._amountWithTax = value;
            }
        }

        public string AmountWithoutTax
        {
            get
            {
                return this._amountWithoutTax;
            }
            set
            {
                this._amountWithoutTax = value;
            }
        }

        public string DiscountWithTax
        {
            get
            {
                return this._discountWithTax;
            }
            set
            {
                this._discountWithTax = value;
            }
        }

        public string DiscountWithoutTax
        {
            get
            {
                return this._discountWithoutTax;
            }
            set
            {
                this._discountWithoutTax = value;
            }
        }

        public string DetailKind
        {
            get
            {
                return this._detailKind;
            }
            set
            {
                this._detailKind = value;
            }
        }

        public InvoiceDetail()
        {
            this._itemName = "";
            this._itemSpec = "";
            this._unit = "";
            this._unitPrice = "";
            this._quantity = "";
            this._taxRate = "";
            this._taxItem = "";
            this._amountWithTax = "";
            this._amountWithoutTax = "";
            this._taxAmount = "";
            this._discountWithTax = "";
            this._discountWithoutTax = "";
            this._discountTaxAmount = "";
            this._priceMethod = "0";
            this._goodsTaxNo = "";
            this._goodsNoVer = "";
            this._taxPer = "0";
            this._taxperCon = "";
            this._zeroTax = "";
            this._corpGoodsNo = "";
            this._taxDedunction = "";
        }
    }


}
