/**
 *  (C) 2006-2007 Mircea-Cristian Racasan <darx_kies@gmx.net>
 * 
 *  Licensed under the terms of the GNU GPL License version 2.
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using SharpOS.AOT.IR.Operators;
using Mono.Cecil;

namespace SharpOS.AOT.IR.Operands
{
    [Serializable]
    public abstract class Operand
    {
        public enum ConvertType
        {
            NotSet
            , Conv_I
            , Conv_I1
            , Conv_I2
            , Conv_I4
            , Conv_I8
            , Conv_Ovf_I
            , Conv_Ovf_I_Un
            , Conv_Ovf_I1
            , Conv_Ovf_I1_Un
            , Conv_Ovf_I2
            , Conv_Ovf_I2_Un
            , Conv_Ovf_I4
            , Conv_Ovf_I4_Un
            , Conv_Ovf_I8
            , Conv_Ovf_I8_Un
            , Conv_Ovf_U
            , Conv_Ovf_U_Un
            , Conv_Ovf_U1
            , Conv_Ovf_U1_Un
            , Conv_Ovf_U2
            , Conv_Ovf_U2_Un
            , Conv_Ovf_U4
            , Conv_Ovf_U4_Un
            , Conv_Ovf_U8
            , Conv_Ovf_U8_Un
            , Conv_R_Un
            , Conv_R4
            , Conv_R8
            , Conv_U
            , Conv_U1
            , Conv_U2
            , Conv_U4
            , Conv_U8
        }

        public enum InternalSizeType
        {
            NotSet
            , I1
            , I2
            , I4
            , I8
            , R4
            , R8
        }

        public Operand()
        {
        }

        private int register = int.MinValue;

        public int Register
        {
            get { return register; }
            set { register = value; }
        }

        public bool IsRegisterSet
        {
            get
            {
                return register != int.MinValue;
            }
        }

        private int stack = int.MinValue;

        public int Stack
        {
            get { return stack; }
            set { stack = value; }
        }

        private ConvertType convertTo = ConvertType.NotSet;

        public ConvertType ConvertTo
        {
            get { return convertTo; }
            set { convertTo = value; }
        }

        private InternalSizeType sizeType = InternalSizeType.NotSet;

        public InternalSizeType SizeType
        {
            get { return sizeType; }
            set { sizeType = value; }
        }

        public void SetSizeType(string type)
        {
            if (type.EndsWith("*") == true)
            {
                type = type.Substring(0, type.Length - 1);
            }

            if (type.Equals("System.Boolean") == true)
            {
                this.sizeType = InternalSizeType.I1;
            }
            else if (type.Equals("bool") == true)
            {
                this.sizeType = InternalSizeType.I1;
            }

            else if (type.Equals("System.Byte") == true)
            {
                this.sizeType = InternalSizeType.I1;
            }
            else if (type.Equals("System.SByte") == true)
            {
                this.sizeType = InternalSizeType.I1;
            }

            else if (type.Equals("char") == true)
            {
                this.sizeType = InternalSizeType.I2;
            }
            else if (type.Equals("short") == true)
            {
                this.sizeType = InternalSizeType.I2;
            }
            else if (type.Equals("ushort") == true)
            {
                this.sizeType = InternalSizeType.I2;
            }
            else if (type.Equals("System.UInt16") == true)
            {
                this.sizeType = InternalSizeType.I2;
            }
            else if (type.Equals("System.Int16") == true)
            {
                this.sizeType = InternalSizeType.I2;
            }

            else if (type.Equals("int") == true)
            {
                this.sizeType = InternalSizeType.I4;
            }
            else if (type.Equals("uint") == true)
            {
                this.sizeType = InternalSizeType.I4;
            }
            else if (type.Equals("System.UInt32") == true)
            {
                this.sizeType = InternalSizeType.I4;
            }
            else if (type.Equals("System.Int32") == true)
            {
                this.sizeType = InternalSizeType.I4;
            }

            else if (type.Equals("long") == true)
            {
                this.sizeType = InternalSizeType.I8;
            }
            else if (type.Equals("ulong") == true)
            {
                this.sizeType = InternalSizeType.I8;
            }
            else if (type.Equals("System.UInt64") == true)
            {
                this.sizeType = InternalSizeType.I8;
            }
            else if (type.Equals("System.Int64") == true)
            {
                this.sizeType = InternalSizeType.I8;
            }

            else if (type.Equals("float") == true)
            {
                this.sizeType = InternalSizeType.R4;
            }
            else if (type.Equals("System.Single") == true)
            {
                this.sizeType = InternalSizeType.R4;
            }

            else if (type.Equals("double") == true)
            {
                this.sizeType = InternalSizeType.R8;
            }
            else if (type.Equals("System.Double") == true)
            {
                this.sizeType = InternalSizeType.R8;
            }

            else
            {
                throw new Exception("'" + type + "' not supported.");
            }
        }

        public InternalSizeType ConvertSizeType
        {
            get
            {
                return Operand.GetType(this.convertTo);
            }
        }

        public static InternalSizeType GetType(ConvertType type)
        {
            switch (type)
            {
                case Operand.ConvertType.Conv_I1:
                case Operand.ConvertType.Conv_Ovf_I1:
                case Operand.ConvertType.Conv_Ovf_I1_Un:
                case Operand.ConvertType.Conv_Ovf_U1:
                case Operand.ConvertType.Conv_Ovf_U1_Un:
                case Operand.ConvertType.Conv_U1:
                    return InternalSizeType.I1;

                case Operand.ConvertType.Conv_I2:
                case Operand.ConvertType.Conv_Ovf_I2:
                case Operand.ConvertType.Conv_Ovf_I2_Un:
                case Operand.ConvertType.Conv_Ovf_U2:
                case Operand.ConvertType.Conv_Ovf_U2_Un:
                case Operand.ConvertType.Conv_U2:
                    return InternalSizeType.I2;

                case Operand.ConvertType.Conv_I:
                case Operand.ConvertType.Conv_I4:
                case Operand.ConvertType.Conv_Ovf_I:
                case Operand.ConvertType.Conv_Ovf_I_Un:
                case Operand.ConvertType.Conv_Ovf_I4:
                case Operand.ConvertType.Conv_Ovf_I4_Un:
                case Operand.ConvertType.Conv_Ovf_U:
                case Operand.ConvertType.Conv_Ovf_U_Un:
                case Operand.ConvertType.Conv_Ovf_U4:
                case Operand.ConvertType.Conv_Ovf_U4_Un:
                case Operand.ConvertType.Conv_U:
                case Operand.ConvertType.Conv_U4:
                    return InternalSizeType.I4;

                case Operand.ConvertType.Conv_I8:
                case Operand.ConvertType.Conv_Ovf_I8:
                case Operand.ConvertType.Conv_Ovf_I8_Un:
                case Operand.ConvertType.Conv_Ovf_U8:
                case Operand.ConvertType.Conv_Ovf_U8_Un:
                case Operand.ConvertType.Conv_U8:
                    return InternalSizeType.I8;

                case Operand.ConvertType.Conv_R_Un:
                case Operand.ConvertType.Conv_R4:
                    return InternalSizeType.R4;

                case Operand.ConvertType.Conv_R8:
                    return InternalSizeType.R8;

                default:
                    throw new Exception("'" + type + "' not supported.");
            }
        }

        public Operand(Operator _operator, Operand[] operands)
        {
            this._operator = _operator;
            this.operands = operands;
        }

        private Operator _operator = null;

        public Operator Operator
        {
            get { return _operator; }
        }

        protected Operand[] operands = null;

        public virtual Operand[] Operands
        {
            get
            {
                return operands;
            }
            set
            {
                this.operands = value;
            }
        }

        private int stamp = int.MinValue;

        public int Stamp
        {
            get { return stamp; }
            set { stamp = value; }
        }


        private int version = 0;

        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        public void Replace(Dictionary<string, Operand> registerValues)
        {
            if (this.operands == null)
            {
                return;
            }

            for (int i = 0; i < this.operands.Length; i++)
            {
                Operand operand = this.operands[i];

                operand.Replace(registerValues);

                if (operand is Register && registerValues.ContainsKey(operand.ToString()) == true)
                {
                    this.operands[i] = registerValues[operand.ToString()];
                }
            }
        }

        public SharpOS.AOT.IR.Operands.Operand Clone()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            binaryFormatter.Serialize(memoryStream, this);
            memoryStream.Seek(0, SeekOrigin.Begin);

            SharpOS.AOT.IR.Operands.Operand operand = (SharpOS.AOT.IR.Operands.Operand)binaryFormatter.Deserialize(memoryStream);

            return operand;
        }

        public virtual string ID
        {
            get
            {
                return this.ToString();
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string operatorValue = string.Empty;

            /*if (this._operator != null)
            {
                stringBuilder.Append(this._operator.ToString() + " ");
            }

            if (this.operands != null && this.operands.Length > 0)
            {
                foreach (Operand operand in operands)
                {
                    if (operand != operands[0])
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(operand.ToString());
                }

            }*/

            if (this._operator != null)
            {
                operatorValue = this._operator.ToString();
            }

            if (this.operands != null && this.operands.Length > 0)
            {
                if (this.operands.Length == 1)
                {
                    stringBuilder.Append(operatorValue + " (" + this.operands[0].ToString() + ")");
                }
                else if (this.operands.Length == 2)
                {
                    stringBuilder.Append("(" + this.operands[0].ToString() + ") " + operatorValue + " (" + this.operands[1].ToString() + ")");
                }
                else
                {
                    stringBuilder.Append(operatorValue + " (");

                    foreach (Operand expression in operands)
                    {
                        if (expression != operands[0])
                        {
                            stringBuilder.Append(", ");
                        }

                        stringBuilder.Append(expression.ToString());
                    }

                    stringBuilder.Append(")");
                }
            }
            else
            {
                stringBuilder.Append(operatorValue);
            }

            return stringBuilder.ToString().Trim();
        }
    }
}