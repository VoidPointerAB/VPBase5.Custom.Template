using VPBase.Shared.Core.Helpers.Testing;

namespace VPBase.Custom.Core.Data.DataPalettes
{
    /// <summary>
    /// Note: The text Randomizer never get the same random text value INDEX twice in a row,
    /// if there is more than two rows etc. It uses all texts in the array first before it starts a new round
    public class CustomSampleRandomText
    {
        // SharedBaseRandomText in the VPBase.Shared.Core.Helpers.Testing contains some general randomize texts to use,
        // Example: the most 100 common firstnames for men, firstnames and women and lastnames in sweden. This i pretty poweful to use in combination.
        // Please add more randomized texts in the shared base class if it is GENERAL to be used in different demo test data cases.

        public static DataPaletteTextRandomizer AnimalsWithDescription = new DataPaletteTextRandomizer(new Random(), new List<string>()  // Note: Same random value is used in every startup of the application
        {
            "Stekel;En salt insekt. Steklar är en ordning i djurklassen insekter med fullständig förvandling.",
            "Bönsyrsa;En annan salt insekt. Bönsyrsor är en ordning insekter med över 2 300 olika arter som är indelade i 16 olika familjer",
            "Mops;Mops är en hundras från Storbritannien men med ett historiskt ursprung i Kina. Den är en dvärghund av molossertyp och sällskapshund",
            "Sphynx;Sphynx är en utåtriktad och busig katt som tycker om människor och älskar uppmärksamhet. Den hälsar ofta på sina ägare när de kommer hem och är mycket talför",
            "Räka;Äkta räkor är en infraordning i ordningen tiofotade kräftdjur och omfattar flera tusen arter, som nordhavsräka, sandräka och tångräka",
            "Elefant (grekiska: elephas);Elefant är ett mycket stort däggdjur med snabel och betar. Den utgör nutidens största landdjur med en vikt som uppmätts till tio ton och en längd på över tio meter från snabelspets till svansspets",
            "Taraʹntel (italienska tarantola);Ett räligt djur. Benämning på vissa sydeuropeiska vargspindlar."
        });

        public static DataPaletteTextRandomizer FruitsWithDescription = new DataPaletteTextRandomizer(new Random(), new List<string>()
        {
            "Äpple;Är en frukt som växer på träd",
            "Bananer;En gul frukt och namnet för ätliga bär som härstammar från stora blommande örter. Frukterna kan ha olika storlek, färg och fasthet, men är ofta långsmala och krökta, med mjukt, stärkelserikt fruktkött.",
            "Apelsin (Ciʹtrus × auraʹntium Sinensis-gruppen);Är ett litet träd med vita blommor inom citrussläktet. Trädet har 5 till drygt 10 cm stora frukter med orangefärgat skal och saftigt sötsyrligt fruktkött. Apelsiner äts oftast färska, som juice och som marmelad",
            "Blodapelsin;Exempel på blodapelsiner är Moro, Tarocco och Sanguinello. Ibland säljs blodapelsin som röd apelsin",
            "Citron (Citrus × limon);Citronen är inte en naturlig art utan en hybrid mellan andra arter i släktet Citrus, såsom exempelvis storcitrus (C. ×aurantium) och suckatcitron (C. medica). Frukten bergamott (C. × limon Bergamia-Gruppen) har samma ursprung som citronen",
            "Sharon, kaki, eller persimon;Är ett fruktträd av släktet Diospyros. Ibland kallas arten enbart persimon, men detta namn används även om andra arter i samma släkte; i synnerhet gäller detta arten med det ursprungliga namnet persimon",
            "Pomerans;Är en citrusfrukt, skapad som en hybrid mellan pompelmus och mandarin"
        });
    }
}