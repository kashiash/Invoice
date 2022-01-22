using Common.Module.Utils;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using KrajeWaluty.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KrajeWaluty.Module.Utils
{
    public class WalutyHelper

    {


        private static void WczytajKursy(IObjectSpace directObjectSpace,List<Notowanie> notowania)
        {
            foreach (var notowanie in notowania)
            {
                WczytajNotowanie(directObjectSpace,notowanie);

            }
        }

        private static void WczytajNotowanie(IObjectSpace directObjectSpace,Notowanie notowanie)
        {
            var effectiveDate = notowanie.effectiveDate;
            foreach (var kurs in notowanie.rates)
            {
                var waluta = directObjectSpace.GetObjectByKey<Waluta>(kurs.code);
                //  Guard.ArgumentNotNull(waluta, "Brak waluty");
                if (waluta != null)
                {

                    var kursWaluty = directObjectSpace.FindObject<KursWaluty>(CriteriaOperator.Parse("Waluta = ? And DataKursu = ?", waluta, effectiveDate));
                    if (kursWaluty == null)
                    {
                        kursWaluty = directObjectSpace.CreateObject<KursWaluty>();
                        kursWaluty.Waluta = waluta;
                        kursWaluty.DataKursu = effectiveDate;
                    }
                    if (kurs.bid > 0)
                        kursWaluty.KursSkupu = kurs.bid;

                    if (kurs.mid > 0)
                        kursWaluty.KursSredni = kurs.mid;

                    if (kurs.ask > 0)
                        kursWaluty.KursSprzedazy = kurs.ask;
                }


            }
        }


        public static void WczytajKrajeIWaluty()
        {

                using (XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(AppSettings.ConnectionString, null))
                {
                    using (IObjectSpace directObjectSpace = directProvider.CreateObjectSpace())
                    {
                        XafTypesInfo.Instance.RegisterEntity(typeof(Kraj));
                        XafTypesInfo.Instance.RegisterEntity(typeof(Waluta));

                        DodajKrajeIWaluty(directObjectSpace);
                        directObjectSpace.CommitChanges();

                    }
                }
          
        }

        public static void WczytajKursy(List<Notowanie> res)
        {
            if (res != null)
            {
                using (XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(AppSettings.ConnectionString, null))
                {
                    using (IObjectSpace directObjectSpace = directProvider.CreateObjectSpace())
                    {
                        XafTypesInfo.Instance.RegisterEntity(typeof(KursWaluty));
                        XafTypesInfo.Instance.RegisterEntity(typeof(Waluta));
                    
                        WczytajKursy(directObjectSpace,res);
                        directObjectSpace.CommitChanges();

                    }
                }
            }
        }

        public static void DodajKrajeIWaluty(IObjectSpace ObjectSpace)
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
