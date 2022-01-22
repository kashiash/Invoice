using Common.Module.Utils;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using KrajeWaluty.Module.BusinessObjects;
using KrajeWaluty.Module.Utils;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PobierzWaluty
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pobieramy kursy walut!");



            //for (int i = 2002; i < 2021; i++)
            //{
            //    PobierzKursyNBP(i,"A");
            //    PobierzKursyNBP(i, "C");
            //}

            WalutyHelper.WczytajKrajeIWaluty();

            KursyNBPHelper.PobierzKursyNBP("A");
            KursyNBPHelper.PobierzKursyNBP( "C");

            Console.WriteLine("Zakonczono import wciśnij enter aby zakopńczyc program");
            Console.ReadLine();
        }


        public static void DodajKraje(IObjectSpace ObjectSpace)
        {

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))

            {

                RegionInfo ri = null;

                try

                {
                    ri = new RegionInfo(ci.Name);
                }

                catch

                {
                    continue;
                }

                var a = ri.EnglishName;
                var a1 = ri.NativeName;
                var a2 = ri.ThreeLetterISORegionName;
                var a3 = ri.CurrencyEnglishName;
                var a4 = ri.CurrencyNativeName;
                var a5 = ri.CurrencySymbol;
                var a6 = ri.ISOCurrencySymbol;

                var waluta = ObjectSpace.FindObject<Waluta>(new BinaryOperator(nameof(Waluta.Symbol), ri.ISOCurrencySymbol));
                if (waluta == null)
                {
                    waluta = ObjectSpace.CreateObject<Waluta>();
                    waluta.Symbol = ri.ISOCurrencySymbol;
                    waluta.Nazwa = ri.CurrencyEnglishName;
                    waluta.LokalnaNazwa = ri.CurrencyNativeName;
                    waluta.LokalnySymbol = ri.CurrencySymbol;
                }

                var kraj = ObjectSpace.FindObject<Kraj>(new BinaryOperator(nameof(Kraj.Symbol), ri.ThreeLetterISORegionName));
                if (kraj == null)
                {
                    kraj = ObjectSpace.CreateObject<Kraj>();
                    kraj.Symbol = ri.ThreeLetterISORegionName;
                    kraj.Nazwa = ri.EnglishName;
                    kraj.LokalnySymbol = ri.TwoLetterISORegionName;
                    kraj.LokalnaNazwa = ri.NativeName;
                    kraj.GeoId = ri.GeoId;
                    kraj.Waluta = waluta;
                    kraj.IsMetric = ri.IsMetric;

                }
                waluta.Kraj = kraj;
            }
        }



    }

}
