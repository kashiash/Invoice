﻿using DevExpress.Data.Filtering;
using LiczbyNaSlowaNETCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JKXAF.CustomFunctions
{
    public class KwotaSlownie : ICustomFunctionOperatorBrowsable
    {
        public FunctionCategory Category
        {
            get
            {
                return FunctionCategory.Text;
            }
        }
        public string Description
        {
            get
            {
                return "KwotaSlownie(decimal Kwota)\r\nKonwertuje kwotę na opis słowny";
            }
        }
        public bool IsValidOperandCount(int count)
        {
            return count == 1;
        }
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return true;
        }
        public int MaxOperandCount
        {
            get
            {
                return 1;
            }
        }
        public int MinOperandCount
        {
            get
            {
                return 1;
            }
        }
        public object Evaluate(params object[] operands)
        {

            var options = new NumberToTextOptions
            {
                Stems = true,
                Currency = Currency.PLN,
            };
            var kwotaStr = operands[0];
            decimal kwota = Decimal.Parse(kwotaStr.ToString());

            string res = NumberToText.Convert(kwota, options); ;
            return res;
        }
        public string Name
        {
            get
            {
                return "KwotaSlownie";
            }
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(string);
        }
    }
}