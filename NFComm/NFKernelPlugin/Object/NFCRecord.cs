//-----------------------------------------------------------------------
// <copyright file="NFCRecord.cs">
//     Copyright (C) 2015-2015 lvsheng.huang <https://github.com/ketoo/NFrame>
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace NFrame
{
    public class NFCRecord : NFIRecord
    {

		public NFCRecord(NFGUID self, string strRecordName, int nRow, NFIDataList varData)
		{
			mSelf = self;
			mnRow = nRow;
			mstrRecordName = strRecordName;
            mVarRecordType = new NFCDataList(varData);
		}

        //==============================================
		public override bool IsUsed(int nRow)
		{
			if (mhtRecordVec.ContainsKey(nRow))
			{
				return true;
			}
			
			return false;
		}
		
		public override int GetRows()
        {
			return mnRow;
        }
		
        public override int GetCols()
        {
			return mVarRecordType.Count();
        }

        public override NFIDataList.VARIANT_TYPE GetColType(int nCol)
        {
			return mVarRecordType.GetType(nCol);
        }

        public override NFIDataList GetColsData()
        {
            return mVarRecordType;
        }


        // add data
        public override int AddRow(int nRow)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				return AddRow(nRow, mVarRecordType);
			}
			
			return -1;
        }

        public override int AddRow(int nRow, NFIDataList var)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				if (!mhtRecordVec.ContainsKey(nRow))
				{
					mhtRecordVec[nRow] = new NFCDataList(var);

                    if (null != doHandleDel)
                    {
                        doHandleDel(mSelf, mstrRecordName, eRecordOptype.Add, nRow, 0, NFIDataList.NULL_TDATA, NFIDataList.NULL_TDATA);
                    }
					return nRow;
				}
			}
			
			
            return -1;
        }

        // set data
        public override int SetValue(int nRow, NFIDataList var)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				if (!mhtRecordVec.ContainsKey(nRow))
				{
					AddRow(nRow);
				}
				
				mhtRecordVec[nRow] = var;
				return nRow;
			}
            return -1;
        }

        public override bool SetInt(int nRow, int nCol, Int64 value)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				if (!mhtRecordVec.ContainsKey(nRow))
				{
					AddRow(nRow);
				}
				NFIDataList valueList = (NFIDataList)mhtRecordVec[nRow];
				if (valueList.GetType(nCol) == NFIDataList.VARIANT_TYPE.VTYPE_INT)
				{
					if (valueList.IntVal(nCol) != value)
					{
                        NFIDataList.TData oldValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_INT);
                        NFIDataList.TData newValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_INT);

                        oldValue.Set(valueList.IntVal(nCol));
                        newValue.Set(value);

                        valueList.SetInt(nCol, value);
	                   
	                    if (null != doHandleDel)
	                    {
	                        doHandleDel(mSelf, mstrRecordName, eRecordOptype.Updata, nRow, nCol, oldValue, newValue);
	                    }
	                }
				}
				return true;
				
			}
            return false;
        }

        public override bool SetFloat(int nRow, int nCol, float value)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				if (!mhtRecordVec.ContainsKey(nRow))
				{
					AddRow(nRow);
				}
				NFIDataList valueList = (NFIDataList)mhtRecordVec[nRow];
				if (valueList.GetType(nCol) == NFIDataList.VARIANT_TYPE.VTYPE_FLOAT)
				{
					if (valueList.FloatVal(nCol) - value > NFIDataList.EPS_DOUBLE
						|| valueList.FloatVal(nCol) - value < -NFIDataList.EPS_DOUBLE)
					{
                        NFIDataList.TData oldValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_FLOAT);
                        NFIDataList.TData newValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_FLOAT);

                        oldValue.Set(valueList.FloatVal(nCol));
                        newValue.Set(value);

	                    valueList.SetFloat(nCol, value);
	
	                    if (null != doHandleDel)
	                    {
	                        doHandleDel(mSelf, mstrRecordName, eRecordOptype.Updata, nRow, nCol, oldValue, newValue);
	                    }
	                }
	
				}
	
				return true;
			}
			return false;
        }

        public override bool SetDouble(int nRow, int nCol, double value)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				if (!mhtRecordVec.ContainsKey(nRow))
				{
					AddRow(nRow);
				}

                NFIDataList valueList = (NFIDataList)mhtRecordVec[nRow];
                if (valueList.GetType(nCol) == NFIDataList.VARIANT_TYPE.VTYPE_DOUBLE)
                {
                    if (valueList.DoubleVal(nCol) - value > NFIDataList.EPS_DOUBLE
                        || valueList.DoubleVal(nCol) - value < -NFIDataList.EPS_DOUBLE)
                    {
                        NFIDataList.TData oldValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_DOUBLE);
                        NFIDataList.TData newValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_DOUBLE);

                        oldValue.Set(valueList.DoubleVal(nCol));
                        newValue.Set(value);

                        valueList.SetDouble(nCol, value);
	
	                    if (null != doHandleDel)
	                    {
	                        doHandleDel(mSelf, mstrRecordName, eRecordOptype.Updata, nRow, nCol, oldValue, newValue);
	                    }
	                }
				}
	
				return true;
			}
			return false;
        }

        public override bool SetString(int nRow, int nCol, string value)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				if (!mhtRecordVec.ContainsKey(nRow))
				{
					AddRow(nRow);
				}
				NFIDataList valueList = (NFIDataList)mhtRecordVec[nRow];
				if (valueList.GetType(nCol) == NFIDataList.VARIANT_TYPE.VTYPE_STRING)
				{
					if (valueList.StringVal(nCol) != value)
					{
                        NFIDataList.TData oldValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_STRING);
                        NFIDataList.TData newValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_STRING);

                        oldValue.Set(valueList.StringVal(nCol));
                        newValue.Set(value);

                        valueList.SetString(nCol, value);
	
	                    if (null != doHandleDel)
	                    {
	                        doHandleDel(mSelf, mstrRecordName, eRecordOptype.Updata, nRow, nCol, oldValue, newValue);
	                    }
	                }
				}
	
				return true;
			}

			return false;
        }

        public override bool SetObject(int nRow, int nCol, NFGUID value)
        {
			if(nRow >= 0 && nRow < mnRow)
			{
				if (!mhtRecordVec.ContainsKey(nRow))
				{
					AddRow(nRow);
				}
				NFIDataList valueList = (NFIDataList)mhtRecordVec[nRow];
				if (valueList.GetType(nCol) == NFIDataList.VARIANT_TYPE.VTYPE_OBJECT)
				{
					if (valueList.ObjectVal(nCol) != value)
					{
                        NFIDataList.TData oldValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_OBJECT);
                        NFIDataList.TData newValue = new NFIDataList.TData(NFIDataList.VARIANT_TYPE.VTYPE_OBJECT);

                        oldValue.Set(valueList.ObjectVal(nCol));
                        newValue.Set(value);

                        valueList.SetObject(nCol, value);

                        if (null != doHandleDel)
                        {
                            doHandleDel(mSelf, mstrRecordName, eRecordOptype.Updata, nRow, nCol, oldValue, newValue);
                        }
                    }
				}

				return true;
			}

			return false;
        }

        // query data
        public override NFIDataList QueryRow(int nRow)
        {
			if (mhtRecordVec.ContainsKey(nRow))
			{
				return  (NFIDataList)mhtRecordVec[nRow];
			}

            return null;
        }

        public override bool SwapRow(int nOriginRow, int nTargetRow)
        {
			if(nOriginRow >= 0 && nOriginRow < mnRow && nTargetRow >= 0 && nTargetRow < mnRow)
			{
	            NFIDataList valueOriginList = null;
	            NFIDataList valueTargetList = null;
	           
	            if (mhtRecordVec.ContainsKey(nOriginRow))
	            {
	                valueOriginList = (NFIDataList)mhtRecordVec[nOriginRow];
	            }
	            if (mhtRecordVec.ContainsKey(nTargetRow))
	            {
	                valueTargetList = (NFIDataList)mhtRecordVec[nOriginRow];
	            }
	
	            if (null == valueTargetList)
	            {
	                if (mhtRecordVec.ContainsKey(nOriginRow))
	                {
	                    mhtRecordVec.Remove(nOriginRow);
	                }
	            }
	            else
	            {
	               mhtRecordVec[nOriginRow] = valueTargetList;
	            }
	            
	            if (null == valueOriginList)
	            {
	                if (mhtRecordVec.ContainsKey(nTargetRow))
	                {
	                    mhtRecordVec.Remove(nTargetRow);
	                }
	            }
	            else
	            {
	                mhtRecordVec[nTargetRow] = valueOriginList;
	            }
	           
	            if (null != doHandleDel)
	             {
	                 doHandleDel(mSelf, mstrRecordName, eRecordOptype.Swap, nOriginRow, nTargetRow, NFIDataList.NULL_TDATA, NFIDataList.NULL_TDATA);
	             }
	            return true;
			}
			return false;
        }

        public override Int64 QueryInt(int nRow, int nCol)
        {
			NFIDataList valueList = QueryRow(nRow);
			if (null != valueList)
			{
				return valueList.IntVal(nCol);
			}

			return 0;
        }

        public override float QueryFloat(int nRow, int nCol)
        {
			NFIDataList valueList = QueryRow(nRow);
			if (null != valueList)
			{
				return valueList.FloatVal(nCol);
			}

            return 0.0f;
        }

        public override double QueryDouble(int nRow, int nCol)
        {
			NFIDataList valueList = QueryRow(nRow);
			if (null != valueList)
			{
				return valueList.DoubleVal(nCol);
			}

            return 0.0;
        }

        public override string QueryString(int nRow, int nCol)
        {
			NFIDataList valueList = QueryRow(nRow);
			if (null != valueList)
			{
				return valueList.StringVal(nCol);
			}

            return NFIDataList.NULL_STRING;
        }

        public override NFGUID QueryObject(int nRow, int nCol)
        {
			NFIDataList valueList = QueryRow(nRow);
			if (null != valueList)
			{
				return valueList.ObjectVal(nCol);
			}

            return NFIDataList.NULL_OBJECT;
        }

        //public override int FindRow( int nRow );
        public override int FindColValue(int nCol, NFIDataList var)
        {
			for (int i = 0; i < mhtRecordVec.Count; i++ )
			{
				NFIDataList valueList = (NFIDataList)mhtRecordVec[i];
				switch (valueList.GetType(0))
				{
					case NFIDataList.VARIANT_TYPE.VTYPE_INT:
						return FindInt(nCol, var.IntVal(0));

					case NFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
						return FindInt(nCol, var.IntVal(0));

					case NFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
						return FindInt(nCol, var.IntVal(0));

					case NFIDataList.VARIANT_TYPE.VTYPE_STRING:
						return FindInt(nCol, var.IntVal(0));

					case NFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
						return FindObject(nCol, var.ObjectVal(0));
					default:
					break;
				}
			}


            return -1;
        }

        public override int FindInt(int nCol, Int64 value)
        {
			foreach (int i in mhtRecordVec.Keys)
			{
				NFIDataList valueList = (NFIDataList)mhtRecordVec[i];
				if (valueList.IntVal(nCol) == value)
				{
					return i;
				}
			}
            return -1;
        }

        public override int FindFloat(int nCol, float value)
        {
			foreach (int i in mhtRecordVec.Keys)
			{
				NFIDataList valueList = (NFIDataList)mhtRecordVec[i];
				if (valueList.FloatVal(nCol) == value)
				{
					return i;
				}
			}
            return -1;
        }

        public override int FindDouble(int nCol, double value)
        {
			foreach (int i in mhtRecordVec.Keys)
			{
				NFIDataList valueList = (NFIDataList)mhtRecordVec[i];
				if (valueList.DoubleVal(nCol) == value)
				{
					return i;
				}
			}
            return -1;
        }

        public override int FindString(int nCol, string value)
        {
			foreach (int i in mhtRecordVec.Keys)
			{
				NFIDataList valueList = (NFIDataList)mhtRecordVec[i];
				if (valueList.StringVal(nCol) == value)
				{
					return i;
				}
			}

            return -1;
        }

        public override int FindObject(int nCol, NFGUID value)
        {
			foreach (int i in mhtRecordVec.Keys)
			{
				NFIDataList valueList = (NFIDataList)mhtRecordVec[i];
				if (valueList.ObjectVal(nCol) == value)
				{
					return i;
				}
			}

            return -1;
        }

        public override bool Remove(int nRow)
        {
			if (mhtRecordVec.Contains(nRow))
            {
				if (null != doHandleDel)
                {
                    doHandleDel(mSelf, mstrRecordName, eRecordOptype.Del, nRow, 0, NFIDataList.NULL_TDATA, NFIDataList.NULL_TDATA);
                }
				mhtRecordVec.Remove(nRow);
				return true;
            }

            return false;
        }

        public override bool Clear()
        {
            for (int i = 0; i < mhtUseState.Count; ++i )
            {
                if (IsUsed(i))
                {
                    Remove(i);

                    mhtUseState[i] = 0;
                }
            }

			mhtRecordVec.Clear();

            return true;
        }

		public override void RegisterCallback(RecordEventHandler handler)
		{
			doHandleDel += handler;
		}

        public override string GetName()
        {
            return mstrRecordName;
        }

        public override void SetName(string strName)
        {
            mstrRecordName = strName;
        }

		RecordEventHandler doHandleDel;

		NFIDataList mVarRecordType;
        Hashtable mhtRecordVec = new Hashtable();
        Dictionary<int, int> mhtUseState = new Dictionary<int, int>();

		NFGUID mSelf;
		string mstrRecordName;
		int mnRow;
    }
}