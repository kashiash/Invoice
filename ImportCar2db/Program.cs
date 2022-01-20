
using CarsDb.Module.Utils;
using System;


namespace ImportCar2db
{
    class Program
    {
        static void Main(string[] args)
        {

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            var MakeImporter = new Car2dbMakeImporter();
            MakeImporter.Import(".\\car2db\\car_make.csv");

            var ModelImporter = new Car2dbModelImporter();
            ModelImporter.Import(".\\car2db\\car_model.csv");

            var GenerationImporter = new Car2dbGenerationImporter();
            GenerationImporter.Import(".\\car2db\\car_generation.csv");

            var SerieImporter = new Car2dbSerieImporter();
            SerieImporter.Import(".\\car2db\\car_serie.csv");

            var TrimImporter = new Car2dbTrimImporter();
            TrimImporter.Import(".\\car2db\\car_trim.csv");

            var EqupimentImporter = new Car2dbEquipmentImporter();
            EqupimentImporter.Import(".\\car2db\\car_equipment.csv");

            var SpecificationsImporter = new Car2dbSpecificationImporter();
            SpecificationsImporter.Import(".\\car2db\\car_specification.csv");

            var SpecificationsValueImporter = new Car2dbSpecificationValueImporter();
            SpecificationsValueImporter.Import(".\\car2db\\car_specification_value.csv");



            Console.WriteLine("Import opcji");
            var OptionImporter = new Car2dbOptionImporter();
            OptionImporter.Import(".\\car2db\\car_option.csv");

            Console.WriteLine("Import wartosci opcji");
            var OptionValueImporter = new Car2dbOptionValueImporter();
            OptionValueImporter.Import(".\\car2db\\car_option_value.csv");
            watch.Stop();

            Console.WriteLine($"Complete Execution Time: {watch.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }
    }
}
