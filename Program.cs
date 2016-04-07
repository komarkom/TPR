using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace tpr_3_2
{
    class Data
    {
        public Data()
        {
        }
        public int OwnerCountRoomForConsolidate { get; set; }
        public int ConsolidateCountRoom { get; set; }
        public int AllCountRoom { get; set; }
        public float PriceForBuisness { get; set; }
        public float PriceForTourist { get; set; }
        public float PriceForTouroperate { get; set; }
        public int DemandRoomTourist { get; set; }
        public int DemandRoomBuisness { get; set; }
        public bool UseRandomDistribution { get; set; }

        public bool CheckData()
        {
            if (AllCountRoom >= 0
                && PriceForBuisness >= 0 && PriceForTourist >= 0 && PriceForTouroperate >= 0
                && DemandRoomTourist >= 0 && DemandRoomBuisness >= 0)
            {
                if (DemandRoomBuisness > AllCountRoom)
                {
                    DemandRoomBuisness = AllCountRoom;
                }
                if (DemandRoomTourist > AllCountRoom)
                {
                    DemandRoomTourist = AllCountRoom;
                }
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            var json = Console.ReadLine(); // Пример ниже
//            string json = @"{ AllCountRoom: '200', PriceForBuisness: '4000', PriceForTourist: '3000', PriceForTouroperate: '2000', DemandRoomTourist: '90', DemandRoomBuisness: '80', UseRandomDistribution: '0'}";
            Data data = new Data();
            try
            {
                JObject jsonData = JObject.Parse(json);

                foreach (var pair in jsonData)
                {
                    switch (pair.Key)
                    {
                        case "AllCountRoom":
                            data.AllCountRoom = (int)pair.Value;
                            continue;
                        case "PriceForBuisness":
                            data.PriceForBuisness = (float)pair.Value;
                            continue;
                        case "PriceForTourist":
                            data.PriceForTourist = (float)pair.Value;
                            continue;
                        case "PriceForTouroperate":
                            data.PriceForTouroperate = (float)pair.Value;
                            continue;
                        case "DemandRoomTourist":
                            data.DemandRoomTourist = (int)pair.Value;
                            continue;
                        case "DemandRoomBuisness":
                            data.DemandRoomBuisness = (int)pair.Value;
                            continue;
                        case "UseRandomDistribution":
                            data.UseRandomDistribution = (int)pair.Value != 0;
                            continue;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Parse error");
                Console.WriteLine(exception.Message);
                return 1;
            }

            if (!data.CheckData())
            {
                Console.WriteLine("Bad parametrs");
                return 2;
            }

            try
            {
                // ------------------------- рассчет плотности распределения
                float[] randValforTourist = new float[data.AllCountRoom + 1];
                randValforTourist[data.DemandRoomTourist] = 1000;

                float[] randValforBuisness = new float[data.AllCountRoom + 1];
                randValforBuisness[data.DemandRoomBuisness] = 1000;

                if (data.UseRandomDistribution)
                {
                    int key1 = (int)(data.DemandRoomTourist * 0.4);
                    randValforTourist[key1] = 550;
                    float key1Step1 = 500 / (float)key1;
                    float key1Step2 = 500 / (float)(data.DemandRoomTourist - key1);

                    for (int i = 0; i < key1; i++)
                    {
                        randValforTourist[i] = (int)(i * key1Step1);
                    }
                    for (int i = 1; i < data.DemandRoomTourist - key1; i++)
                    {
                        randValforTourist[key1 + i] = 500 + (int)(i * key1Step2);
                    }

                    int key2 = (int)(data.DemandRoomTourist + (data.AllCountRoom - data.DemandRoomTourist) * 0.4);
                    randValforTourist[key2] = 500;
                    float key2Step1 = 500 / (float)(key2 - data.DemandRoomTourist);
                    float key2Step2 = 500 / (float)(data.AllCountRoom - key2);

                    for (int i = data.DemandRoomTourist + 1; i < key2; i++)
                    {
                        randValforTourist[i] = 1000 - (int)((i - data.DemandRoomTourist) * key2Step1);
                    }
                    int j = 0;
                    for (int i = data.AllCountRoom; i > key2; i--)
                    {
                        randValforTourist[i] = (int)(j * key2Step2);
                        j++;
                    }

                    int key3 = (int)(data.DemandRoomBuisness * 0.4);
                    randValforBuisness[key3] = 500;
                    float key3Step1 = 500 / (float)key1;
                    float key3Step2 = 500 / (float)(data.DemandRoomBuisness - key1);
                    for (int i = 0; i < key3; i++)
                    {
                        randValforBuisness[i] = (int)(i * key3Step1);
                    }
                    for (int i = 1; i < data.DemandRoomBuisness - key3; i++)
                    {
                        randValforBuisness[key3 + i] = 500 + (int)(i * key3Step2);
                    }

                    int key4 = (int)(data.DemandRoomBuisness + (data.AllCountRoom - data.DemandRoomBuisness) * 0.4);
                    randValforBuisness[key4] = 500;
                    float key4Step1 = 500 / (float)(key4 - data.DemandRoomBuisness);
                    float key4Step2 = 500 / (float)(data.AllCountRoom - key4);
                    for (int i = data.DemandRoomBuisness + 1; i < key4; i++)
                    {
                        randValforBuisness[i] = 1000 - (int)((i - data.DemandRoomBuisness) * key4Step1);
                    }
                    j = 0;
                    for (int i = data.AllCountRoom; i > key4; i--)
                    {
                        randValforBuisness[i] = (int)(j * key4Step2);
                        j++;
                    }


                }
                else
                {
                    //тут надо звполнить массивы
                    //randValforBuisness, randValforTourist случайными значениями, можно в интервале {0..100}
                    var rand = new Random();
                    for (int i = 0; i <= data.AllCountRoom; i++)
                    {
                        randValforTourist[i] = rand.Next(100);
                        randValforBuisness[i] = rand.Next(100);
                    }
                }

                float totalCiunter1 = 0, totalCounter2 = 0;
                for (int i = 0; i <= data.AllCountRoom; i++)
                {
                    totalCiunter1 += randValforTourist[i];
                    totalCounter2 += randValforBuisness[i];
                }

                for (int i = 0; i <= data.AllCountRoom; i++)
                {
                    randValforTourist[i] /= totalCiunter1;
                    randValforBuisness[i] /= totalCounter2;
                }

                // ------------------------- рассчет функции распределения
                float[] randFunctionForTourist = new float[data.AllCountRoom + 1];
                float[] randFunctionForBuisness = new float[data.AllCountRoom + 1];

                float first = 0, second = 0;

                for (int i = 0; i <= data.AllCountRoom; i++)
                {
                    first += randValforTourist[i];
                    second += randValforBuisness[i];
                    randFunctionForTourist[i] = first;
                    randFunctionForBuisness[i] = second;
                }

                // ------------------------- расчет решений с точки зрения конcолидатора - владельца
                first = (data.PriceForBuisness - data.PriceForTouroperate) / data.PriceForBuisness;
                second = (data.PriceForTourist - data.PriceForTouroperate) / data.PriceForTourist;

                int counter1 = 0, counter2 = 0;
                while (first > randFunctionForBuisness[counter1])
                {
                    counter1++;
                }
                while (second > randFunctionForTourist[counter2])
                {
                    counter2++;
                }

                data.OwnerCountRoomForConsolidate = data.AllCountRoom - counter1;
                data.ConsolidateCountRoom = counter2;

                // ------------------------- рассчет согласованного решения
                int averCount = Math.Abs(counter1 - counter2);
                int minCount = Math.Min(counter1, counter2);

                float[] intersection = new float[averCount];
                for (int i = 0; i < averCount; i++)
                {
                    intersection[i] = Math.Abs(randFunctionForTourist[minCount + i] - 1 + randFunctionForBuisness[minCount + i]);
                }

                float minIntersection = intersection[0];
                int counter3 = 0;
                for (int i = 1; i < averCount; i++)
                {
                    if (intersection[i] < minIntersection) counter3 = i;
                }

                data.PriceForTouroperate = (data.PriceForTourist * (1 - randFunctionForTourist[counter3 + minCount]));
                Console.WriteLine("Owner count room for touroperate:    " + data.OwnerCountRoomForConsolidate + "  Touroperate count room:    " + data.ConsolidateCountRoom + "  Price:    " + data.PriceForTouroperate);
            }
            catch (Exception)
            {
                Console.WriteLine("Сalculations error");
                return 3;
            }
            Console.ReadKey();
            return 0;
        }
    }
}
