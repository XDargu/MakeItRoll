using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DataManager
{
    public enum Market
    {
        GooglePlay = 0,
        Amazon = 1
    }

    public static Market currentMaket = Market.GooglePlay;

    public static float userMPS = 0f;

    public static float meters = 0;

    public static float userSpeed = 0.2f;

    public static float goldenRollTime = 30f;

    public static int goldenRollChance = 2;

    public static float toiletCapacity = 1000f;
    public static float toiletMPS = 500f / 3600f;

    public static bool WCOpen = true;

    public static bool NoAds
    {
        get
        {
            if (PlayerPrefs.HasKey("no_ads")) { return true; }
            else { return false; }
        }
    }

    public static bool IsDoublePaperPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey("double_paper")) { return true; }
            else { return false; }
        }
    }

    public static bool IsHappyHour;
    public static DateTime happyHourTime;

    public static void EnableDoublePaper()
    {
        PlayerPrefs.SetInt("double_paper", 1);
    }

    public static void DoubleMeters()
    {
        GameObject.FindObjectOfType<StatsManager>().AddMeters(meters * 2f);
        meters = meters * 2f;
    }

    public static void x1000Meters()
    {
        GameObject.FindObjectOfType<StatsManager>().AddMeters(meters * 1000f);
        meters = meters * 1000f;
    }

    public static void x100Production()
    {
        PlayerPrefs.SetInt("x100active", 1);
        PlayerPrefs.SetString("x100StartTime", DateTime.Now.ToString());
        IsHappyHour = true;
        happyHourTime = DateTime.Now;
    }

    public static void DisableHappyHour()
    {
        PlayerPrefs.DeleteKey("x100active");
        PlayerPrefs.DeleteKey("x100StartTime");
        IsHappyHour = false;        
    }

    /***
     *  DATOS DE PRODUCTOS
     ***/

    public struct InAppProduct
    {
        public string icon;
        public string ID;
        public string name;
        public string description;
        public string price;

        public InAppProduct(string ID, string icon, string name, string description, string price)
        {
            this.icon = icon;
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.price = price;
        }
    }

    public static InAppProduct[] inAppProducts = new InAppProduct[] {
        new InAppProduct("double_paper", "ProdX2", "Double Production", "All your toilet paper production doubles!", "0,99$"),
        new InAppProduct("double_meters", "MetersX2", "Double meters", "Double your current amount of meters.", "0,99$"),
        new InAppProduct("x100_production", "ProdX100", "Happy hour", "For one hour, you make 100 times more meters!", "0,99$"),
        new InAppProduct("meters_1000", "MetersX1000", "Overdrive", "You get 1000 times your current amount of meters.", "1,99$")
    };

    public static bool loadedOnlineInAppProducts = false;

    public static void GetOnlineInAppProductDetails()
    {
        for (int i = 0; i < inAppProducts.Length; i++)
        {
            /*GoogleProductTemplate product = AndroidInAppPurchaseManager.instance.inventory.GetProductDetails(inAppProducts[i].ID);
            inAppProducts[i].name = product.title.Substring(0, product.title.Length - "(Make It Roll: Toilet Paper Fun)".Length);
            inAppProducts[i].description = product.description;
            inAppProducts[i].price = product.price;*/
        }
        loadedOnlineInAppProducts = true;
    }

    /***
     *  DATOS DE LA TIENDA
     ***/
    public struct StoreItem
    {
        public string ID;
        public string name;
        public string description;
        public float price;
        public float baseIncrement;

        public StoreItem(string ID, string name, string description, float price, float baseIncrement)
        {
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.price = price;
            this.baseIncrement = baseIncrement;
        }
    }

    public static StoreItem[] storeItems = new StoreItem[] {
        new StoreItem("SI0", "Plant", "Money don't grow on trees, but toilet paper does!", 10, 0.1f),
        new StoreItem("SI1", "Printer", "Isn't this illegal?", 35, 0.5f),
        new StoreItem("SI2", "Fountain", "Who needs the fountain of youth? We have the fountain of toilet paper!", 100, 3),
        new StoreItem("SI3", "Plantation", "'Ahhh... look at those fresh ripe toilet rolls, isn't nature splendid?'", 300, 7),
        new StoreItem("SI4", "Factory", "The industrial revolution reaches the toilet paper industry", 650, 15),
        new StoreItem("SI5", "Mine", "'We search in the bowels of the Earth to find the most valuable toilet rolls'", 2000, 40),
        new StoreItem("SI6", "Nuclear plant", "Fission the atom to get toilet paper", 5500, 100),
        new StoreItem("SI7", "Derricks", "In the deepest places, the toilet rolls of our ancestors awaits...", 25000, 300),
        new StoreItem("SI8", "Volcano", "Be careful with all those paper rolls on fire!", 150000, 1000),
        new StoreItem("SI9", "Cloning machine", "Clones toilet rolls to get more toilet rolls!", 1000000, 3000),
        new StoreItem("SI10", "Space station", "When the Earth runs out of toilet paper we must search... in space", 7500000, 10000),
        new StoreItem("SI11", "Transmuter", "Something in, toilet rolls out", 60000000, 35000),
        new StoreItem("SI12", "Planet", "We have discovered a solid toilet paper planet", 450000000, 120000),
        new StoreItem("SI13", "Worm hole", "Worm hole to the Toiletverse, where everything is made of toilet paper!", 4000000000, 400000),
        new StoreItem("SI14", "Nanobots", "Little robots that turns matter into toilet paper", 39999999999, 1500000),
        new StoreItem("SI15", "God", "The supreme creator brings you a hand making toilet rolls", 333333333333, 5500000),
    };

    public static Dictionary<string, int> storeItemsData = new Dictionary<string,int>();

    /***
     *  DATOS DE MEJORAS
     ***/

    public struct ToiletUpgrade
    {
        public string ID;
        public string prevID;
        public string name;
        public string description;
        public float price;
        public float speedSumIncrement;
        public float speedMulIncrement;
        public float capacitySumIncrement;
        public float capacityMulIncrement;

        public ToiletUpgrade(string ID, string prevID, string name, string description, float price, float speedSumIncrement, float speedMulIncrement, float capacitySumIncrement, float capacityMulIncrement)
        {
            this.ID = ID;
            this.prevID = prevID;
            this.name = name;
            this.description = description;
            this.price = price;
            this.speedMulIncrement = speedMulIncrement;
            this.speedSumIncrement = speedSumIncrement;
            this.capacityMulIncrement = capacityMulIncrement;
            this.capacitySumIncrement = capacitySumIncrement;
        }
    }

    public struct RollUpgrade
    {
        public string ID;
        public string prevID;
        public string name;
        public string description;
        public float price;
        public float speedSumIncrement;
        public float speedMulIncrement;
        public float capacitySumIncrement;
        public float capacityMulIncrement;

        public RollUpgrade(string ID, string prevID, string name, string description, float price, float speedSumIncrement, float speedMulIncrement, float capacitySumIncrement, float capacityMulIncrement)
        {
            this.ID = ID;
            this.prevID = prevID;
            this.name = name;
            this.description = description;
            this.price = price;
            this.speedMulIncrement = speedMulIncrement;
            this.speedSumIncrement = speedSumIncrement;
            this.capacityMulIncrement = capacityMulIncrement;
            this.capacitySumIncrement = capacitySumIncrement;
        }
    }

    public struct GoldenRollUpgrade
    {
        public string ID;
        public string name;
        public string description;
        public float price;
        public float timeSumIncrement;
        public float timeMulIncrement;
        public float frequencySumIncrement;
        public float frequencyMulIncrement;

        public GoldenRollUpgrade(string ID, string name, string description, float price, float timeSumIncrement, float timeMulIncrement, float frequencySumIncrement, float frequencyMulIncrement)
        {
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.price = price;
            this.timeSumIncrement = timeSumIncrement;
            this.timeMulIncrement = timeMulIncrement;
            this.frequencySumIncrement = frequencySumIncrement;
            this.frequencyMulIncrement = frequencyMulIncrement;
        }
    }

    public struct StoreItemUpgrade
    {
        public string ID;
        public string storeItemID;
        public int storeItemsNeeded;
        public string name;
        public string description;
        public float price;
        public float sumIncrement;
        public float mulIncrement;

        public StoreItemUpgrade(string ID, string storeItemID, int storeItemsNeeded, string name, string description, float price, float sumIncrement, float mulIncrement)
        {
            this.ID = ID;
            this.storeItemID = storeItemID;
            this.storeItemsNeeded = storeItemsNeeded;
            this.name = name;
            this.description = description;
            this.price = price;
            this.sumIncrement = sumIncrement;
            this.mulIncrement = mulIncrement;
        }
    }

    public static ToiletUpgrade[] toiletUpgrades = new ToiletUpgrade[] { 
        // Base 1000 capacidad
        new ToiletUpgrade("UPT0", null, "Reinforced tank", "Toilet gains 9,000 meters of capacity", 1000, 0f, 0f, 9000f, 0f), // 5,000
        new ToiletUpgrade("UPT1", "UPT0", "Extra toilet bowl", "Toilet gains 50 times its capacity", 50000, 0f, 0f, 0f, 50f), // 20,000
        new ToiletUpgrade("UPT2", "UPT1", "Super-sized tank", "Toilet gains 250 times its capacity", 5000000, 0f, 0f, 0f, 250f), // 2,000,000
        new ToiletUpgrade("UPT3", "UPT2", "Industrial pipes", "Toilet gains 1,000 times its capacity", 3000000000, 0f, 0f, 0f, 1000f), // 1,000,000,000
        new ToiletUpgrade("UPT4", "UPT3", "Black hole draining system", "Toilet gains 2,000 times its capacity", 3000000000000, 0f, 0f, 0f, 2000f), // 1,000,000,000,000

        // Base 500 por hora
        new ToiletUpgrade("UPTS0", null, "Overflow system", "Toilet is filled 10 times faster. ", 3500, 0f, 10f, 0f, 0f), // 1,000
        new ToiletUpgrade("UPTS1", "UPTS0", "Refined ballcock", "Toilet is filled 50 times faster", 200000, 0f, 50f, 0f, 0f), // 4,000
        new ToiletUpgrade("UPTS2", "UPTS1", "Push harder", "Toilet is filled 250 times faster", 25000000, 0f, 250f, 0f, 0f), // 400,000
        new ToiletUpgrade("UPTS3", "UPTS2", "Kitten helpers", "Toilet is filled 500 times faster", 10000000000, 0f, 500f, 0f, 0f), // 200,000,000
        new ToiletUpgrade("UPTS4", "UPTS3", "Time travel", "Toilet is filled 1,000 times faster", 10000000000000, 0f, 1000f, 0f, 0f), // 200,000,000,000
    };

    public static RollUpgrade[] rollUpgrades = new RollUpgrade[] {
        // Base: 3MPS
        new RollUpgrade("UPR0", null, "2 ply toilet paper", "Toilet paper drags are 5 times as efficient", 1000, 0f, 5f, 0f, 0f), // 30 MPS
        new RollUpgrade("UPR1", "UPR0", "4 ply toilet paper", "Toilet paper drags are 10 times as efficient", 25000, 0f, 10f, 0f, 0f), // 300 MPS
        new RollUpgrade("UPR2", "UPR1", "8 ply toilet paper", "Toilet paper drags are 25 times as efficient", 7500000, 0f, 25f, 0f, 0f), // 7,500 MPS
        new RollUpgrade("UPR3", "UPR2", "16 ply toilet paper", "Toilet paper drags are 50 times as efficient", 5000000000, 0f, 50f, 0f, 0f), // 375,000 MPS
        new RollUpgrade("UPR4", "UPR3", "32 ply toilet paper", "Toilet paper drags are 100 times as efficient", 5000000000000, 0f, 100f, 0f, 0f), // 37,500,000 MPS

        // Base: 15M - 3,33 seg
        new RollUpgrade("UPR10", null, "Bigger rolls", "Each toilet roll gains +185m of capacity", 2000, 0f, 0f, 185f, 0f), //  200 M -- 6,66 seg
        new RollUpgrade("UPR11", "UPR10", "Extra rolls", "Toilet rolls gains 10 times its capacity", 30000, 0f, 0f, 0f, 10f), // 2,000 M -- 6,66 seg
        new RollUpgrade("UPR12", "UPR11", "Huge rolls", "Toilet rolls gains 30 times its capacity", 10000000, 0f, 0f, 0f, 30f), // 60,000 M -- 8 seg
        new RollUpgrade("UPR13", "UPR12", "Mega rolls", "Toilet rolls gains 60 times its capacity", 12500000000, 0f, 0f, 0f, 60f), // 3,600,000 M -- 9,6 seg
        new RollUpgrade("UPR14", "UPR13", "Infinite rolls", "Toilet rolls gains 120 times its capacity", 25000000000000, 0f, 0f, 0f, 120f), // 432,000,000 M -- 11,52 seg
    };

    public static StoreItemUpgrade[] storeItemUpgrades = new StoreItemUpgrade[] {
        new StoreItemUpgrade("UP0", "SI0", 1, "Flower up", "All plants gains +0.1 m/s", 100, 0.1f, 0f),
        new StoreItemUpgrade("UP1", "SI0", 1, "High quality fertilizer", "Plants are twice as efficient", 1000, 0f, 2f),
        new StoreItemUpgrade("UP2", "SI0", 10, "Monster fertilizer", "Plants are twice as efficient", 10000, 0f, 2f),
        new StoreItemUpgrade("UP3", "SI0", 50, "Selective pruning", "Plants are twice as efficient", 500000, 0f, 2f),
        new StoreItemUpgrade("UP4", "SI0", 100, "Transgenic toilet paper", "Plants are twice as efficient", 10000000, 0f, 2f),
        
        new StoreItemUpgrade("UP5", "SI1", 1, "RGB printing", "All printers gains +0.3 m/s", 350, 0.3f, 0f),
        new StoreItemUpgrade("UP6", "SI1", 1, "Bigger printers", "Printers are twice as efficient", 3500, 0f, 2f),
        new StoreItemUpgrade("UP7", "SI1", 10, "Laser printers", "Printers are twice as efficient", 35000, 0f, 2f),
        new StoreItemUpgrade("UP8", "SI1", 50, "3D Printing", "Printers are twice as efficient", 1750000, 0f, 2f),
        new StoreItemUpgrade("UP9", "SI1", 100, "4D Printing", "Printers are twice as efficient", 35000000, 0f, 2f),
        
        new StoreItemUpgrade("UP10", "SI2", 1, "Superpipes", "All fountains gains +2 m/s", 1000, 2f, 0f),
        new StoreItemUpgrade("UP11", "SI2", 1, "Double jet", "Fountains are twice as efficient", 10000, 0f, 2f),
        new StoreItemUpgrade("UP12", "SI2", 10, "Megapipes", "Fountains are twice as efficient", 100000, 0f, 2f),
        new StoreItemUpgrade("UP13", "SI2", 50, "Triple jet", "Fountains are twice as efficient", 5000000, 0f, 2f),
        new StoreItemUpgrade("UP14", "SI2", 100, "Hyperpipes", "Fountains are twice as efficient", 100000000, 0f, 2f),
        
        new StoreItemUpgrade("UP15", "SI3", 1, "Automatic irrigation", "All plantations gains +3 m/s", 3000, 3, 0f),
        new StoreItemUpgrade("UP16", "SI3", 1, "Insecticide", "Plantations are twice as efficient", 30000, 0f, 2f),
        new StoreItemUpgrade("UP17", "SI3", 10, "Greenhouse", "Plantations are twice as efficient", 300000, 0f, 2f),
        new StoreItemUpgrade("UP18", "SI3", 50, "Robot harvester", "Plantations are twice as efficient", 15000000, 0f, 2f),
        new StoreItemUpgrade("UP19", "SI3", 100, "Genetically modified seeds", "Plantations are twice as efficient", 300000000, 0f, 2f),
        
        new StoreItemUpgrade("UP20", "SI4", 1, "Industrial revolution", "All factories gains +8 m/s", 6500, 8f, 0f),
        new StoreItemUpgrade("UP21", "SI4", 1, "Skilled workers", "Factories are twice as efficient", 65000, 0f, 2f),
        new StoreItemUpgrade("UP22", "SI4", 10, "Extra hours", "Factories are twice as efficient", 650000, 0f, 2f),
        new StoreItemUpgrade("UP23", "SI4", 50, "Assembly line", "Factories are twice as efficient", 32500000, 0f, 2f),
        new StoreItemUpgrade("UP24", "SI4", 100, "Automation", "Factories are twice as efficient", 650000000, 0f, 2f),
        
        new StoreItemUpgrade("UP25", "SI5", 1, "TNT", "All mines gains +24 m/s", 20000, 24f, 0f),
        new StoreItemUpgrade("UP26", "SI5", 1, "Electric wagons", "Mines are twice as efficient", 200000, 0f, 2f),
        new StoreItemUpgrade("UP27", "SI5", 10, "Diamond picks", "Mines are twice as efficient", 2000000, 0f, 2f),
        new StoreItemUpgrade("UP28", "SI5", 50, "Vein detector", "Mines are twice as efficient", 100000000, 0f, 2f),
        new StoreItemUpgrade("UP29", "SI5", 100, "Opencast mining", "Mines are twice as efficient", 2000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP30", "SI6", 1, "Optimized fission", "All nuclear plants gains +70 m/s", 55000, 70f, 0f),
        new StoreItemUpgrade("UP31", "SI6", 1, "Enriched toilet rolls", "Nuclear plants are twice as efficient", 550000, 0f, 2f),
        new StoreItemUpgrade("UP32", "SI6", 10, "Leaden toilet paper", "Nuclear plants are twice as efficient", 5500000, 0f, 2f),
        new StoreItemUpgrade("UP33", "SI6", 50, "Waste recycling", "Nuclear plants are twice as efficient", 275000000, 0f, 2f),
        new StoreItemUpgrade("UP34", "SI6", 100, "Toilet paper fusion", "Nuclear plants are twice as efficient", 5500000000, 0f, 2f),
        
        new StoreItemUpgrade("UP35", "SI7", 1, "Spiral drill", "All derricks gains +220 m/s", 250000, 220f, 0f),
        new StoreItemUpgrade("UP36", "SI7", 1, "Pumpjack", "Derricks are twice as efficient", 2500000, 0f, 2f),
        new StoreItemUpgrade("UP37", "SI7", 10, "Twin extractors", "Derricks are twice as efficient", 25000000, 0f, 2f),
        new StoreItemUpgrade("UP38", "SI7", 50, "Liquid toilet paper", "Derricks are twice as efficient", 1250000000, 0f, 2f),
        new StoreItemUpgrade("UP39", "SI7", 100, "Gassy toilet paper", "Derricks are twice as efficient", 25000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP40", "SI8", 1, "Fireproof toilet paper", "All volcanos gains +720 m/s", 1500000, 720f, 0f),
        new StoreItemUpgrade("UP41", "SI8", 1, "Pumice toilet rolls", "Volcanos are twice as efficient", 15000000, 0f, 2f),
        new StoreItemUpgrade("UP42", "SI8", 10, "Geothermal extraction", "Volcanos are twice as efficient", 150000000, 0f, 2f),
        new StoreItemUpgrade("UP43", "SI8", 50, "Induced eruptions", "Volcanos are twice as efficient", 7500000000, 0f, 2f),
        new StoreItemUpgrade("UP44", "SI8", 100, "Lava absorbent toilet paper", "Volcanos are twice as efficient", 150000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP45", "SI9", 1, "Stem cells", "All cloning machines gains +2,540 m/s", 10000000, 2540f, 0f),
        new StoreItemUpgrade("UP46", "SI9", 1, "Toilet paper genome map", "Cloning machines are twice as efficient", 100000000, 0f, 2f),
        new StoreItemUpgrade("UP47", "SI9", 10, "Toilet roll DNA cracking", "Cloning machines are twice as efficient", 1000000000, 0f, 2f),
        new StoreItemUpgrade("UP48", "SI9", 50, "Hybridization", "Cloning machines are twice as efficient", 50000000000, 0f, 2f),
        new StoreItemUpgrade("UP49", "SI9", 100, "Bio-toilet rolls", "Cloning machines are twice as efficient", 1000000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP50", "SI10", 1, "Low orbit toilet paper cannon", "All space stations gains +9,000 m/s", 75000000, 9000f, 0f),
        new StoreItemUpgrade("UP51", "SI10", 1, "Space mining", "Space stations are twice as efficient", 750000000, 0f, 2f),
        new StoreItemUpgrade("UP52", "SI10", 10, "Space elevator", "Space stations are twice as efficient", 7500000000, 0f, 2f),
        new StoreItemUpgrade("UP53", "SI10", 50, "Artificial gravity", "Space stations are twice as efficient", 375000000000, 0f, 2f),
        new StoreItemUpgrade("UP54", "SI10", 100, "Deep space extracting missions", "Space stations are twice as efficient", 7500000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP55", "SI11", 1, "Alchemy engine", "All transformers gains +32,000 m/s", 600000000, 32000f, 0f),
        new StoreItemUpgrade("UP56", "SI11", 1, "Heavy metal transmutation", "Transformers are twice as efficient", 6000000000, 0f, 2f),
        new StoreItemUpgrade("UP57", "SI11", 10, "High temperature alembic", "Transformers are twice as efficient", 60000000000, 0f, 2f),
        new StoreItemUpgrade("UP58", "SI11", 50, "Philosopher’s toilet roll", "Transformers are twice as efficient", 3000000000000, 0f, 2f),
        new StoreItemUpgrade("UP59", "SI11", 100, "Elixir of toilet paper", "Transformers are twice as efficient", 60000000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP60", "SI12", 1, "Mining base", "All planets gains +32,000 m/s", 4500000000, 126000f, 0f),
        new StoreItemUpgrade("UP61", "SI12", 1, "Colony", "Planets are twice as efficient", 45000000000, 0f, 2f),
        new StoreItemUpgrade("UP62", "SI12", 10, "Interplanetary trade", "Planets are twice as efficient", 450000000000, 0f, 2f),
        new StoreItemUpgrade("UP63", "SI12", 50, "Space capitalism", "Planets are twice as efficient", 22500000000000, 0f, 2f),
        new StoreItemUpgrade("UP64", "SI12", 100, "East Milky Way Company", "Planets are twice as efficient", 450000000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP65", "SI13", 1, "Quantum accelerator", "All worm holes gains +500,000 m/s", 40000000000, 500000f, 0f),
        new StoreItemUpgrade("UP66", "SI13", 1, "Quantum teleportation", "Worm holes are twice as efficient", 400000000000, 0f, 2f),
        new StoreItemUpgrade("UP67", "SI13", 10, "Regular teleportation", "Worm holes are twice as efficient", 4000000000000, 0f, 2f),
        new StoreItemUpgrade("UP68", "SI13", 50, "Black hole stabilizer", "Worm holes are twice as efficient", 200000000000000, 0f, 2f),
        new StoreItemUpgrade("UP69", "SI13", 100, "WARP device", "Worm holes are twice as efficient", 4000000000000000, 0f, 2f),
        
        new StoreItemUpgrade("UP70", "SI14", 1, "Framework update", "All nanobots gains +1,999,999 m/s", 399999999999, 1999999f, 0f),
        new StoreItemUpgrade("UP71", "SI14", 1, "Root access", "Nanobots are twice as efficient", 3999999999999, 0f, 2f),
        new StoreItemUpgrade("UP72", "SI14", 10, "IPv8 update", "Nanobots are twice as efficient", 39999999999999, 0f, 2f),
        new StoreItemUpgrade("UP73", "SI14", 50, "Tiny distance manipulator", "Nanobots are twice as efficient", 1999999999999999, 0f, 2f),
        new StoreItemUpgrade("UP74", "SI14", 100, "Picobots", "Nanobots are twice as efficient", 39999999999999999, 0f, 2f),
        
        new StoreItemUpgrade("UP75", "SI15", 1, "Sacred offerings", "All gods gains +13,333,333 m/s", 3333333333333, 13333333f, 0f),
        new StoreItemUpgrade("UP76", "SI15", 1, "Absolute morality", "Gods are twice as efficient", 33333333333333, 0f, 2f),
        new StoreItemUpgrade("UP77", "SI15", 10, "Polytheism", "Gods are twice as efficient", 333333333333333, 0f, 2f),
        new StoreItemUpgrade("UP78", "SI15", 50, "Mankind pledge", "Gods are twice as efficient", 16666666666666666, 0f, 2f),
        new StoreItemUpgrade("UP79", "SI15", 100, "The one above everything", "Gods are twice as efficient", 333333333333333333, 0f, 2f)
    };    

    public static Dictionary<string, bool> upgradesData = new Dictionary<string, bool>();

    /***
     *  MÉTODOS DE GUARDADO / CARGADO
     ***/

    public static void LoadMeters()
    {
        if (PlayerPrefs.HasKey("meters"))
        {
            meters = PlayerPrefs.GetFloat("meters");
        }
        else
        {
            meters = 0;
        }
    }

    public static void SaveMeters()
    {
        PlayerPrefs.SetFloat("meters", meters);
    }

    public static void LoadStoreItems()
    {
        foreach (StoreItem storeItem in storeItems) {
            if (PlayerPrefs.HasKey(storeItem.ID))
            {
                storeItemsData[storeItem.ID] = PlayerPrefs.GetInt(storeItem.ID);
            }
            else
            {
                storeItemsData[storeItem.ID] = 0;
            }
        }
    }

    public static void SaveStoreItems()
    {
        foreach (StoreItem storeItem in storeItems)
        {
            if (storeItemsData.ContainsKey(storeItem.ID))
            {
                PlayerPrefs.SetInt(storeItem.ID, storeItemsData[storeItem.ID]);
            }
            else
            {
                PlayerPrefs.SetInt(storeItem.ID, 0);
            }
        }
    }

    public static void SaveUpgrades()
    {
        foreach (StoreItemUpgrade storeItemUpgrade in storeItemUpgrades)
        {
            if (upgradesData.ContainsKey(storeItemUpgrade.ID))
            {
                if (upgradesData[storeItemUpgrade.ID] == true)
                {
                    PlayerPrefs.SetInt(storeItemUpgrade.ID, 1);
                }
            }
        }

        foreach (RollUpgrade rollUpgrade in rollUpgrades)
        {
            if (upgradesData.ContainsKey(rollUpgrade.ID))
            {
                if (upgradesData[rollUpgrade.ID] == true)
                {
                    PlayerPrefs.SetInt(rollUpgrade.ID, 1);
                }
            }
        }

        foreach (ToiletUpgrade toiletUpgrade in toiletUpgrades)
        {
            if (upgradesData.ContainsKey(toiletUpgrade.ID))
            {
                if (upgradesData[toiletUpgrade.ID] == true)
                {
                    PlayerPrefs.SetInt(toiletUpgrade.ID, 1);
                }
            }
        }
    }

    public static void DeleteUpgrades()
    {
        foreach (StoreItemUpgrade storeItemUpgrade in storeItemUpgrades)
        {
            if (PlayerPrefs.HasKey(storeItemUpgrade.ID))
            {
                PlayerPrefs.DeleteKey(storeItemUpgrade.ID);
            }
        }

        foreach (RollUpgrade rollUpgrade in rollUpgrades)
        {
            if (PlayerPrefs.HasKey(rollUpgrade.ID))
            {
                PlayerPrefs.DeleteKey(rollUpgrade.ID);
            }
        }

        foreach (ToiletUpgrade toiletUpgrade in toiletUpgrades)
        {
            if (PlayerPrefs.HasKey(toiletUpgrade.ID))
            {
                PlayerPrefs.DeleteKey(toiletUpgrade.ID);
            }
        }
    }

    public static void LoadUpgrades()
    {
        foreach (StoreItemUpgrade storeItemUpgrade in storeItemUpgrades)
        {
            if (PlayerPrefs.HasKey(storeItemUpgrade.ID))
            {
                upgradesData[storeItemUpgrade.ID] = PlayerPrefs.GetInt(storeItemUpgrade.ID) == 1;
            }
            else
            {
                upgradesData[storeItemUpgrade.ID] = false;
            }
        }

        foreach (RollUpgrade rollUpgrade in rollUpgrades)
        {
            if (PlayerPrefs.HasKey(rollUpgrade.ID))
            {
                upgradesData[rollUpgrade.ID] = PlayerPrefs.GetInt(rollUpgrade.ID) == 1;
            }
            else
            {
                upgradesData[rollUpgrade.ID] = false;
            }
        }

        foreach (ToiletUpgrade toiletUpgrade in toiletUpgrades)
        {
            if (PlayerPrefs.HasKey(toiletUpgrade.ID))
            {
                upgradesData[toiletUpgrade.ID] = PlayerPrefs.GetInt(toiletUpgrade.ID) == 1;
            }
            else
            {
                upgradesData[toiletUpgrade.ID] = false;
            }
        }
    }

    /**
     * MÉTODOS DE CÁLCULOS
     **/

    public static float GetStoreItemPrice(float basePrice, int storeItems)
    {
        return basePrice * Mathf.Pow(1.15f, storeItems);
    }

    static float GetStoreItemMPS(string ID)
    {
        float mps = 0;
        float baseIncrement = 0;
        float sumIncrement = 0;
        float mulIncrement = 1;

        foreach (StoreItem storeItem in storeItems)
        {
            if (storeItem.ID == ID)
            {
                baseIncrement = storeItem.baseIncrement;
            }
        }

        foreach (StoreItemUpgrade storeItemUpgrade in storeItemUpgrades)
        {
            if (storeItemUpgrade.storeItemID == ID)
            {
                if (upgradesData.ContainsKey(storeItemUpgrade.ID))
                {
                    if (upgradesData[storeItemUpgrade.ID] == true)
                    {
                        sumIncrement += storeItemUpgrade.sumIncrement;
                        if (storeItemUpgrade.mulIncrement > 0)
                            mulIncrement *= storeItemUpgrade.mulIncrement;
                    }
                }
            }
        }

        mps = (baseIncrement + sumIncrement) * mulIncrement;

        return mps;
    }

    static float GetStoreItemsMPS()
    {
        float mps = 0;
        
        foreach (StoreItem storeItem in storeItems)
        {
            mps += storeItemsData[storeItem.ID] * GetStoreItemMPS(storeItem.ID);
        }

        return mps;
    }

    public static float GetGeneralMultiplier()
    {
        return 1;
    }

    /**
     *  MÉTODOS DE CACHEO DE DATOS
     **/

    public static Dictionary<string, float> storeItemsPrice = new Dictionary<string, float>();
    public static Dictionary<string, float> storeItemsMPS = new Dictionary<string,float>();
    public static float totalMPS = 0;
    public static float userMPSMulIncrement = 0f;
    public static float userMPSSumIncrement = 1f;
    public static float rollCapacity = 15f;

    public static void UpdateRollData()
    {
        float capacity = 15f;
        float sumIncrement = 0f;
        float mulIncrement = 1f;

        float speedSumIncrement = 0f;
        float speedMulIncrement = 1f;

        foreach (RollUpgrade rollUpgrade in rollUpgrades)
        {
            if (upgradesData.ContainsKey(rollUpgrade.ID))
            {
                if (upgradesData[rollUpgrade.ID] == true)
                {
                    sumIncrement += rollUpgrade.capacitySumIncrement;
                    if (rollUpgrade.capacityMulIncrement > 0)
                        mulIncrement *= rollUpgrade.capacityMulIncrement;

                    speedSumIncrement += rollUpgrade.speedSumIncrement;
                    if (rollUpgrade.speedMulIncrement > 0)
                        speedMulIncrement *= rollUpgrade.speedMulIncrement;
                }
            }
        }

        rollCapacity = (capacity + sumIncrement) * mulIncrement;

        userMPSSumIncrement = speedSumIncrement;
        userMPSMulIncrement = speedMulIncrement;
    }

    public static void UpdateToiletData(bool fromUpdate)
    {
        float capacity = 500f;
        float sumIncrement = 0f;
        float mulIncrement = 1f;

        float speed = 500f / 3600f;
        float speedSumIncrement = 0f;
        float speedMulIncrement = 1f;

        foreach (ToiletUpgrade toiletUpgrade in toiletUpgrades)
        {
            if (upgradesData.ContainsKey(toiletUpgrade.ID))
            {
                if (upgradesData[toiletUpgrade.ID] == true)
                {
                    sumIncrement += toiletUpgrade.capacitySumIncrement;
                    if (toiletUpgrade.capacityMulIncrement > 0)
                        mulIncrement *= toiletUpgrade.capacityMulIncrement;

                    speedSumIncrement += toiletUpgrade.speedSumIncrement;
                    if (toiletUpgrade.speedMulIncrement > 0)
                        speedMulIncrement *= toiletUpgrade.speedMulIncrement;
                }
            }
        }

        float Tcapacity = (capacity + sumIncrement) * mulIncrement;
        toiletMPS = (speed + speedSumIncrement) * speedMulIncrement;

        // Si la capacidad ha aumentado, es necesario reducir el fill de la barra
        if (fromUpdate)
        {
            if (Tcapacity != toiletCapacity)
            {
                StatsManager statsManager = GameObject.FindObjectOfType<StatsManager>();

                float newFill = toiletCapacity * statsManager.GetFill() / Tcapacity;
                statsManager.SetFill(newFill);
            }
        }

        toiletCapacity = Tcapacity;
    }

    public static void UpdateStoreItemsMPS()
    {
        foreach (StoreItem storeItem in storeItems)
        {
            storeItemsMPS[storeItem.ID] = GetStoreItemMPS(storeItem.ID);
        }
    }

    public static void UpdateStoreItemsPrice()
    {
        foreach (StoreItem storeItem in storeItems)
        {
            storeItemsPrice[storeItem.ID] = GetStoreItemPrice(storeItem.price, storeItemsData[storeItem.ID]);
        }
    }

    public static void UpdateTotalMPS()
    {
        float mps = 0;
        
        foreach (StoreItem storeItem in storeItems)
        {
            mps += storeItemsMPS[storeItem.ID] * storeItemsData[storeItem.ID];
        }

        totalMPS = mps;
    }

    /**
     * EVENTOS
     **/

    public struct Event
    {
        public string ID;
        public string title;
        public string description;
        public string image;
        public float metersToPlay;

        public Event(string ID, string title, string description, string image, float metersToPlay)
        {
            this.ID = ID;
            this.title = title;
            this.description = description;
            this.image = image;
            this.metersToPlay = metersToPlay;
        }
    }

    public static Event[] events = new Event[] {
        //new Event("EV0", "Wrapping news", "With 40,075,017 meters, the amount of produced toilet paper is enough to circle the Earth!", "", 15)
    };

    public static Dictionary<string, bool> eventData = new Dictionary<string,bool>();

    public static void SaveEvents()
    {
        foreach (Event ev in events)
        {
            if (eventData.ContainsKey(ev.ID))
            {
                PlayerPrefs.SetInt(ev.ID, 1);
            }
        }
    }

    public static void LoadEvents()
    {
        foreach (Event ev in events)
        {
            if (PlayerPrefs.HasKey(ev.ID))
            {
                eventData[ev.ID] = true;
            }
            else
            {
                eventData[ev.ID] = false;
            }
        }
    }

    public static void DeleteEvents()
    {
        foreach (Event ev in events)
        {
            if (PlayerPrefs.HasKey(ev.ID))
            {
                PlayerPrefs.DeleteKey(ev.ID);
            }
        }
    }

    public static void FireEvent(string ID)
    {
        eventData[ID] = true;
    }

    /**
     * OTROS
     **/

    public static void Reset()
    {
        GameObject.FindObjectOfType<MoveDown>().ReInitVariables();

        meters = 0;
        totalMPS = 0f;
        userMPS = 0f;
        SaveMeters();

        upgradesData.Clear();
        storeItemsData.Clear();        
        eventData.Clear();

        DeleteUpgrades();
        DeleteEvents();

        SaveStoreItems();
        SaveUpgrades();
        SaveEvents();

        LoadStoreItems();
        LoadUpgrades();
        LoadEvents();

        UpdateRollData();
        UpdateToiletData(false);
        UpdateStoreItemsMPS();
        UpdateStoreItemsPrice();
        UpdateTotalMPS();

        GameObject.FindObjectOfType<StatsManager>().ResetFill(false);
        meters = 0;
    }
    
}
